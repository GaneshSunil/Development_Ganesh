using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNTransactionMetadata))]
    public partial class GNTransaction : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNTransactionMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string Description { get; set; }
        [Display(Name = "Invoice Detail")]
        public System.Guid GNInvoiceDetailId { get; set; }
        [Display(Name = "Product")]
        public System.Guid GNProductId { get; set; }
        [Display(Name = "Transaction Type")]
        public int GNTransactionTypeId { get; set; }
        [Display(Name = "Ext Txn ID")]
        public string ExternalTxnId { get; set; }
        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public double TotalCost { get; set; }
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        [Display(Name = "Invoice Detail")]
        public virtual GNInvoiceDetail InvoiceDetail { get; set; }
        [Display(Name = "Transaction Type")]
        public virtual GNTransactionType TransactionType { get; set; }
        [Display(Name = "Product")]
        public virtual GNProduct Product { get; set; }
    }
}
