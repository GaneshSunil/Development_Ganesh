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
using GenomeNext.Cloud.Storage;
using System.Configuration;
using System.Data.SqlClient;
using GenomeNext.App;
using GenomeNext.Utility;
using GenomeNext.Billing;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    //[AuthorizeRedirect]
    public class CloudFilesController : GNEntityController<GNCloudFile>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ENTITY = "CLOUD_FILE";

        public TransactionService transactionService { get; set; }
        public AccountService accountService { get; set; }

        public CloudFilesController()
            : base()
        {
            //instantiate services
            entityService = new CloudFileService(base.db, base.identityDB);
            transactionService = new TransactionService(base.db);
            accountService = new AccountService(this.db);
        }

        private void InitCloudServices()
        {
            ((CloudFileService)entityService).InitCloudServices(UserContact.GNOrganization.AWSConfigId);
        }

        // GET: CloudFiles
        [AuthorizeRedirect(Roles = ("GN_ADMIN"))]
        public override async Task<ActionResult> Index()
        {
            List<GNCloudFile> cloudFiles = null;

            if (User.IsInRole("GN_ADMIN"))
            {
                cloudFiles = await entityService.FindAll(UserContact, IndexStart(), IndexEnd(),
                    new Dictionary<string, object> { { "CloudFileCategory.Name", "FASTQ File" } });
            }
            else
            {
                cloudFiles = await entityService.FindAll(UserContact, IndexStart(), IndexEnd(),
                    new Dictionary<string, object> { { "CloudFileCategory.Name", "FASTQ File" } });
            }

            PopulateSelectLists();

            return View(cloudFiles);
        }

        // GET: CloudFiles/Download/5
        public async Task<ActionResult> Download(Guid? id)
        {
            auditResult = audit.LogEvent(UserContact, (Guid)id, this.ENTITY, this.Request.UserHostAddress, EVENT_DOWNLOAD_FILE);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GNCloudFile cloudFile = await db.GNCloudFiles.FindAsync(id);

            if (cloudFile == null)
            {
                return HttpNotFound();
            }
            
            InitCloudServices();

            string contentDisposition = "attachment; filename=" + 
                ((CloudFileService)entityService).GetCloudFileName(cloudFile);

            int cloudFileSignURLTimeOutMins = 15;
            int.TryParse(ConfigurationManager.AppSettings["CloudFileSignURLTimeOutMins"], 
                out cloudFileSignURLTimeOutMins);

            Uri signedURL = ((CloudFileService)entityService)
                .cloudStorageService
                .GetSignedUrl(new Uri(cloudFile.FileURL), TimeSpan.FromMinutes(cloudFileSignURLTimeOutMins), "GET", contentDisposition);

            //record transaction
            if (cloudFile != null)
            {
                int s3BucketMatchCount = UserContact.GNOrganization.AWSConfig.AWSResources.Count(r => r.ARN == cloudFile.Volume);

                if(s3BucketMatchCount != 0)
                {
                    string txnTypeKey = "STORAGE_S3_DOWNLOAD";
                    string description = cloudFile.Description;
                    double valueUsed = ((double)cloudFile.FileSize / (double)(1024 * 1024 * 1024));
                    string valueUnits = "GB";

                    GNTransaction txn =
                        await this.transactionService.CreateTransaction(
                            UserContact, txnTypeKey, description, valueUsed, valueUnits);

                    if (txn != null)
                    {
                        bool success = await this.transactionService.ApplyTransactionTotalToBestPaymentMethod(
                            UserContact, txn, UserContact.GNOrganization.Account.BillingMode);
                    }
                }
            }
            else
            {
                LogUtil.Error(logger, "Unable to record transaction for File Download due to a NULL Cloud File / Entity object.");
            }

            return Redirect(signedURL.AbsoluteUri);
        }

        // GET: CloudFiles/Download/5
        public async Task<ActionResult> DownloadQcFile(Guid? id)
        {
            auditResult = audit.LogEvent(UserContact, (Guid)id, this.ENTITY, this.Request.UserHostAddress, EVENT_DOWNLOAD_FILE);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GNCloudFile cloudFile = await db.GNCloudFiles.FindAsync(id);

            if (cloudFile == null)
            {
                return HttpNotFound();
            }

            InitCloudServices();

            string contentDisposition = "attachment; filename=" +
                ((CloudFileService)entityService).GetQcCloudFileName(cloudFile);

            //s3://dev-gn-s3-01/fb49d146-df03-4e2f-b573-6c4262f9282f/VCFs/summary.pdf
            string qcReportUrl = cloudFile.QcStatsReportLocation.Replace("s3://dev-gn-s3-01", "https://dev-gn-s3-01.s3.amazonaws.com").Trim();

            int cloudFileSignURLTimeOutMins = 15;
            int.TryParse(ConfigurationManager.AppSettings["CloudFileSignURLTimeOutMins"],
                out cloudFileSignURLTimeOutMins);

            Uri signedURL = ((CloudFileService)entityService)
                .cloudStorageService
                .GetSignedUrl(new Uri(qcReportUrl), TimeSpan.FromMinutes(cloudFileSignURLTimeOutMins), "GET", contentDisposition);

            //record transaction
            if (cloudFile != null)
            {
                int s3BucketMatchCount = UserContact.GNOrganization.AWSConfig.AWSResources.Count(r => r.ARN == cloudFile.Volume);

                if (s3BucketMatchCount != 0)
                {
                    string txnTypeKey = "STORAGE_S3_DOWNLOAD";
                    string description = cloudFile.Description;
                    double valueUsed = ((double)cloudFile.FileSize / (double)(1024 * 1024 * 1024));
                    string valueUnits = "GB";

                    GNTransaction txn =
                        await this.transactionService.CreateTransaction(
                            UserContact, txnTypeKey, description, valueUsed, valueUnits);

                    if (txn != null)
                    {
                        bool success = await this.transactionService.ApplyTransactionTotalToBestPaymentMethod(
                            UserContact, txn, UserContact.GNOrganization.Account.BillingMode);
                    }
                }
            }
            else
            {
                LogUtil.Error(logger, "Unable to record transaction for File Download due to a NULL Cloud File / Entity object.");
            }

            return Redirect(signedURL.AbsoluteUri);
        }

        public ActionResult CreateNotAllowed()
        {
            return View();
        }

        public override GNCloudFile CreateOnLoad()
        {
            GNCloudFile cloudFile = base.CreateOnLoad();

            if(cloudFile == null)
            {
                cloudFile = new GNCloudFile();
            }

            GNCloudFileCategory cloudFileCat = this.db.GNCloudFileCategories.Where(c => c.Name == "FASTQ File").FirstOrDefault();

            if(cloudFileCat != null)
            {
                cloudFile.GNCloudFileCategoryId = cloudFileCat.Id;
                cloudFile.CloudFileCategory = cloudFileCat;
            }

            auditResult = audit.LogEvent(UserContact, cloudFile.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPLOAD_FILE);

            return cloudFile;
        }

        public override async Task<ActionResult> Create()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);
            
            //init storage transaction logging
            CloudFileService cloudFileService = (CloudFileService)this.entityService;
            await cloudFileService.InitStorageTransactionLogging(UserContact);

            //double authAmount = 30.0;
            //bool isAuthAllowedForAccount = await accountService.IsAuthAllowedForAccount(UserContact, authAmount);
            Guid guidSampleId = Guid.Parse(Request["sampleId"]);
            ViewBag.Sample = db.GNSamples.Where(a => a.Id.Equals(guidSampleId)).FirstOrDefault();

            //if (isAuthAllowedForAccount)
            //{
                return await base.Create();
            //}
            //else
            //{
            //    return RedirectToAction("NotAllowed","Error");
            //}
        }

        // POST: CloudFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(GNCloudFile gNCloudFile)
        {
            string sampleId = Request["sampleId"];
            string analysisRequestId = Request["analysisRequestId"];

            if (!string.IsNullOrEmpty(sampleId))
            {
                return RedirectToAction("Edit", "Samples", new { id = sampleId, analysisRequestId = analysisRequestId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateViaJSON(GNCloudFile cloudFile)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                InitCloudServices();

                cloudFile.SampleId = Request["sampleId"];
                cloudFile = await entityService.Insert(UserContact, cloudFile);
                result["success"] = true;
                result["cloudFile"] = cloudFile;
            }
            catch (Exception ex)
            {
                result["success"] = false;

                string error = ex.Message;
                foreach (var vError in db.GetValidationErrors())
                {
                    foreach (var vErrorObj in vError.ValidationErrors)
                    {
                        error += "\n" + vErrorObj.PropertyName + " : " + vErrorObj.ErrorMessage;
                    }
                }

                result["error"] = error;

                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return Json(result);
        }

        public async Task<JsonResult> GetCloudFilesForSample(Guid sampleId)
        {
            var sample = await this.db.GNSamples
                .Where(s => s.Id == sampleId)
                .FirstOrDefaultAsync();

            List<GNCloudFile> cloudFilesForSample = 
                sample == null 
                ? new List<GNCloudFile>() 
                : sample.CloudFiles.ToList();

            List<Dictionary<string, string>> cloudFilesForSampleAsJSON = new List<Dictionary<string, string>>();

            foreach (var cloudFile in cloudFilesForSample)
            {
                var cloudFileAsJSON = new Dictionary<string,string>();
                cloudFileAsJSON.Add("id",cloudFile.Id.ToString());
                cloudFileAsJSON.Add("name",cloudFile.Description);
                cloudFileAsJSON.Add("status", "UPLOADED");
                cloudFilesForSampleAsJSON.Add(cloudFileAsJSON);
            }

            return Json(cloudFilesForSampleAsJSON, JsonRequestBehavior.AllowGet);
        }

        // POST: CloudFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE+ " " +id);

            InitCloudServices();

            string sampleId = Request["sampleId"];
            string analysisRequestId = Request["analysisRequestId"];

            int result = await ((CloudFileService)entityService).Delete(UserContact, sampleId, ConvertEntityId(id));

            if (id != null && !string.IsNullOrEmpty(sampleId))
            {
                return RedirectToAction("Edit", "Samples", new {id = sampleId, analysisRequestId = analysisRequestId });
            }
            else
            {
                return RedirectToAction("Index");            
            }
        }

        public async Task<ActionResult> RemoveFromSample(Guid? id)
        {
            InitCloudServices();

            string sampleId = Request["sampleId"];
            string analysisRequestId = Request["analysisRequestId"];

            auditResult = audit.LogEvent(UserContact, Guid.Parse(sampleId), this.ENTITY, this.Request.UserHostAddress, EVENT_REMOVE_FILE_FROM_SAMPLE + " " + id);

            int result = await ((CloudFileService)entityService).Delete(UserContact, sampleId, id);

            if (result != 0)
            {
                return RedirectToAction("Details", "Samples", new { id = sampleId, analysisRequestId = analysisRequestId });
            }

            PopulateSelectLists();

            return View();
        }

        public override GNCloudFile PopulateSelectLists(GNCloudFile entity = null)
        {
            InitCloudServices();

            string sampleId = Request["sampleId"];
            string analysisRequestId = Request["analysisRequestId"];

            if (!string.IsNullOrEmpty(sampleId))
            {
                ViewBag.SampleId = sampleId;
                ViewBag.Sample = db.GNSamples.Find(Guid.Parse(sampleId));
            }

            if (!string.IsNullOrEmpty(analysisRequestId))
            {
                ViewBag.AnalysisRequestId = analysisRequestId;
                ViewBag.AnalysisRequest = db.GNAnalysisRequests.Find(Guid.Parse(analysisRequestId));
            }

            //ViewBag.GNCloudFileCategoryId = new SelectList(db.GNCloudFileCategories, "Id", "Name");

            ViewBag.awsAccessKeyId = ((CloudFileService)entityService).cloudStorageService.AWSConfigEntity.AWSAccessKeyId;
            ViewBag.awsRegionSystemName = ((CloudFileService)entityService).cloudStorageService.AWSConfigEntity.AWSRegionSystemName;
            ViewBag.s3Bucket = ((CloudFileService)entityService).cloudStorageService.FetchAWSS3Bucket().ARN;

            return base.PopulateSelectLists(entity);
        }
    }
}
