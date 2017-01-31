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
    [AuthorizeRedirect(Roles="GN_ADMIN")]
    public class AWSConfigsController : GNEntityController<AWSConfig>
    {
        public AWSConfigsController()
            : base()
        {
            entityService = new AWSConfigService(base.db);
        }

        public override AWSConfig PopulateSelectLists(AWSConfig entity = null)
        {
            if(entity != null)
            {
                ViewBag.AWSRegionSystemName = new SelectList(db.AWSRegions, "AWSRegionSystemName", "AWSRegionSystemName", entity.AWSRegionSystemName);
            }
            else
            {
                ViewBag.AWSRegionSystemName = new SelectList(db.AWSRegions, "AWSRegionSystemName", "AWSRegionSystemName");
            }

            return base.PopulateSelectLists(entity);
        }
    }
}
