using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNCloudFileMetadata))]
    public partial class GNCloudFile : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string SampleId { get; set; }
        public bool IsExternallyHosted { get; set; }
    }

    public partial class GNCloudFileMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }

        [Display(Name = "File URL")]
        public string FileURL { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Volume")]
        public string Volume { get; set; }

        [Display(Name = "Folder Path")]
        public string FolderPath { get; set; }

        [Display(Name = "File Size")]
        public int FileSize { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int GNCloudFileCategoryId { get; set; }

        [Display(Name = "Category")]
        public virtual GNCloudFileCategory CloudFileCategory { get; set; }
    }
}
