using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNOrganizationMetadata))]
    public partial class GNOrganization : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string getName()
        {
            return "fix this issue soon";
        }

        public TimeZoneInfo OrgTimeZoneInfo 
        { 
            get 
            {
                TimeZoneInfo destTZInfo = null;

                if (!string.IsNullOrEmpty(this.UTCOffsetDescription))
                {
                    var timeZones = TimeZoneInfo.GetSystemTimeZones();
                    string destUTC = this.UTCOffsetDescription.Trim();
                    destTZInfo = timeZones.Where(t => t.DisplayName == destUTC).FirstOrDefault();
                }

                if(destTZInfo == null)
                {
                    destTZInfo = TimeZoneInfo.Utc;
                }

                return destTZInfo;
            }
        }
    }

    public class GNOrganizationMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }

        [Display(Name="Primary Contact")]
        public Nullable<System.Guid> GNContactId { get; set; }

        public virtual ICollection<GNTeam> Teams { get; set; }
        
        public virtual ICollection<GNContact> Contacts { get; set; }

        [Display(Name = "Primary Contact")]
        public virtual GNContact OrgMainContact { get; set; }
        
        [Display(Name = "Account")]
        public virtual GNAccount Account { get; set; }

        [Display(Name = "Time Zone")]
        public string UTCOffsetDescription { get; set; }

        [Display(Name = "Settings Template")]
        public string GNSettingsTemplate { get; set; }
        
        public virtual ICollection<GNSample> Samples { get; set; }
    }
}
