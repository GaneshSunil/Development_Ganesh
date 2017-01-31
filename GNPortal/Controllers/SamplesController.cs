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
using System.Data.SqlClient;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;
using System.Data.Entity.Infrastructure;
using GenomeNext.Utility;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class SamplesController : GNEntityController<GNSample>
    {
        public CloudFileService cloudFileService { get; set; }
        private readonly string ENTITY = "SAMPLE";

        public SamplesController()
            : base()
        {
            entityService = new SampleService(base.db,base.identityDB);
            cloudFileService = new CloudFileService(base.db, base.identityDB);
        }

        public override async Task<ActionResult> Index()
        {
            auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_INDEX_UI);
            
            PopulateSelectLists();

            if (!string.IsNullOrEmpty(Request["addSampleRelationship"]))
            {
                EvalCanCreate();


                Dictionary<string, object> filters = new Dictionary<string, object>();
                filters.Add("Gender", Request["Gender"]);
                filters.Add("GNSampleTypeId", Request["GNSampleTypeId"]);
                filters.Add("GNOrganizationId", Request["GNOrganizationId"]);
                filters.Add("GNLeftSampleId", Request["GNLeftSampleId"]);
                filters.Add("CreatedBy", Request["CreatedBy"]);

                List<GNSample> GNSamples = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                
                return View(GNSamples);
            }

            return await base.Index();
        }
        
        public override void EvalCanCreate()
        {
            EvalCanCreateSample();
            ViewBag.CanCreate = ViewBag.CanCreateSample;
        }

        public async Task<ActionResult> CreatePedigree()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(Request["GNLeftSampleId"]), this.ENTITY, this.Request.UserHostAddress, "CREATE_PEDIGREE");
            int result = await ((SampleService)entityService).AddRelationship(Request["GNLeftSampleId"], Request["GNRightSampleId"], Request["GNSampleRelationshipTypeId"], UserContact);
            return RedirectToAction("Details", "Samples", new { id = Request["GNLeftSampleId"] });
        }


        public async Task<ActionResult> RemovePedigree(string relId, string leftSampleId, string rightSampleId)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(Request["leftSampleId"]), this.ENTITY, this.Request.UserHostAddress, "REMOVE_PEDIGREE");
            int id = Int32.Parse(relId);
            Guid sampleId = Guid.Parse(Request["leftSampleId"]);

            int result = await ((SampleService)entityService).RemoveRelationship(id, Guid.Parse(leftSampleId), Guid.Parse(rightSampleId));
            return RedirectToAction("Details", "Samples", new { id = leftSampleId });            
        }

        public GNSample getSampleInfo(string id)
        {
           return entityService.db.GNSamples.Find(Guid.Parse(id));
        }
        
        public async Task<ActionResult> List()
        {
            PopulateSelectLists();
            EvalCanCreate();




            Dictionary<string, object> filters = new Dictionary<string, object>();

            if (Request["GNSampleTypeId"] != null)
            {
                filters.Add("GNSampleTypeId", Request["GNSampleTypeId"]);
            }

            if (Request["GNSampleQualifierCode"] != null)
            {
                filters.Add("GNSampleQualifierCode", Request["GNSampleQualifierCode"]);
            }
            /*
            if (Request["GroupId"] != null)
            {
                filters.Add("GroupId", Request["GroupId"]);
            }
            */
            if (Request["IsReady"] != null)
            {
                filters.Add("IsReady", Request["IsReady"]);
            }

            if (!string.IsNullOrEmpty(Request["filters"]))
            {
                string reqFilters = Request["filters"];

                string[] reqFilterArray = reqFilters.Split('|');

                if (reqFilterArray != null && reqFilterArray.Count() != 0)
                {
                    foreach (string reqFilter in reqFilterArray)
                    {
                        string[] reqFilterPartsArray = reqFilter.Split(':');

                        if (reqFilterPartsArray != null && reqFilterPartsArray.Count() == 2)
                        {
                            filters.Add(reqFilterPartsArray[0], reqFilterPartsArray[1]);
                        }
                    }
                }
            }
            
            List<GNSample> GNSamples = new List<GNSample>();

            if (!string.IsNullOrEmpty(Request["analysisRequestId"]))
            {
                Guid guidAnalysisRequest = Guid.Parse(Request["analysisRequestId"]);
                ViewBag.AnalysisName = db.GNAnalysisRequests.Where(a => a.Id.Equals(guidAnalysisRequest)).FirstOrDefault().Description;
            }

            if (!string.IsNullOrEmpty(Request["addSampleRelationship"]))
            {
                int sampleRel = Int32.Parse(Request["GNSampleRelationshipTypeId"]);
                Guid thisSample = Guid.Parse(Request["GNLeftSampleId"]);

                int maxRelAllowed = db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.MaxRelationships).FirstOrDefault();
                int thisSampleRelIdCount = db.GNSampleRelationships.Where(a => a.GNLeftSampleId.Equals(thisSample) && a.GNSampleRelationshipTypeId == sampleRel).Count();

                if(thisSampleRelIdCount < maxRelAllowed)
                {
                    //continue
                    filters.Add("Gender", db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.Gender).FirstOrDefault());
                    filters.Add("GNOrganizationId", Request["GNOrganizationId"]);
                    filters.Add("GNLeftSampleId", Request["GNLeftSampleId"]);
                    GNSamples = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                }
                else 
                {
                    string sampleRelName = db.GNSampleRelationshipTypes.Where(a => a.Id == sampleRel).Select(b => b.Name).FirstOrDefault();

                    //return error and return to the Details page
                    string add_s = "";
                    if(thisSampleRelIdCount > 1)
                    {
                        add_s = "s";
                    }
                    ViewBag.PedigreeErrorMessage = "This Sample already has " + thisSampleRelIdCount + " " + sampleRelName.ToLower() + add_s + ", the maximum number allowed.";
                    GNSample sample = db.GNSamples.Where(a => a.Id.Equals(thisSample)).FirstOrDefault();
                }                
            }
            else
            {
                string sampleType = Request["GNSampleTypeId"];
                string qualifier = Request["GNSampleQualifierCode"];
                if(Request["filters"] != null)
                {

                    string filters1 = Request["filters"];
                    string[] filterResults = filters1.Split(':');
                }

                //string sampleName = 
                GNSamples = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
            }
            
            return View(GNSamples);
        }

        public override GNSample DetailsOnLoad(GNSample sample)
        {
            auditResult = audit.LogEvent(UserContact, sample.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_DETAILS_UI);

            sample.IsValidPairEnded = ((SampleService)entityService).IsValidPairEnded(sample);
            sample.IsValidSingleEnded = ((SampleService)entityService).IsValidSingleEnded(sample);

            List<GNSampleRelationshipType> RelationshipTypes = db.GNSampleRelationshipTypes.OrderBy(a => a.MaxRelationships).ThenBy(a => a.Name).ToList();
            ViewBag.GNSampleRelationshipTypes = new SelectList(RelationshipTypes, "Id", "Name");

            if (Request["analysisRequestId"] != null)
            {
                Guid AnalysisRequestId = Guid.Parse(Request["analysisRequestId"]);
                GNAnalysisRequest AnalysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(AnalysisRequestId)).FirstOrDefault();
                ViewBag.CurrentAnalysisRequest = AnalysisRequest;
                GNProject Project = db.GNProjects.Where(a => a.Id.Equals(AnalysisRequest.GNProjectId)).FirstOrDefault();
                ViewBag.CurrentProject = Project;
                Guid TeamId = Project.Teams.FirstOrDefault().Id;
                GNTeam Team = db.GNTeams.Where(a => a.Id.Equals(TeamId)).FirstOrDefault();
                ViewBag.CurrentTeam = Team;
            }

            return base.DetailsOnLoad(sample);
        }

        public override GNSample PopulateSelectLists(GNSample sample = null)
        {
            if(sample == null)
            {
                sample = new GNSample();
                sample.IsReady = true;
            }

            //load analysis requests
            string analysisRequestId = Request["analysisRequestId"];
            if (!string.IsNullOrEmpty(analysisRequestId))
            {
                ViewBag.AnalysisRequestId = analysisRequestId;
                ViewBag.AnalysisRequest = entityService.db.GNAnalysisRequests.Find(Guid.Parse(analysisRequestId));
            }

            //load sample types
            var sampleTypes = entityService.db.GNSampleTypes.OrderBy(t => t.Name);
            var replicates = entityService.db.GNReplicates.OrderBy(t => t.Code);
            var sampleQualifiers = entityService.db.GNSampleQualifiers.OrderBy(t => t.Name);
            var sampleQualifierGroups = entityService.db.GNSampleQualifierGroups.OrderBy(t => t.Name);


            string GNSampleQualifierCode = Request["GNSampleQualifierCode"];
            if (!string.IsNullOrEmpty(GNSampleQualifierCode))
            {
                sampleQualifiers = entityService.db.GNSampleQualifiers.Where(a => a.GNSampleQualifierGroupCode.Equals(GNSampleQualifierCode)).OrderBy(t => t.Name);
                sampleQualifierGroups = entityService.db.GNSampleQualifierGroups.Where(a => a.Code.Equals(GNSampleQualifierCode)).OrderBy(t => t.Name);
            }

            //get analysis type and pre-select sample type
            if (!string.IsNullOrEmpty(Request["analysisTypeId"]))
            {
                GNAnalysisType analysisType = entityService.db.GNAnalysisTypes.Find(int.Parse(Request["analysisTypeId"]));
                GNSampleType sampleType = sampleTypes.Where(st => st.Name == analysisType.Name).FirstOrDefault();
                if(sample != null && sampleType != null)
                {
                    sample.SampleType = sampleType;
                    sample.GNSampleTypeId = sampleType.Id;
                }
            }

            //get analysis type and pre-select sample qualifier
            //tfrege201603 workonthis
            /*
            if (!string.IsNullOrEmpty(Request["analysisTypeCode"]))
            {
                GNAnalysisType analysisType = entityService.db.GNAnalysisTypes.Find(int.Parse(Request["analysisTypeCode"]));
                GNSampleType sampleType = sampleTypes.Where(st => st.Name == analysisType.Name).FirstOrDefault();
                if (sample != null && sampleType != null)
                {
                    sample.SampleType = sampleType;
                    sample.GNSampleTypeId = sampleType.Id;
                }
            }
            */

            //load sample types (with pre-selected) into select list
            if(sample != null)
            {
                ViewBag.GNSampleTypeId = new SelectList(sampleTypes, "Id", "Name", sample.GNSampleTypeId);
                ViewBag.GNSampleQualifierCode = new SelectList(sampleQualifiers, "Code", "Name", sample.GNSampleQualifier);
              // ViewBag.GNReplicateCode = new SelectList(replicates, "Code", "Name", sample.GNReplicateCode);

                if (Request["GNSampleQualifierGroupCode"] != null)
                {
                    sample.GNSampleQualifierCode = Request["GNSampleQualifierGroupCode"];
                }

                if(sample.GNSampleQualifierCode != null)
                {
                    ViewBag.GNSampleQualifierGroupCode = new SelectList(sampleQualifierGroups, "Code", "Name", sample.GNSampleQualifierCode); 
                }
                else
                {
                    ViewBag.GNSampleQualifierGroupCode = new SelectList(sampleQualifierGroups, "Code", "Name"); 
                }

                
                        
                        
                List<GNSampleRelationshipType> RelationshipTypes = db.GNSampleRelationshipTypes.OrderBy(a => a.MaxRelationships).ThenBy(a => a.Name).ToList();
                ViewBag.GNSampleRelationshipTypes = new SelectList(RelationshipTypes, "Id", "Name");
            }
            else
            {
                ViewBag.GNSampleTypeId = new SelectList(sampleTypes, "Id", "Name");
                ViewBag.GNSampleQualifierCode = new SelectList(sampleQualifiers, "Code", "Name");
                ViewBag.GNSampleQualifierGroupCode = new SelectList(sampleQualifierGroups, "Code", "Name");
              //  ViewBag.GNReplicateCode = new SelectList(replicates, "Code", "Name");
            }

            sample.CloudFiles = cloudFileService.EvalEntityListSecurity(UserContact, sample.CloudFiles.ToList());

            sample.IsValidPairEnded = ((SampleService)entityService).IsValidPairEnded(sample);
            
            return base.PopulateSelectLists(sample);
        }

        public override GNSample CreateOnSubmit(GNSample sample)
        {
            auditResult = audit.LogEvent(UserContact, sample.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            sample.CurrentAnalysisRequestId = Request["analysisRequestId"];
            
            var GNSampleQualifierGroupCode = Request["GNSampleQualifierGroupCode"];

            if (GNSampleQualifierGroupCode.Equals("RNA"))
            {
                sample.GNSampleQualifierCode = "RNA";
            }
            
            //validate sample type against analysis type
            if (!string.IsNullOrEmpty(Request["analysisTypeId"]) && !string.IsNullOrEmpty(Request["GNSampleTypeId"]))
            {
                GNAnalysisType analysisType = entityService.db.GNAnalysisTypes.Find(int.Parse(Request["analysisTypeId"]));
                GNSampleType sampleType = entityService.db.GNSampleTypes.Find(int.Parse(Request["GNSampleTypeId"]));

                if(analysisType != null && sampleType != null && analysisType.Name != sampleType.Name)
                {
                    ModelState.AddModelError("GNSampleTypeId", "Sample Type does not match Analysis Type of '" + analysisType.Name + "'");
                }
            }

            return base.CreateOnSubmit(sample);
        }

        public override ActionResult CreateOnSuccess(GNSample sample)
        {

            var groupId = Request["GroupId"];
            sample.CurrentAnalysisRequestId = Request["analysisRequestId"];

            if (!string.IsNullOrEmpty(groupId) && !string.IsNullOrEmpty(sample.CurrentAnalysisRequestId))
            {
                Guid guidGroupId = Guid.Parse(groupId);
                Guid guidAnalysisId = Guid.Parse(sample.CurrentAnalysisRequestId);
                db.GNAnalysisRequestGroups.Where(a => a.GNAnalysisRequestId.Equals(guidAnalysisId) && a.Id.Equals(guidGroupId)).FirstOrDefault().GNSamples.Add(sample);
                db.SaveChanges();
            }

            //redirect to file upload
            if (!string.IsNullOrEmpty(Request["analysisRequestId"]))
            {
                return RedirectToAction("Create", "CloudFiles", new { sampleId = sample.Id, analysisRequestId = Request["analysisRequestId"] });
            }
            else
            {
                return RedirectToAction("Create", "CloudFiles", new { sampleId = sample.Id });
            }
        }

        public override GNSample EditOnSubmit(GNSample sample)
        {

            return base.EditOnSubmit(sample);
        }

        public override ActionResult EditOnSuccess(GNSample sample)
        {
            auditResult = audit.LogEvent(UserContact, sample.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            if (!string.IsNullOrEmpty(Request["analysisRequestId"]))
            {
                return RedirectToAction("Details", "Samples", new { id = sample.Id, analysisRequestId = Request["analysisRequestId"] });
            }
            else
            {
                return RedirectToAction("Details", "Samples", new { id = sample.Id });
            }
        }

        public async Task<ActionResult> AddToAnalysisRequest(Guid? id)
        {
            string analysisRequestId = Request["analysisRequestId"];
            Guid GAnalysisRequestId = Guid.Parse(analysisRequestId);

            if (id != null && !string.IsNullOrEmpty(analysisRequestId))
            {
                int result = await ((SampleService)entityService).AddSampleToAnalysisRequest(id, analysisRequestId);
                Guid sampleId = Guid.Parse(id.ToString());
                
                //tfrege 2016.03.21 Check if also a group Id was passed (in which case it's an RNA sample)
                if (!string.IsNullOrEmpty(Request["GroupId"]))
                {
                    Guid SampleId = Guid.Parse(id.ToString());
                    Guid GroupId = Guid.Parse(Request["GroupId"]);
                    int result2 = await ((SampleService)entityService).AddSampleToAnalysisRequestGroup(SampleId, GroupId);
                }


                auditResult = audit.LogEvent(UserContact, sampleId, this.ENTITY, this.Request.UserHostAddress, "ADD_TO_ANALYSIS " + analysisRequestId.ToString());

                int addedSampleTypeId = db.GNSamples.Where(a => a.Id.Equals(sampleId)).FirstOrDefault().GNSampleTypeId;

                GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(GAnalysisRequestId)).FirstOrDefault();
                if(analysisRequest.GNAnalysisRequestTypeCode=="TUMORNORMAL")
                {
                    return RedirectToAction("Details", "AnalysisRequests", new { id = analysisRequestId });
                }
                else if(!string.IsNullOrEmpty(Request["GroupId"]))
                {
                    return RedirectToAction("List", "Samples", new { analysisRequestId = analysisRequestId, GNSampleTypeId = addedSampleTypeId, GroupId = Request["GroupId"] });
                }
                else
                {
                    return RedirectToAction("List", "Samples", new { analysisRequestId = analysisRequestId, GNSampleTypeId = addedSampleTypeId });
                }
                
            }

            ViewBag.AnalysisRequestId = analysisRequestId;

            return View();
        }

        public async Task<ActionResult> RemoveFromAnalysisRequest(Guid? id)
        {
            string analysisRequestId = Request["analysisRequestId"];
            bool returnToAnalysis = (string.IsNullOrEmpty(Request["returnToAnalysis"]) ? false : bool.Parse(Request["returnToAnalysis"]));

            if (id != null && !string.IsNullOrEmpty(analysisRequestId))
            {
                int result = await ((SampleService)entityService).RemoveSampleFromAnalysisRequest(id, analysisRequestId);

                //tfrege 2016.03.21 Check if also a group Id was passed (in which case it's an RNA sample)
                if (!string.IsNullOrEmpty(Request["GroupId"]))
                {
                    int result2 = await ((SampleService)entityService).RemoveSampleFromAnalysisRequestGroup(id, Guid.Parse(Request["GroupId"]));
                }

                if(returnToAnalysis)
                {
                    Guid GAnalysisRequestId = Guid.Parse(analysisRequestId);
                    GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(GAnalysisRequestId)).FirstOrDefault();
                    if (analysisRequest.GNAnalysisRequestTypeCode == "DNA")
                    {
                        return RedirectToAction("Edit", "AnalysisRequests", new { id = analysisRequestId }); 
                    }
                    else
                    {
                        return RedirectToAction("Details", "AnalysisRequests", new { id = analysisRequestId });
                    }                    
                }
                else
                {
                    Guid sampleId = Guid.Parse(id.ToString());

                    auditResult = audit.LogEvent(UserContact, sampleId, this.ENTITY, this.Request.UserHostAddress, "REMOVE_FROM_ANALYSIS " + analysisRequestId.ToString());

                    int addedSampleTypeId = db.GNSamples.Where(a => a.Id.Equals(sampleId)).FirstOrDefault().GNSampleTypeId;
                    //return RedirectToAction("List", "Samples", new { analysisRequestId = analysisRequestId, GNSampleTypeId = addedSampleTypeId });

                    if (!string.IsNullOrEmpty(Request["GroupId"]))
                    {
                        return RedirectToAction("List", "Samples", new { analysisRequestId = analysisRequestId, GNSampleTypeId = addedSampleTypeId, GroupId = Request["GroupId"] });
                    }
                    else
                    {
                        return RedirectToAction("List", "Samples", new { analysisRequestId = analysisRequestId, GNSampleTypeId = addedSampleTypeId });
                    }
                }
            }

            ViewBag.AnalysisRequestId = analysisRequestId;

            return View();
        }

        public async Task<ActionResult> DeleteAllFiles(string id)
        {
            await DoDeleteAllFiles(id);

            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, "DELETE_ALL_FILES");

            return RedirectToAction("Details", "Samples", new { id = id, analysisRequestId = Request["analysisRequestId"] });
        }

        public override async Task<ActionResult> Delete(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);

            ViewResult view = (ViewResult)await base.Delete(id);

            view.ViewName = "Details";
            view.ViewBag.IsDelete = true;

            return view;
        }

        public override async Task<ActionResult> DeleteConfirmed(string id)
        {
            /*
            await DoDeleteAllFiles(id);
            
            var ctx = ((IObjectContextAdapter)this.db).ObjectContext;
            ctx.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins,this.db.GNSamples);
            */
            return await base.DeleteConfirmed(id);
        }

        private async Task DoDeleteAllFiles(string id)
        {
            GNSample sample = await FindEntity(id); //await ((SampleService)entityService).Find(id);

            cloudFileService.InitCloudServices(UserContact.GNOrganization.AWSConfigId);

            var sampleCloudFileIds = sample.CloudFiles.Select(f => f.Id);

            foreach (var fileId in sampleCloudFileIds)
            {
                int result = await cloudFileService.Delete(UserContact, id.ToString(), fileId);
            }
        }

    }
}
