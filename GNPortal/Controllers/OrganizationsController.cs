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
using GenomeNext.Portal.ControllerExtensions;
using GenomeNext.App;
using System.Collections.ObjectModel;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class OrganizationsController : GNEntityController<GNOrganization>
    {
        private readonly string ENTITY = "ORGANIZATION";

        public OrganizationsController() : base()
        {
            entityService = new OrganizationService(base.db);
        }

        public override GNOrganization PopulateSelectLists(GNOrganization organization = null)
        {

            //load sample types
            var awsConfigs = entityService.db.AWSConfigs.OrderBy(t => t.AWSRegionSystemName);
            var settingsTemplates = db.GNSettingsTemplates.OrderBy(t => t.Name);
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            //load sample types (with pre-selected) into select list
            if (organization != null)
            {
                ViewBag.AWSConfigId = new SelectList(awsConfigs, "Id", "AWSAccessKeyId", organization.AWSConfigId);
                ViewBag.UTCOffsetDescription = new SelectList(timeZones, "DisplayName", "DisplayName", organization.UTCOffsetDescription.Trim());
                ViewBag.GNSettingsTemplateId = new SelectList(settingsTemplates, "Id", "Name", organization.GNSettingsTemplateId);
            }
            else
            {
                ViewBag.AWSConfigId = new SelectList(awsConfigs, "Id", "AWSAccessKeyId");
                ViewBag.UTCOffsetDescription = new SelectList(timeZones, "DisplayName", "DisplayName");
                ViewBag.GNSettingsTemplateId = new SelectList(settingsTemplates, "Id", "Name");
            }


            return base.PopulateSelectLists(organization);
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public override async Task<ActionResult> Create()
        {
            return await base.Create();
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(GNOrganization entity)
        {
            return await base.Create(entity);
        }

        public override ActionResult CreateOnSuccess(GNOrganization entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            return RedirectToAction("Edit", new { id = entity.Id });
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public override async Task<ActionResult> Edit(string id)
        {
            return await base.Edit(id);
        }

        [AuthorizeRedirect(Roles="GN_ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(GNOrganization entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            return await base.Edit(entity);
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public override async Task<ActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);

            return await base.DeleteConfirmed(id);
        }

        public async Task<ActionResult> MakePrimary(Guid contactId, Guid organizationId)
        {
            var org = await this.entityService.db.GNOrganizations.FindAsync(organizationId);

            if (org != null && contactId != null && contactId != Guid.Empty)
            {
                org.GNContactId = contactId;
                await this.entityService.db.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = organizationId });
        }
        public override async Task<ActionResult> Index()
        {
            auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_INDEX_UI);

            Dictionary<string, object> filters = this.IndexFilters();
            if (!string.IsNullOrEmpty(Request["Name"])) { filters.Add("Name", Request["Name"]); }
            
            if (filters.Count() > 0)
            {
                List<GNOrganization> GNOrganizations = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                return View(GNOrganizations);
            }

            return await base.Index();
        }
    }
}
