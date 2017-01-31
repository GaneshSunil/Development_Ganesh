using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSampleRelationshipMetadata))]
    public partial class GNSampleRelationship : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNSampleRelationshipMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "This sample")]
        public System.Guid GNLeftSampleId { get; set; }

        [Display(Name = "Related sample")]
        public System.Guid GNRightSampleId { get; set; }

        [Display(Name = "Relationship")]
        public int GNSampleRelationshipTypeId { get; set; }
    }
}
