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
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class SharedPurchaseOrderOrganizationsController : GNEntityController<GNSharedPurchaseOrderOrganization>
    {
        public SharedPurchaseOrderOrganizationsController()
            : base()
        {
            entityService = new SharedPurchaseOrderOrganizationService(base.db);
        }

        public override GNSharedPurchaseOrderOrganization PopulateSelectLists(GNSharedPurchaseOrderOrganization poorg = null)
        {
            var invoices = db.GNInvoices.OrderByDescending(a => a.InvoiceStartDate);
            var purchaseOrders = db.GNPurchaseOrders.OrderByDescending(a => a.CreateDateTime);

            ViewBag.GNInvoiceId = new SelectList(invoices, "Id", "ExternalNumberWithStartDate", poorg.GNInvoiceId);
            ViewBag.GNPurchaseOrderId = new SelectList(purchaseOrders, "Id", "PurchaseOrderExternalNumWithDateAndAmount", poorg.GNPurchaseOrderId);

            return poorg;
        }

        public override async Task<ActionResult> Create(GNSharedPurchaseOrderOrganization entity)
        {
            entity.CreatedBy = UserContact.Id;

            Guid invoiceId = entity.GNInvoiceId;
            entity.GNInvoice = db.GNInvoices.Find(invoiceId);

            Guid poId = entity.GNPurchaseOrderId;
            entity.GNPurchaseOrder = db.GNPurchaseOrders.Find(poId);
            entity.Notes = entity.Notes.Trim();
            return await base.Create(entity);
        }

        public override ActionResult EditOnSuccess(GNSharedPurchaseOrderOrganization entity)
        {
            return RedirectToAction("Index");
        }

        public override ActionResult CreateOnSuccess(GNSharedPurchaseOrderOrganization entity)
        {
            return RedirectToAction("Index");
        }
    }
}
