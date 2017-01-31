using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAccountThresholdMetadata))]
    public partial class GNAccountThreshold : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNAccountThresholdMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        [Display(Name = "Threshold Amount")]
        [DataType(DataType.Currency)]
        public double ThresholdAmount { get; set; }
        public int Rank { get; set; }
        [Display(Name = "Threshold Type")]
        public string ThresholdType { get; set; }
        [Display(Name = "Threshold Interval")]
        public int ThresholdInterval { get; set; }
        [Display(Name = "Account")]
        public System.Guid GNAccountId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNAccount Account { get; set; }
    }
}