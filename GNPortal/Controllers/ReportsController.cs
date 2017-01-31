using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Data.EntityModel;
using System.Data.SqlClient;
using GenomeNext.Cloud.Messaging;
using System.Configuration;
using GenomeNext.Cloud.Messaging.Model;
using GenomeNext.Cloud.Storage;
using GenomeNext.App;
using GenomeNext.Notification;
using GenomeNext.Billing;
using GenomeNext.Portal.Attributes;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using GenomeNext.Portal.Models;

namespace GenomeNext.Portal.Controllers
{
    public class ReportsController : GNEntityController<GNAnalysisRequest>
    {
        private readonly string ENTITY = "REPORTS";

        public SampleService sampleService { get; set; }
        public AccountService accountService { get; set; }

        public ReportsController()
            : base()
        {
            entityService = new AnalysisRequestService(base.db, base.identityDB);
            sampleService = new SampleService(base.db, base.identityDB);
        }

        public async Task<ActionResult> AnnotationAnalysis()
        {
            Guid RequestId = Guid.Parse(Request["analysisRequestId"]);
            GNAnalysisRequest AnalysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(RequestId)).FirstOrDefault();
            ViewBag.AnalysisRequest = AnalysisRequest;
            ViewBag.ProjectName = AnalysisRequest.Project.Name;
            ViewBag.TeamName = AnalysisRequest.Project.Teams.FirstOrDefault().Name;
            ViewBag.ProjectLead = AnalysisRequest.Project.ProjectLead.FullName;
            ViewBag.AnalysisRequestCreateDate = AnalysisRequest.CreateDateTime;

            ViewBag.Templates = string.Join("<br />", AnalysisRequest.GNAnalysisRequestGNTemplates.Select(a => a.GNTemplate.Name).ToList());
                
            ViewBag.ListOfGenes = string.Join(", ", AnalysisRequest.GNAnalysisRequestGNTemplates.FirstOrDefault().GNTemplate.GNTemplateGenes.Select(a => a.GeneCode).ToList());

            IQueryable<tmpAnnotatedTable> records = ((AnalysisRequestService)entityService).getTmpAnnotatedTable();
            ViewBag.Results = records;
            return View("AnnotationAnalysis");
        }
    }
}
