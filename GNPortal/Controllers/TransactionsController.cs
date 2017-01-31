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
    public class TransactionsController : GNEntityController<GNTransaction>
    {
        public TransactionsController() : base()
        {
            entityService = new TransactionService(base.db);
        }

        public override GNTransaction PopulateSelectLists(GNTransaction transaction = null)
        {
            transaction = base.PopulateSelectLists(transaction);

            if(transaction != null)
            {
                ViewBag.GNTransactionTypeId = new SelectList(entityService.db.GNTransactionTypes, "Id", "Name", transaction.GNTransactionTypeId);
                ViewBag.GNProductId = new SelectList(entityService.db.GNProducts, "Id", "Name", transaction.GNProductId);
            }
            else
            {
                ViewBag.GNTransactionTypeId = new SelectList(entityService.db.GNTransactionTypes, "Id", "Name");
                ViewBag.GNProductId = new SelectList(entityService.db.GNProducts, "Id", "Name");
            }

            if (transaction == null)
            {
                transaction = new GNTransaction();
            }

            if (!string.IsNullOrEmpty(Request["invoiceDetailId"]))
            {
                transaction.InvoiceDetail = entityService.db.GNInvoiceDetails.Find(Guid.Parse(Request["invoiceDetailId"]));
                transaction.GNInvoiceDetailId = transaction.InvoiceDetail.Id;
            }

            return transaction;
        }

        public override ActionResult CreateOnSuccess(GNTransaction transaction)
        {
            return RedirectToAction("Edit", "InvoiceDetails", new { id = transaction.GNInvoiceDetailId });
        }

        public override ActionResult EditOnSuccess(GNTransaction transaction)
        {
            return RedirectToAction("Edit", "InvoiceDetails", new { id = transaction.GNInvoiceDetailId });
        }

        public override string GetParentIdForEntityOnDelete(string id)
        {
            GNInvoiceDetail invoiceDetail = null;

            if (!string.IsNullOrEmpty(id))
            {
                invoiceDetail = entityService.db.GNTransactions.Find(Guid.Parse(id)).InvoiceDetail;
            }

            if (invoiceDetail != null)
            {
                id = invoiceDetail.Id.ToString();
            }

            return id;
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Edit", "InvoiceDetails", new { id = Guid.Parse(id) });
        }
    }
}
