﻿using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Cloud.Messaging.Model;
using GenomeNext.Cloud.Messaging.Model.GN;
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
    public class AnalysisStatusListener : QueueListener<AnalysisStatusMessage>
    {
        public override IGNCloudMessageService<AnalysisStatusMessage> GetService()
        {
            AWSConfig awsConfig = new GNEntityModelContainer().AWSConfigs.FirstOrDefault();
            return new AnalysisStatusCloudMessageService(awsConfig.Id, AnalysisStatusCloudMessageService.QUEUE_NAME);
        }

    }
}
