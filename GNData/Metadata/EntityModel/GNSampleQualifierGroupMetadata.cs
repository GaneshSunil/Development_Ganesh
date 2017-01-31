using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSampleQualifierGroupMetadata))]
    public partial class GNSampleQualifierGroup : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNSampleQualifierGroupMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Code")]
        public System.String Code { get; set; }

        [Display(Name = "Group")]
        public System.String Name { get; set; }

    }
}
