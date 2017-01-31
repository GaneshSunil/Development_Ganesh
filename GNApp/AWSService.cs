using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data;
using GenomeNext.Cloud.Compute;
using Amazon.EC2.Model;

namespace GenomeNext.App
{
    public class AWSConfigService : GNEntityService<AWSConfig>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AWSConfigService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AWSConfig>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AWSConfig> entities =
                await db.AWSConfigs
                .ToListAsync();

            return entities;
        }

        public override async Task<AWSConfig> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AWSConfigs.FindAsync(keys);
        }
    }

    public class AWSRegionService : GNEntityService<AWSRegion>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AWSRegionService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AWSRegion>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AWSRegion> entities =
                await db.AWSRegions
                .ToListAsync();

            return entities;
        }

        public override async Task<AWSRegion> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AWSRegions.FindAsync(keys);
        }
    }

    public class AWSResourceService : GNEntityService<AWSResource>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AWSResourceService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AWSResource>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AWSResource> entities =
                await db.AWSResources
                .ToListAsync();

            return entities;
        }

        public override async Task<AWSResource> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AWSResources.FindAsync(keys);
        }
    }

    public class AWSResourceTypeService : GNEntityService<AWSResourceType>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AWSResourceTypeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AWSResourceType>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AWSResourceType> entities =
                await db.AWSResourceTypes
                .ToListAsync();

            return entities;
        }

        public override async Task<AWSResourceType> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AWSResourceTypes.FindAsync(keys);
        }
    }

    public class AWSComputeEnvironmentService : GNEntityService<AWSComputeEnvironment>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private GNCloudComputeService cloudComputeService { get; set; }
        public AnalysisRequestService analysisRequestService { get; set; }

        public AWSComputeEnvironmentService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;

            AWSConfig awsConfig = db.AWSConfigs.FirstOrDefault();
            cloudComputeService = new GNCloudComputeService(awsConfig.Id);
            analysisRequestService = new AnalysisRequestService(db, new Data.IdentityModel.IdentityModelContainer());
        }

        public override async Task<List<AWSComputeEnvironment>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AWSComputeEnvironment> entities =
                await db.AWSComputeEnvironments
                .OrderBy(a=>a.Id)
                .ToListAsync();

            return entities;
        }

        public override async Task<AWSComputeEnvironment> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AWSComputeEnvironments.FindAsync(keys);
        }

        public void UpdateComputeCapacityInDB()
        {
            foreach (var awsComputeEnv in this.db.AWSComputeEnvironments)
            {
                //get subnet by id
                var subnets = cloudComputeService.GetSubNets(null,
                    new List<Filter> { new Filter("subnet-id", new List<string> { awsComputeEnv.Subnet }) });

                if (subnets != null && subnets.Count == 1 && subnets[0].SubnetId == awsComputeEnv.Subnet)
                {
                    //store subnet ip available count
                    awsComputeEnv.IPAvailCount = subnets[0].AvailableIpAddressCount;

                    //get instances running in subnet
                    var computeInstances = cloudComputeService.GetComputeInstances(null,
                        new List<Filter> { new Filter("subnet-id", new List<string> { awsComputeEnv.Subnet }) });

                    //store instance running count
                    if (computeInstances != null && computeInstances.Sum(i => i.Instances.Count) != 0)
                    {
                        awsComputeEnv.InstanceRunningCount = computeInstances.Sum(i => i.Instances.Count);
                    }
                    else
                    {
                        awsComputeEnv.InstanceRunningCount = 0;
                    }
                }

                //calc instance pending count
                if (awsComputeEnv.Id.ToLower().StartsWith("spot_"))
                {
                    //get spot requests for subnet
                    var computeSpotRequests = cloudComputeService.GetComputeSpotRequests(null, null);
                    //new List<Filter> { new Filter("launched-availability-zone", new List<string> { awsComputeEnv.AvailZone }) });

                    //store instance pending count
                    if (computeSpotRequests != null && computeSpotRequests.Count != 0)
                    {
                        awsComputeEnv.InstancePendingCount =
                            computeSpotRequests.Count(i => 
                                i.State == Amazon.EC2.SpotInstanceState.Open 
                                && i.LaunchSpecification.NetworkInterfaces.FirstOrDefault().SubnetId == awsComputeEnv.Subnet)
                            * awsComputeEnv.MaxInstanceRequiredPerAnalysis;
                    }
                    else
                    {
                        awsComputeEnv.InstancePendingCount = 0;
                    }
                }
                else
                {
                    List<GNAnalysisRequest> analysesPendingAndNotStarted =
                        analysisRequestService.FetchAnalysesPendingAndNotStarted(cloudComputeService, awsComputeEnv.Id);

                    if (analysesPendingAndNotStarted != null && analysesPendingAndNotStarted.Count != 0)
                    {

                        int subnetCountInSameVPC =
                            this.db.AWSComputeEnvironments.Count(env => env.VPC == awsComputeEnv.VPC);

                        awsComputeEnv.InstancePendingCount =
                            (int)Math.Ceiling((double)(analysesPendingAndNotStarted.Count / subnetCountInSameVPC))
                            * awsComputeEnv.MaxInstanceRequiredPerAnalysis;
                    }
                    else
                    {
                        awsComputeEnv.InstancePendingCount = 0;
                    }
                }
            }

            this.db.SaveChanges();
        }    
    }
}
