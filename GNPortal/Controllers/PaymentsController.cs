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
    public class PaymentsController : GNEntityController<GNPayment>
    {
        private readonly string ENTITY = "PAYMENT";

        public PaymentsController()
            : base()
        {
            entityService = new PaymentService(base.db);
        }

        public override GNPayment PopulateSelectLists(GNPayment payment = null)
        {
            payment = base.PopulateSelectLists(payment);

            var paymentMethods = db.GNPaymentMethods.Where(pm=>pm.Account.Organization.Id == UserContact.GNOrganizationId);

            if (payment != null)
            {
                ViewBag.GNPaymentMethodId = new SelectList(paymentMethods, "Id", "PaymentMethodType.Name", payment.GNPaymentMethodId);
            }
            else
            {
                ViewBag.GNPaymentMethodId = new SelectList(paymentMethods, "Id", "PaymentMethodType.Name");
            }

            if (payment == null)
            {
                payment = new GNPayment();
                payment.PaymentDate = DateTime.Now;
            }

            LoadInvoiceForPayment(payment);

            return payment;
        }

        public override GNPayment DetailsOnLoad(GNPayment payment)
        {
            auditResult = audit.LogEvent(UserContact, payment.Id, this.ENTITY, this.Request.UserHostAddress, "LOAD_DETAILS_UI");
            
            payment = base.DetailsOnLoad(payment);

            LoadInvoiceForPayment(payment);

            return payment;
        }

        public override GNPayment CreateOnSubmit(GNPayment payment)
        {
            auditResult = audit.LogEvent(UserContact, payment.Id, this.ENTITY, this.Request.UserHostAddress, "CREATE");
            
            if (string.IsNullOrEmpty(payment.ExternalTxnId))
            {
                payment.ExternalTxnId = "N/A";
            }

            AddInvoiceToPayment(payment);

            return base.CreateOnSubmit(payment);
        }

        public override ActionResult CreateOnSuccess(GNPayment payment)
        {
            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                return RedirectToAction("Details", new { id = payment.Id, invoiceId = Request["invoiceId"] });
            }
            else
            {
                return RedirectToAction("Details", new { id = payment.Id });
            }
        }

        public override GNPayment EditOnSubmit(GNPayment payment)
        {
            auditResult = audit.LogEvent(UserContact, payment.Id, this.ENTITY, this.Request.UserHostAddress, "EDIT");
            
            if (string.IsNullOrEmpty(payment.ExternalTxnId))
            {
                payment.ExternalTxnId = "N/A";
            }

            AddInvoiceToPayment(payment);

            return base.EditOnSubmit(payment);
        }

        public override ActionResult EditOnSuccess(GNPayment payment)
        {
            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                return RedirectToAction("Details", new { id = payment.Id, invoiceId = Request["invoiceId"] });
            }
            else
            {
                return RedirectToAction("Details", new { id = payment.Id });
            }
        }

        public override GNPayment DeleteOnLoad(GNPayment entity)
        {
            return base.DeleteOnLoad(DetailsOnLoad(entity));
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNInvoice invoice = null;

            if (!string.IsNullOrEmpty(id))
            {
                invoice = entityService.db.GNPayments.Find(Guid.Parse(id)).Invoices.FirstOrDefault();
            }

            if (invoice != null)
            {
                id = invoice.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Edit", "Invoices", new { id = Guid.Parse(id) });
        }

        private void LoadInvoiceForPayment(GNPayment payment)
        {
            if(payment != null)
            {
                if(payment.Invoices != null && payment.Invoices.Count != 0)
                {
                    payment.Invoice = payment.Invoices.FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(Request["invoiceId"]))
                {
                    payment.Invoice = entityService.db.GNInvoices.Find(Guid.Parse((Request["invoiceId"])));
                }

                if (payment.Invoice != null)
                {
                    payment.GNInvoiceId = payment.Invoice.Id;
                }
            }
        }

        private void AddInvoiceToPayment(GNPayment payment)
        {
            if (payment.GNInvoiceId != Guid.Empty)
            {
                payment.Invoices = new List<GNInvoice>();
                payment.Invoices.Add(
                    entityService.db.GNInvoices.Find(payment.GNInvoiceId));
            }
        }
    }
}
