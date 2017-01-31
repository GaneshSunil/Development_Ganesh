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

namespace GenomeNext.Portal.Controllers
{
    public class NotificationSendersController : GNEntityController<GNNotificationSender>
    {
        public NotificationSendersController()
            : base()
        {
            entityService = new NotificationSenderService(base.db);
        }
        
        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Create(GNNotificationSender entity)
        {
            return await base.Create(entity);
        }

        public override ActionResult CreateOnSuccess(GNNotificationSender entity)
        {
            GNCloudEmailService email = new GNCloudEmailService();
            email.VerifySenderEmailAddress(entity.Sender);

            return RedirectToAction("Index");
        }

    }
}

