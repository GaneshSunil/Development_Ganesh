using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNPaymentMethodMetadata))]
    public partial class GNPaymentMethod : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNPaymentMethodMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        [Display(Name = "Payment Method Type")]
        public int GNPaymentMethodTypeId { get; set; }
        public string Description { get; set; }
        [Display(Name = "Last Four Digits")]
        public string LastFourDigits { get; set; }
        [Display(Name = "PCI Token")]
        public string PCITokenId { get; set; }
        [Display(Name = "Is Default?")]
        public bool IsDefault { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Used For Recurrent Payments")]
        public bool UsedForRecurrentPayments { get; set; }
        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime ExpirationDate { get; set; }
        [Display(Name = "Account")]
        public System.Guid GNAccountId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNAccount Account { get; set; }
        [Display(Name = "Payment Method Type")]
        public virtual GNPaymentMethodType PaymentMethodType { get; set; }
    }
}
