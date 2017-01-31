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
    public class PurchaseOrdersController : GNEntityController<GNPurchaseOrder>
    {
        public PurchaseOrdersController()
            : base()
        {
            entityService = new PurchaseOrderService(base.db);
        }

        public override GNPurchaseOrder PopulateSelectLists(GNPurchaseOrder po = null)
        {
            po = base.PopulateSelectLists(po);

            if(po == null || po == default(GNPurchaseOrder))
            {
                GNInvoice poInvoice = null;
                if (!string.IsNullOrEmpty(Request["invoiceId"]))
                {
                    poInvoice = this.entityService.db.GNInvoices.Find(Guid.Parse(Request["invoiceId"]));
                }

                if(poInvoice != null)
                {
                    po = new GNPurchaseOrder
                    {
                        StartDate = poInvoice.InvoiceStartDate,
                        EndDate = poInvoice.InvoiceEndDate,
                        Account = poInvoice.Account,
                        GNAccountId = poInvoice.Account.Id
                    };

                }
            }
            else
            {
                po.Account = this.db.GNAccounts.Find(po.GNAccountId);
            }

            return po;
        }

        public override GNPurchaseOrder CreateOnSubmit(GNPurchaseOrder po)
        {
            po = base.CreateOnSubmit(po);

            if(po.Id == Guid.Empty)
            {
                po.Id = Guid.NewGuid();
            }

            GNInvoice poInvoice = null;
            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                poInvoice = this.entityService.db.GNInvoices.Find(Guid.Parse(Request["invoiceId"]));

                po.PurchaseOrderInvoices.Add(new GNPurchaseOrderGNInvoice { 
                    Invoices_Id = poInvoice.Id,
                    PurchaseOrders_Id = po.Id,
                    TotalApplied = 0.0,
                    CreatedBy = UserContact.Id,
                    CreateDateTime = DateTime.Now
                });

                po.GNAccountId = poInvoice.Account.Id;
            }

            return po;
        }

        public override ActionResult CreateOnSuccess(GNPurchaseOrder po)
        {
            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                return RedirectToAction("Edit", "PurchaseOrders", new { id = po.Id, invoiceId = Request["invoiceId"] });
            }
            else
            {
                return RedirectToAction("Edit", "PurchaseOrders", new { id = po.Id });
            }
        }

        public override GNPurchaseOrder EditOnSubmit(GNPurchaseOrder po)
        {
            po = base.EditOnSubmit(po);

            return po;
        }

        public override ActionResult EditOnSuccess(GNPurchaseOrder po)
        {
            if (!string.IsNullOrEmpty(Request["invoiceId"]))
            {
                return RedirectToAction("Details", "PurchaseOrders", new { id = po.Id, invoiceId = Request["invoiceId"] });
            }
            else
            {
                return RedirectToAction("Details", "PurchaseOrders", new { id = po.Id });
            }
        }

        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Index", "PurchaseOrders");
        }

        public async Task<ActionResult> AddToInvoiceToPurchaseOrder(Guid purchaseOrderId, Guid invoiceId)
        {
            this.db.GNPurchaseOrderGNInvoice.Add(new GNPurchaseOrderGNInvoice
            {
                PurchaseOrders_Id = purchaseOrderId,
                Invoices_Id = invoiceId,
                TotalApplied = 0.0
            });

            await this.db.SaveChangesAsync();

            return RedirectToAction("Edit", "PurchaseOrders", new { id = purchaseOrderId });
        }

        public async Task<ActionResult> RemoveInvoiceFromPurchaseOrder(Guid purchaseOrderId, Guid invoiceId)
        {
            this.db.GNPurchaseOrderGNInvoice.Remove(new GNPurchaseOrderGNInvoice 
            { 
                PurchaseOrders_Id = purchaseOrderId,
                Invoices_Id = invoiceId
            });

            await this.db.SaveChangesAsync();

            return RedirectToAction("Edit", "PurchaseOrders", new { id = purchaseOrderId });
        }
    }
}
