using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNNotificationLogMetadata))]
    public partial class GNNotificationLog : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string Topic { get; set; }

    }

    public class GNNotificationLogMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Topic")]
        public int GNNotificationTopicId { get; set; }

        [Display(Name = "Notification Service Response")]
        public string NotificationServiceResponse { get; set; }
    }
}
