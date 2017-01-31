using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Cloud.Messaging.Model.GN;
using GenomeNext.Cloud.Messaging.Model.SES;
using GenomeNext.Cloud.Messaging.Model.SQS;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;
using GenomeNext.Notification;
using GenomeNext.Cloud.Compute;
using System.Configuration;
using Newtonsoft.Json;
using GenomeNext.Cloud.Storage;

namespace GenomeNext.App
{
    /// <summary>
    /// Message Consumer for 'GN_SAMPLE_REQUEST' queue
    /// </summary>
    public class QcReportService : GNCloudMessageService<QcReport>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_QC_PORTALQC";

        public QcReportService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public QcReportService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(QcReport qcReportSqsMessage, object queueMessage)
        {
            System.Console.WriteLine("***\n ****** Sample Name: " + qcReportSqsMessage.entityID);
            System.Console.WriteLine("***\n ****** Sample type: " + qcReportSqsMessage.entityType);
            System.Console.WriteLine("***\n ---->QC REPORT PROCESSING FOR ENTITY: " + qcReportSqsMessage.entityID + " (" + qcReportSqsMessage.entityType + ")");

            /**
             * Find Sample in NewSamplesBatch
             * 
             */

            bool success = false;
            try
            {
                Guid entityId = Guid.Parse(qcReportSqsMessage.entityID);
                if (qcReportSqsMessage.entityType.ToUpper().Equals("SAMPLE"))
                {
                    GNSample sample = db.GNSamples.Find(entityId);

                    //Find Batch
                    //GNNewSampleBatch Batch = db.GNNewSampleBatchSamples.Where(a => a.GNSample.Id.Equals(sample.Id)).FirstOrDefault().GNNewSampleBatch;
                    

                    System.Console.WriteLine("***\n ----> SAMPLE FOUND: " + sample.Name);
                    GNCloudFileCategory fileCategory = db.GNCloudFileCategories.Where(a => a.Id == 1).FirstOrDefault();

                    string description = qcReportSqsMessage.fileName;
                    string volume = "dev-gn-s3-01";
                    string folderPath = qcReportSqsMessage.fileLocation.Substring(qcReportSqsMessage.fileLocation.IndexOf("fastq/"));
                    string fileURL = "https://dev-gn-s3-01.s3.amazonaws.com/" + folderPath + description;

                    System.Console.WriteLine("***\n ----> fileURL: " + fileURL);

                    //check first if file already exists
                    int cloudFileExists = db.GNCloudFiles.Where(a => a.FileURL.Equals(fileURL)).Count();

                    System.Console.WriteLine("\n\n\n ----> FILE EXISTS?: " + cloudFileExists + " - " + sample.Id + " - " + fileURL);

                    //if not, insert
                    if (cloudFileExists == 0)
                    {
                        GNCloudFile cloudFile = new GNCloudFile
                        {
                            Id = Guid.NewGuid(),
                            GNCloudFileCategoryId = fileCategory.Id, //FASTQ
                            CloudFileCategory = fileCategory,
                            FileName = folderPath + description,
                            FileURL = fileURL,
                            FolderPath = folderPath,
                            Volume = volume,
                            Description = description,
                            FileSize = Int64.Parse(qcReportSqsMessage.fileSize) * 1024,
                            AWSRegionSystemName = db.AWSRegions.FirstOrDefault().AWSRegionSystemName,
                            CreateDateTime = DateTime.Now,
                            CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                            SampleId = sample.Id.ToString(),
                            QcStatsAvailable = (qcReportSqsMessage.qcResult.Equals("true")),
                            QcStatsReportLocation = qcReportSqsMessage.qcReportLocation
                        };
                        db.GNCloudFiles.Add(cloudFile);
                        sample.CloudFiles.Add(cloudFile);
                        System.Console.WriteLine("***\n ----> FILE ADDED TO SAMPLE: " + cloudFile.Id);
                        /*
                        Batch.TotalNumberOfFastqFilesCompleted = Batch.TotalNumberOfFastqFilesCompleted + 1;

                        if (Batch.TotalNumberOfFastqFiles == Batch.TotalNumberOfFastqFilesCompleted)
                        {
                            SampleBatchRequestService batchService = new SampleBatchRequestService();
                            batchService.QualityControl(Batch);
                        }*/
                    }
                }
                else
                {
                    //Update VCF-QC for an existing VCF

                    System.Console.WriteLine("***\n ****** UPDATING ANALYSIS : " + qcReportSqsMessage.entityID);
                    System.Console.WriteLine("***\n ******  " + qcReportSqsMessage.entityType);

                    GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Find(entityId);
                    System.Console.WriteLine("***\n ******  CreateDateTime" + analysisRequest.CreateDateTime);

                    GNCloudFile vcfFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(qcReportSqsMessage.fileName)).FirstOrDefault();
                    System.Console.WriteLine("***\n ******  File found " + vcfFile.FileURL);
                    vcfFile.QcStatsReportLocation = qcReportSqsMessage.qcReportLocation;
                    vcfFile.QcStatsAvailable = true;
                    System.Console.WriteLine("***\n ----> Analysis Request: " + analysisRequest.Description);
                    System.Console.WriteLine("***\n ----> VCF File Found: " + vcfFile.Description);
                    
                }
                
                db.SaveChanges();
                success = true;
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to process QCReport Message.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }

    }



    public class StartQcReportService : GNCloudMessageService<StartVcfStatsReport>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_VCFQC";

        public StartQcReportService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public StartQcReportService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }


        public bool NotifyQCSystem(GNAnalysisRequest analysisRequest, string bucket, string key)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            string fileLocation = bucket + "/" + key;
            string vcfPaths = "s3://" + bucket + "/" + key;
            string fileName = key.Substring(key.LastIndexOf("/") + 1);
            fileLocation = fileLocation.Substring(0, (fileLocation.LastIndexOf("/")));

            try
            {
                StartVcfStatsReport startStatsReport = new StartVcfStatsReport
                {
                    entityId = analysisRequest.Id.ToString(),
                    entityType = "ANALYSIS",
                    fileLocation = fileLocation,
                    filename = fileName,
                    vcfPaths = vcfPaths
                };

                string filename = analysisRequest.Id.ToString() + ".txt";
                //string filename = "gn_sample_vcfqc.json";
                //System.Threading.Thread.Sleep(5000);

                System.Console.WriteLine("***\n ******  Sending message " + analysisRequest.CreateDateTime);
                this.SendMessage(startStatsReport);
                this.StoreMessage(startStatsReport, "telma.gn.com", filename);
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
