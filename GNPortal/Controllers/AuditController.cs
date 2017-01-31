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
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using GenomeNext.Portal.Attributes;
using GenomeNext.Cloud.CloudNoSQL;
using GenomeNext.Data.Metadata.Audit;
using CsvHelper;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenomeNext.Portal.Controllers
{    
    public class AuditController : GNEntityController<GNAudit>
    {
        private readonly string ENTITY = "AUDIT_LOGS";

        public AuditController()
            : base()
        {
            entityService = new AuditService(base.db);
        }


        public override async Task<ActionResult> Index()
        {
            if (Request != null) // && ( (Request["searchDateFrom"] != null && Request["searchDateFrom"].Trim() != "") || (Request["searchEntityId"].Trim() != "")) )
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                if (Request["searchDateFrom"] != null && Request["searchDateFrom"].Trim() != "")
                {
                    filters.Add("searchDateFrom", Request["searchDateFrom"].ToUpper());
                }
               /* else
                {
                    filters.Add("searchDateFrom", String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-30)));
                }*/

                if(Request["searchDateTo"] != null && Request["searchDateTo"].Trim() != "")
                {
                    filters.Add("searchDateTo", Request["searchDateTo"].ToUpper());
                }
               /* else
                {
                    filters.Add("searchDateTo", String.Format("{0:MM/dd/yyyy}", DateTime.Now));
                }*/

                if (!User.IsInRole("GN_ADMIN"))
                {
                    filters.Add("searchOrganization", UserContact.GNOrganizationId.ToString());
                }
                else if (Request["searchOrganization"] != null && Request["searchOrganization"].Trim() != "")
                {
                    filters.Add("searchOrganization", Request["searchOrganization"]);
                }

                if (Request["searchActor"] != null && Request["searchActor"].Trim() != "")
                {
                    filters.Add("searchActor", Request["searchActor"].ToLower());
                }

                if (Request["searchAction"] != null && Request["searchAction"].Trim() != "")
                {
                    filters.Add("searchAction", Request["searchAction"].ToUpper());
                }

                if (Request["searchEntityType"] != null && Request["searchEntityType"].Trim() != "")
                {
                    filters.Add("searchEntityType", Request["searchEntityType"].ToUpper());
                }

                if (Request["searchEntityId"] != null && Request["searchEntityId"].Trim() != "")
                {
                    filters.Add("searchEntityId", Request["searchEntityId"].ToLower());
                }

                if (Request["searchIP"] != null && Request["searchIP"].Trim() != "")
                {
                    filters.Add("searchIP", Request["searchIP"].ToUpper());
                }

                if (Request["maxRecords"] != null && Request["maxRecords"].Trim() != "")
                {
                    filters.Add("maxRecords", Request["maxRecords"]);
                }
                else
                {
                    filters.Add("maxRecords", "100000");
                }

                List<GNAudit> GNAudits1 = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                List<GNAudit> GNAudits = GNAudits1.OrderByDescending(o => o.TimestampNumeric).ToList();

                if (Request["outputFormat"] != null && Request["outputFormat"].Equals("xls"))
                {

                    auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, EVENT_DOWNLOAD_FILE);
            
                    string reportFileName = "Audits_" + DateTime.Now + "_" + UserContact.Email + ".xls";
                    GridView gv = new GridView();
                    
                    gv.DataSource = GNAudits;

                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + reportFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                    //Response.ContentType = "application/force-download";
                }
                else
                {
                    return View(GNAudits);
                }
           
            }
            

            ViewResult viewResult = (ViewResult)await base.Index();
           
           // List<GNAudit> records = new GNCloudNoSQLService().GetEvents();
            //var records = entityService.FindAll();
          //  ViewBag.records = records;

            return viewResult;
        }
    }
}


/*
 AmazonDynamoDBClient client = new AmazonDynamoDBClient();

 */