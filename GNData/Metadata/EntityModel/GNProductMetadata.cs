using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNProductMetadata))]
    public partial class GNProduct : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNProductMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Product Type")]
        public int GNProductTypeId { get; set; }
        [Display(Name = "Subscribe Frequency")]
        public int SubscribeFrequency { get; set; }
        [DataType(DataType.Currency)]
        public double Cost { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Min Range Value")]
        public double MinRangeValue { get; set; }
        [Display(Name = "Max Range Value")]
        public double MaxRangeValue { get; set; }
        [Display(Name = "Value Units")]
        public string ValueUnits { get; set; }
        [Display(Name = "Account Type")]
        public int GNAccountTypeId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        [Display(Name = "Product Type")]
        public virtual GNProductType ProductType { get; set; }
        [Display(Name = "Product Subscriptions")]
        public virtual ICollection<GNAccountProductSubscription> AccountProductSubscriptions { get; set; }
        [Display(Name = "Transactions")]
        public virtual ICollection<GNTransaction> Transactions { get; set; }
        [Display(Name = "Account Type")]
        public virtual GNAccountType AccountType { get; set; }
    }
}
