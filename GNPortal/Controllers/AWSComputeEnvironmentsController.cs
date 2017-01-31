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
    public class AWSComputeEnvironmentsController : GNEntityController<AWSComputeEnvironment>
    {
        public AWSComputeEnvironmentsController()
            : base()
        {
            entityService = new AWSComputeEnvironmentService(base.db);
        }

        protected override object ConvertEntityId(string id)
        {
            return id;
        }

        public override AWSComputeEnvironment PopulateSelectLists(AWSComputeEnvironment entity = null)
        {
            if(entity != null)
            {
                ViewBag.AWSConfigId = new SelectList(db.AWSConfigs, "Id", "AWSAccessKeyId", entity.AWSConfigId);
            }
            else
            {
                ViewBag.AWSConfigId = new SelectList(db.AWSConfigs, "Id", "AWSAccessKeyId");
            }

            return base.PopulateSelectLists(entity);
        }
    }
}
