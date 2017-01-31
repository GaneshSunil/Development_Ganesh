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

namespace GenomeNext.Portal.Controllers
{
    public class SampleRelationshipsController : GNEntityController<GNSampleRelationship>
    {
        public SampleRelationshipsController()
            : base()
        {
            entityService = new SampleRelationshipService(base.db);
        }
        
        public override GNSampleRelationship PopulateSelectLists(GNSampleRelationship sampleRelationship = null)
        {
            sampleRelationship = base.PopulateSelectLists(sampleRelationship);


            /*
             Missing for the relationship type dropdown:
             *  - Filter out the values that the sample has maxed out (i.e. exclude FATHER if the sample already has one related)
             */
            int selectedValue = 0;

            if (sampleRelationship != null)
            {
                selectedValue = sampleRelationship.GNSampleRelationshipTypeId;
            }

            if (!string.IsNullOrEmpty(Request["GNSampleRelationshipTypeId"]))
            {
                selectedValue = int.Parse(Request["GNSampleRelationshipTypeId"]);
            }

            ViewBag.GNSampleRelationshipType = db.GNSampleRelationshipTypes.Find(selectedValue);

            
            ViewBag.GNSampleRelationshipTypeId = new SelectList(db.GNSampleRelationshipTypes, "Id", "Name", selectedValue);
            
   

            if (!string.IsNullOrEmpty(Request["GNLeftSampleId"]))
            {
                sampleRelationship = new GNSampleRelationship();
                sampleRelationship.GNLeftSample = this.getSampleInfo(Request["GNLeftSampleId"]);
                sampleRelationship.GNLeftSampleId = sampleRelationship.GNLeftSample.Id;
            }

            return sampleRelationship;
        }

        public GNSample getSampleInfo(string id)
        {
           return entityService.db.GNSamples.Find(Guid.Parse(id));
        }

        public override async Task<ActionResult> Index()
        {
            ViewBag.GNSample = this.getSampleInfo(Request["GNLeftSampleId"]);
            ViewBag.GNSampleRelationshipTypeId = new SelectList(db.GNSampleRelationshipTypes, "Id", "Name");  
            return await base.Index();
        }

        public override GNSampleRelationship CreateOnLoad()
        {

            GNSampleRelationship sampleRelationship = new GNSampleRelationship();

            /*
             I need:
             * All info from Left sample
             * 
             */

            if (!string.IsNullOrEmpty(Request["GNLeftSampleId"]))
            {
                sampleRelationship.GNLeftSample = this.getSampleInfo(Request["GNLeftSampleId"]);
                sampleRelationship.GNLeftSampleId = sampleRelationship.GNLeftSample.Id;
                
            }
            
            if(!string.IsNullOrEmpty(Request["GNRightSampleId"]))
            {
                sampleRelationship.GNRightSample = this.getSampleInfo(Request["GNRightSampleId"]);
                sampleRelationship.GNRightSampleId = sampleRelationship.GNRightSample.Id;
            }

            if (!string.IsNullOrEmpty(Request["GNSampleRelationshipTypeId"]))
            {
                GNSampleRelationshipType relationshipType = entityService.db.GNSampleRelationshipTypes.Find(int.Parse(Request["GNSampleRelationshipTypeId"]));
                ViewBag.GNSampleRelationshipType = relationshipType;
                ViewBag.GNLeftSampleRelationshipTypeCount = sampleRelationship.GNLeftSample.GNSampleLeftRelationships.Where(sam => sam.GNSampleRelationshipType.Id == relationshipType.Id).Count();
            }

            return sampleRelationship;
        }

        public override Dictionary<string, object> IndexFilters()
        {
            Dictionary<string, object> sampleRelationshipFilters = new Dictionary<string,object>();
            sampleRelationshipFilters.Add("LeftSampleId", Request["GNLeftSampleId"]);
            return sampleRelationshipFilters;
            //        return new Dictionary<string, object> { {"LeftSampleId", Request["GNLeftSampleId"]} };
        }

/*        public ActionResult Pedigree()
        {
            return View()
        }*/

        public override ActionResult CreateOnSuccess(GNSampleRelationship sampleRelationship)
        {
            return RedirectToAction("Details", "Samples", new { id = sampleRelationship.GNLeftSampleId });
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNSample sample = null;

            if (!string.IsNullOrEmpty(id))
            {
                sample = entityService.db.GNSampleRelationships.Find(int.Parse(id)).GNLeftSample;
            }

            if (sample != null)
            {
                id = sample.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Index", "SampleRelationships", new { GNLeftSampleId = Guid.Parse(id) });
        }
        
        public async Task<ActionResult> ImportPedigree()
        {
            Guid sampleId = Guid.Parse(Request["sampleId"]);
            string analysisRequestId = Request["analysisRequestId"];
            Guid guidAnalysisRequestId = Guid.Parse(analysisRequestId);

            List<GNSampleRelationship> sampleRelationships = db.GNSampleRelationships.Where(t => t.GNLeftSampleId.Equals(sampleId)).ToList();
            
            foreach(GNSampleRelationship sampleRel in sampleRelationships)
            {
                if(db.GNAnalysisRequestGNSamples.Count(t => t.GNAnalysisRequestId == guidAnalysisRequestId && t.GNSampleId == sampleRel.GNRightSample.Id) == 0)
                {
                    SampleService ss = new SampleService(db, identityDB);
                    int result = await ss.AddSampleToAnalysisRequest(sampleRel.GNRightSample.Id, analysisRequestId);
                }
            }
            return RedirectToAction("Edit", "AnalysisRequests", new { id = analysisRequestId });
        }
    }


}
