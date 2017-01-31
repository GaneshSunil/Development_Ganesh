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
using GenomeNext.Billing;

namespace GenomeNext.Portal.Controllers
{
    public class OrgAccountProductSubscriptionsController : GNEntityController<GNAccountProductSubscription>
    {
        public OrgAccountProductSubscriptionsController()
            : base()
        {
            entityService = new AccountProductSubscriptionService(base.db);
        }

        public override GNAccountProductSubscription PopulateSelectLists(GNAccountProductSubscription accountProductSubscription = null)
        {
            accountProductSubscription = base.PopulateSelectLists(accountProductSubscription);

            var subFreqIntervals = from GNAccountProductSubscription.SubscriptionFrequencyInterval s
                               in Enum.GetValues(typeof(GNAccountProductSubscription.SubscriptionFrequencyInterval))
                                   select new { Id = (int)s, Name = s.ToString() };

            if (accountProductSubscription != null)
            {
                ViewBag.GNProductId = new SelectList(db.GNProducts, "Id", "Name", accountProductSubscription.GNProductId);
                ViewBag.SubscribeFrequency = new SelectList(subFreqIntervals, "Id", "Name", accountProductSubscription.SubscribeFrequency);
            }
            else
            {
                ViewBag.GNProductId = new SelectList(db.GNProducts, "Id", "Name");
                ViewBag.SubscribeFrequency = new SelectList(subFreqIntervals, "Id", "Name");
            }

            if (accountProductSubscription == null)
            {
                accountProductSubscription = new GNAccountProductSubscription();
                accountProductSubscription.StartDate = DateTime.Now;
                accountProductSubscription.EndDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(Request["accountId"]))
            {
                accountProductSubscription.Account = entityService.db.GNAccounts.Find(Guid.Parse((Request["accountId"])));
                if (accountProductSubscription.Account != null)
                {
                    accountProductSubscription.GNAccountId = accountProductSubscription.Account.Id;
                }
            }

            return accountProductSubscription;
        }

        public override ActionResult CreateOnSuccess(GNAccountProductSubscription accountProductSubscription)
        {
            return RedirectToAction("Edit", "OrgAccounts", new { id = accountProductSubscription.GNAccountId });
        }

        public override ActionResult EditOnSuccess(GNAccountProductSubscription accountProductSubscription)
        {
            return RedirectToAction("Edit", "OrgAccounts", new { id = accountProductSubscription.GNAccountId });
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNAccount account = null;

            if (!string.IsNullOrEmpty(id))
            {
                account = entityService.db.GNAccounts.Find(Guid.Parse(id));
            }

            if (account != null)
            {
                id = account.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Edit", "OrgAccounts", new { id = Guid.Parse(id) });
        }
    }
}
