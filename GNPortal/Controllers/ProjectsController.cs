using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Models;
using GenomeNext.Portal.ControllerExtensions;
using System.Data.SqlClient;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class ProjectsController : GNEntityController<GNProject>
    {
        private readonly string ENTITY = "PROJECT";

        public ProjectsController()
            : base()
        {
            entityService = new ProjectService(base.db, base.identityDB);
        }

        public override void EvalCanCreate()
        {
            EvalCanCreateProject();
            ViewBag.CanCreate = ViewBag.CanCreateProject; 
        }

        public override GNProject CreateOnLoad()
        {
            EvalCanCreateTeam();
            return base.CreateOnLoad();
        }

        public override GNProject CreateOnSubmit(GNProject project)
        {
            auditResult = audit.LogEvent(UserContact, project.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            project.ProjectLeadId = Request["ProjectLeadList"];
            project.TeamId = Request["TeamsList"];
            project.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            project.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            return base.CreateOnSubmit(project);
        }

        public override GNProject DetailsOnLoad(GNProject project)
        {
            auditResult = audit.LogEvent(UserContact, project.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_DETAILS_UI);

            //Breadcrumbs Info
            ViewBag.CurrentProject = project;
            Guid TeamId = project.Teams.FirstOrDefault().Id;
            GNTeam Team = db.GNTeams.Where(a => a.Id.Equals(TeamId)).FirstOrDefault();
            ViewBag.CurrentTeam = Team;

            //action privileges
            EvalCanCreateAnalysisRequest();

            return base.DetailsOnLoad(project);
        }

        public override ActionResult CreateOnSuccess(GNProject project)
        {
            if (!string.IsNullOrEmpty(Request["teamId"]))
            {
                return RedirectToAction("Details", new { id = project.Id, teamId = Request["teamId"] });
            }
            else
            {
                return RedirectToAction("Details", new { id = project.Id });
            }
        }

        public override GNProject EditOnSubmit(GNProject project)
        {
            auditResult = audit.LogEvent(UserContact, project.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            project.ProjectLeadId = Request["ProjectLeadList"];
            project.TeamId = Request["TeamsList"];

            return base.EditOnSubmit(project);
        }

        public override ActionResult EditOnSuccess(GNProject project)
        {
            if (!string.IsNullOrEmpty(Request["teamId"]))
            {
                return RedirectToAction("Details", new { id = project.Id, teamId = Request["teamId"] });
            }
            else
            {
                return RedirectToAction("Details", new { id = project.Id });
            }
        }

        public override GNProject PopulateSelectLists(GNProject project = null)
        {
            IQueryable<GNTeam> teams = null;

            if (project != null && project.Teams != null && project.Teams.FirstOrDefault() != null)
            {
                ViewBag.TeamsSize = 1;
                teams = db.GNTeams.Where(t => t.OrganizationId == UserContact.GNOrganizationId);
                ViewBag.TeamsList = new SelectList(teams, "Id", "Name", project.Teams.FirstOrDefault().Id);
            }
            else
            {
                var teamId = Request["teamId"];
                if (string.IsNullOrEmpty(teamId))
                {
                    teamId = Request["TeamsList"];
                }

                if (((ProjectService)entityService).aspNetRoleService.IsUserContactAdmin(UserContact) 
                    || UserContact.IsInRole("ORG_MANAGER"))
                {
                    teams = db.GNTeams
                        .Where(t => t.OrganizationId == UserContact.GNOrganizationId).OrderBy(a => a.Name);
                }
                else
                {
                    teams = db.GNTeams
                        .Where(t => t.OrganizationId == UserContact.GNOrganizationId)
                        .Where(t => t.TeamMembers.Select(tm => tm.GNContactId).Contains(UserContact.Id)).OrderBy(a => a.Name);
                }
                ViewBag.TeamsSize = teams.Count();
                ViewBag.TeamsList = new SelectList(teams, "Id", "Name", teamId);

            }

            if (project != null && (project.Teams == null || project.Teams.Count == 0) && teams != null && teams.Count() != 0)
            {
                project.Teams = teams.ToList();
            }

            if(project != null && project.ProjectLead != null)
            {
                project.ProjectLeadId = project.ProjectLead.Id.ToString();
            }

            EvalCanCreateAnalysisRequest();

            return base.PopulateSelectLists(project);
        }

        public JsonResult GetContactsForTeam()
        {
            var teamId = Request["teamId"];
            string role = Request["excludeRole"].ToString().Trim().ToUpper();


            string roleId = identityDB.AspNetRoles.Where(a => a.Name.Equals(role)).FirstOrDefault().Id;
            int hierarchyId = int.Parse(identityDB.AspNetRoles.Where(a => a.Name.Equals(role)).FirstOrDefault().HierarchyOrder.ToString());
            
            var teamMembers = db.GNTeams
                                        .Find(Guid.Parse(teamId)).TeamMembers
                                            .Select(tm => tm.Contact)
                                                .Where(ct => ct.GNContactRoles.Any(id => id.AspNetRoleId != roleId))
                                                    .OrderBy(a => a.FirstName)
                                        .ToList();
            
            List<GNContact> contacts = new List<GNContact>();
            foreach (var tm in teamMembers)
            {
                contacts.Add(new GNContact
                {
                    Id = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    Email = tm.Email
                });
            }
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public override async Task<ActionResult> Delete(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);

            ViewResult view = (ViewResult)await base.Delete(id);

            view.ViewName = "Details";
            view.ViewBag.IsDelete = true;
            view.ViewBag.CanCreateAnalysisRequest = false;

            return view;
        }
    }
}
