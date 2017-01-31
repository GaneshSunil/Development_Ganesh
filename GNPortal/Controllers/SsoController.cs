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
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
   
    public class SsoController : GNEntityController<GNLog>
    {
        public SsoController()
            : base()
        {
            //entityService = new LogEntityService(base.db);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
