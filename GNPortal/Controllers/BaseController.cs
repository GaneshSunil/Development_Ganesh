using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using System.Threading.Tasks;
using System.Net;
using GenomeNext.App;
using System.Data.Entity;
using GenomeNext.Utility;
using GenomeNext.Data;
using GenomeNext.Data.Metadata.Audit;
using GenomeNext.Cloud.CloudNoSQL;

namespace GenomeNext.Portal.Controllers
{
    public class GNEntityController<T> : BaseController<T>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public new GNEntityService<T> entityService
        {
            get
            {
                return (GNEntityService<T>)base.entityService;
            }
            set
            {
                base.entityService = value;
            }
        }
    }

    public class IdentityController<T> : BaseController<T>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public new IdentityEntityService<T> entityService
        {
            get
            {
                return (IdentityEntityService<T>)base.entityService;
            }
            set
            {
                base.entityService = value;
            }
        }
    }

    public abstract class BaseController<T> : BaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //
        //PROPERTIES

        public BaseEntityService<T> entityService { get; set; }

        //
        //METHODS
        //

        // GET: Entity<T>
        public virtual async Task<ActionResult> Index()
        {
            EvalCanCreate();

            ViewResult view = View(await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), IndexFilters()));

            //derive box height class
            string boxHeightClass = "-entity-box";
            if (Request["display"] != null && Request["display"] == "list")
            {
                boxHeightClass = "-entity-list";
            }

            if (User.IsInRole("GN_ADMIN"))
            {
                boxHeightClass = "-admin" + boxHeightClass;
            }
            boxHeightClass = ControllerContext.RouteData.Values["controller"].ToString().ToLower() + boxHeightClass;
            view.ViewBag.BoxHeightClass = boxHeightClass;

            return view;
        }

        public virtual void EvalCanCreate()
        {
            if (!UserContact.IsInRole("COLLABORATOR"))
            {
                ViewBag.CanCreate = true;
            }            
        }

        public virtual int IndexStart()
        {
            int start = 0;

            if (!string.IsNullOrEmpty(Request["start"]))
            {
                start = int.Parse(Request["start"]);
            }

            return start;
        }

        public virtual int IndexEnd()
        {
            int end = 10;

            if (!string.IsNullOrEmpty(Request["end"]))
            {
                end = int.Parse(Request["end"]);
            }

            return end;
        }

        public virtual Dictionary<string, object> IndexFilters()
        {
            Dictionary<string, object> filters = new Dictionary<string, object>();

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

            return filters;
        }

        // GET: Entity<T>/Details/5
        public virtual async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            T entity = await FindEntity(id);

            if (entity == null || !GetEntitySecurityProp(entity, "CanView"))
            {

                return RedirectToAction("Unauthorized", "Error");
                //return HttpNotFound();
            }
            
            entity = DetailsOnLoad(entity);

            return View(entity);
        }

        public virtual T DetailsOnLoad(T entity)
        {
            return entity;
        }

        // GET: Entity<T>/Create
        public virtual async Task<ActionResult> Create()
        {
            return View(CreateOnLoad());
        }

        public virtual T CreateOnLoad()
        {
            return PopulateSelectLists();
        }

        // POST: Entity<T>/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(T entity)
        {
            entity = CreateOnSubmit(entity);

            if (ModelState.IsValid)
            {
                try
                {
                    entity = await this.entityService.Insert(UserContact, entity);

                    return CreateOnSuccess(entity);
                }
                catch (Exception ex)
                {
                    this.AddModelStateErrorsFromException(db, ModelState, ex);
                }
            }
            
            entity = CreateOnModelStateInValid(entity);

            return View(entity);
        }

        public virtual T CreateOnSubmit(T entity)
        {
            return entity;
        }

        public virtual ActionResult CreateOnSuccess(T entity)
        {
            return RedirectToAction("Details", new { id = entity.GetType().GetProperty("Id").GetValue(entity) });
        }

        public virtual T CreateOnModelStateInValid(T entity)
        {
            return PopulateSelectLists(entity);
        }

        // GET: Entity<T>/Edit/5
        public virtual async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            T entity = await FindEntity(id);

            if (entity == null || !GetEntitySecurityProp(entity, "CanView") || !GetEntitySecurityProp(entity, "CanEdit"))
            {
                return HttpNotFound();
            }

            entity = EditOnLoad(entity);

            return View(entity);
        }

        public virtual T EditOnLoad(T entity)
        {
            return PopulateSelectLists(entity);
        }

        // POST: Entity<T>/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(T entity)
        {
            entity = EditOnSubmit(entity);

            if (ModelState.IsValid)
            {
                try
                {
                   entity = await this.entityService.Update(UserContact, entity);
                    
                    return EditOnSuccess(entity);
                }
                catch (Exception ex)
                {
                    this.AddModelStateErrorsFromException(db, ModelState, ex);
                }
            }

            entity = EditOnModelStateInValid(entity);

            return View(entity);
        }

        public virtual T EditOnSubmit(T entity)
        {
            return entity;
        }

        public virtual ActionResult EditOnSuccess(T entity)
        {
            return RedirectToAction("Details", new { id = entity.GetType().GetProperty("Id").GetValue(entity) });
        }

        public virtual T EditOnModelStateInValid(T entity)
        {
            return PopulateSelectLists(entity);
        }

        // GET: Entity<T>/Delete/5
        public virtual async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            T entity = await FindEntity(id);
            
            if (entity == null 
                || !GetEntitySecurityProp(entity,"CanView")
                || !GetEntitySecurityProp(entity, "CanDelete"))
            {
                return HttpNotFound();
            }

            entity = DeleteOnLoad(entity);

            return View(entity);
        }


        public virtual T DeleteOnLoad(T entity)
        {
            return entity;
        }

        // POST: Entity<T>/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(string id)
        {
            string parentId = GetParentIdForEntityOnDelete(id);

            T entity = await FindEntity(id);

            if(!GetEntitySecurityProp(entity,"CanView") || !GetEntitySecurityProp(entity, "CanDelete"))
            {
                throw new Exception("Not allowed to delete item.");
            }

            int result = await this.entityService.Delete(UserContact, ConvertEntityId(id));

            if(result != 0)
            {
                return DeleteOnSuccess(parentId);
            }
            else
            {
                throw new Exception("Unable to delete item.");
            }
        }

        public virtual string GetParentIdForEntityOnDelete(string id)
        {
            return null;
        }

        public virtual ActionResult DeleteOnSuccess(string parentId = null)
        {
            return RedirectToAction("Index");
        }

        public virtual T PopulateSelectLists(T entity = default(T))
        {
            return entity;
        }

        protected async Task<T> FindEntity(string id)
        {
            return await this.entityService.Find(UserContact, ConvertEntityId(id));
        }

        protected virtual object ConvertEntityId(string id)
        {
            var idPropType = typeof(T).GetProperty("Id").PropertyType;
            if (idPropType == typeof(Guid))
            {
                return Guid.Parse(id);
            }
            else if (idPropType == typeof(Int32) || idPropType == typeof(Int64))
            {
                return int.Parse(id);
            }
            else
            {
                return id;
            }
        }

        protected bool GetEntitySecurityProp(T entity, string propName)
        {
            bool entityAccessVal = false;

            try
            {
                if (entity.GetType().GetProperties().Select(p=>p.Name).Contains(propName))
                {
                    entityAccessVal = (bool)entity.GetType().GetProperty(propName).GetValue(entity);
                }
                else
	            {
                    entityAccessVal = true;
	            }
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Unable to read propName[" + propName + "] for GetEntitySecurityProp().", e);
            }

            return entityAccessVal;
        }

        protected void AddModelStateErrorsFromException(DbContext db, ModelStateDictionary modelState, Exception ex)
        {
            if (ex.Message.Contains("EntityValidation"))
            {
                foreach (var dbValError in db.GetValidationErrors())
                {
                    foreach (var error in dbValError.ValidationErrors)
                    {
                        if (!string.IsNullOrEmpty(error.PropertyName))
                        {
                            LogUtil.Warn(logger, error.PropertyName + " : " + error.ErrorMessage,ex);
                            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                        else
                        {
                            LogUtil.Warn(logger, error.ErrorMessage, ex);
                            modelState.AddModelError("", error.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                LogUtil.Warn(logger, ex.Message, ex);
                modelState.AddModelError("", BuildUserFriendlyErrorMsg(ex.Message));
            }
        }

        private string BuildUserFriendlyErrorMsg(string errorMsgToDisplay)
        {
            if (!User.IsInRole("GN_ADMIN"))
            {
                if (errorMsgToDisplay.Contains("CHECK constraint"))
                {
                    string entityCtlName = "item";
                    if (ControllerContext.RouteData.Values["Controller"] != null)
                    {
                        entityCtlName = (string)ControllerContext.RouteData.Values["Controller"];
                        if(entityCtlName.EndsWith("s"))
                        {
                            entityCtlName = entityCtlName.Substring(0, entityCtlName.Length - 1);
                        }
                    }

                    string action = "perform requested action for";
                    if (errorMsgToDisplay.Contains("create"))
                    {
                        action = "create";
                    }
                    else if (errorMsgToDisplay.Contains("update"))
                    {
                        action = "update";
                    }

                    string msgFormat = "Unable to {0} this {1}, because it already exists.";
                    errorMsgToDisplay = String.Format(msgFormat, action, entityCtlName);
                }
                else
                {
                    errorMsgToDisplay = "Unable to perform action. It is not allowed or an error has occurred.";
                }
            }

            return errorMsgToDisplay;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entityService = null;
            }
            base.Dispose(disposing);
        }
    }

    public abstract class BaseController : Controller
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IdentityModelContainer identityDB { get; set; }
        public GNEntityModelContainer db { get; set; }
        protected ApplicationUserManager userManager;
        protected GNCloudNoSQLService audit = new GNCloudNoSQLService();
        public static readonly string EVENT_VIEW = "VIEW";
        public static readonly string EVENT_INSERT = "CREATE";
        public static readonly string EVENT_UPDATE = "UPDATE";
        public static readonly string EVENT_DELETE = "DELETE";
        public static readonly string EVENT_ARCHIVE = "ARCHIVE";
        public static readonly string EVENT_ADD = "ADD";
        public static readonly string EVENT_REMOVE = "REMOVE";
        public static readonly string EVENT_REMOVE_FILE_FROM_SAMPLE = "REMOVE_FILE_FROM_SAMPLE";        
        public static readonly string EVENT_UPLOAD_FILE = "UPLOAD";
        public static readonly string EVENT_DOWNLOAD_FILE = "DOWNLOAD";
        public static readonly string EVENT_LOAD_INDEX_UI = "VIEW_INDEX_UI";
        public static readonly string EVENT_LOAD_DETAILS_UI = "VIEW_DETAILS_UI";
        public static readonly string EVENT_START_ANALYSIS = "START_ANALYSIS";
        public static readonly string EVENT_MARK_ANALYSIS_FAILED = "MARK_ANALYSIS_FAILED";
        public static readonly string EVENT_UPLOAD_FILES = "UPLOAD_FILES";
        public static readonly string EVENT_RUN_TERTIARY = "RUN_TERTIARY_ANALYSIS";
        public bool auditResult;

        public GNContact UserContact
        {
            get
            {
                return (GNContact)ViewBag.ContactForUser;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? base.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        //
        //CONSTRUCTORS
        //

        public BaseController()
        {
            identityDB = new IdentityModelContainer();
            db = new GNEntityModelContainer();
        }

        //
        //METHODS
        //

        protected void EvalCanCreateTeam()
        {
            //eval CanCreate
            GNTeam team = new GNTeam
            {
                OrganizationId = UserContact.GNOrganizationId
            };
            team = new TeamService(this.db, this.identityDB).EvalEntitySecurity(UserContact, team);
            ViewBag.CanCreateTeam = team.CanCreate;

            if (UserContact.IsInRole("COLLABORATOR"))
            {
                ViewBag.CanCreateTeam = false;
            }

        }

        protected void EvalCanCreateProject()
        {
            //eval CanCreate
            GNProject project = new GNProject
            {
                Teams = new List<GNTeam>(){
                    new GNTeam
                    {
                        OrganizationId = UserContact.GNOrganizationId
                    }
                }
            };
            project = new ProjectService(this.db, this.identityDB).EvalEntitySecurity(UserContact, project);
            ViewBag.CanCreateProject = project.CanCreate;

            if (UserContact.IsInRole("COLLABORATOR"))
            {
                ViewBag.CanCreateProject = false;
            }
        }

        protected void EvalCanCreateAnalysisRequest()
        {
            //eval CanCreate
            GNAnalysisRequest analysisRequest = new GNAnalysisRequest
            {
                Project = new GNProject
                {
                    Teams = new List<GNTeam>()
                    {
                        new GNTeam
                        {
                            OrganizationId = UserContact.GNOrganizationId
                        }
                    }
                }
            };
            analysisRequest = new AnalysisRequestService(this.db, this.identityDB).EvalEntitySecurity(UserContact, analysisRequest);
            ViewBag.CanCreateAnalysisRequest = analysisRequest.CanCreate;

            if (UserContact.IsInRole("COLLABORATOR"))
            {
                ViewBag.CanCreateAnalysisRequest = false;
            }
        }

        protected void EvalCanCreateSample()
        {
            //eval CanCreate
            GNSample sample = new GNSample
            {
                GNOrganizationId = UserContact.GNOrganizationId
            };
            sample = new SampleService(this.db, this.identityDB).EvalEntitySecurity(UserContact, sample);
            ViewBag.CanCreateSample = sample.CanCreate;

            if (UserContact.IsInRole("COLLABORATOR"))
            {
                ViewBag.CanCreateSample = false;
            }
        }

        protected void EvalCanViewContact()
        {
            GNContact contact = new GNContact();
            contact = new ContactService(this.db, this.identityDB).EvalEntitySecurity(UserContact, contact);
            ViewBag.CanViewContact = contact.CanView;
        }

        public bool EvalEnableSampleIndexImport()
        {
            bool enableSampleIndexImport = false;

            if (ViewBag.EnableSampleIndexImport == null && ViewBag.OrgConfigSettings != null)
            {
                List<GenomeNext.Data.EntityModel.GNSettingsTemplateConfig> orgConfigSettings = ViewBag.OrgConfigSettings;
                string enableSampleIndexImportStrVal = orgConfigSettings
                    .Where(s => 
                        s.GNSettingsTemplateField.ConfigType == "SAMPLE_INDEX_IMPORT" 
                        && s.GNSettingsTemplateField.Id == "ENABLE_IMPORT_UI")
                    .Select(s => s.Value)
                    .FirstOrDefault();
                bool.TryParse(enableSampleIndexImportStrVal, out enableSampleIndexImport);
            }

            return enableSampleIndexImport;
        }

        public bool EvalCanExportAnalysisReport()
        {
            bool canExportAnalysisReport = false;

            if (ViewBag.CanExportAnalysisReport == null && ViewBag.OrgConfigSettings != null)
            {
                List<GenomeNext.Data.EntityModel.GNSettingsTemplateConfig> orgConfigSettings = ViewBag.OrgConfigSettings;
                string canExportAnalysisReportStrVal = orgConfigSettings
                    .Where(s =>
                        s.GNSettingsTemplateField.ConfigType == "ANALYSIS_REPORT_EXPORT"
                        && s.GNSettingsTemplateField.Id == "ENABLE_ANALYSIS_REPORT_EXPORT_UI_BUTTON")
                    .Select(s => s.Value)
                    .FirstOrDefault();
                bool.TryParse(canExportAnalysisReportStrVal, out canExportAnalysisReport);
            }

            return canExportAnalysisReport;
        }

        protected string GetURLScheme()
        {
            string scheme = "https";
            try
            {
                scheme = Request.Url.Scheme;
            }
            catch (Exception ex)
            {
                scheme = "https";
                LogUtil.Warn(logger, "Error getting URL Scheme", ex);
            }

            return scheme;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                identityDB.Dispose();
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
    }

}