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
    [MetadataType(typeof(GNSettingsTemplateFieldMetadata))]
    public partial class GNSettingsTemplateField : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string IdWithConfigType { get { return !string.IsNullOrEmpty(this.ConfigType) ? this.ConfigType + " : " + this.Id : this.Id;} }   
    }

    public partial class GNSettingsTemplateFieldMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Config Type")]
        public string ConfigType { get; set; }
    }

}
