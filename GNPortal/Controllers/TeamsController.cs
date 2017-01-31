using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Data.EntityModel;
using GenomeNext.Portal.Models;
using GenomeNext.Data.IdentityModel;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class TeamsController : GNEntityController<GNTeam>
    {
        private readonly string ENTITY = "TEAM";
        public ProjectService projectService { get; set; }
        public ContactService contactService { get; set; }

        public TeamsController()
            : base()
        {
            entityService = new TeamService(base.db, base.identityDB);
            projectService = new ProjectService(base.db, base.identityDB);
            contactService = new ContactService(base.db, base.identityDB);
        }

        public override void EvalCanCreate()
        {
            EvalCanCreateTeam();
            ViewBag.CanCreate = ViewBag.CanCreateTeam;
        }

        public override GNTeam DetailsOnLoad(GNTeam entity)
        {
            EvalCanCreateProject();
            EvalCanViewContact();

            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_DETAILS_UI);
            
            //Breadcrumbs Info
            ViewBag.CurrentTeam = entity;

            foreach (var item in entity.TeamMembers)
            {
                if (item.CreatedByContact == null && item.CreatedBy != null)
                {
                    item.CreatedByContact = db.GNContacts.Find(item.CreatedBy);
                }
            }

            return base.DetailsOnLoad(entity);
        }

        public override GNTeam PopulateSelectLists(GNTeam team = null)
        {
            Guid teamGNContactId = Guid.Empty;
            Guid teamOrganizationId = Guid.Empty;

            if (team != null)
            {
                teamGNContactId = team.GNContactId;
                teamOrganizationId = team.OrganizationId;
            }
            else
            {
                teamGNContactId = UserContact.Id;
                teamOrganizationId = UserContact.GNOrganizationId;
            }

            List<GNContact> teamManagers = new List<GNContact>();
            var orgContacts = this.entityService.db.GNContacts
                .Include(c=>c.GNContactRoles)
                .Where(c => c.GNOrganizationId == teamOrganizationId);

            foreach (var orgContact in orgContacts)
	        {
                //get role data
                foreach (var contactRole in orgContact.GNContactRoles)
                {
                    contactRole.AspNetRole = this.identityDB.AspNetRoles.Find(contactRole.AspNetRoleId);
                }

                //lookup if is team member
                bool isTeamMember = true;
                if(team != null && this.db.GNTeams.Count(t=>t.Id == team.Id) != 0)
                {
                    isTeamMember = (this.entityService.db.GNTeamMembers
                        .Count(tm => tm.GNContactId == orgContact.Id && tm.GNTeamId == team.Id) == 0) ? false : true;
                }

                //filter out non-team members, non-managers
                if (isTeamMember && (orgContact.IsInRole("ORG_MANAGER") || orgContact.IsInRole("TEAM_MANAGER")))
                {
                    teamManagers.Add(orgContact);
                }
	        }
            ViewBag.GNContactId = new SelectList(teamManagers, "Id", "FullNameWithEmail", teamGNContactId);

            EvalCanCreateProject();

            return team;
        }

        public override ActionResult CreateOnSuccess(GNTeam entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            if (!string.IsNullOrEmpty(Request["inProjectCreate"]) && bool.Parse(Request["inProjectCreate"]))
            {
                return RedirectToAction("Create","Projects", new { teamId = entity.Id });
            }
            else
            {
                return base.CreateOnSuccess(entity);
            }
        }

        public override async Task<ActionResult> Delete(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);

            ViewResult view = (ViewResult)await base.Delete(id);
            
            view.ViewName = "Details";
            view.ViewBag.IsDelete = true;

            return view;
        }
    }
}
