using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAnalysisAdaptorMetadata))]
    public partial class GNAnalysisAdaptor : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNAnalysisAdaptorMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {

        [Display(Name = "Adaptor")]
        public string Code { get; set; }
        [Display(Name = "Adaptor")]
        public string Name { get; set; }
       
    }
}
