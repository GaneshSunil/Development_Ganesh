using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNBulkImportStatusMetadata))]
    public partial class GNBulkImportStatus : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public bool IsComplete 
        { 
            get 
            {
                bool isComplete = false;

                if ((ImportStartDateTime != null
                    && this.TotalRecordCount == (this.ImportedRecordCount + this.FailedRecordCount + this.DuplicateRecordCount))
                    || ImportEndDateTime != null)
                {
                    isComplete = true;
                }

                return isComplete;
            } 
        }
    }

    public partial class GNBulkImportStatusMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }

        public string FileURIType { get; set; }

        [Display(Name = "File URI")]
        public string FileURI { get; set; }
        
        public string FileExtension { get; set; }
        
        public string FileMimeType { get; set; }

        [Display(Name="Total # of Records")]
        public long TotalRecordCount { get; set; }

        [Display(Name = "# Imported Records")]
        public long ImportedRecordCount { get; set; }

        [Display(Name = "# Failed Records")]
        public long FailedRecordCount { get; set; }

        [Display(Name = "# Duplicate Records")]
        public long DuplicateRecordCount { get; set; }

        [Display(Name = "# Teams Created")]
        public long TeamsCreatedCount { get; set; }

        [Display(Name = "# Projects Created")]
        public long ProjectsCreatedCount { get; set; }

        [Display(Name = "# Analyses Created")]
        public long AnalysesCreatedCount { get; set; }

        [Display(Name = "# Samples Created")]
        public long SamplesCreatedCount { get; set; }

        [Display(Name = "# Files Created")]
        public long FilesCreatedCount { get; set; }

        [Display(Name = "Import Start Date/Time")]
        public Nullable<System.DateTime> ImportStartDateTime { get; set; }

        [Display(Name = "Import End Date/Time")]
        public Nullable<System.DateTime> ImportEndDateTime { get; set; }

        public virtual ICollection<GNBulkImportLog> BulkImportLogs { get; set; }
    }
}
