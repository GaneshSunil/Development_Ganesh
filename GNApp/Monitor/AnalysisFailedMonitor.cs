using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Compute;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenomeNext.Billing;
using Amazon.EC2.Model;

namespace GenomeNext.App.Monitor
{
    public class AnalysisFailedMonitor : IConsoleApp
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static GNCloudComputeService cloudComputeService { get; set; }
        private static bool MARK_ANALYSIS_IN_ERROR_FEATURE_ACTIVE = false;
        private static bool TERMINATE_INSTANCE_FEATURE_ACTIVE = false;
        private static bool RESTART_ANALYSIS_FEATURE_ACTIVE = false;
        private static bool FORCE_RESTART_ANALYSIS = false;

        public void Init()
        {
            InitServices();
        }

        public void Run()
        {
            Monitor();
        }

        private void InitServices()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            //get mark analysis in error feature setting
            bool.TryParse(
                System.Configuration.ConfigurationManager.AppSettings["MarkAnalysisInErrorFeatureActive"],
                out MARK_ANALYSIS_IN_ERROR_FEATURE_ACTIVE);

            //get restart feature setting
            bool.TryParse(
                System.Configuration.ConfigurationManager.AppSettings["TerminateInstanceFeatureActive"],
                out TERMINATE_INSTANCE_FEATURE_ACTIVE);

            //get restart feature setting
            bool.TryParse(
                System.Configuration.ConfigurationManager.AppSettings["RestartAnalysisFeatureActive"],
                out RESTART_ANALYSIS_FEATURE_ACTIVE);

            //get force restart feature setting
            bool.TryParse(
                System.Configuration.ConfigurationManager.AppSettings["ForceRestartAnalysis"],
                out FORCE_RESTART_ANALYSIS);

