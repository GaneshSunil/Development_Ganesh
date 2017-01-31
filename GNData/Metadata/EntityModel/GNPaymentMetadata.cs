using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNPaymentMetadata))]
    public partial class GNPayment : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public GNInvoice Invoice { get; set; }
        public Guid GNInvoiceId { get; set; }
    }

    public partial class GNPaymentMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public double TotalAmount { get; set; }
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime PaymentDate { get; set; }
        [Display(Name = "Ext Txn ID")]
        public string ExternalTxnId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        [Display(Name = "Payment Method")]
        public Nullable<System.Guid> GNPaymentMethodId { get; set; }

        public virtual ICollection<GNInvoice> Invoices { get; set; }
        [Display(Name = "Payment Method")]
        public virtual GNPaymentMethod PaymentMethod { get; set; }
    }
}
