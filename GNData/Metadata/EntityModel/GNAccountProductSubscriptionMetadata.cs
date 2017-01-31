using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAccountProductSubscriptionMetadata))]
    public partial class GNAccountProductSubscription : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public enum SubscriptionFrequencyInterval
        {
            NONE = 0,
            DAILY = 365,
            WEEKLY = 52,
            MONTHLY = 12,
            QUARTERLY = 4,
            SEMI_ANNUALY = 2,
            YEARLY = 1
        }
    }

    public partial class GNAccountProductSubscriptionMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        [Display(Name = "Account")]
        public System.Guid GNAccountId { get; set; }
        [Display(Name = "Product")]
        public System.Guid GNProductId { get; set; }
        [Display(Name = "Used")]
        public string CurrentValueUsed { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Subscribe Frequency")]
        public int SubscribeFrequency { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
