using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSampleTypeMetadata))]
    public partial class GNSampleType : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNSampleTypeMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }

        [Display(Name="Sample Type")]
        public string Name { get; set; }

        [Display(Name = "Value")]
        public string TypeValue { get; set; }
    }
}
