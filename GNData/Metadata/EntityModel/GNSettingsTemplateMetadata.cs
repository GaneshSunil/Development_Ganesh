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
    [MetadataType(typeof(GNSettingsTemplateMetadata))]
    public partial class GNSettingsTemplate : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        
    }

    public partial class GNSettingsTemplateMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "# Configs")]
        public virtual ICollection<GNSettingsTemplateConfig> GNSettingsTemplateConfigs { get; set; }

        [Display(Name = "# Organizations")]
        public virtual ICollection<GNOrganization> GNOrganizations { get; set; }
    }

}
