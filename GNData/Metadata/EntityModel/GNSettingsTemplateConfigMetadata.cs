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
    [MetadataType(typeof(GNSettingsTemplateConfigMetadata))]
    public partial class GNSettingsTemplateConfig : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        
    }

    public partial class GNSettingsTemplateConfigMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        public string Value { get; set; }

        [Display(Name = "Settings Template")]
        public virtual GNSettingsTemplate GNSettingsTemplate { get; set; }
        [Display(Name = "Settings Field")]
        public virtual GNSettingsTemplateField GNSettingsTemplateField { get; set; }
    }

}
