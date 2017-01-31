using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.EntityModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Portal.Models;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class UsersController : IdentityController<AspNetUser>
    {
        public OrganizationService organizationService { get; set; }

        public UsersController()
            : base()
        {
            entityService = new AspNetUserService(base.identityDB);
            organizationService = new OrganizationService(base.db);
        }

        public override async Task<ActionResult> Index()
        {
            ((AspNetUserService)base.entityService).userManager = base.UserManager;

            ViewResult viewResult = (ViewResult)await base.Index();

            List<AspNetUser> users = (List<AspNetUser>)viewResult.Model;

            foreach (var user in users)
            {
                user.ContactCount = organizationService.db.GNContacts.Count(c => c.AspNetUserId == user.Id);

                if (user.AspNetUserRoles != null && user.AspNetUserRoles.Count(r => r.AspNetRole.Name == "GN_ADMIN") == 1)
                {
                    user.IsAdmin = true;
                }
            }

            return viewResult;
        }

        public override AspNetUser PopulateSelectLists(AspNetUser user = null)
        {
            if (user != null)
            {
                if (user.AspNetUserRoles != null && user.AspNetUserRoles.Count(r => r.AspNetRole.Name == "GN_ADMIN") == 1)
                {
                    user.IsAdmin = true;
                }

                ViewBag.UserContacts = db.GNContacts.Where(c => c.AspNetUserId == user.Id);
            }

            return base.PopulateSelectLists(user);
        }

        public override AspNetUser DetailsOnLoad(AspNetUser user)
        {
            if (user.AspNetUserRoles != null && user.AspNetUserRoles.Count(r => r.AspNetRole.Name == "GN_ADMIN") == 1)
            {
                user.IsAdmin = true;
            }

            ViewBag.UserContacts = db.GNContacts.Where(c => c.AspNetUserId == user.Id);

            return base.DetailsOnLoad(user);
        }

        public override AspNetUser CreateOnLoad()
        {
            if (!string.IsNullOrEmpty(Request["organizationId"]))
            {
                ViewBag.Organization = organizationService.db.GNOrganizations.Find(Guid.Parse(Request["organizationId"]));
            }

            return base.CreateOnLoad();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(UserViewModel model)
        {
            AspNetUser user = new AspNetUser();
            user.Email = model.Email;
            user.EmailConfirmed = model.EmailConfirmed;
            user.IsAdmin = model.IsAdmin;
            user.Password = model.Password;

            user = CreateOnSubmit(user);

            if (ModelState.IsValid)
            {
                try
                {
                    user = await this.entityService.Insert(UserContact, user);

                    return CreateOnSuccess(user);
                }
                catch (Exception ex)
                {
                    this.AddModelStateErrorsFromException(db, ModelState, ex);
                }
            }

            user = CreateOnModelStateInValid(user);

            return View("Create", model);
        }

        public override AspNetUser CreateOnSubmit(AspNetUser entity)
        {
            ((AspNetUserService)entityService).userManager = UserManager;

            if (entity.IsAdmin)
            {
                string[] selectedRoles = {entityService.db.AspNetRoles.Where(r => r.Name == "GN_ADMIN").FirstOrDefault().Id};
                entity.RoleIds = new List<string>(selectedRoles);
            }

            if (!string.IsNullOrEmpty(Request["organizationId"]))
            {
                entity.DefaultOrganizationId = Request["organizationId"];
            }

            return base.CreateOnSubmit(entity);
        }

        public override ActionResult CreateOnSuccess(AspNetUser entity)
        {
            if (!string.IsNullOrEmpty(entity.DefaultOrganizationId))
            {
                return RedirectToAction("Create", "Contacts", new { aspNetUserId = entity.Id, organizationId = entity.DefaultOrganizationId, teamId = Request["teamId"] });
            }
            else
            {
                return RedirectToAction("Edit", new { id = entity.Id });
            }
        }

        public override AspNetUser EditOnSubmit(AspNetUser entity)
        {
            if (entity.IsAdmin)
            {
                string[] selectedRoles = { entityService.db.AspNetRoles.Where(r => r.Name == "GN_ADMIN").FirstOrDefault().Id };
                entity.RoleIds = new List<string>(selectedRoles);
            }

            return base.EditOnSubmit(entity);
        }

        public override ActionResult EditOnSuccess(AspNetUser entity)
        {
            return RedirectToAction("Details", new { id = entity.Id, organizationId = entity.DefaultOrganizationId });
        }

        public ActionResult MakeDefault()
        {
            var contactId = Request["contactId"];
            var aspNetUserId = Request["aspNetUserId"];

            var contact = db.GNContacts.Find(Guid.Parse(contactId));
            if (contact != null)
            {
                var aspNetUser = identityDB.AspNetUsers.Find(aspNetUserId);

                if (aspNetUser != null)
                {
                    aspNetUser.DefaultOrganizationId = contact.GNOrganizationId.ToString();
                    aspNetUser.Password = ".";
                    aspNetUser.PasswordConfirm = ".";
                    identityDB.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                    identityDB.SaveChanges();
                }
            }

            return RedirectToAction("Edit", "Users", new { id = aspNetUserId, organizationId = Request["organizationId"] });
        }


        public override async Task<ActionResult> Delete(string id)
        {
            ViewResult view = (ViewResult)await base.Delete(id);

            view.ViewName = "Details";
            view.ViewBag.IsDelete = true;

            return view;
        }
    }
}
