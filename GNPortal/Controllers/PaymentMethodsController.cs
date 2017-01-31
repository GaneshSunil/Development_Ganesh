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
    public class PaymentMethodsController : GNEntityController<GNPaymentMethod>
    {
        public PaymentMethodsController()
            : base()
        {
            entityService = new PaymentMethodService(base.db);
        }

        public override GNPaymentMethod PopulateSelectLists(GNPaymentMethod paymentMethod = null)
        {
            paymentMethod = base.PopulateSelectLists(paymentMethod);

            var paymentMethodTypes = db.GNPaymentMethodTypes;

            if (paymentMethod != null)
            {
                ViewBag.GNPaymentMethodTypeId = new SelectList(paymentMethodTypes, "Id", "Name", paymentMethod.GNPaymentMethodTypeId);
            }
            else
            {
                ViewBag.GNPaymentMethodTypeId = new SelectList(paymentMethodTypes, "Id", "Name");
            }

            if(paymentMethod == null)
            {
                paymentMethod = new GNPaymentMethod();
            }

            if (!string.IsNullOrEmpty(Request["accountId"]))
            {
                paymentMethod.Account = entityService.db.GNAccounts.Find(Guid.Parse((Request["accountId"])));
                if (paymentMethod.Account != null)
                {
                    paymentMethod.GNAccountId = paymentMethod.Account.Id;
                }
            }

            return paymentMethod;
        }

        public override GNPaymentMethod CreateOnSubmit(GNPaymentMethod entity)
        {
            entity.PCITokenId = "X";
            entity.LastFourDigits = "X";
            entity.ExpirationDate = DateTime.MaxValue;

            return base.CreateOnSubmit(entity);
        }

        public override GNPaymentMethod EditOnSubmit(GNPaymentMethod entity)
        {
            if (string.IsNullOrEmpty(entity.PCITokenId))
            {
                entity.PCITokenId = "X";
            }

            if (string.IsNullOrEmpty(entity.LastFourDigits))
            {
                entity.LastFourDigits = "X";
            }
            
            return base.EditOnSubmit(entity);
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNAccount acct = null;

            if (!string.IsNullOrEmpty(id))
            {
                acct = entityService.db.GNPaymentMethods.Find(Guid.Parse(id)).Account;
            }

            if (acct != null)
            {
                id = acct.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Edit", "OrgAccounts", new { id = Guid.Parse(id) });
        }
    }
}