            if (cloudComputeService == null)
            {
                LogUtil.Info(logger, GetType().Name + ".InitServices()...");
                System.Console.WriteLine("\n" + GetType().Name + ".InitServices()...");

                try
                {
                    AWSConfig awsConfig = new GNEntityModelContainer().AWSConfigs.FirstOrDefault();
                    cloudComputeService = new GNCloudComputeService(awsConfig.Id);
                }
                catch (Exception ex)
                {
                    LogUtil.Warn(logger, ex.Message, ex);
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        private void Monitor()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, GetType().Name + ".Monitor()...");
            System.Console.WriteLine("\n" + GetType().Name + ".Monitor()...");

            try
            {
                //init analysis service
                var analysisRequestService = new AnalysisRequestService(new GNEntityModelContainer(), new IdentityModelContainer());

                //fetch analyses With Errors But Not Marked In Error
                IQueryable<GNAnalysisRequest> analysesWithErrorsButNotMarkedInError = 
                    analysisRequestService.FetchAnalysesWithErrorsButNotMarkedInError();
                if (analysesWithErrorsButNotMarkedInError != null && analysesWithErrorsButNotMarkedInError.Count() != 0)
                {
                    LogUtil.Info(logger, "analysesWithErrorsButNotMarkedInError count = " + analysesWithErrorsButNotMarkedInError.Count());
                    System.Console.WriteLine("analysesWithErrorsButNotMarkedInError count = " + analysesWithErrorsButNotMarkedInError.Count());
                    
                    foreach (var analysisRequest in analysesWithErrorsButNotMarkedInError)
                    {
                        //handle analysis failure (mark as failed and initiate refund)
                        MarkAnalysisAsFailed(analysisRequestService, analysisRequest);
                    }
                }

                //fetch analyses Marked In Error
                IQueryable<GNAnalysisRequest> analysesMarkedInError = analysisRequestService.FetchAnalysesMarkedInError();
                if (analysesMarkedInError != null && analysesMarkedInError.Count() != 0)
                {
                    LogUtil.Info(logger, "analysesMarkedInError count = " + analysesMarkedInError.Count());
                    System.Console.WriteLine("analysesMarkedInError count = " + analysesMarkedInError.Count());
                    
                    foreach (var analysisRequest in analysesMarkedInError)
                    {
                        //terminate AWS instance for analysis request in error
                        TerminateInstancesForAnalysis(analysisRequest);

                        //handle analysis failure (auto re-start)
                        ReStartFailedAnalysis(analysisRequestService, analysisRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Warn(logger, ex.Message, ex);
                System.Console.WriteLine(ex.Message);
            }
        }

        private static void TerminateInstancesForAnalysis(GNAnalysisRequest analysisRequest)
        {
            List<Reservation> analysisMasterInstances = null;
            List<Reservation> analysisWorkerInstances = null;

            if(TERMINATE_INSTANCE_FEATURE_ACTIVE)
            {
                LogUtil.Info(logger, "Terminating Instances for AR ID ["+analysisRequest.Id+"]...");
                System.Console.WriteLine("\nTerminating Instances for AR ID ["+analysisRequest.Id+"]...");

                try
                {
                    //lookup analysis master instances
                    analysisMasterInstances = cloudComputeService.GetComputeInstances(
                        new Dictionary<string, List<string>> 
                                { 
                                    {"Name", new List<string> { "churchill_"+analysisRequest.Id+"_master" }} 
                                });

                    //lookup analysis worker instances
                    analysisWorkerInstances = cloudComputeService.GetComputeInstances(
                        new Dictionary<string, List<string>> 
                                { 
                                    {"Name", new List<string> { "churchill_"+analysisRequest.Id+"_worker" }} 
                                });

                    //send termination commands to stale AWS instances
                    try
                    {
                        List<string> instancesToTerminate = new List<string>();

                        //terminate master instances
                        if (analysisMasterInstances != null && analysisMasterInstances.Sum(r => r.Instances.Count) != 0)
                        {
                            instancesToTerminate.AddRange(
                                analysisMasterInstances.SelectMany(r => r.Instances).Select(i => i.InstanceId).ToList());
                        }

                        //terminate worker instances
                        if (analysisWorkerInstances != null && analysisWorkerInstances.Sum(r => r.Instances.Count) != 0)
                        {
                            instancesToTerminate.AddRange(
                                analysisWorkerInstances.SelectMany(r => r.Instances).Select(i => i.InstanceId).ToList());
                        }

                        //submit instance termination request to AWS
                        if (instancesToTerminate != null && instancesToTerminate.Count != 0)
                        {
                            var terminateResult = cloudComputeService.TerminateComputeInstances(instancesToTerminate);
                        }
                    }
                    catch (Exception e)
                    {
                        LogUtil.Error(logger, "Unable to Terminate Instances for stale Analysis " + analysisRequest.Id + " : " + e.Message, e);
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error(logger, "Unable to Lookup Instances for stale Analysis " + analysisRequest.Id + " : " + e.Message, e);
                }
            }
        }

        private static void MarkAnalysisAsFailed(AnalysisRequestService analysisRequestService, GNAnalysisRequest analysisRequest)
        {
            if (MARK_ANALYSIS_IN_ERROR_FEATURE_ACTIVE)
            {
                try
                {
                    //get analysis request from db
                    GNAnalysisRequest ar = analysisRequestService.db.GNAnalysisRequests
                        .Include(a => a.AnalysisType)
                        .Include(a => a.AnalysisStatus)
                        .Include(a => a.AnalysisResult)
                        .Where(a => a.Id == analysisRequest.Id)
                        .FirstOrDefault();

                    //mark analysis as failed
                    if (!ar.IsFailedRequest)
                    {
                        LogUtil.Info(logger, "Mark Analysis As Failed for AR ID [" + ar.Id + "]...");
                        System.Console.WriteLine("\nMark Analysis As Failed for AR ID [" + ar.Id + "]...");

                        analysisRequestService.MarkAnalysisAsFailed(analysisRequestService, ar);
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error(logger, "Unable to Process Failure of Analysis " + analysisRequest.Id + " : " + e.Message, e);
                }
            }
        }

        private static void ReStartFailedAnalysis(AnalysisRequestService analysisRequestService, GNAnalysisRequest analysisRequest)
        {
            if(RESTART_ANALYSIS_FEATURE_ACTIVE)
            {
                try
                {
                    //get analysis request from db
                    GNAnalysisRequest ar = analysisRequestService.db.GNAnalysisRequests
                        .Include(a => a.AnalysisType)
                        .Include(a => a.GNAnalysisRequestGNSamples.Select(s => s.GNSample).Select(s => s.CloudFiles))
                        .Include(a => a.AnalysisStatus)
                        .Include(a => a.AnalysisResult)
                        .Where(a => a.Id == analysisRequest.Id)
                        .FirstOrDefault();

                    //determine if analysis re-start is allowed
                    ar = analysisRequestService.IsValidSampleSet(ar);
                    ar.CanReStartAnalysis = analysisRequestService.IsAnalysisRestartAllowed(ar, FORCE_RESTART_ANALYSIS);

                    //restart analysis
                    if (ar.CanReStartAnalysis && ar.IsFailedRequest)
                    {
                        LogUtil.Info(logger, "Restart Analysis for AR ID [" + ar.Id + "]...");
                        System.Console.WriteLine("\nRestart Analysis for AR ID [" + ar.Id + "]...");

                        var t = Task.Run(async delegate
                        {
                            var arService = new AnalysisRequestService(new GNEntityModelContainer(), new IdentityModelContainer());

                            //get user contact
                            GNContact userContact = arService.db.GNContacts.Find(ar.CreatedBy);

                            if (arService.analysisRequestPendingCloudMessageService == null || arService.cloudStorageService == null)
                            {
                                arService.InitCloudServices(userContact.GNOrganization.AWSConfigId);
                            }

                            await arService.StartAnalysis(userContact, ar.Id);
                        });
                    }
                    else
                    {
                        LogUtil.Info(logger, "UNABLE to Restart Analysis for AR ID [" + ar.Id + "]...");
                        System.Console.WriteLine("\nUNABLE to Restart Analysis for AR ID [" + ar.Id + "]...");
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error(logger, "Unable to Process Failure of Analysis " + analysisRequest.Id + " : " + e.Message, e);
                }
            }
        }
    }
}
