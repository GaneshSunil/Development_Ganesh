using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNInvoiceDetailMetadata))]
    public partial class GNInvoiceDetail : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNInvoiceDetailMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string Description { get; set; }
        [Display(Name = "Discount Type")]
        public string DiscountType { get; set; }
        [Display(Name = "Discount Amount")]
        public double DiscountAmount { get; set; }
        [Display(Name = "Unit Cost")]
        [DataType(DataType.Currency)]
        public double UnitCost { get; set; }
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        [Display(Name = "Sub-Total")]
        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }
        [DataType(DataType.Currency)]
        public double Total { get; set; }
        [Display(Name = "Invoice")]
        public System.Guid GNInvoiceId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNInvoice Invoice { get; set; }
        public virtual ICollection<GNTransaction> Transactions { get; set; }
    }
}
