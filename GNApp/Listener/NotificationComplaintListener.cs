using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Cloud.Messaging.Model;
using GenomeNext.Cloud.Messaging.Model.GN;
using GenomeNext.Cloud.Messaging.Model.SES;
using GenomeNext.Cloud.Messaging.Model.SQS;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenomeNext.App.Listener
{
    public class NotificationComplaintListener : QueueListener<AmazonSqsNotification>
    {
        public override IGNCloudMessageService<AmazonSqsNotification> GetService()
        {
            AWSConfig awsConfig = new GNEntityModelContainer().AWSConfigs.FirstOrDefault();
            return new NotificationSesComplaintMessageService(awsConfig.Id, NotificationSesComplaintMessageService.QUEUE_NAME);
        }
    }
}
