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
    public class AWSResourcesController : GNEntityController<AWSResource>
    {
        public AWSResourcesController()
            : base()
        {
            entityService = new AWSResourceService(base.db);
        }

        public override AWSResource PopulateSelectLists(AWSResource entity = null)
        {
            if(entity != null)
            {
                ViewBag.AWSConfigId = new SelectList(db.AWSConfigs, "Id", "AWSAccessKeyId", entity.AWSConfigId);
                ViewBag.AWSResourceTypeId = new SelectList(db.AWSResourceTypes, "Id", "Name", entity.AWSResourceTypeId);
            }
            else
            {
                ViewBag.AWSConfigId = new SelectList(db.AWSConfigs, "Id", "AWSAccessKeyId");
                ViewBag.AWSResourceTypeId = new SelectList(db.AWSResourceTypes, "Id", "Name");
            }

            return base.PopulateSelectLists(entity);
        }
    }
}
