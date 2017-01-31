using GenomeNext.App;
using GenomeNext.Data.EntityModel;
using GenomeNext.Portal.Attributes;
using GenomeNext.Portal.Models;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class BulkImportController : BaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BulkImportService bulkImportService { get; set; }

        public BulkImportController() : base()
        {
            this.bulkImportService = new BulkImportService();
        }
        
        // GET: BulkImport
        public async Task<ActionResult> Index()
        {
            return RedirectToAction("MyImports");
        }

        // GET: BulkImport/Import
        public ActionResult Import()
        {
            ViewBag.awsAccessKeyId = UserContact.GNOrganization.AWSConfig.AWSAccessKeyId;
            ViewBag.awsRegionSystemName = UserContact.GNOrganization.AWSConfig.AWSRegionSystemName;
            ViewBag.s3Bucket = this.bulkImportService.FetchAWSS3BucketForBulkImport().ARN;

            return View();
        }

        // GET: BulkImport/MyImports
        public async Task<ActionResult> MyImports()
        {
            var model = await this.bulkImportService.bulkImportStatusService.FindMyImports(UserContact);
            return View(model);
        }

        //TEMP Tfrege
        public void DebugIssue()
        {
            Guid id = Guid.Parse("27144d51-597e-438d-80ee-e85c25583da5");
            GNBulkImportStatus status = db.GNBulkImportStatus.Find(id);
            status.CreatedByContact = UserContact;

            var test = true;
            

         this.bulkImportService.DoImport(status);
            
        }

        // GET: BulkImport/ImportStatus
        public async Task<ActionResult> ImportStatus(Guid id)
        {
            var model = await this.bulkImportService.bulkImportStatusService.Find(id);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ImportSuccess()
        {
            try
            {
                Dictionary<string, object> result = null;

                string bucket = Request["bucket"];
                string key = Request["key"];
                string name = Request["name"];
                string uuid = Request["uuid"];

                if (!string.IsNullOrEmpty(bucket) && !string.IsNullOrEmpty(key))
                {
                    result = new Dictionary<string, object>{
                        {
                            "complete", 
                            await this.bulkImportService.PersistBulkImport(bucket,key,name,uuid,UserContact)
                        }
                    };
                }
                else
                {
                    result = new Dictionary<string, object>{
                        {"error","An error occurred during upload of the file!!"}
                    };
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                // Try and handle malformed POST body
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(BulkImportViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.ImportFile != null && model.ImportFile.ContentLength > 0)
                {
                    //get file extension
                    string fileExtension = System.IO.Path.GetExtension(model.ImportFile.FileName);

                    if (fileExtension != null 
                        && new List<string>() { ".xls", ".xlsx", ".xml" }
                        .Contains(fileExtension.ToLower()))
                    {
                        fileExtension = fileExtension.ToLower();

                        //save file to server local disk
                        string fileLocation = SaveFileToServerLocalDisk(model);

                        GNBulkImportStatus bulkImportStatus = 
                            await this.bulkImportService.PerformBulkImport(fileLocation, fileExtension, UserContact);

                        return RedirectToAction("ImportStatus", new { id = bulkImportStatus.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unsupported file extension!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "File is empty!");
                }
            }

            return View("Import");
        }

        private string SaveFileToServerLocalDisk(BulkImportViewModel model)
        {
            string fileLocation = null;

            try 
            {
                string fileDir = Server.MapPath("~/App_Data/BulkImport/" + Guid.NewGuid() + "/");
                fileLocation = fileDir + model.ImportFile.FileName;

                //delete existing file
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }

                //create missing directory
                if (!System.IO.Directory.Exists(fileDir))
                {
                    System.IO.Directory.CreateDirectory(fileDir);
                }

                //save file to location
                model.ImportFile.SaveAs(fileLocation);
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error saving file to server's local disk!", e);
            }

            return fileLocation;
        }
        */
    }
}