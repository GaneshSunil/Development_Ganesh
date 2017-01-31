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

namespace GenomeNext.Portal.Controllers
{
    public class NotificationTopicSubscribersController : GNEntityController<GNNotificationTopicSubscriber>
    {
        public NotificationTopicSubscribersController()
            : base()
        {
            entityService = new NotificationTopicSubscriberService(base.db);
        }
        public virtual async Task<ActionResult> MyNotifications()
        {
            Dictionary<string, object> filters = this.IndexFilters();
            filters.Add("GNContactId", UserContact.Id);
            filters.Add("IsSubscriptionOptional", "Y");
            return View(await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters));
        }

        public async Task<ActionResult> SaveSubscriptions()
        {
            return View(await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), IndexFilters()));
        }
        

        public override async Task<ActionResult> Index()
        {
            PopulateSelectLists();
            
            Dictionary<string, object> filters = this.IndexFilters();
            if (!string.IsNullOrEmpty(Request["GNNotificationTopicId"])) { filters.Add("GNNotificationTopicId", Request["GNNotificationTopicId"]); }
            if (!string.IsNullOrEmpty(Request["Topic"])) { filters.Add("Topic", Request["Topic"]); }
            if (!string.IsNullOrEmpty(Request["Organization"])) { filters.Add("Organization", Request["Organization"]); }
            if (!string.IsNullOrEmpty(Request["Subscriber"])) { filters.Add("Subscriber", Request["Subscriber"]); }
            if (!string.IsNullOrEmpty(Request["Email"])) { filters.Add("Email", Request["Email"]); }
            if (!string.IsNullOrEmpty(Request["IsSubscribed"])) { filters.Add("IsSubscribed", Request["IsSubscribed"]); }
            
            if (filters.Count() > 0)
            {
                List<GNNotificationTopicSubscriber> GNNotificationSubscribers = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), filters);
                return View(GNNotificationSubscribers);
            }

            return await base.Index();
        }

    }
}

