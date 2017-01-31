using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using GenomeNext.Cloud.CloudNoSQL;
using System.Reflection;
using GenomeNext.Data;
using GenomeNext.Data.Metadata;

namespace GenomeNext.App
{
    public class TeamService : GNEntityService<GNTeam>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "TEAM";

        public AspNetRoleService aspNetRoleService { get; set; }
        public AspNetUserRoleService aspNetUserRoleService { get; set; }

        public TeamService(GNEntityModelContainer db, IdentityModelContainer identityDB)
        {
            base.db = db;

            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.aspNetUserRoleService = new AspNetUserRoleService(identityDB);
        }

        public override async Task<List<GNTeam>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNTeam> teams = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                teams = db.GNTeams
                    .Include(t => t.TeamLead)
                    .Include(t => t.Organization)
                    .Include(t => t.TeamMembers);
            }
            //ORG_MANAGER
            else if (userContact.IsInRole("ORG_MANAGER"))
            {
                teams = db.GNTeams
                    .Include(t => t.TeamLead)
                    .Include(t => t.Organization)
                    .Include(t => t.TeamMembers)
                    .Where(t => t.OrganizationId == userContact.GNOrganizationId);
            }
            //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                teams = db.GNTeams
                    .Include(t => t.TeamLead)
                    .Include(t => t.Organization)
                    .Include(t => t.TeamMembers)
                    .Where(t => t.OrganizationId == userContact.GNOrganizationId)
                    .Where(t => t.TeamMembers.Select(tm => tm.GNContactId).Contains(userContact.Id));
            }

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    teams = teams.Where(t => t.Organization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Team Name"))
                {
                    filterVal = (string)filters["Team Name"];
                    teams = teams.Where(t => t.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Team Lead"))
                {
                    filterVal = (string)filters["Team Lead"];
                    teams = teams.Where(t => t.TeamLead.FirstName.Contains(filterVal) || t.TeamLead.LastName.Contains(filterVal));
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    teams = teams
                        .Where(t => t.Name.Contains(filterVal)
                            || t.Organization.Name.Contains(filterVal)
                            || t.TeamLead.FirstName.Contains(filterVal) 
                            || t.TeamLead.LastName.Contains(filterVal));
                }
            }

            //Order By Results
            teams = teams
                .OrderBy(t => t.TeamLead.LastName)
                .OrderBy(t => t.Name)
                .OrderBy(t => t.Organization.Name)
                .OrderByDescending(t => t.CreateDateTime);

            //Limit Result Size
            teams = teams.Skip(start).Take(end - start);

            return EvalEntityListSecurity(userContact, await teams.ToListAsync());
        }

        public override async Task<GNTeam> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNTeams.FindAsync(keys);
        }

        public override async Task<GNTeam> Insert(GNContact userContact, object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNTeam team = await base.Insert(userContact, entity);

            var teamManagerRole = (await this.aspNetRoleService.FindAll()).SingleOrDefault(r => r.Name == "TEAM_MANAGER");

            GNTeamMember teamMember = new GNTeamMember()
            {
                GNTeamId = team.Id,
                GNContactId = team.GNContactId,
                Team = await Find(userContact, team.Id),
                Contact = db.GNContacts.Find(team.GNContactId),
                AssignDate = DateTime.Now,
                CreateDateTime = DateTime.Now,
                CreatedBy = userContact.Id,
                CreatedByContact = db.GNContacts.Find(userContact.Id)
            };
            db.GNTeamMembers.Add(teamMember);
            await db.SaveChangesAsync();

            return team;
        }

        public override async Task<int> Delete(GNContact userContact, params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            var entityKey = (Guid)keys[0];
            db.GNTeamMembers.RemoveRange(db.GNTeamMembers.Where(tm => tm.GNTeamId == entityKey));
            int result = await db.SaveChangesAsync();

            result = await base.Delete(userContact, entityKey);

            return result;
        }

        public override GNTeam EvalEntitySecurity(GNContact userContact, GNTeam team)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (team != null)
            {
                //set to false by default
                team.CanCreate = false;
                team.CanView = false;
                team.CanEdit = false;
                team.CanDelete = false;

                bool isTeamOwnedByOrganization = IsTeamOwnedByOrganization(userContact, team);

                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    team.CanCreate = true;
                    team.CanView = true;
                    team.CanEdit = true;
                    team.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    team.CanCreate = true;

                    if (isTeamOwnedByOrganization)
                    {
                        team.CanView = true;
                        team.CanEdit = true;
                        team.CanDelete = true;
                    }
                }
                //COLLABORATOR
                else if (userContact.IsInRole("COLLABORATOR"))
                {
                    team.CanCreate = false;
                    team.CanEdit = false;
                    team.CanDelete = false;
                    team.CanView = false;
                    if (isTeamOwnedByOrganization)
                    {
                        bool isAssignedToTeam = IsUserAssignedToTeam(userContact, team);
                        bool isAssignedToTeamAsLead = IsUserAssignedToTeamAsLead(userContact, team);

                        if (isAssignedToTeam || isAssignedToTeamAsLead)
                        {
                            team.CanView = true;
                        }                        
                    }
                }
                //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
                else
                {
                    if (isTeamOwnedByOrganization)
                    {
                        bool isAssignedToTeam = IsUserAssignedToTeam(userContact, team);
                        bool isAssignedToTeamAsLead = IsUserAssignedToTeamAsLead(userContact, team);

                        if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeamAsLead)
                        {
                            team.CanView = true;
                            team.CanEdit = true;
                        }
                        else if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeam)
                        {
                            team.CanView = true;
                        }
                        else if (userContact.IsInRole("PROJECT_MANAGER") && isAssignedToTeam)
                        {
                            team.CanView = true;
                        }
                        else if (userContact.IsInRole("TEAM_MEMBER") && isAssignedToTeam)
                        {
                            team.CanView = true;
                        }
                    }
                }

                //Prevent Deletion of Teams with sub-projects or assigned team members
                if (team.CanDelete
                    && ((team.Projects != null && team.Projects.Count != 0)))
                {
                    team.CanDelete = false;
                }
            }

            return team;
        }

        public bool IsTeamOwnedByOrganization(GNContact userContact, GNTeam team)
        {
            if (team != null)
            {
                return userContact.GNOrganizationId == team.OrganizationId;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserAssignedToTeam(GNContact userContact, GNTeam team)
        {
            if (team != null)
            {
                return team.TeamMembers.Count(t => t.GNContactId == userContact.Id) != 0;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserAssignedToTeamAsLead(GNContact userContact, GNTeam team)
        {
            if (team != null && team.TeamLead == null)
            {
                team.TeamLead = db.GNContacts.Find(team.GNContactId);
            }

            if (userContact != null && team != null && team.TeamLead != null)
            {
                return team.TeamLead.Id == userContact.Id;
            }
            else
            {
                return false;
            }
        }
    }

    public class TeamMemberService : GNEntityService<GNTeamMember>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AspNetRoleService aspNetRoleService { get; set; }

        public TeamMemberService(GNEntityModelContainer db, IdentityModelContainer identityDB)
        {
            base.db = db;

            this.aspNetRoleService = new AspNetRoleService(identityDB);
        }

        public override async Task<List<GNTeamMember>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNTeamMember> teamMembers =
                await db.GNTeamMembers
                .Include(g => g.Team)
                .Include(g => g.Contact)
                .Include(g => g.CreatedByContact)
                .ToListAsync();

            return teamMembers;
        }

        public override async Task<GNTeamMember> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            GNTeamMember teamMember = await db.GNTeamMembers.FindAsync(keys);
            return teamMember;
        }

        public override async Task<GNTeamMember> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            ((GNTeamMember)entity).AssignDate = DateTime.Now;

            ((GNTeamMember)entity).CreateDateTime = DateTime.Now;

            return await base.Insert(entity);
        }
    }
}
