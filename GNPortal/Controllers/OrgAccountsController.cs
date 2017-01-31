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
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class OrgAccountsController : GNEntityController<GNAccount>
    {
        public OrgAccountsController()
            : base()
        {
            entityService = new AccountService(base.db);
        }

        public override GNAccount PopulateSelectLists(GNAccount acct = null)
        {
            acct = base.PopulateSelectLists(acct);

            var orgs = db.GNOrganizations.OrderBy(o=>o.Name);

            //billing mode type select list
            var billingModeTypeSelectList = new SelectList(
                new Dictionary<string, string>(), "key", "value",
                ((acct == null) ? null : acct.BillingMode));
            ((Dictionary<string, string>)billingModeTypeSelectList.Items).Add(
                GNAccount.BillingModeType.INVOICE.GetCode(), GNAccount.BillingModeType.INVOICE.GetName());

            //discount type select list
            var discountTypeSelectList = new SelectList(
                new Dictionary<string, string>(), "key","value",
                ((acct == null) ? null : acct.DefaultDiscountType));
            ((Dictionary<string, string>)discountTypeSelectList.Items).Add(
                GNAccount.DiscountType.FLAT.GetCode(), GNAccount.DiscountType.FLAT.GetName());
            ((Dictionary<string, string>)discountTypeSelectList.Items).Add(
                GNAccount.DiscountType.PERCENT.GetCode(), GNAccount.DiscountType.PERCENT.GetName());

            if (acct != null)
            {
                ViewBag.GNOrganizationId = new SelectList(orgs, "Id", "Name", acct.Organization.Id);
                ViewBag.GNAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description",acct.GNAccountTypeId);
                ViewBag.BillingMode = billingModeTypeSelectList;
                ViewBag.DefaultDiscountType = discountTypeSelectList;
                
                if(acct.AccountOwner != null)
                {
                    acct.AccountOwnerId = acct.AccountOwner.Id;
                }
                if (acct.BillingContact != null)
                {
                    acct.BillingContactId = acct.BillingContact.Id;
                }
                if (acct.MailingContact != null)
                {
                    acct.MailingContactId = acct.MailingContact.Id;
                }
            }
            else
            {
                ViewBag.GNOrganizationId = new SelectList(orgs, "Id", "Name");
                ViewBag.GNAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description");
                ViewBag.BillingMode = billingModeTypeSelectList;
                ViewBag.DefaultDiscountType = discountTypeSelectList;
            }

            return acct;
        }

        public override GNAccount CreateOnSubmit(GNAccount acct)
        {
            acct = base.CreateOnSubmit(acct);

            acct.Organization = db.GNOrganizations.Find(Guid.Parse(Request["GNOrganizationId"]));

            if(acct != null && acct.Organization != null)
            {
                acct.MailingContact = acct.Organization.OrgMainContact;
                acct.BillingContact = acct.Organization.OrgMainContact;
                acct.AccountOwner = acct.Organization.OrgMainContact;
            }

            return acct;
        }

        public override ActionResult CreateOnSuccess(GNAccount acct)
        {
            if (acct.BillingMode == GNAccount.BillingModeType.INVOICE.GetCode())
            {
                ((AccountService)entityService).AddCheckPaymentMethod(UserContact, acct);
            }

            ((AccountService)entityService).SetPaymentMethodDefaultFlags(acct.Id);

            return base.CreateOnSuccess(acct);
        }

        public override ActionResult EditOnSuccess(GNAccount acct)
        {
            acct = ((AccountService)entityService).FindAccountWithPaymentMethods(acct.Id);

            var checkCode = GNPaymentMethodType.Types.CHECK.GetCode();
            if (acct.BillingMode == GNAccount.BillingModeType.INVOICE.GetCode() 
                && acct.PaymentMethods.Count(pm => 
                (pm.IsActive == true && pm.PaymentMethodType.Name == checkCode)) == 0)
            {
                ((AccountService)entityService).AddCheckPaymentMethod(UserContact, acct);
            }

            ((AccountService)entityService).SetPaymentMethodDefaultFlags(acct.Id);

            return base.EditOnSuccess(acct);
        }
    }
}
