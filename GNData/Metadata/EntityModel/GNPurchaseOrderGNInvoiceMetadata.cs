using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNPurchaseOrderGNInvoiceMetadata))]
    public partial class GNPurchaseOrderGNInvoice : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNPurchaseOrderGNInvoiceMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Purchase Order")]
        public System.Guid PurchaseOrders_Id { get; set; }
        [Display(Name = "Invoice")]
        public System.Guid Invoices_Id { get; set; }
        [Display(Name = "Total Applied")]
        [DataType(DataType.Currency)]
        public double TotalApplied { get; set; }

        [Display(Name = "Purchase Order")]
        public virtual GNPurchaseOrder PurchaseOrder { get; set; }
        public virtual GNInvoice Invoice { get; set; }
    }
}