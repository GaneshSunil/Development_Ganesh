using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(AWSConfigMetadata))]
    public partial class AWSConfig : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string getMaskedAWSSecretAccessKey()
        {
            return "*****"+this.AWSSecretAccessKey.Substring(this.AWSSecretAccessKey.Length - 10);    
        }
    }

    public partial class AWSConfigMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {

        public System.Guid Id { get; set; }
        [Display(Name = "AWS Access Key Id")]
        public string AWSAccessKeyId { get; set; }
        [Display(Name = "AWS Secret Access Key")]
        public string AWSSecretAccessKey { get; set; }
        [Display(Name = "AWS Region")]
        public string AWSRegionSystemName { get; set; }

        [Display(Name = "AWS Resources")]
        public virtual ICollection<AWSResource> AWSResources { get; set; }
        [Display(Name = "AWS Region")]
        public virtual AWSRegion AWSRegion { get; set; }
        [Display(Name = "Organizations")]
        public virtual ICollection<GNOrganization> GNOrganizations { get; set; }
    }
}
