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
    public class AWSRegionsController : GNEntityController<AWSRegion>
    {
        public AWSRegionsController()
            : base()
        {
            entityService = new AWSRegionService(base.db);
        }

        protected override object ConvertEntityId(string id)
        {
            return id;
        }

        public override ActionResult CreateOnSuccess(AWSRegion entity)
        {
            return RedirectToAction("Details", new { id = entity.AWSRegionSystemName});
        }

        public override ActionResult EditOnSuccess(AWSRegion entity)
        {
            return RedirectToAction("Details", new { id = entity.AWSRegionSystemName});
        }

    }

}
