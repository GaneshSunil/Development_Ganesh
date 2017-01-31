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
    public class SettingsTemplateFieldsController : GNEntityController<GNSettingsTemplateField>
    {
        public SettingsTemplateFieldsController()
            : base()
        {
            entityService = new SettingsTemplateFieldService(base.db);
        }

        protected override object ConvertEntityId(string id)
        {
            return id;
        }

        public override GNSettingsTemplateField EditOnLoad(GNSettingsTemplateField field)
        {
            field.Id = field.Id.Trim();
            return base.DetailsOnLoad(field);
        }

        public override GNSettingsTemplateField EditOnSubmit(GNSettingsTemplateField field)
        {
            field.CreatedBy = UserContact.Id;
            field.CreateDateTime = DateTime.Now;    

            return base.EditOnSubmit(field);
        }

    }

}

