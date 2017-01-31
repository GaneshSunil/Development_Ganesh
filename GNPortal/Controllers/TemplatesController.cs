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
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Models;
using GenomeNext.Portal.ControllerExtensions;
using System.Data.SqlClient;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    public class TemplatesController : GNEntityController<GNTemplate>
    {
        private readonly string ENTITY = "TEST_TEMPLATE";

        public TemplatesController()
            : base()
        {
            entityService = new TemplateService(base.db);
        }
        
        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Create(GNTemplate entity)
        {
            return await base.Create(entity);
        }

        public async Task<ActionResult> AddGeneToTemplate()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "ADD_GENE_TO_TEMPLATE " + Request["GeneCode"].Trim().ToUpper());
            int resultCode = 1;
            int TemplateId = Int32.Parse(Request["Id"]);
            string GeneCode = Request["GeneCode"].Trim().ToUpper();
            
            if (GeneCode == "")
            {
                resultCode = -3;
            }
            else
            {
                GNGene gene = db.GNGenes.Where(a => a.GeneCode.Equals(GeneCode)).FirstOrDefault();
                
                if (gene != null)
                {
                    GNTemplateGene tempGene = db.GNTemplateGenes.Where(a => a.GNTemplateId == TemplateId && a.GeneCode.Equals(GeneCode)).FirstOrDefault();
                    if (tempGene != null)
                    {
                        resultCode = -1;
                        //association template-gene already exists
                    }
                    else
                    {
                        //insert
                        int result = await ((TemplateService)entityService).AddRelationship(TemplateId, gene, UserContact);
                    }
                }
                else
                {
                    resultCode = -2;
                }
            }            

            //int result = await ((SampleService)entityService).RemoveRelationship(id, Guid.Parse(leftSampleId), Guid.Parse(rightSampleId));
            return RedirectToAction("Details", "Templates", new { id = TemplateId, resultCode = resultCode, geneCode = GeneCode });            
        }

        public async Task<ActionResult> RemoveGeneFromTemplate()
        {
            int resultCode = 2;
            //     auditResult = audit.LogEvent(UserContact, Guid.Parse(Request["leftSampleId"]), this.ENTITY, this.Request.UserHostAddress, "REMOVE_PEDIGREE");
            int AssociationId = Int32.Parse(Request["relId"]);
            GNTemplateGene tempGene = db.GNTemplateGenes.Where(a => a.Id == AssociationId).FirstOrDefault();

            if(tempGene != null)
            {
                auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "REMOVE_GENE_FROM_TEMPLATE " + tempGene.GNTemplateId + " : " + tempGene.GeneCode);
                int TemplateId = tempGene.GNTemplateId;
                int result = await ((TemplateService)entityService).RemoveGeneFromTemplate(AssociationId);
                return RedirectToAction("Details", "Templates", new { id = TemplateId , resultCode = resultCode, geneCode = tempGene.GeneCode});
            }
            
            //int result = await ((SampleService)entityService).RemoveRelationship(id, Guid.Parse(leftSampleId), Guid.Parse(rightSampleId));
            return RedirectToAction("Index", "Templates");
        }

        public override GNTemplate PopulateSelectLists(GNTemplate project = null)
        {
            IQueryable<GNOrganization> organizations = null;

            if (UserContact.IsInRole("GN_ADMIN"))
            {
                ViewBag.TeamsSize = 1;
                organizations = db.GNOrganizations;
            }
            else
            {
                organizations = db.GNOrganizations.Where(t => t.Id == UserContact.GNOrganizationId);
            }
            organizations = organizations.OrderBy(a => a.Name);
            ViewBag.GNOrganizationId = new SelectList(organizations, "Id", "Name", UserContact.GNOrganizationId);

            return base.PopulateSelectLists(project);
        }

        public async Task<ActionResult> AddToAnalysisRequest(int id)
        {
            string analysisRequestId = Request["analysisRequestId"];

            if (id != null && !string.IsNullOrEmpty(analysisRequestId))
            {
                int result = await ((TemplateService)entityService).AddTemplateToAnalysisRequest(id, analysisRequestId, UserContact);
                return RedirectToAction("List", "Templates", new { analysisRequestId = analysisRequestId });
            }

            ViewBag.AnalysisRequestId = analysisRequestId;

            return RedirectToAction("List", "Templates", new { analysisRequestId = analysisRequestId });
        }

        public async Task<ActionResult> RemoveFromAnalysisRequest(int id)
        {
            string analysisRequestId = Request["analysisRequestId"];
            bool returnToAnalysis = (string.IsNullOrEmpty(Request["returnToAnalysis"]) ? false : bool.Parse(Request["returnToAnalysis"]));

            if (id != null && !string.IsNullOrEmpty(analysisRequestId))
            {
                int result = await ((TemplateService)entityService).RemoveTemplateFromAnalysisRequest(id, analysisRequestId);
                return RedirectToAction("List", "Templates", new { analysisRequestId = analysisRequestId });
            }

            ViewBag.AnalysisRequestId = analysisRequestId;
            
            return RedirectToAction("List", "Templates", new { analysisRequestId = analysisRequestId });
        }

        public async Task<ActionResult> List()
        {
            PopulateSelectLists();
            EvalCanCreate();

            Dictionary<string, object> filters = new Dictionary<string, object>();

            List<GNTemplate> GNTemplates = new List<GNTemplate>();

            if (!string.IsNullOrEmpty(Request["analysisRequestId"]))
            {
                Guid guidAnalysisRequest = Guid.Parse(Request["analysisRequestId"]);
                GNAnalysisRequest AnalysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(guidAnalysisRequest)).FirstOrDefault();
                ViewBag.AnalysisRequest = AnalysisRequest;
                ViewBag.AnalysisName = AnalysisRequest.Description;
                ViewBag.AnalysisRequestId = guidAnalysisRequest;
            }

            if (!string.IsNullOrEmpty(Request["addSampleRelationship"]))
            {
                int sampleRel = Int32.Parse(Request["GNSampleRelationshipTypeId"]);
                Guid thisSample = Guid.Parse(Request["GNLeftSampleId"]);

                int maxRelAllowed = db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.MaxRelationships).FirstOrDefault();
                int thisSampleRelIdCount = db.GNSampleRelationships.Where(a => a.GNLeftSampleId.Equals(thisSample) && a.GNSampleRelationshipTypeId == sampleRel).Count();

                if (thisSampleRelIdCount < maxRelAllowed)
                {
                    //continue
                    filters.Add("Gender", db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.Gender).FirstOrDefault());
                    filters.Add("GNOrganizationId", Request["GNOrganizationId"]);
                    filters.Add("GNLeftSampleId", Request["GNLeftSampleId"]);
                    GNTemplates = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                }
                else
                {
                    string sampleRelName = db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.Name).FirstOrDefault();

                    //return error and return to the Details page
                    string add_s = "";
                    if (thisSampleRelIdCount > 1)
                    {
                        add_s = "s";
                    }
                    ViewBag.PedigreeErrorMessage = "This Sample already has " + thisSampleRelIdCount + " " + sampleRelName.ToLower() + add_s + ", the maximum number allowed.";
                    GNSample sample = db.GNSamples.Where(a => a.Id.Equals(thisSample)).FirstOrDefault();
                }
            }

            GNTemplates = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);

            return View(GNTemplates);
        }
    }
}
