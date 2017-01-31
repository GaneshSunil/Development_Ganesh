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
    [AuthorizeRedirect]
    public class AnalysisRequestsController : GNEntityController<GNAnalysisRequest>
    {
        private readonly string ENTITY = "ANALYSIS_REQUEST";

        public SampleService sampleService { get; set; }
        public AccountService accountService { get; set; }

        public AnalysisRequestsController()
            : base()
        {
            entityService = new AnalysisRequestService(base.db, base.identityDB);
            sampleService = new SampleService(base.db, base.identityDB);
            accountService = new AccountService(base.db);
        }

        private void InitCloudServices()
        {
            AnalysisRequestService arService = (AnalysisRequestService)entityService;

            if(arService.analysisRequestPendingCloudMessageService == null || arService.cloudStorageService == null)
            {
                arService.InitCloudServices(UserContact.GNOrganization.AWSConfigId);
            }
        }

        public override async Task<ActionResult> Index()
        {
            ViewResult view = (ViewResult)await base.Index();
            //base.EvalCanCreate();
            


            if (UserContact.IsInRole("GN_ADMIN"))
            {
                ViewBag.Organizations = db.GNOrganizations.SortBy("Name"); //.Where(a => a.OrganizationId.Equals(UserContact.GNOrganizationId));
            }
            else
            {
                ViewBag.Organizations = db.GNOrganizations.Where(a => a.Id.Equals(UserContact.GNOrganizationId)).SortBy("Name"); 
            }
            

            
            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add("OrganizationId", Request["OrganizationId"]);
            filters.Add("projectId", Request["projectId"]);
            filters.Add("teamId", Request["teamId"]);

            List<GNAnalysisRequest> GNAnalysisRequests = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
            foreach (var analysisRequest in (IList<GNAnalysisRequest>)GNAnalysisRequests)
            {
                EvalActionPrivileges(analysisRequest);

            }


            return View(GNAnalysisRequests);
        }

        public override void EvalCanCreate()
        {
            EvalCanCreateAnalysisRequest();
            ViewBag.CanCreate = ViewBag.CanCreateAnalysisRequest;
        }

        public  async Task<ActionResult> debugIssue()
        {
            var test = true;
            string id1 = Request["id"];
            Guid id = Guid.Parse(id1);
            await ((AnalysisRequestService)entityService).StartAnalysis(UserContact, id);  //telma debugging
            return View();
        }

        public override GNAnalysisRequest DetailsOnLoad(GNAnalysisRequest analysisRequest)
        {
            EvalActionPrivileges(analysisRequest);

            GNCloudStorageService st = new GNCloudStorageService();
            
          //  bool res = ((AnalysisRequestService)entityService).tempPersistStatus(analysisRequest);


            auditResult = audit.LogEvent(UserContact, analysisRequest.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_DETAILS_UI);

            //In case during the creation of result files all where left as type=6, review and update to the corredct types.
            try
            {
                if (analysisRequest.AnalysisResult != null && analysisRequest.AnalysisResult.ResultFiles != null)
                {
                    if (analysisRequest.AnalysisResult.ResultFiles.Where(a => a.GNCloudFileCategoryId == 6).Count() > 0)
                    {
                        ((AnalysisRequestService)entityService).UpdateFileExtensions(analysisRequest.AnalysisResult);
                    }
                }         
            }
            catch (Exception e)
            {
            }

            //Breadcrumbs Info
            ViewBag.CurrentAnalysisRequest = analysisRequest;
            GNProject Project = db.GNProjects.Where(a => a.Id.Equals(analysisRequest.GNProjectId)).FirstOrDefault();
            ViewBag.CurrentProject = Project;
            Guid TeamId = Project.Teams.FirstOrDefault().Id;
            GNTeam Team = db.GNTeams.Where(a => a.Id.Equals(TeamId)).FirstOrDefault();
            ViewBag.CurrentTeam = Team;

            ViewBag.DateVCFSentToCartagenia = "NO";
            try
            {
                string stsCartagenia = "CARTAGENIA";
                GNAnalysisStatus statusCartagenia = db.GNAnalysisStatus.Where(a => a.GNAnalysisRequestId == analysisRequest.Id && a.Status == stsCartagenia).FirstOrDefault();
                if(statusCartagenia != null)
                {
                    ViewBag.DateVCFSentToCartagenia = TimeZoneInfo.ConvertTime((DateTime)statusCartagenia.CreateDateTime, analysisRequest.Project.Teams.FirstOrDefault().Organization.OrgTimeZoneInfo).ToString();
                        
                }
            }
            catch (Exception notFound)
            {
                ViewBag.DateVCFSentToCartagenia = "NO";
            }

            
            GNSettingsTemplate Template = db.GNOrganizations.Where(a => a.Id.Equals(Team.OrganizationId)).FirstOrDefault().GNSettingsTemplate;
            ViewBag.TemplateConfigTertiaryFrequency = (Double.Parse(Template.GNSettingsTemplateConfigs.Where(a => a.GNSettingsTemplateField.Id.Equals("TERTIARY_FREQUENCY")).FirstOrDefault().Value) * 100);
            ViewBag.TemplateConfigTertiarySplicingTreshold = Template.GNSettingsTemplateConfigs.Where(a => a.GNSettingsTemplateField.Id.Equals("TERTIARY_SPLICING_THRESHOLD")).FirstOrDefault().Value;
            

            //2016.02.04 if the last status was error due to a corrupted file, attach the name of the file to the short status
            ViewBag.CorruptFilenameFound = "NA";
            if(analysisRequest.CurrentStatusShort.Equals("ERROR"))
            {
                string lastStatusReported = analysisRequest.AnalysisStatus.LastOrDefault().Message;
                if(lastStatusReported.Contains("corrupt file"))
                {
                    string corruptFileName = lastStatusReported.Substring(lastStatusReported.LastIndexOf('/') + 1);
                    ViewBag.CorruptFilenameFound = analysisRequest.CurrentStatusShort + ": corrupt file found " + corruptFileName;
                }
            }


            if (Request["sampleId"] != null)
            {
                Guid sampleId = Guid.Parse(Request["sampleId"]);
                GNSample Sample = db.GNSamples.Where(a => a.Id.Equals(sampleId)).FirstOrDefault();
                ViewBag.CurrentSample = Sample;
            }

            return base.DetailsOnLoad(analysisRequest);
        }

        public override GNAnalysisRequest CreateOnSubmit(GNAnalysisRequest entity)
        {
            InitCloudServices();
            
            entity.AnalysisType = db.GNAnalysisTypes.Find(int.Parse(Request["AnalysisTypeId"]));
           
            //entity.CreatedBy = UserContact;
            entity.AWSRegionSystemName = UserContact.GNOrganization.AWSConfig.AWSRegionSystemName;

            return base.CreateOnSubmit(entity);
        }

        public override ActionResult CreateOnSuccess(GNAnalysisRequest entity)
        {
            if (Request["sampleId"] != null)
            {
                Guid sampleId = Guid.Parse(Request["sampleId"]);
                GNAnalysisRequestGNSample AnalysisSample = new GNAnalysisRequestGNSample();
                AnalysisSample.GNSampleId = sampleId;
                AnalysisSample.AffectedIndicator = "U";
                AnalysisSample.TargetIndicator = "N";
                entity.GNAnalysisRequestGNSamples.Add(AnalysisSample);
                db.SaveChanges();
            }

            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);
            
            return RedirectToAction("Details", new { id = entity.Id, projectId = Request["projectId"], teamId = Request["teamId"] });
        }

        public override GNAnalysisRequest EditOnLoad(GNAnalysisRequest analysisRequest)
        {
            auditResult = audit.LogEvent(UserContact, analysisRequest.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            EvalActionPrivileges(analysisRequest);
            LoadUnImportedSampleRelationships(analysisRequest);

            return base.EditOnLoad(analysisRequest);
        }

        private static void LoadUnImportedSampleRelationships(GNAnalysisRequest analysisRequest)
        {
            foreach (var sample in analysisRequest.GNAnalysisRequestGNSamples)
            {
                foreach (var leftRel in sample.GNSample.GNSampleLeftRelationships)
                {
                    bool alreadyImported = false;
                    foreach (var arSample in analysisRequest.GNAnalysisRequestGNSamples)
                    {
                        if (leftRel.GNRightSampleId == arSample.GNSampleId)
                        {
                            alreadyImported = true;
                            break;
                        }
                    }

                    if (!alreadyImported)
                    {
                        if (sample.UnImportedSampleRelationships == null)
                        {
                            sample.UnImportedSampleRelationships = new List<GNSampleRelationship>();
                        }
                        sample.UnImportedSampleRelationships.Add(leftRel);
                    }
                }
            }
        }

        private void EvalActionPrivileges(GNAnalysisRequest analysisRequest)
        {
            //determine if analysis has valid sample set
            analysisRequest = ((AnalysisRequestService)entityService).IsValidSampleSet(analysisRequest);

            //determine if analysis is editable within this view controller context
            if (analysisRequest.CanEdit 
                && analysisRequest.CurrentProgress == 0
                && (analysisRequest.AnalysisStatus == null || analysisRequest.AnalysisStatus.Count == 0)
                )
            {
                analysisRequest.CanEdit = true;
            }
            else
            {
                analysisRequest.CanEdit = false;
            }

            //determine if analysis "normal" start is allowed
            analysisRequest.CanStartAnalysis = ((AnalysisRequestService)entityService).IsAnalysisStartAllowed(analysisRequest);

            //determine if analysis re-start is allowed
            analysisRequest.CanReStartAnalysis = ((AnalysisRequestService)entityService).IsAnalysisRestartAllowed(analysisRequest);
        }

        public override GNAnalysisRequest EditOnSubmit(GNAnalysisRequest analysisRequest)
        {
            analysisRequest.AnalysisTypeId = Request["AnalysisTypeId"];
            return base.EditOnSubmit(analysisRequest);
        }

        public override ActionResult EditOnSuccess(GNAnalysisRequest entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);
            return RedirectToAction("Details", new { id = entity.Id, projectId = Request["projectId"], teamId = Request["teamId"] });
        }

        [HttpGet]
        public async Task<ActionResult> StartAnalysis(Guid analysisRequestId)
        {
            auditResult = audit.LogEvent(UserContact, analysisRequestId, this.ENTITY, this.Request.UserHostAddress, EVENT_START_ANALYSIS);
            
            try
            {
                //fetch analysis request from db
                var analysisRequest = this.db.GNAnalysisRequests
                    .Include(ar => ar.GNAnalysisRequestGNSamples)
                    .Include(ar => ar.GNAnalysisRequestGNSamples.Select(s => s.GNSample).Select(s => s.CloudFiles))
                    .Include(ar => ar.AnalysisStatus)
                    .Include(ar => ar.AnalysisResult)
                    .Where(ar => ar.Id == analysisRequestId)
                    .FirstOrDefault();

                //determine analysis actions allowed
                EvalActionPrivileges(analysisRequest);

                if (analysisRequest.CanStartAnalysis || analysisRequest.CanReStartAnalysis || UserContact.IsInRole("GN_ADMIN"))
                {
                    await ((AnalysisRequestService)entityService).StartAnalysis(UserContact, analysisRequestId);

                    return RedirectToAction("Details", new { id = analysisRequestId });
                }
                else
                {
                    return RedirectToAction("Details", new { id = analysisRequestId });
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.ToLower().Contains("billing"))
                {
                    return RedirectToAction("NotAllowed", "Error");
                }
                else
                {
                    throw ex;
                }
            }
        }

        public override GNAnalysisRequest CreateOnLoad()
        {
            EvalCanCreateProject();
            return base.CreateOnLoad();
        }

        public override GNAnalysisRequest PopulateSelectLists(GNAnalysisRequest entity = null)
        {
            //Load Projects
            List<GNProject> projects = new ProjectService(this.db, this.identityDB).FindMyProjects(UserContact).ToList();
            ViewBag.ProjectsCount = projects.Count;

            if (entity != null)
            {
                ViewBag.GNProjectId = new SelectList(projects, "Id", "NameWithTeam", entity.GNProjectId);
            }
            else
            {
                ViewBag.GNProjectId = new SelectList(projects, "Id", "NameWithTeam", Request["projectId"]);
            }

            ViewBag.AffectedIndicators = new SelectList(db.GNAnalysisSampleAffectedIndicators, "Code", "Name");
            ViewBag.GNAnalysisAdaptorCode = new SelectList(db.GNAnalysisAdaptors, "Code", "Name");

            if(Request["sampleId"] != null)
            {
                Guid sampleId = Guid.Parse(Request["sampleId"]);

                GNSample sample = db.GNSamples.Find(sampleId);

                int SampleType = sample.SampleType.Id;
                var analysisTypes = db.GNAnalysisTypes.Where(a => a.Id.Equals(SampleType)).OrderBy(t => t.Name);
                //var analysisRequestTypes = db.GNAnalysisRequestTypes.
                ViewBag.AnalysisTypeId = new SelectList(analysisTypes, "Id", "Name");

                string defaultValue = sample.GNSampleQualifierCode;
                if (sample.GNSampleQualifierCode == "TUMOR")
                {
                    defaultValue = "TUMORNORMAL";
                }

                var analysisRequestTypes = db.GNAnalysisRequestTypes.Where(a => a.Code.Equals(defaultValue));
                ViewBag.GNAnalysisRequestTypeCode = new SelectList(analysisRequestTypes, "Code", "Name", defaultValue);
            }
            else
            {                
                //Load Analysis Types
                var analysisTypes = db.GNAnalysisTypes.OrderBy(t => t.Name);
                var analysisRequestTypes = db.GNAnalysisRequestTypes.OrderBy(t => t.Name);
                if (entity != null)
                {
                    if(entity.AnalysisType != null)
                    {
                        entity.AnalysisTypeId = entity.AnalysisType.Id + "";
                    }
                    
                    ViewBag.AnalysisTypeId = new SelectList(analysisTypes, "Id", "Name", entity.AnalysisTypeId);
                    ViewBag.GNAnalysisRequestTypeCode = new SelectList(analysisRequestTypes, "Code", "Name", entity.AnalysisTypeId);
                }
                else
                {
                    ViewBag.AnalysisTypeId = new SelectList(analysisTypes, "Id", "Name", Request["AnalysisTypeId"]);
                    ViewBag.GNAnalysisRequestTypeCode = new SelectList(analysisRequestTypes, "Code", "Name", Request["GNAnalysisRequestTypeCode"]);
                }
            }

            return base.PopulateSelectLists(entity);
        }

        public JsonResult SaveAffectedIndicator(Guid analysisRequestId, Guid sampleId, string affectedIndicator)
        {
            bool saveResult = false;

            if (!string.IsNullOrEmpty(affectedIndicator) && analysisRequestId != Guid.Empty && sampleId != Guid.Empty)
            {
                //get analysis request sample object
                var analysisRequestSample = this.entityService.db.GNAnalysisRequestGNSamples
                    .Where(ars =>
                        (ars.GNAnalysisRequestId == analysisRequestId
                        && ars.GNSampleId == sampleId)
                    )
                    .FirstOrDefault();

                if (analysisRequestSample != null)
                {
                    //set indicator
                    analysisRequestSample.AffectedIndicator = affectedIndicator;

                    //save to db
                    saveResult = (this.entityService.db.SaveChanges() != 0) ? true : false;
                }
            }

            return Json(new { success = saveResult }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTargetIndicator(Guid analysisRequestId, Guid sampleId)
        {
            bool saveResult = false;

            if (analysisRequestId != Guid.Empty && sampleId != Guid.Empty)
            {
                //get analysis request sample objects
                var analysisRequestSamples = this.entityService.db.GNAnalysisRequestGNSamples
                    .Where(ars =>
                        (ars.GNAnalysisRequestId == analysisRequestId)
                    );

                if (analysisRequestSamples != null && analysisRequestSamples.Count() != 0)
                {
                    foreach (var analysisRequestSample in analysisRequestSamples)
                    {
                        if(analysisRequestSample.GNSampleId == sampleId)
                        {
                            //set indicator to Y
                            analysisRequestSample.TargetIndicator = "Y";
                        }
                        else
                        {
                            //set indicator to N
                            analysisRequestSample.TargetIndicator = "N";
                        }
                    }

                    //save to db
                    saveResult = (this.entityService.db.SaveChanges() != 0) ? true : false;
                }
            }

            return Json(new { success = saveResult }, JsonRequestBehavior.AllowGet);
        }


        public override async Task<ActionResult> Delete(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);
            
            ViewResult view = new ViewResult();
            var result = await base.Delete(id);
            string typeName = result.GetType().Name;

            if(typeName == "HttpNotFoundResult")
            {                
                Guid guidId = Guid.Parse(id);
                GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Where(a => a.Id.Equals(guidId)).FirstOrDefault();
                return RedirectToAction("Details", new { id = guidId, errorhttpnotfound = true });
            }
            
            view = (ViewResult)result;
            view.ViewName = "Details";
            view.ViewBag.IsDelete = true;

            return view;
        }

        [AuthorizeRedirect(Roles="GN_ADMIN")]
        public async Task<ActionResult> MarkAsFailedRequest(Guid id)
        {
            auditResult = audit.LogEvent(UserContact, id, this.ENTITY, this.Request.UserHostAddress, EVENT_MARK_ANALYSIS_FAILED);
            
            GNAnalysisRequest analysisRequest = await this.db.GNAnalysisRequests.FindAsync(id);

            if(analysisRequest != null)
            {
                if(analysisRequest.AnalysisResult == null)
                {
                    analysisRequest.AnalysisResult = new GNAnalysisResult
                    {
                        Id = Guid.NewGuid(),
                        AWSRegionSystemName = analysisRequest.AWSRegionSystemName,
                        AnalysisStartDateTime = analysisRequest.CreateDateTime.GetValueOrDefault(),
                        AnalysisEndDateTime = DateTime.Now,
                        CreateDateTime = DateTime.Now,
                        CreatedBy = analysisRequest.CreatedBy,
                        AnalysisRequest = analysisRequest
                    };
                }

                analysisRequest.AnalysisResult.Success = false;

                await this.db.SaveChangesAsync();

                //if analysis result is a failure - process refund
                TransactionService transactionService = new TransactionService(this.db);
                await transactionService.ProcessRefundForAnalysisFailure(analysisRequest, analysisRequest.AnalysisResult);
            }

            return RedirectToAction("Details", new { id = id });
        }

        public async Task<ActionResult> ExportAnalysisReport()
        {
            List<GNAnalysisRequest> analysesForReport = null;

            //get team id
            string teamIdStr = Request["teamId"];
            Guid teamId = Guid.Empty;
            Guid.TryParse(teamIdStr, out teamId);

            //get project id
            string projectIdStr = Request["projectId"];
            Guid projectId = Guid.Empty;
            Guid.TryParse(projectIdStr, out projectId);

            //get sample id
            string sampleIdStr = Request["sampleId"];
            Guid sampleId = Guid.Empty;
            Guid.TryParse(sampleIdStr, out sampleId);

            //init report file name
            string reportFileName = "ANALYSES";

            if (sampleId != Guid.Empty)
            {
                analysesForReport =
                    await this.db.GNAnalysisRequests
                    .Include(r=>r.GNAnalysisRequestGNSamples)
                    .Where(r => r.GNAnalysisRequestGNSamples.Count(s=>s.GNSampleId == sampleId) != 0)
                    .OrderByDescending(r=>r.CreateDateTime)
                    .ToListAsync();

                reportFileName = this.db.GNSamples.Find(sampleId).Name
                    .Replace(" ", "_").ToUpper() + "_" + reportFileName;
            }
            else if (projectId != Guid.Empty)
            {
                analysesForReport =
                    await this.db.GNAnalysisRequests
                    .Where(r => r.GNProjectId == projectId)
                    .OrderByDescending(r => r.CreateDateTime)
                    .ToListAsync();

                reportFileName = this.db.GNProjects.Find(projectId).NameWithTeam
                    .Replace(" ", "_").ToUpper() + "_" + reportFileName;
            }
            else if (teamId != Guid.Empty)
            {
                analysesForReport =
                    await this.db.GNAnalysisRequests
                    .Where(r => r.Project.Teams.FirstOrDefault().Id == teamId)
                    .OrderByDescending(r => r.CreateDateTime)
                    .ToListAsync();

                reportFileName = this.db.GNTeams.Find(teamId).Name
                    .Replace(" ", "_").ToUpper() + "_" + reportFileName;
            }

            if(analysesForReport != null)
            {
                List<AnalysisReportDataRow> analysisReportDataRows = new List<AnalysisReportDataRow>();

                foreach (var analysis in analysesForReport)
                {
                    if(analysis.AnalysisResult != null 
                        && analysis.AnalysisResult.ResultFiles != null
                        && analysis.AnalysisResult.ResultFiles.Count != 0)
                    {
                        foreach (var resultFile in analysis.AnalysisResult.ResultFiles)
                        {
                            analysisReportDataRows.Add(new AnalysisReportDataRow
                            {
                                ANALYSIS_NAME = analysis.Description,
                                STATUS = analysis.CurrentStatusShort,
                                START_DATE_TIME = 
                                    (analysis.AnalysisResult != null && analysis.AnalysisResult.AnalysisStartDateTime != null ? 
                                    TimeZoneInfo.ConvertTime(analysis.AnalysisResult.AnalysisStartDateTime, UserContact.GNOrganization.OrgTimeZoneInfo).ToString() : ""),
                                END_DATE_TIME = 
                                    (analysis.AnalysisResult != null && analysis.AnalysisResult.AnalysisEndDateTime != null ? 
                                    TimeZoneInfo.ConvertTime(analysis.AnalysisResult.AnalysisEndDateTime, UserContact.GNOrganization.OrgTimeZoneInfo).ToString() : ""),
                                TOTAL_TIME = analysis.TotalTimeLapse,
                                RESULT_FILE_BUCKET = resultFile.Volume,
                                RESULT_FILE_KEY = resultFile.FileName, 
                                RESULT_FILE_SIZE = resultFile.FileSize.ToString()
                            });
                        }
                    }
                    else
                    {
                        analysisReportDataRows.Add(new AnalysisReportDataRow
                        {
                            ANALYSIS_NAME = analysis.Description,
                            STATUS = analysis.CurrentStatusShort,
                            START_DATE_TIME =
                                (analysis.AnalysisResult != null && analysis.AnalysisResult.AnalysisStartDateTime != null ?
                                TimeZoneInfo.ConvertTime(analysis.AnalysisResult.AnalysisStartDateTime, UserContact.GNOrganization.OrgTimeZoneInfo).ToString() : ""),
                            END_DATE_TIME =
                                (analysis.AnalysisResult != null && analysis.AnalysisResult.AnalysisEndDateTime != null ?
                                TimeZoneInfo.ConvertTime(analysis.AnalysisResult.AnalysisEndDateTime, UserContact.GNOrganization.OrgTimeZoneInfo).ToString() : ""),
                            TOTAL_TIME = analysis.TotalTimeLapse,
                            RESULT_FILE_BUCKET = "",
                            RESULT_FILE_KEY = "",
                            RESULT_FILE_SIZE = ""
                        });
                    }
                }

                GridView gv = new GridView();
                gv.DataSource = analysisReportDataRows;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + reportFileName + ".xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        public async Task<ActionResult> ReRunTertiaryAnalysis(Guid analysisRequestId)
        {
            auditResult = audit.LogEvent(UserContact, analysisRequestId, this.ENTITY, this.Request.UserHostAddress, EVENT_RUN_TERTIARY);
            
            try
            {
                await ((AnalysisRequestService)entityService).StartTertiaryAnalysis(UserContact, analysisRequestId);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("billing"))
                {
                    return RedirectToAction("NotAllowed", "Error");
                }
                else
                {
                    throw ex;
                }
            }
            
            return RedirectToAction("Details", "AnalysisRequests", new { id = Request["analysisRequestId"] });
        }


        public async Task<ActionResult> AddNewGroup(Guid analysisRequestId)
        {
            var newGroupName = Request["NewGroupName"];
            GNAnalysisRequestGroup GNGroup = new GNAnalysisRequestGroup();
            GNGroup.Id = Guid.NewGuid();
            GNGroup.Name = newGroupName;
            GNGroup.CreateDateTime = DateTime.Now;
            GNGroup.CreatedBy = UserContact.Id;
            GNGroup.GNAnalysisRequestId = analysisRequestId;

            ((AnalysisRequestService)entityService).CreateAnalysisRequestGroup(analysisRequestId, GNGroup);

            return RedirectToAction("Details", "AnalysisRequests", new { id = Request["analysisRequestId"] });
 
        }

        public async Task<ActionResult> RemoveGroup(Guid GroupId)
        {
            GNAnalysisRequestGroup Group = db.GNAnalysisRequestGroups.Where(a => a.Id.Equals(GroupId)).FirstOrDefault();
            
            ((AnalysisRequestService)entityService).DeleteAnalysisRequestGroup(Group);

            return RedirectToAction("Details", "AnalysisRequests", new { id = Request["analysisRequestId"] });

        }

        public async Task<ActionResult> AddGroupsAssociation(Guid analysisRequestId, Guid ControlGroupId, Guid ComparisonGroupId)
        {
            ((AnalysisRequestService)entityService).CreateAnalysisRequestGroupAssociation(analysisRequestId, ControlGroupId, ComparisonGroupId);

            return RedirectToAction("Details", "AnalysisRequests", new { id = analysisRequestId, tab = "ComparisonSetup" });
        }

        public async Task<ActionResult> DeleteGroupsAssociation(Guid analysisRequestId, Guid ControlGroupId, Guid ComparisonGroupId)
        {
            ((AnalysisRequestService)entityService).DeleteAnalysisRequestGroupAssociation(analysisRequestId, ControlGroupId, ComparisonGroupId);

            return RedirectToAction("Details", "AnalysisRequests", new { id = analysisRequestId, tab = "ComparisonSetup" });
        }


        //public async Task<ActionResult> BamViewer(GNAnalysisRequest analysisRequest)
        public async Task BamViewer(string analysisId)
        {
            Guid analysisRequestId;
            if(analysisId != null)
            {
                analysisRequestId = Guid.Parse(analysisId);
            }
            else
            {
                analysisRequestId = Guid.Parse(Request["analysisRequestId"]);
            }
            
            GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Find(analysisRequestId);

            //string analysisRequestId = analysisRequest.Id.ToString();
           // string[] allKeys = Request.Form.AllKeys;
            List<GNSample> samplesSelected = new List<GNSample>();

            List<GNAnalysisRequestGNSample> samplesList = analysisRequest.GNAnalysisRequestGNSamples.ToList();
            foreach (GNAnalysisRequestGNSample sample in samplesList)
            {
                samplesSelected.Add(sample.GNSample);
            }

            await ((AnalysisRequestService)entityService).PrepareLaunchViewer(analysisRequest, samplesSelected, this.UserContact);
            
            ((AnalysisRequestService)entityService).WaitForBamViewerResponse(analysisRequest.Id);

            /**
             * Once the message is there, build the URL and take the user there
             */

            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

            string url = baseUrl + "/jb/bamviewer/" + analysisRequest.Id.ToString();
            ViewBag.ViewerUrl = url;
            Response.AddHeader("Message", "Please wait...");
            Response.AddHeader("REFRESH", "1;URL=" + url); //add a wait of 1 sec
           // Response.Redirect(url);
            //Server.Transfer(url);
            //return View();
        }


        public async Task<ActionResult> RenderJbrowse(Guid analysisRequestId)
        {
            
            return View();
        }


    }
}
