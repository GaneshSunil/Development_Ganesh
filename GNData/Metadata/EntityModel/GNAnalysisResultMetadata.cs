using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAnalysisResultMetadata))]
    public partial class GNAnalysisResult : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNAnalysisResultMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        
        public bool Success { get; set; }

        [Display(Name="Analysis Start Date Time")]
        public System.DateTime AnalysisStartDateTime { get; set; }

        [Display(Name = "Analysis End Date Time")]
        public System.DateTime AnalysisEndDateTime { get; set; }

        [Display(Name = "AWS Region")]
        public string AWSRegionSystemName { get; set; }

        [Display(Name = "Result Files")]
        public virtual ICollection<GNCloudFile> ResultFiles { get; set; }

        [Display(Name = "AWS Region")]
        public virtual AWSRegion AWSRegion { get; set; }

        [Display(Name = "Analysis Status")]
        public virtual ICollection<GNAnalysisStatus> AnalysisStatus { get; set; }

        [Display(Name = "Analysis Request")]
        public virtual GNAnalysisRequest AnalysisRequest { get; set; }
    }
}
