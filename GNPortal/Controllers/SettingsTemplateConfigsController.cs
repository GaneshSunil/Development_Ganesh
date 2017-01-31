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
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class SettingsTemplateConfigsController : GNEntityController<GNSettingsTemplateConfig>
    {
        private readonly string ENTITY = "SETTINGS_TEMPLATE_CONFIG";

        public SettingsTemplateConfigsController()
            : base()
        {
            entityService = new SettingsTemplateConfigService(base.db);
        }

        public override GNSettingsTemplateConfig PopulateSelectLists(GNSettingsTemplateConfig config = null)
        {
            int tId = int.Parse(Request["templateId"]);
            var SettingsTemplates = db.GNSettingsTemplates.Where(a => a.Id == tId).ToList();

            if (config != null && config.GNSettingsTemplateField != null)
            {
                var SettingsFields = 
                    db.GNSettingsTemplateFields
                    .Where(f => f.Id.Equals(config.GNSettingsTemplateField.Id)).OrderBy(a => a.Name)
                    .OrderBy(f=>f.ConfigType)
                    .ToList();
                ViewBag.GNSettingsTemplateFieldsList = new SelectList(SettingsFields, "Id", "IdWithConfigType", config.GNSettingsTemplateField.Id);
                ViewBag.GNSettingsTemplatesList = new SelectList(SettingsTemplates, "Id", "Description", config.GNSettingsTemplate.Id);
            } 
            else
            {
                var SettingsFields =
                    db.GNSettingsTemplateFields.OrderBy(a => a.Name)
                    .OrderBy(f => f.ConfigType)
                    .ToList();
                ViewBag.GNSettingsTemplateFieldsList = new SelectList(SettingsFields, "Id", "IdWithConfigType");
                ViewBag.GNSettingsTemplatesList = new SelectList(SettingsTemplates, "Id", "Description");
            }

            return base.PopulateSelectLists(config);
        }


        public override GNSettingsTemplateConfig CreateOnSubmit(GNSettingsTemplateConfig conf)
        {
            bool auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT + " " + conf.Id);

            int GNSettingsTemplateId = int.Parse(Request["GNSettingsTemplatesList"]);
            string GNSettingsTemplateField = Request["GNSettingsTemplateFieldsList"].ToString().Trim();
            string Value = Request["Value"].ToString().Trim();

            GNSettingsTemplateConfig config = new GNSettingsTemplateConfig();
            config.GNSettingsTemplate = db.GNSettingsTemplates.Where(a => a.Id == GNSettingsTemplateId).FirstOrDefault();
            config.GNSettingsTemplateField = db.GNSettingsTemplateFields.Where(a => a.Id.Equals(GNSettingsTemplateField)).FirstOrDefault();
            config.Value = Value;

            return base.CreateOnSubmit(config);
        }

        public override GNSettingsTemplateConfig EditOnSubmit(GNSettingsTemplateConfig config)
        {
            bool auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE + " " + config.Id);

            int GNSettingsTemplateId = int.Parse(Request["GNSettingsTemplatesList"]);
            string strGNSettingsTemplateField = Request["GNSettingsTemplateFieldsList"].ToString().Trim();
            string Value = Request["Value"].ToString().Trim();

            GNSettingsTemplate settingsTemplate = db.GNSettingsTemplates.Where(a => a.Id == GNSettingsTemplateId).FirstOrDefault();
            GNSettingsTemplateField GNSettingsTemplateField = db.GNSettingsTemplateFields.Where(a => a.Id.Equals(strGNSettingsTemplateField)).FirstOrDefault();

            GNSettingsTemplateConfig dbConfig = db.GNSettingsTemplateConfigs.Find(config.Id);
            dbConfig.Value = Value;
            dbConfig.GNSettingsTemplateField = GNSettingsTemplateField;
            dbConfig.GNSettingsTemplate = settingsTemplate;

            db.SaveChanges();

            return dbConfig;
        }

        public override ActionResult CreateOnSuccess(GNSettingsTemplateConfig config)
        {
            return RedirectToAction("Details", "SettingsTemplate", new { id = config.GNSettingsTemplate.Id });
        }


        public override ActionResult EditOnSuccess(GNSettingsTemplateConfig config)
        {
            return RedirectToAction("Details", "SettingsTemplate", new { id = config.GNSettingsTemplate.Id });
        }


        public override ActionResult DeleteOnSuccess(string id = null)
        {
            bool auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE + " " + id);

            return RedirectToAction("Details", "SettingsTemplate", new { id = Request["templateId"] });
        }

    }
}


