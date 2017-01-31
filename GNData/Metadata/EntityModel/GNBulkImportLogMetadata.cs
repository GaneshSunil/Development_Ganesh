using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNBulkImportLogMetadata))]
    public partial class GNBulkImportLog : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNBulkImportLogMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string RecordRowId { get; set; }
        public bool IsError { get; set; }
        public bool IsDuplicate { get; set; }
        public string Message { get; set; }
        public System.Guid GNBulkImportStatusId { get; set; }

        public virtual GNBulkImportStatus BulkImportStatus { get; set; }
    }
}
