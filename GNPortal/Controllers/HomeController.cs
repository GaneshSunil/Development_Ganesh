using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Portal.Attributes;
using GenomeNext.Billing;
using System.Threading.Tasks;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class HomeController : BaseController
    {
        private readonly string ENTITY = "HOME";

        public ActionResult Index()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_DASHBOARD_UI");

            ViewResult view = View();

            IQueryable<GNAnalysisRequest> myAnalysisRequests = null;
            IQueryable<GNSample> mySamples = null;
            IQueryable<GNTeam> myTeams = null;
            IQueryable<GNProject> myProjects = null;
            IQueryable<GNSequencerJob> myOpenSequencerJobs = null;
            
            if (User.IsInRole("GN_ADMIN"))
            {
                myTeams = db.GNTeams;
                myProjects = db.GNProjects;
                myAnalysisRequests = db.GNAnalysisRequests;
                mySamples = db.GNSamples;
                myOpenSequencerJobs = db.GNSequencerJobs.Where(a => !a.Status.Equals("COMPLETED") && !a.Status.Equals("ERROR"));
            }
            else if (UserContact.IsInRole("ORG_MANAGER"))
            {
                myAnalysisRequests = db.GNAnalysisRequests
                    .Where(a => a.Project.Teams.FirstOrDefault().OrganizationId == UserContact.GNOrganizationId);

                mySamples = db.GNSamples.Where(s => s.GNOrganizationId == UserContact.GNOrganizationId);

                myTeams = db.GNTeams.Where(t => t.OrganizationId == UserContact.GNOrganizationId);

                myProjects = db.GNProjects.Where(p => p.Teams.FirstOrDefault().OrganizationId == UserContact.GNOrganizationId);

                myOpenSequencerJobs = db.GNSequencerJobs.Where(a => a.GNOrganizationId == UserContact.GNOrganizationId && !a.Status.Equals("COMPLETED") && !a.Status.Equals("ERROR"));
            }
            else
            {
                myTeams = db.GNTeams
                    .Where(t => (
                        t.OrganizationId == UserContact.GNOrganizationId
                        && t.TeamMembers.Count(tm => tm.GNContactId == UserContact.Id) != 0
                        )
                    );

                myProjects = myTeams.SelectMany(t => t.Projects);

                myAnalysisRequests = myProjects.SelectMany(p=>p.AnalysisRequests);

                mySamples = db.GNSamples
                    .Where(s => s.GNOrganizationId == UserContact.GNOrganizationId);

                myOpenSequencerJobs = db.GNSequencerJobs.Where(a => a.GNOrganizationId == UserContact.GNOrganizationId && !a.Status.Equals("COMPLETED") && !a.Status.Equals("ERROR"));
            }

            view.ViewData["RecentAnalysisRequests"] = 
                myAnalysisRequests
                .OrderByDescending(ar => ar.AnalysisStatus.OrderByDescending(st => st.CreateDateTime).FirstOrDefault().CreateDateTime)
                .Take(5)
                .ToList();

            view.ViewData["RecentSamples"] = 
                mySamples
                .OrderByDescending(s => s.CreateDateTime)
                .Take(5)
                .ToList();

            view.ViewData["MyProjects"] =
                myProjects
                .OrderByDescending(p => p.CreateDateTime)
                .Take(5)
                .ToList();

            view.ViewData["MyTeams"] =
                myTeams
                .OrderByDescending(p => p.CreateDateTime)
                .Take(5)
                .ToList();

            view.ViewData["myOpenSequencerJobs"] = myOpenSequencerJobs.Count();

            EvalCanCreateAnalysisRequest();
            EvalCanCreateSample();
            EvalCanCreateProject();
            EvalCanCreateTeam();

           this.GetRemainingBudget(UserContact).ConfigureAwait(false);

            return view;
        }


        private async Task<bool> GetRemainingBudget(GNContact UserContact)
        {
            bool result = await (new InvoiceService(db)).fetchCurrentInvoice(UserContact);
            return result;
        }

    }
}