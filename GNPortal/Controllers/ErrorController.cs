using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }

        //NotFound - 403
        public ActionResult Unauthorized()
        {
            Response.StatusCode = 200;// 403; 
            return View();
        }

        //NotFound - 404
        public ActionResult NotFound()
        {
            Response.StatusCode = 200;// 404;
            return View();
        }

        public ActionResult NotAllowed()
        {
            return View();
        }
    }
}