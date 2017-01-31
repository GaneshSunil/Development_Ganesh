using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Messaging;
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
    public class BulkImportListener : QueueListener<BulkImportMessage>
    {
        public override IGNCloudMessageService<BulkImportMessage> GetService()
        {
            AWSConfig awsConfig = new GNEntityModelContainer().AWSConfigs.FirstOrDefault();
            return new BulkImportCloudMessageService(awsConfig.Id, BulkImportCloudMessageService.QUEUE_NAME);
        }

    }
}