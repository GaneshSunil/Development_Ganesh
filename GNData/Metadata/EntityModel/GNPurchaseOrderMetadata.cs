using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNPurchaseOrderMetadata))]
    public partial class GNPurchaseOrder : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        [Display(Name = "Total Applied")]
        [DataType(DataType.Currency)]
        public double TotalApplied { get { return (this.PurchaseOrderInvoices != null) ? this.PurchaseOrderInvoices.Sum(poi => poi.TotalApplied) : 0.0; } }

        [DataType(DataType.Currency)]
        public double Balance { get { return this.Total - this.TotalApplied; } }
        
        public string PurchaseOrderExternalNumWithDateAndAmount
        {
            get
            {
                return "PO Num: " + this.ExternalPONum + " for $"+ this.Total+". Balance: $"+ this.Balance +". Created On: " + this.CreateDateTime.ToString() + " Under Org: " + this.Account.Organization.Name;
            }
        }
    }

    public partial class GNPurchaseOrderMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        [Display(Name = "External PO #")]
        public string ExternalPONum { get; set; }
        [DataType(DataType.Currency)]
        public double Total { get; set; }
        [Display(Name = "Start Date")]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Account")]
        public System.Guid GNAccountId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNAccount Account { get; set; }
        [Display(Name = "Invoices")]
        public virtual ICollection<GNPurchaseOrderGNInvoice> PurchaseOrderInvoices { get; set; }
    }
}