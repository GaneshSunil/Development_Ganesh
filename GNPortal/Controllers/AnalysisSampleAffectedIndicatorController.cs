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
    public class AnalysisSampleAffectedIndicatorController : GNEntityController<GNAnalysisSampleAffectedIndicator>
    {
        public AnalysisSampleAffectedIndicatorController()
            : base()
        {
            entityService = new AnalysisSampleAffectedIndicatorService(base.db);
        }
        
        public override ActionResult CreateOnSuccess(GNAnalysisSampleAffectedIndicator affectedIndicator)
        {
            return RedirectToAction("Index", "AnalysisSampleAffectedIndicator");
        }
        public override ActionResult EditOnSuccess(GNAnalysisSampleAffectedIndicator affectedIndicator)
        {
            return RedirectToAction("Index", "AnalysisSampleAffectedIndicator");
        }
    }
}
