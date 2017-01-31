using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSequencerJobMetadata))]
    public partial class GNSequencerJob : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public class GNSequencerJobMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Required]
        [Display(Name = "Sequencer Job")]
        public Guid Id { get; set; }
        
    }
}
