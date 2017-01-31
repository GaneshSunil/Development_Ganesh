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
using GenomeNext.Cloud.Messaging;
using GenomeNext.Notification;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class NotificationLogsController : GNEntityController<GNNotificationLog>
    {
        public NotificationLogsController()
            : base()
        {
            entityService = new NotificationLogService(base.db);
        }

        public async Task<ActionResult> Resend(GNNotificationLog gnLogId)
        {
            var req = Request;

            GNNotificationLog gnNotificationLog = db.GNNotificationLogs.Where(t => t.Id.Equals(gnLogId.Id)).FirstOrDefault();

            GNCloudEmailService email = new GNCloudEmailService();
            email.ReSendEmail(gnNotificationLog);

            await base.Details(gnNotificationLog.Id.ToString());
            return RedirectToAction("Details", "NotificationLogs", new { id = gnNotificationLog.Id });
        }
        public override async Task<ActionResult> Index()
        {
            PopulateSelectLists();

            Dictionary<string, object> filters = this.IndexFilters();
            if (!string.IsNullOrEmpty(Request["GNNotificationTopicId"])) { filters.Add("GNNotificationTopicId", Request["GNNotificationTopicId"]); }
            if (!string.IsNullOrEmpty(Request["Topic"])) { filters.Add("Topic", Request["Topic"]); }
            if (!string.IsNullOrEmpty(Request["Sender"])) { filters.Add("Sender", Request["Sender"]); }
            if (!string.IsNullOrEmpty(Request["Addressee"])) { filters.Add("Addressee", Request["Addressee"]); }
            if (!string.IsNullOrEmpty(Request["Subject"])) { filters.Add("Subject", Request["Subject"]); }
            if (!string.IsNullOrEmpty(Request["NotificationServiceResponse"])) { filters.Add("NotificationServiceResponse", Request["NotificationServiceResponse"]); }
            if (!string.IsNullOrEmpty(Request["Date"])) { filters.Add("Date", Request["Date"]); }
   
            if(filters.Count() > 0)
            {
                List<GNNotificationLog> GNNotificationLogs = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                return View(GNNotificationLogs);
            }

            return await base.Index();
        }
    }
}

