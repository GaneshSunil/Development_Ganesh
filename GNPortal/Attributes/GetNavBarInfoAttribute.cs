using GenomeNext.Billing;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Controllers;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace GenomeNext.Portal.Attributes
{
    public class GetNavBarInfoAttribute : ActionFilterAttribute
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
            GetNavBarInfo(filterContext);
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

        private void GetNavBarInfo(ActionExecutingContext filterContext)
        {
            if (filterContext != null && filterContext.Controller != null && filterContext.Controller.GetType().GetProperties().Count(p=>p.Name=="db") != 0)
            { 
                GNEntityModelContainer db = (GNEntityModelContainer)filterContext.Controller.GetType().GetProperty("db").GetValue(filterContext.Controller);

                GetCurrentTeam(filterContext, db);
                GetCurrentProject(filterContext, db);
                GetCurrentAnalysisRequest(filterContext, db);
                GetCurrentSample(filterContext, db);
                GetRemainingBudget(filterContext, db);
            }
        }

        /*
        private async Task<double> GetRemainingBudget(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            GNContact userContact = (GNContact)filterContext.Controller.ViewBag.ContactForUser;

            if (userContact != null
                && (userContact.IsInRole("GN_ADMIN") || userContact.IsInRole("ORG_MANAGER")))
            {
                var myAccount = db.GNAccounts
                    .Where(a => a.Organization.Id == userContact.GNOrganizationId)
                    .FirstOrDefault();

                if (myAccount != null)
                {
                    bool result = await (new InvoiceService(db)).fetchCurrentInvoice(userContact);

                    filterContext.Controller.ViewBag.MyRemainingBudget =// myAccount.AvailableCredits;
                        (new AccountService(db)).GetRemainingBudget(userContact, myAccount);
                }
            }
            return filterContext.Controller.ViewBag.MyRemainingBudget;
        }
        */

        private static void GetRemainingBudget(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            GNContact userContact = (GNContact)filterContext.Controller.ViewBag.ContactForUser;

            if (userContact != null
                && (userContact.IsInRole("GN_ADMIN") || userContact.IsInRole("ORG_MANAGER")))
            {
                var myAccount = db.GNAccounts
                    .Where(a => a.Organization.Id == userContact.GNOrganizationId)
                    .FirstOrDefault();

                if (myAccount != null)
                {
                    filterContext.Controller.ViewBag.MyRemainingBudget =// myAccount.AvailableCredits;
                        (new AccountService(db)).GetRemainingBudget(userContact, myAccount);
                }
            }
        }
        
        private static void GetCurrentTeam(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            try
            {
                GNTeam currentTeam = null;
                string teamId = filterContext.HttpContext.Request["teamId"];

                if (!string.IsNullOrEmpty(teamId))
                {
                    currentTeam = db.GNTeams.Find(Guid.Parse(teamId));
                }

                if (currentTeam != null)
                {
                    filterContext.Controller.ViewBag.CurrentTeam = currentTeam;
                }
                else
                {
                    filterContext.Controller.ViewBag.CurrentTeam = null;
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to retrieve Current Team!!", e);
            }
        }
        private static void GetCurrentProject(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            try
            {
                GNProject currentProject = null;
                string projectId = filterContext.HttpContext.Request["projectId"];

                if (!string.IsNullOrEmpty(projectId))
                {
                    currentProject = db.GNProjects.Find(Guid.Parse(projectId));
                }

                if (currentProject != null)
                {
                    filterContext.Controller.ViewBag.CurrentProject = currentProject;
                }
                else
                {
                    filterContext.Controller.ViewBag.CurrentProject = null;
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to retrieve Current Project!!", e);
            }
        }
        private static void GetCurrentAnalysisRequest(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            try
            {
                GNAnalysisRequest currentAnalysisRequest = null;
                string analysisRequestId = filterContext.HttpContext.Request["analysisRequestId"];

                if (!string.IsNullOrEmpty(analysisRequestId))
                {
                    currentAnalysisRequest = db.GNAnalysisRequests.Find(Guid.Parse(analysisRequestId));
                }

                if (currentAnalysisRequest != null)
                {
                    filterContext.Controller.ViewBag.CurrentAnalysisRequest = currentAnalysisRequest;
                }
                else
                {
                    filterContext.Controller.ViewBag.CurrentAnalysisRequest = null;
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to retrieve Current AnalysisRequest!!", e);
            }
        }
        private static void GetCurrentSample(ActionExecutingContext filterContext, GNEntityModelContainer db)
        {
            try
            {
                GNSample currentSample = null;
                string sampleId = filterContext.HttpContext.Request["sampleId"];

                if (string.IsNullOrEmpty(sampleId))
                {
                    sampleId = filterContext.HttpContext.Request["GNLeftSampleId"];
                }

                if (!string.IsNullOrEmpty(sampleId))
                {
                    currentSample = db.GNSamples.Find(Guid.Parse(sampleId));
                }

                if (currentSample != null)
                {
                    filterContext.Controller.ViewBag.CurrentSample = currentSample;
                }
                else
                {
                    filterContext.Controller.ViewBag.CurrentSample = null;
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to retrieve Current Sample!!", e);
            }
        }
    }
}
