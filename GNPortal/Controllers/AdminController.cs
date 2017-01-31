using GenomeNext.Portal.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles=("GN_ADMIN"))]
    public class AdminController : BaseController
    {
        private readonly string ENTITY = "ADMIN_UI";

        // GET: Admin
        public ActionResult Index()
        {
           /*
            List<Guid> guids = new List<Guid>();

            for(int i=0; i< 100; i++)
            {
                guids.Add(Guid.NewGuid());
            }

            ViewBag.guisList = guids;
            */



            bool auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOAD_ADMIN_UI");

            return View();
        }
    }
}