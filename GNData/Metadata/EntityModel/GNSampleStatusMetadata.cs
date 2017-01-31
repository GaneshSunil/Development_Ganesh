using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSampleStatusMetadata))]
    public partial class GNSampleStatus : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        
    }

    public partial class GNSampleStatusMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Progress")]
        public string Message { get; set; }
        [Display(Name = "Error?")]
        public bool IsError { get; set; }

    }
}
