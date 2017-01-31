using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNNotificationTopicSubscriberMetadata))]
    public partial class GNNotificationTopicSubscriber : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public bool IsSubscribedBool
        {
            get { return IsSubscribed == "Y";  }
        }



    }

    public class GNNotificationTopicSubscriberMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        
    }
}
