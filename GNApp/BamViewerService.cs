using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data;
using GenomeNext.Cloud.CloudNoSQL;
using System.Reflection;
using System.Threading.Tasks;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Cloud.Messaging.Model.GN;
using System.IO;
using System.Diagnostics;


using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


using Amazon.SQS;
using Amazon.SQS.Model;


namespace GenomeNext.App
{
    public class BamViewerService : GNCloudMessageService<BamViewer>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_BAM_VIEWER";
        public Guid matchingAnalysisId;

        public BamViewerService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public BamViewerService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }



        public bool CheckNewMessages()
        {
            bool result = this.ConsumeMessages();

            return result;
        }


        /**
         * Write new methods to consume messages from SQS
         */
        public bool ConsumeMessages(int visibilityTimeout = 2)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            bool result = false;
            try
            {
                var response = AWSSQSClient.ReceiveMessage(new Amazon.SQS.Model.ReceiveMessageRequest
                {
                    QueueUrl = this.queueURL,
                    VisibilityTimeout = visibilityTimeout
                });

                while (response != null && response.Messages.Count != 0)
                {
                    foreach (var message in response.Messages)
                    {
                        result = ConsumeMessage(message);
                        if(result)
                        {
                            //it means we found the message we were looking for, no need to keep looking
                            break;
                        }
                    }

                    if (result)
                    {
                        //it means we found the message we were looking for, no need to keep looking                        
                        break;
                    }

                    response = AWSSQSClient.ReceiveMessage(new Amazon.SQS.Model.ReceiveMessageRequest
                    {
                        QueueUrl = this.queueURL,
                        VisibilityTimeout = visibilityTimeout
                    });
                }
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to consume one or more Messages", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                
                throw e2;
            }

            return result;
        }

        private static void ProcessXcopy(string SolutionDirectory, string TargetDirectory)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            //Give the name as Xcopy
            startInfo.FileName = "xcopy";
            //make the window Hidden
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Send the Source and destination as Arguments to the process
            startInfo.Arguments = "\"" + SolutionDirectory + "\"" + " " + "\"" + TargetDirectory + "\"" + @" /e /y /I";
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

        public bool ConsumeMessage(object queueMessage)
        {
            bool success = false;
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            Message message = (Message)queueMessage;

            try
            {
                //deserialize message
                BamViewer messageObj = DeserializeMessage(message);

                if (messageObj != null)
                {
                    success = this.ProcessMessage(messageObj, message);
                    if (success)
                    {
                        //delete message from queue

                        string mkdirCmd = "D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId;
                        Directory.CreateDirectory(mkdirCmd);
                        
                        string cmd = "xcopy C:\\jb\\ " + this.matchingAnalysisId + " D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId + " /E /H";
                     
                       // string SourcePath = @"Z:\" + this.matchingAnalysisId + "\\";
                        string SourcePath = @"\\10.102.3.186\home\ubuntu\sync\" + this.matchingAnalysisId + "\\";
                        string DestinationPath = @" D:\Sites\GNPortal-1.1\jbrowse\" + this.matchingAnalysisId + "\\";
                        //Now Create all of the directories
                        foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                            SearchOption.AllDirectories))
                            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                        //Copy all the files & Replaces any files with the same name
                        foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                            SearchOption.AllDirectories))
                            System.IO.File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);






                        cmd = "/J D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId + "\\data\\names D:\\Sites\\GNPortal-1.1\\jbrowse\\names";
                        Process.Start("mklink", cmd);
                        cmd = "/J D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId + "\\data\\seq D:\\Sites\\GNPortal-1.1\\jbrowse\\seq";
                        Process.Start("mklink", cmd);
                        cmd = "/J D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId + "\\data\\tracks\\Exons D:\\Sites\\GNPortal-1.1\\jbrowse\\Exons";
                        Process.Start("mklink", cmd);
                        cmd = "/J D:\\Sites\\GNPortal-1.1\\jbrowse\\" + this.matchingAnalysisId + "\\data\\tracks\\HumanGenes D:\\Sites\\GNPortal-1.1\\jbrowse\\HumanGenes";
                        Process.Start("mklink", cmd);


                        success = DeleteMessage(message);
                    }
                }
                else
                {
                    System.Console.WriteLine(">>>>>>>> messageObj is null ");
                }
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("!!!!! Unable to consume Message !!!!!", e1);
                LogUtil.Warn(logger, e2.Message + "\n" + (message != null ? message.Body : ""), e2);
            }
            return success;
        }


        public override bool ProcessMessage(BamViewer message, object queueMessage)
        {
            bool success = false;
            try
            {
                Guid entityId = Guid.Parse(message.analysisId);
                if(entityId.Equals(this.matchingAnalysisId))
                {
                    success = true;
                }
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to process BamViewer Message.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }


        public void CopyFiles(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFiles(diSourceSubDir, nextTargetSubDir);
            }
        }
    }


    public class BamConfigFileService : GNCloudMessageService<BamConfigFile>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_BAM_VIEWER";
        public Guid matchingAnalysisId;

        public BamConfigFileService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public BamConfigFileService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }


        /**
         * JSON Format
         */
        public void PrepareBamViewer(GNAnalysisRequest analysisRequest)
        {
            GNCloudFile GNCloudVCF = (from a in analysisRequest.AnalysisResult.ResultFiles
                                      where a.GNCloudFileCategoryId == 2
                                      select a).FirstOrDefault<GNCloudFile>();
            string vcfFile = GNCloudVCF.Description.Substring(GNCloudVCF.Description.LastIndexOf('/') + 1);
            string fileContents = "";
            //List<string>.Enumerator enumerator = samplesSelected.GetEnumerator();

            BamConfigFile bamRequest = new BamConfigFile();
            bamRequest.analysisId = analysisRequest.Id.ToString();
            bamRequest.analysisDescription = analysisRequest.Description;
            bamRequest.vcfFilename = vcfFile;
            bamRequest.listOfSamples = new List<AnalysisResultSample>();
            bamRequest.totalNumberOfSamples = analysisRequest.GNAnalysisRequestGNSamples.Count();

            try
            {
                int lineNum = 0;
                foreach (GNAnalysisRequestGNSample analysisSampleInput in analysisRequest.GNAnalysisRequestGNSamples)
                {
                    lineNum++;
                    GNSample sampleInput = analysisSampleInput.GNSample;
                    string sampleInputName = sampleInput.Name;
                    sampleInputName = sampleInputName.Replace(" ", "_").Replace("(", "_").Replace(")", "_").ToUpper();

                    string baiFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string bamFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bam") && !a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string item = bamFile + " " + baiFile + " " + analysisRequest.Id.ToString() + " " + vcfFile + " " + sampleInput.Name + " " + lineNum + " " + analysisRequest.Description.Replace(" ", "_").Trim().ToUpper();

                    fileContents = fileContents + item + "\n";

                    //   await this.transactionService.CreateBillingTransactionsPerAnalysisSample(userContact, analysisRequest, analysisRequest.AnalysisType.Name, analysisRequest.Description, false, false, "REPORT_BAM_VIEWER", sampleInput);
                    bamRequest.listOfSamples.Add(new AnalysisResultSample
                    {
                        index = lineNum,
                        sampleId = sampleInput.Id.ToString(),
                        name = sampleInputName,
                        baiFilename = baiFile,
                        bamFilename = bamFile
                    }
                        );
                }

            }
            catch (Exception e)
            {
                logger.Error("Error while launching BAM Viewer " + e.Message);
            }


            try
            {
                System.Console.WriteLine("***\n ******  Sending message ");
                this.SendMessage(bamRequest);
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw;
            }
        }


    }


    public class BamViewerRequestService : GNCloudMessageService<BamViewerRequest>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "CARTAGENIA_TRANSFER";
        public Guid matchingAnalysisId;

        public BamViewerRequestService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public BamViewerRequestService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }


        /**
         * JSON Format
         */
        public void PrepareBamViewer(GNAnalysisRequest analysisRequest)
        {
            GNCloudFile GNCloudVCF = (from a in analysisRequest.AnalysisResult.ResultFiles
                                      where a.GNCloudFileCategoryId == 2
                                      select a).FirstOrDefault<GNCloudFile>();
            string vcfFilePath = GNCloudVCF.FileURL.Substring(0, GNCloudVCF.Description.LastIndexOf('/'));
            string vcfFile = GNCloudVCF.Description.Substring(GNCloudVCF.Description.LastIndexOf('/') + 1);
            string fileContents = "";
            //List<string>.Enumerator enumerator = samplesSelected.GetEnumerator();

            
            BamConfigFile bamRequest = new BamConfigFile();
            bamRequest.analysisId = analysisRequest.Id.ToString();
            bamRequest.analysisDescription = analysisRequest.Description;
            bamRequest.vcfFilename = vcfFile;
            bamRequest.listOfSamples = new List<AnalysisResultSample>();
            bamRequest.totalNumberOfSamples = analysisRequest.GNAnalysisRequestGNSamples.Count();

            try
            {
                int lineNum = 0;
                foreach (GNAnalysisRequestGNSample analysisSampleInput in analysisRequest.GNAnalysisRequestGNSamples)
                {
                    lineNum++;
                    GNSample sampleInput = analysisSampleInput.GNSample;
                    string sampleInputName = sampleInput.Name;
                    sampleInputName = sampleInputName.Replace(" ", "_").Replace("(", "_").Replace(")", "_").ToUpper();

                    string baiFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string bamFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bam") && !a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string item = bamFile + " " + baiFile + " " + analysisRequest.Id.ToString() + " " + vcfFile + " " + sampleInput.Name + " " + lineNum + " " + analysisRequest.Description.Replace(" ", "_").Trim().ToUpper();

                    fileContents = fileContents + item + "\n";

                    //   await this.transactionService.CreateBillingTransactionsPerAnalysisSample(userContact, analysisRequest, analysisRequest.AnalysisType.Name, analysisRequest.Description, false, false, "REPORT_BAM_VIEWER", sampleInput);
                    bamRequest.listOfSamples.Add(new AnalysisResultSample
                    {
                        index = lineNum,
                        sampleId = sampleInput.Id.ToString(),
                        name = sampleInputName,
                        baiFilename = baiFile,
                        bamFilename = bamFile
                    }
                        );
                }

            }
            catch (Exception e)
            {
                logger.Error("Error while launching BAM Viewer " + e.Message);
            }

            GNNewSampleBatch batch = analysisRequest.GNAnalysisRequestGNSamples.FirstOrDefault().GNSample.GNNewSampleBatchSample.GNNewSampleBatch;

            BamViewerRequest bamViewerRequest = new BamViewerRequest {
                      bamViewerURL = vcfFilePath, 
                      batchId = batch.BatchId,
                      organization = batch.GNSequencerJob.GNOrganization.Name,
                      project = batch.GNSequencerJob.Project,
                      repository = batch.Repository,
                      vcfFilepath = vcfFilePath,
                      bamConfigFile = bamRequest

                        };

            try
            {
                System.Console.WriteLine("***\n ******  Sending message ");
                this.SendMessage(bamViewerRequest);
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw;
            }
        }


    }


    public class BamPostAnalysisService : GNCloudMessageService<AnalysisCompletionMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "CARTAGENIA_TRANSFER";

        public BamPostAnalysisService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public BamPostAnalysisService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }


        public bool Notify(GNAnalysisRequest analysisRequest)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            
            try
            {
                GNCloudFile GNCloudVCF = (from a in analysisRequest.AnalysisResult.ResultFiles
                                          where a.GNCloudFileCategoryId == 2
                                          select a).FirstOrDefault<GNCloudFile>();

                GNNewSampleBatch batch = analysisRequest.GNAnalysisRequestGNSamples.FirstOrDefault().GNSample.GNNewSampleBatchSample.GNNewSampleBatch;
                AnalysisCompletionMessage message = new AnalysisCompletionMessage
                {
                    analysisId = analysisRequest.Id.ToString(),
                    batchId = batch.BatchId,
                    project = batch.Project,
                    repository = batch.Repository,
                    bamViewerURL = "https://secure.genomenext.net/GNPortal-1.3/AnalysisRequests/BamViewer?analysisId=" + analysisRequest.Id.ToString(),
                    vcfFilename = GNCloudVCF.FileURL.Replace("https://", "s3://").Replace(".s3.amazonaws.com", "")
                };

                System.Console.WriteLine("***\n ******  Sending message " + analysisRequest.CreateDateTime);
                this.SendMessage(message);
            }
            catch (Exception e1)
            {
                System.Console.WriteLine("***\n ******  Exception Sending message " + e1.InnerException + e1.Message + e1.StackTrace);
                Exception e2 = new Exception("Unable to send notification to queue.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                return false;
            }

            return true;
        }

    }

}
