using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using GenomeNext.Utility;
using GenomeNext.Cloud.CloudNoSQL;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;
using GenomeNext.Data.Metadata;

namespace GenomeNext.App
{
    public class ProjectService : GNEntityService<GNProject>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "PROJECT";

        public AspNetRoleService aspNetRoleService { get; set; }
        public TeamService teamService { get; set; }

        public ProjectService(GNEntityModelContainer db, IdentityModelContainer identityDB)
            : base(db)
        {
            base.db = db;
            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.teamService = new TeamService(db,identityDB);
        }

        public IQueryable<GNProject> FindMyProjects(GNContact userContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNProject> projects = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                projects = db.GNProjects;
            }
            //ORG_MANAGER
            else if (userContact.IsInRole("ORG_MANAGER"))
            {
                projects = db.GNProjects
                    .Where(p => p.Teams.Select(t => t.OrganizationId).Contains(userContact.GNOrganizationId));
            }
            //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                projects = db.GNTeams
                    .Where(t => (
                        t.OrganizationId == userContact.GNOrganizationId
                        && t.TeamMembers.Count(tm => tm.GNContactId == userContact.Id) != 0
                        )
                    ).SelectMany(t => t.Projects);
            }

            return projects;
        }

        public override async Task<List<GNProject>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNProject> projects = FindMyProjects(userContact);

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    projects = projects.Where(p => p.Teams.FirstOrDefault().Organization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Team Name"))
                {
                    filterVal = (string)filters["Team Name"];
                    projects = projects.Where(p => p.Teams.FirstOrDefault().Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Project Name"))
                {
                    filterVal = (string)filters["Project Name"];
                    projects = projects.Where(p => p.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Project Lead"))
                {
                    filterVal = (string)filters["Project Lead"];
                    projects = projects.Where(p => p.ProjectLead.FirstName.Contains(filterVal) || p.ProjectLead.LastName.Contains(filterVal));
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    projects = projects
                        .Where(p => p.Name.Contains(filterVal)
                            || p.Teams.FirstOrDefault().Organization.Name.Contains(filterVal)
                            || p.Teams.FirstOrDefault().Name.Contains(filterVal)
                            || p.ProjectLead.FirstName.Contains(filterVal)
                            || p.ProjectLead.LastName.Contains(filterVal));
                }
            }

            //Order By Results
            projects = projects
                .OrderBy(p => p.ProjectLead.LastName)
                .OrderBy(p => p.Teams.FirstOrDefault().Organization.Name)
                .OrderBy(p => p.Teams.FirstOrDefault().Name)
                .OrderBy(p => p.Name)
                .OrderByDescending(t => t.CreateDateTime);

            //Limit Result Size
            projects = projects.Skip(start).Take(end - start);

            return EvalEntityListSecurity(userContact, await projects.ToListAsync());
        }

        public override async Task<GNProject> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNProjects.FindAsync(keys);
        }

        public override async Task<GNProject> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNProject project = (GNProject)entity;

            try
            {
                project.Id = Guid.NewGuid();
                project.CreateDateTime = DateTime.Now;

                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNProjects] " +
                    "([Id],[Name],[Description],[StartDate],[EndDate],[CreatedBy],[CreateDateTime],[ProjectLead_Id]) " +
                    "VALUES " +
                    "(@projectId, " +
                    "@projectName, " +
                    "@projectDescription, " +
                    "@projectStartDate, " +
                    "@projectEndDate, " +
                    "@projectCreatedBy, " +
                    "@projectCreateDateTime, " +
                    "@projectProjectLeadId)",
                    new SqlParameter("@projectId", project.Id),
                    new SqlParameter("@projectName", project.Name),
                    new SqlParameter("@projectDescription", project.Description),
                    new SqlParameter("@projectStartDate", project.StartDate),
                    new SqlParameter("@projectEndDate", project.EndDate),
                    new SqlParameter("@projectCreatedBy", project.CreatedBy.Value),
                    new SqlParameter("@projectCreateDateTime", project.CreateDateTime),
                    new SqlParameter("@projectProjectLeadId", Guid.Parse(project.ProjectLeadId)));

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNTeamGNProject]([Teams_Id],[Projects_Id]) " +
                    "VALUES(@teamId,@projectId)",
                    new SqlParameter("@teamId", Guid.Parse(project.TeamId)),
                    new SqlParameter("@projectId", project.Id));

                tx.Commit();

                bool auditResult = new GNCloudNoSQLService().LogEvent(project.CreatedByContact, project.Id, this.ENTITY, "N/A", "EVENT_INSERT");
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to create project.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger,e2.Message, e2);
                throw e2;
            }

            return project;
        }

        public override async Task<GNProject> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNProject project = (GNProject)entity;

            try
            {
                db.Entry(project).State = EntityState.Modified;

                await db.SaveChangesAsync();

                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "UPDATE [gn].[GNProjects] SET [ProjectLead_Id] = @projectLeadId " +
                    "WHERE [Id] = @projectId",
                    new SqlParameter("@projectLeadId", Guid.Parse(project.ProjectLeadId)),
                    new SqlParameter("@projectId", project.Id));

                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNTeamGNProject] " +
                    "WHERE [Projects_Id] = @projectId",
                    new SqlParameter("@projectId", project.Id));

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNTeamGNProject]([Teams_Id],[Projects_Id]) " +
                    "VALUES(@teamId,@projectId)",
                    new SqlParameter("@teamId", Guid.Parse(project.TeamId)),
                    new SqlParameter("@projectId", project.Id));

                tx.Commit();
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to update project.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger,e2.Message, e2);
                throw e2;
            }

            return project;
        }

        public override async Task<int> Delete(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                var tx = db.Database.BeginTransaction();

                result = db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNTeamGNProject] " +
                    "WHERE [Projects_Id] = @projectId",
                    new SqlParameter("@projectId", keys[0]));

                result = db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNProjects] " +
                    "WHERE [Id] = @projectId",
                    new SqlParameter("@projectId", keys[0]));

                tx.Commit();

                return result;
            
            });
        }

        public override GNProject EvalEntitySecurity(GNContact userContact, GNProject project)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (project != null)
            {
                bool isProjectOwnedByOrganization = IsProjectOwnedByOrganization(userContact, project);

                //CREATE Privileges
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    project.CanCreate = true;
                }
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    project.CanCreate = true;
                }
                else if (userContact.IsInRole("TEAM_MANAGER"))
                {
                    project.CanCreate = true;
                }
                else if (userContact.IsInRole("PROJECT_MANAGER"))
                {
                    project.CanCreate = true;
                }
                else if (userContact.IsInRole("TEAM_MEMBER"))
                {
                    project.CanCreate = false;
                }
            
                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    project.CanView = true;
                    project.CanEdit = true;
                    project.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    if (isProjectOwnedByOrganization)
                    {
                        project.CanView = true;
                        project.CanEdit = true;
                        project.CanDelete = true;
                    }
                }
                //COLLABORATOR
                else if (userContact.IsInRole("COLLABORATOR"))
                {
                    project.CanCreate = false;
                    project.CanEdit = false;
                    project.CanDelete = false;
                    project.CanView = false;
                    if (isProjectOwnedByOrganization)
                    {
                        var teamProject = project.Teams.FirstOrDefault();

                        bool isAssignedToTeamOfProject = teamService.IsUserAssignedToTeam(userContact, teamProject);
                        bool isAssignedToTeamAsLeadOfProject = teamService.IsUserAssignedToTeamAsLead(userContact, teamProject);

                        if (isAssignedToTeamOfProject || isAssignedToTeamAsLeadOfProject)
                        {
                            project.CanView = true;
                        }                        
                    }
                }
                //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
                else if (isProjectOwnedByOrganization)
                {
                    var teamProject = project.Teams.FirstOrDefault();

                    if (teamProject == null && project != null && !string.IsNullOrEmpty(project.TeamId))
                    {
                        teamProject = base.db.GNTeams.Find(Guid.Parse(project.TeamId));
                    }

                    bool isAssignedToTeamOfProject = teamService.IsUserAssignedToTeam(userContact, teamProject);
                    bool isAssignedToTeamAsLeadOfProject = teamService.IsUserAssignedToTeamAsLead(userContact, teamProject);
                    bool isCreatorOfProject = project.CreatedBy != null && project.CreatedBy != Guid.Empty && project.CreatedBy == userContact.Id;

                    if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeamAsLeadOfProject)
                    {
                        project.CanView = true;
                        project.CanEdit = true;
                        project.CanDelete = true;
                    }
                    else if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeamOfProject)
                    {
                        project.CanView = true;
                        project.CanEdit = true;
                        project.CanDelete = true;
                    }
                    else if (userContact.IsInRole("PROJECT_MANAGER") && isAssignedToTeamOfProject)
                    {
                        project.CanView = true;

                        if (isCreatorOfProject)
                        {
                            project.CanEdit = true;
                            project.CanDelete = true;
                        }
                        else
                        {
                            project.CanEdit = false;
                            project.CanDelete = false;
                        }
                    }
                    else if (userContact.IsInRole("TEAM_MEMBER") && isAssignedToTeamOfProject)
                    {
                        project.CanView = true;
                        project.CanEdit = false;
                        project.CanDelete = false;
                    }
                }

                //Prevent Deletion of Projects with analysis requests
                if (project.CanDelete
                    && (project.AnalysisRequests == null || project.AnalysisRequests.Count == 0))
                {
                    project.CanDelete = true;
                }
                else
                {
                    project.CanDelete = false;
                }
            }

            return project;
        }

        private bool IsProjectOwnedByOrganization(GNContact userContact, GNProject project)
        {
            if (project != null && project.Teams != null && project.Teams.Count != 0)
            {
                return userContact.GNOrganizationId == project.Teams.FirstOrDefault().OrganizationId;
            }
            else if (project != null && project.TeamId != null)
            {
                var team = base.db.GNTeams.Find(Guid.Parse(project.TeamId));
               
                if(team != null)
                {
                    return userContact.GNOrganizationId == team.OrganizationId;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
