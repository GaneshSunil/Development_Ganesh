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
using GenomeNext.Cloud.CloudNoSQL;
using GenomeNext.Data.Metadata.Audit;
using CsvHelper;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace GenomeNext.Portal.Controllers
{
    public class OwinHomeController : GNEntityController<GNAudit>
    {

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.ClaimsIdentity = System.Threading.Thread.CurrentPrincipal.Identity;
            var claimsIdentity = System.Threading.Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            ViewBag.DisplayName = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.GivenName).Value;
            return View();
        }

        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                var owinContext = this.Request.GetOwinContext();
                var authProperties = new AuthenticationProperties();
                authProperties.RedirectUri = new Uri(this.HttpContext.Request.Url, new UrlHelper(this.ControllerContext.RequestContext).Action("PostLogOff")).AbsoluteUri;
                owinContext.Authentication.SignOut(authProperties);
                return View();
            }
            else
            {
                throw new InvalidOperationException("User is not authenticated");
            }
        }

        [AllowAnonymous]
        public ActionResult PostLogOff()
        {
            return View();
        }
    }
}