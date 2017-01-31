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
    public class InvoiceDetailsController : GNEntityController<GNInvoiceDetail>
    {
        public InvoiceDetailsController() : base()
        {
            entityService = new InvoiceDetailService(base.db);
        }

        public override GNInvoiceDetail PopulateSelectLists(GNInvoiceDetail invoiceDetail = null)
        {
            invoiceDetail = base.PopulateSelectLists(invoiceDetail);

            if(invoiceDetail == null)
            {
                invoiceDetail = new GNInvoiceDetail();

                GNAccount acct = entityService.db.GNAccounts.Where(a => a.Organization.Id == UserContact.GNOrganizationId).FirstOrDefault();
                if(acct != null)
                {
                    invoiceDetail.DiscountAmount = acct.DefaultDiscountAmount;
                    invoiceDetail.DiscountType = acct.DefaultDiscountType;
                }
            }

            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                invoiceDetail.Invoice = entityService.db.GNInvoices.Find(Guid.Parse((Request["invoiceId"])));
                if (invoiceDetail.Invoice != null)
                {
                    invoiceDetail.GNInvoiceId = invoiceDetail.Invoice.Id;
                }
            }

            return invoiceDetail;
        }

        public override ActionResult CreateOnSuccess(GNInvoiceDetail invoiceDetail)
        {
            return RedirectToAction("Edit", "Invoices", new { id = invoiceDetail.GNInvoiceId});
        }

        public override ActionResult EditOnSuccess(GNInvoiceDetail invoiceDetail)
        {
            return RedirectToAction("Edit", "Invoices", new { id = invoiceDetail.GNInvoiceId });
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNInvoice invoice = null;

            if (!string.IsNullOrEmpty(id))
            {
                invoice = entityService.db.GNInvoiceDetails.Find(Guid.Parse(id)).Invoice;
            }

            if(invoice != null)
            {
                id = invoice.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Edit", "Invoices", new { id = Guid.Parse(id) });
        }
    }
}
