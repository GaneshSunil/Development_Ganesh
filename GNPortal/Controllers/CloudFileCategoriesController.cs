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
    [AuthorizeRedirect]
    public class CloudFileCategoriesController : GNEntityController<GNCloudFileCategory>
    {
        public CloudFileCategoriesController()
            : base()
        {
            entityService = new CloudFileCategoryService(base.db);
        }

        [HttpGet]
        public async Task<JsonResult> DetailsViaJSON(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            GNCloudFileCategory gNCloudFileCategory = await entityService.Find(id);
            
            if (gNCloudFileCategory == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new Dictionary<string, object> { { "success", false } }, JsonRequestBehavior.AllowGet);
            }

            return Json(gNCloudFileCategory, JsonRequestBehavior.AllowGet);
        }
    }
}
