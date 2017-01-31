using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAnalysisStatusMetadata))]
    public partial class GNAnalysisStatus : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        
    }

    public partial class GNAnalysisStatusMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        [Display(Name = "Progress")]
        public string Status { get; set; }
        public string Message { get; set; }
        public int PercentComplete { get; set; }
        [Display(Name = "Error?")]
        public bool IsError { get; set; }
        public Nullable<System.Guid> GNAnalysisResultId { get; set; }
        public Nullable<System.Guid> GNAnalysisRequestId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNAnalysisResult GNAnalysisResult { get; set; }
        public virtual GNAnalysisRequest GNAnalysisRequest { get; set; }
    }
}
