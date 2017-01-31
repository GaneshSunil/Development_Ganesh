using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Controllers;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Web.Mvc;

namespace GenomeNext.Portal.Attributes
{
    public class GetContactUserAttribute : ActionFilterAttribute
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // Summary:
        //     Called by the ASP.NET MVC framework before the action method executes.
        //
        // Parameters:
        //   filterContext:
        //     The filter context.
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetContactForUser(filterContext);
        }

        // Summary:
        //     Called by the ASP.NET MVC framework after the action method executes.
        //
        // Parameters:
        //   filterContext:
        //     The filter context.
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        //
        // Summary:
        //     Called by the ASP.NET MVC framework before the action result executes.
        //
        // Parameters:
        //   filterContext:
        //     The filter context.
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        //
        // Summary:
        //     Called by the ASP.NET MVC framework after the action result executes.
        //
        // Parameters:
        //   filterContext:
        //     The filter context.
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        private void GetContactForUser(ActionExecutingContext filterContext)
        {
            try
            {
                if (filterContext != null && filterContext.Controller != null
                    && filterContext.Controller.GetType().GetProperties().Count(p => p.Name == "identityDB") != 0
                    && filterContext.Controller.GetType().GetProperties().Count(p => p.Name == "db") != 0)
                {
                    IdentityModelContainer identityDB = (IdentityModelContainer)filterContext.Controller.GetType().GetProperty("identityDB").GetValue(filterContext.Controller);
                    GNEntityModelContainer db = (GNEntityModelContainer)filterContext.Controller.GetType().GetProperty("db").GetValue(filterContext.Controller);

                    var requestUser = filterContext.HttpContext.User;
                    GNContact contact = null;

                    if (requestUser != null && requestUser.Identity != null && requestUser.Identity.IsAuthenticated)
                    {
                        var user = identityDB.AspNetUsers.Where(u => u.UserName == requestUser.Identity.Name).FirstOrDefault();

                        if (user != null)
                        {
                            Guid defaultOrganizationIdGuid = Guid.Empty;
                            if (!string.IsNullOrEmpty(user.DefaultOrganizationId))
                            {
                                defaultOrganizationIdGuid = Guid.Parse(user.DefaultOrganizationId);

                                contact = db.GNContacts
                                    .Where(c => (c.AspNetUserId == user.Id && c.GNOrganizationId == defaultOrganizationIdGuid))
                                    .FirstOrDefault();
                            }
                            else
                            {
                                contact = db.GNContacts
                                    .Where(c => (c.AspNetUserId == user.Id))
                                    .FirstOrDefault();

                                if (contact != null)
                                {
                                    user.DefaultOrganizationId = contact.GNOrganizationId.ToString();
                                    identityDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                                    user.Password = ".";
                                    user.PasswordConfirm = ".";
                                    identityDB.SaveChanges();
                                }
                            }

                            if (contact != null && user != null)
                            {
                                contact.User = user;
                            }

                            if (contact != null && contact.GNContactRoles != null)
                            {
                                foreach (var contactRole in contact.GNContactRoles)
                                {
                                    contactRole.AspNetRole = identityDB.AspNetRoles.Find(contactRole.AspNetRoleId);
                                }
                            }
                        }
                    }

                    if (contact != null)
                    {
                        //set contact
                        filterContext.Controller.ViewBag.ContactForUser = contact;

                        //set organizations for user contact
                        filterContext.Controller.ViewBag.OrganizationsForUser =
                            db.GNContacts
                                .Where(c => c.AspNetUserId == contact.AspNetUserId)
                                .Select(c => c.GNOrganization)
                                .Where(o => o.Id != contact.GNOrganizationId)
                                .ToList();

                        //get org settings template for user
                        var orgConfigSettingsTemplate =
                            db.GNSettingsTemplates
                                .Include(t => t.GNSettingsTemplateConfigs)
                                .Where(t => t.GNOrganizations.Any(o => o.Id == contact.GNOrganizationId))
                                .FirstOrDefault();

                        if (orgConfigSettingsTemplate != null
                            && orgConfigSettingsTemplate.GNSettingsTemplateConfigs != null
                            && orgConfigSettingsTemplate.GNSettingsTemplateConfigs.Count != 0)
                        {
                            filterContext.Controller.ViewBag.OrgConfigSettings =
                                orgConfigSettingsTemplate.GNSettingsTemplateConfigs.ToList();
                        }
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.ContactForUser = null;
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to Get Contact User!!", e);
            }
        }
    }
}
