using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNCloudFileCategoryMetadata))]
    public partial class GNCloudFileCategory : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNCloudFileCategoryMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        [Required]
        [Display(Name = "Category")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Folder Path")]
        public string FolderPath { get; set; }
    }
}
