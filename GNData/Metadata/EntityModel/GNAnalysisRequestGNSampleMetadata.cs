using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAnalysisRequestGNSampleMetadata))]
    public partial class GNAnalysisRequestGNSample : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string AffectedIndicatorDescription
        {
            get
            {
                string description = "Unknown";
                switch (this.AffectedIndicator)
                {
                    case "Y": description = "Yes"; break;
                    case "N": description = "No"; break;
                    default: description = "Unknown"; break;
                }
                return description;
            }
        }
        public string TargetIndicatorDescription
        {
            get
            {
                string description = "Unknown";
                switch (this.TargetIndicator)
                {
                    case "Y": description = "Yes"; break;
                    case "N": description = "No"; break;
                }
                return description;
            }
        }

        public ICollection<GNSampleRelationship> UnImportedSampleRelationships { get; set; }
    }

    public partial class GNAnalysisRequestGNSampleMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
    }
}
