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
using GenomeNext.Portal.Models;

namespace GenomeNext.Portal.Controllers
{
    public class InvoicesController : GNEntityController<GNInvoice>
    {
        private readonly string ENTITY = "INVOICE";

        public InvoicesController()
            : base()
        {
            entityService = new InvoiceService(base.db);
        }

        public override GNInvoice PopulateSelectLists(GNInvoice invoice = null)
        {
            invoice = base.PopulateSelectLists(invoice);

            var accts = db.GNAccounts.OrderBy(a => a.Organization.Name);

            var statuses = from GNInvoice.InvoiceStatus s in Enum.GetValues(typeof(GNInvoice.InvoiceStatus))
                             select new { Id = s.ToString(), Name = s.ToString() };

            if (invoice != null)
            {
                ViewBag.GNAccountId = new SelectList(accts, "Id", "Organization.Name", invoice.GNAccountId);
                ViewBag.Status = new SelectList(statuses, "Id", "Name", invoice.Status.ToString());
            }
            else
            {
                ViewBag.GNAccountId = new SelectList(accts, "Id", "Organization.Name");
                ViewBag.Status = new SelectList(statuses, "Id", "Name");

                invoice = new GNInvoice 
                { 
                    InvoiceStartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, 1),
                    InvoiceEndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month)),
                    NetTerms = 30 
                };
            }

            return invoice;
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public override GNInvoice CreateOnSubmit(GNInvoice invoice)
        {
            invoice = base.CreateOnSubmit(invoice);

            invoice.Status = GNInvoice.InvoiceStatus.CREATED.ToString();
            invoice.InvoiceCycle = String.Format("{0:yyyyMM}", invoice.InvoiceStartDate);

            auditResult = audit.LogEvent(UserContact, invoice.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            return invoice;
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public override GNInvoice EditOnSubmit(GNInvoice invoice)
        {
            auditResult = audit.LogEvent(UserContact, invoice.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            invoice.InvoiceCycle = String.Format("{0:yyyyMM}", invoice.InvoiceStartDate);

            return base.EditOnSubmit(invoice);
        }

        public async Task<ActionResult> PrintDetails(string id)
        {
            Guid invoiceId = Guid.Parse(id);
            ViewBag.isPrintableView = true;
            ViewBag.ContactForUser = UserContact;

            GNInvoice invoice = db.GNInvoices.Where(a => a.Id.Equals(invoiceId)).FirstOrDefault();
            IQueryable<GNTeam> Teams = db.GNTeams.Where(a => a.GNTransactions.FirstOrDefault().InvoiceDetail.GNInvoiceId.Equals(invoice.Id));
            

            List<MyInvoicePrintModel> ViewTeams = new List<MyInvoicePrintModel>();

            foreach (var item in Teams)
            {
                ViewTeams.Add(new MyInvoicePrintModel
                {
                    TeamName = item.Name,
                    TotalPrice = item.GNTransactions.Sum(a => a.TotalPrice),
                    TotalTransactions = item.GNTransactions.Count()
                });
            }


            ViewBag.Teams = ViewTeams;

            auditResult = audit.LogEvent(UserContact, invoice.Id, this.ENTITY, this.Request.UserHostAddress, "PRINT");
            
            return View(invoice);

        }
    }
}
