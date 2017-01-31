using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(AWSComputeEnvironmentMetadata))]
    public partial class AWSComputeEnvironment : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class AWSComputeEnvironmentMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Required]
        [Display(Name = "AWS Env Key")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "VPC ID")]
        public string VPC { get; set; }

        [Required]
        [Display(Name = "Subnet ID")]
        public string Subnet { get; set; }

        [Required]
        [Display(Name = "Availability Zone")]
        public string AvailZone { get; set; }

        [Required]
        [Display(Name = "# Instances Required per Analysis")]
        public int MaxInstanceRequiredPerAnalysis { get; set; }

        [Required]
        [Display(Name = "# Expected Instances in Environment")]
        public int MaxInstanceExpectedCount { get; set; }

        [Display(Name = "# IPs Availabile")]
        public int IPAvailCount { get; set; }
        
        [Display(Name = "# Instances Running")]
        public int InstanceRunningCount { get; set; }
        
        [Display(Name = "# Instances Pending")]
        public int InstancePendingCount { get; set; }
        
        [Display(Name = "AWS Config")]
        public System.Guid AWSConfigId { get; set; }

        [Display(Name = "AWS Config")]
        public virtual AWSConfig AWSConfig { get; set; }
    }
}
