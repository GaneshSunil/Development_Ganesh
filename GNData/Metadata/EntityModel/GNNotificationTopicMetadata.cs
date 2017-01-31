using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNNotificationTopicMetadata))]
    public partial class GNNotificationTopic : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public bool NotifyObjectCreatorBool
        {
            get { return NotifyObjectCreator == "Y";  }
        }

        public string IsSubscriptionOptionalText
        {
            get { return (IsSubscriptionOptional == "Y" ? "Yes" : "No" ); }
        }
        public bool IsSubscriptionOptionalBool
        {
            get { return IsSubscriptionOptional == "Y"; }
        }

        public string[] AddToRoles { get; set; }
        public string[] RemoveToRoles { get; set; }

        public string[] AddCcRoles { get; set; }
        public string[] RemoveCcRoles { get; set; }

        public string[] AddBccRoles { get; set; }
        public string[] RemoveBccRoles { get; set; }

    }

    public class GNNotificationTopicMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Title")]
        public string Title { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }


        [Display(Name = "Creator of the object or event")]
        public string NotifyObjectCreator { get; set; }


        [Display(Name = "Can users unsubscribe?")]
        public string IsSubscriptionOptional { get; set; }

        [Display(Name = "Sending condition")]
        public string SendingCondition { get; set; }
    }
}
