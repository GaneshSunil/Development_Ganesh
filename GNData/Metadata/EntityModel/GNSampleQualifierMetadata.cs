using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSampleQualifierMetadata))]
    public partial class GNSampleQualifier : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNSampleQualifierMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Type")]
        public System.String GNSampleQualifierGroupCode { get; set; }


    }
}
