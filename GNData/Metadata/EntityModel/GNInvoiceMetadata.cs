using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNInvoiceMetadata))]
    public partial class GNInvoice : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string Name 
        { 
            get
            {
                string name = "";

                if(!string.IsNullOrEmpty(this.ExternalInvoiceNum))
                {
                    name += "Invoice #" + this.ExternalInvoiceNum;
                }
                else if(this.Account!= null 
                    && this.Account.Organization != null 
                    && !string.IsNullOrEmpty(this.Account.Organization.Name))
                {
                    name += this.Account.Organization.Name;
                }

                if (InvoiceStartDate != null || InvoiceEndDate != null)
                {
                    name += " (";
                }

                if (InvoiceStartDate != null)
                {
                    name += InvoiceStartDate.ToShortDateString();
                }


                if (InvoiceStartDate != null && InvoiceEndDate != null)
                {
                    name += " - ";
                }

                if (InvoiceEndDate != null)
                {
                    name += InvoiceEndDate.ToShortDateString();
                }

                if (InvoiceStartDate != null || InvoiceEndDate != null)
                {
                    name += ")";
                }

                return name;
            } 
        }
        
        public string ExternalNumberWithStartDate
        {
            get
            {
                return "Invoice Num: "+this.ExternalInvoiceNum + ". Start Date: " + this.InvoiceStartDate.ToShortDateString() + " Organization: " + this.Account.Organization.Name;
            }
        }

        [Display(Name = "Payments Total")]
        [DataType(DataType.Currency)]
        public double PaymentsTotal 
        {
            get
            {
                return this.Payments.Sum(p=>p.TotalAmount);
            }
        }

        [DataType(DataType.Currency)]
        public double Balance
        {
            get
            {
                return this.Total - this.PaymentsTotal - this.TotalAppliedToPOs;
            }
        }

        [DataType(DataType.Currency)]
        public double CreditsTotal { get; set; }

        [DataType(DataType.Currency)]
        public double DebitsTotal { get; set; }

        [DataType(DataType.Currency)]
        public double TotalAppliedToPOs { get; set; }

        public enum InvoiceStatus
        {
            CREATED,
            PAID,
            OVERDUE,
            VOID
        }
    }

    public partial class GNInvoiceMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        [Display(Name = "External Invoice #")]
        public string ExternalInvoiceNum { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime InvoiceStartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime InvoiceEndDate { get; set; }
        public string Status { get; set; }
        [Display(Name = "Total Discount")]
        [DataType(DataType.Currency)]
        public double TotalDiscountAmount { get; set; }
        [Display(Name = "Sub-Total")]
        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }
        [DataType(DataType.Currency)]
        public double Total { get; set; }
        [Display(Name = "Net Terms")]
        public int NetTerms { get; set; }
        [Display(Name = "Account")]
        public System.Guid GNAccountId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNAccount Account { get; set; }
        [Display(Name = "Invoice Details")]
        public virtual ICollection<GNInvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<GNPayment> Payments { get; set; }
        [Display(Name = "Purchase Order(s)")]
        public virtual ICollection<GNPurchaseOrderGNInvoice> PurchaseOrderInvoices { get; set; }
    }
}
