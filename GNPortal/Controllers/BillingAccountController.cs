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
    public class BillingAccountController : GNEntityController<GNBillingAccount>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ENTITY = "BILLING_ACCOUNT";

        public BillingAccountController()
            : base()
        {
            entityService = new BillingAccountService(base.db);
        }

        public override GNBillingAccount PopulateSelectLists(GNBillingAccount acct = null)
        {
            acct = base.PopulateSelectLists(acct);

            var orgs = db.GNOrganizations.OrderBy(o => o.Name);

            //billing mode type select list
            var billingModeTypeSelectList = new SelectList(
                new Dictionary<string, string>(), "key", "value",
                ((acct == null) ? null : acct.BillingMode));
            ((Dictionary<string, string>)billingModeTypeSelectList.Items).Add(
                GNBillingAccount.BillingModeType.INVOICE.GetCode(), GNBillingAccount.BillingModeType.INVOICE.GetName());

            if (acct != null)
            {
                ViewBag.GNOrganizationId = new SelectList(orgs, "Id", "Name", acct.GNOrganization.Id);
                ViewBag.GNBillingAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description", acct.GNAccountTypeId);
                ViewBag.BillingMode = billingModeTypeSelectList;

                if (acct.GNBillingContact != null)
                {
                    acct.BillingContactId = acct.GNBillingContact.Id;
                }
                if (acct.GNMailingContact != null)
                {
                    acct.MailingContactId = acct.GNMailingContact.Id;
                }
            }
            else
            {
                ViewBag.GNOrganizationId = new SelectList(orgs, "Id", "Name");
                ViewBag.GNBillingAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description");
                ViewBag.BillingMode = billingModeTypeSelectList;
            }

            return acct;
        }

        public override GNBillingAccount CreateOnSubmit(GNBillingAccount acct)
        {
            acct = base.CreateOnSubmit(acct);

            acct.GNOrganization = db.GNOrganizations.Find(Guid.Parse(Request["GNOrganizationId"]));

            if (acct != null && acct.GNOrganization != null)
            {
                acct.GNMailingContact = acct.GNOrganization.OrgMainContact;
                acct.GNBillingContact = acct.GNOrganization.OrgMainContact;
            }

            return acct;
        }

        public override ActionResult CreateOnSuccess(GNBillingAccount acct)
        {
            if (acct.BillingMode == GNBillingAccount.BillingModeType.INVOICE.GetCode())
            {
                ((BillingAccountService)entityService).AddCheckPaymentMethod(UserContact, acct);
            }

            ((BillingAccountService)entityService).SetPaymentMethodDefaultFlags(acct.Id);

            return base.CreateOnSuccess(acct);
        }

        public override ActionResult EditOnSuccess(GNBillingAccount acct)
        {
            acct = ((BillingAccountService)entityService).FindAccountWithPaymentMethods(acct.Id);

            var checkCode = GNPaymentMethodType.Types.CHECK.GetCode();
            if (acct.BillingMode == GNBillingAccount.BillingModeType.INVOICE.GetCode()
                && acct.GNBillingPaymentMethods.Count(pm =>
                (pm.IsActive == true && pm.GNPaymentMethodType.Name == checkCode)) == 0)
            {
                ((BillingAccountService)entityService).AddCheckPaymentMethod(UserContact, acct);
            }

            ((BillingAccountService)entityService).SetPaymentMethodDefaultFlags(acct.Id);

            return base.EditOnSuccess(acct);
        }
    }
}
