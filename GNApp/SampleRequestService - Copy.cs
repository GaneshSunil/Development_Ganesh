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
    public class SampleRequestService : GNCloudMessageService<NewSampleResponse>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_REQUEST";

        public SampleRequestService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SampleRequestService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(NewSampleResponse newSampleMessage, object queueMessage)
        {
            System.Console.WriteLine("***\n ****** Sample Name: " + newSampleMessage.name);
            System.Console.WriteLine("***\n ****** Sample type: " + newSampleMessage.type);
            System.Console.WriteLine("***\n ****** Sample repositoryId: " + newSampleMessage.repositoryId);

            bool success = false;
            try
            {
                GNSampleType type = db.GNSampleTypes.Where(a => a.Name.Equals(newSampleMessage.type.ToUpper())).FirstOrDefault();

                System.Console.WriteLine("***\n  type " + type.Name);

                GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessage.repositoryId)).FirstOrDefault();

                System.Console.WriteLine("***\n  Organization " + Organization.Name);
                GNSample sampleExist = db.GNSamples.Where(a => a.Name.Equals(newSampleMessage.name) && a.GNOrganizationId.Equals(Organization.Id)).FirstOrDefault();

                bool createNewSample = false;
                if(sampleExist != null)
                {
                    if(sampleExist.IsReady)
                    {
                        //the sample is ready, let's create a new one with a suffix of (1)
                        newSampleMessage.name = newSampleMessage.name + " (1)";
                        createNewSample = true;
                    }
                    else
                    {
                        //the sample is in the middle of being created, so ignore the message
                        createNewSample = false;
                    }
                }
                else
                {
                    createNewSample = true;
                }

                try
                {
                    if (createNewSample)
                    {
                        GNSample newSample = new GNSample
                        {
                            Id = Guid.NewGuid(),
                            GNOrganizationId = Organization.Id,
                            Name = newSampleMessage.name,
                            Gender = newSampleMessage.gender,
                            GNSampleTypeId = type.Id,
                            GNSampleQualifierCode = newSampleMessage.qualifier,
                            IsReady = false,
                            IsPairEnded = (newSampleMessage.read == "paired-end"),
                            CreateDateTime = DateTime.Now,
                            CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784")
                        };

                        db.GNSamples.Add(newSample);

                        db.SaveChanges();

                        System.Console.WriteLine("***\n  Sample Name: " + newSampleMessage.name);

                        //NOTIFY USER
                        bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "SAMPLE_CREATION",
                                    "telma.frege@genomenext.com",
                                    "Sample Creation",
                                    new Dictionary<string, string>
                                    {
                                        {"SampleName", newSample.Name},
                                        {"SampleId", newSample.Id.ToString()},
                                        {"CreatorName", Organization.OrgMainContact.FullName},
                                        {"CreateDateTime",DateTime.Now.ToString()}
                                    });
                    }
                    
                }
                catch (Exception e1)
                {

                    System.Console.WriteLine("***\n  Exception: " + e1.Message + e1.InnerException + e1.StackTrace);
                }
                

                success = true;
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to Create New Sample.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }

    }



    public class SampleRequestStatusService : GNCloudMessageService<NewSampleStatus>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_STATUS";

        public SampleRequestStatusService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SampleRequestStatusService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }
        
        public override bool ProcessMessage(NewSampleStatus newSampleMessageStatus, object queueMessage)
        {
            System.Console.WriteLine("***\n newSampleMessageStatus Sample Name: " + newSampleMessageStatus.name);

            if(newSampleMessageStatus.batchId.Equals("NO_SAMPLE_FOUND"))
            {
                //processSpecialCases();
            }

            bool success = false;
            try
            {
                GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessageStatus.repositoryId)).FirstOrDefault();
                GNSample sample = db.GNSamples.Where(a => a.Name.Equals(newSampleMessageStatus.name.ToUpper()) && a.GNOrganizationId.Equals(Organization.Id)).FirstOrDefault();

                bool isError = false;
                if(newSampleMessageStatus.isError.Equals("true"))
                {
                    isError = true;
                }

                var tx = db.Database.BeginTransaction();

                string insertSQL =
                    "INSERT INTO [gn].[GNSampleStatus]" +
                    "([Id],[GNSampleId],[SampleName],[Repository],[Message],[PercentComplete],[IsError],[CreatedBy],[CreateDateTime])" +
                    "VALUES " +
                    "(@Id, @GNSampleId, @SampleName, @Repository, @Message, @PercentComplete, @IsError, @CreatedBy, @CreateDateTime)";
                
                db.Database.ExecuteSqlCommand(
                    insertSQL,
                    new SqlParameter("@Id", Guid.NewGuid()),
                    new SqlParameter("@GNSampleId", sample.Id),
                    new SqlParameter("@SampleName", newSampleMessageStatus.name),
                    new SqlParameter("@Repository", newSampleMessageStatus.repositoryId),
                    new SqlParameter("@Message", newSampleMessageStatus.message),
                    new SqlParameter("@PercentComplete", newSampleMessageStatus.percentComplete),
                    new SqlParameter("@IsError", isError),
                    new SqlParameter("@CreatedBy", Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784")),
                    new SqlParameter("@CreateDateTime", DateTime.Now));

                LogUtil.Warn(logger, insertSQL);
                tx.Commit();
                
                success = true;

                if(newSampleMessageStatus.filesBucket != null && newSampleMessageStatus.filesBucket != "")
                {
                    //update files in Sample
                }

                if(isError)
                {
                    //NOTIFY USER
                    bool notifySuccess =
                            new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                "SAMPLE_STATUS_UPDATE_ERROR",
                                "telma.frege@genomenext.com",
                                "Sample Status Update",
                                new Dictionary<string, string>
                                        {
                                            {"SampleName", sample.Name},
                                            {"SampleId", sample.Id.ToString()},
                                            {"CreatorName", Organization.OrgMainContact.FullName},
                                            {"ErrorMessage", newSampleMessageStatus.message},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });
                }

                if (newSampleMessageStatus.percentComplete == 100 || newSampleMessageStatus.percentComplete.Equals("100"))
                {
                    sample.IsReady = true;

                    db.SaveChanges();

                    try
                    {
                        //NOTIFY USER
                        bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "SAMPLE_STATUS_UPDATE_COMPLETE",
                                    "telma.frege@genomenext.com",
                                    "Sample Status Update",
                                    new Dictionary<string, string>
                                        {
                                            {"SampleName", sample.Name},
                                            {"SampleId", sample.Id.ToString()},
                                            {"CreatorName", Organization.OrgMainContact.FullName},
                                            {"ErrorMessage", newSampleMessageStatus.message},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });
                    }
                    catch (Exception e2)
                    {
                        System.Console.WriteLine("***Message " + e2.Message + e2.StackTrace + " **********************************");

                    }
                    
                }
                
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to Update Status of New Sample.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }

    }




    public class SampleBatchRequestService : GNCloudMessageService<NewSampleBatch>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_REQUEST";

        public SampleBatchRequestService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SampleBatchRequestService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(NewSampleBatch newSampleMessage, object queueMessage)
        {
            System.Console.WriteLine("***\n ****** Batch Id: " + newSampleMessage.batchId);
            System.Console.WriteLine("***\n ****** Sample type: " + newSampleMessage.type);
            System.Console.WriteLine("***\n ****** Sample repositoryId: " + newSampleMessage.repositoryId);

            SampleResponseService sampleResponseService = new SampleResponseService();

            GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessage.repositoryId)).FirstOrDefault();

            //Find sequencer job
            GNSequencerJob sequencerJob = db.GNSequencerJobs.Where(a => a.GNOrganizationId.Equals(Organization.Id) && a.Project.Equals(newSampleMessage.project)).FirstOrDefault();
            sequencerJob.Status = "PROCESSING SAMPLES";

            GNNewSampleBatch newSampleBatch = new GNNewSampleBatch { 
                                                         Id = Guid.NewGuid(),
                                                         BatchId = newSampleMessage.batchId,
                                                         GNSequencerJobId = sequencerJob.Id,
                                                         GNSequencerJob = sequencerJob,
                                                         Project = newSampleMessage.project,
                                                         AutoStartAnalysis = (newSampleMessage.autoStartAnalysis.Equals("true")),
                                                         CreateAnalysisPerSample = (newSampleMessage.createAnalysisPerSample.Equals("true")),
                                                         Qualifier = newSampleMessage.qualifier,
                                                         Type = newSampleMessage.type,
                                                         RepositoryId = newSampleMessage.repositoryId,
                                                         Gender = newSampleMessage.gender,
                                                         ReadType = newSampleMessage.read,
                                                         TotalSamples = newSampleMessage.samples.Count(),
                                                         TotalSamplesCompleted = 0,
                                                         CreateDateTime = DateTime.Now
                                                    };

            db.GNNewSampleBatches.Add(newSampleBatch);

            db.SaveChanges();

            bool success = false;
            try
            {
                GNSampleType type = db.GNSampleTypes.Where(a => a.Name.Equals(newSampleMessage.type.ToUpper())).FirstOrDefault();

                System.Console.WriteLine("***\n  type " + type.Name);

                System.Console.WriteLine("***\n  Organization " + Organization.Name);

                GNTeam newTeam = null;
                GNProject newProject = null;
                GNAnalysisType analysisType = null;
                String AnalysisCode = newSampleMessage.qualifier;
                if (newSampleBatch.CreateAnalysisPerSample)
                {
                    /**
                     * 1. Create a Team
                     * 2. Create a Project
                     */
                    GNContact contact = db.GNContacts.Find(Organization.GNContactId);

                    newTeam = new GNTeam
                    {
                        Id = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                        CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                        Name = "TeamBatch" + newSampleBatch.BatchId.Substring(0, 15),
                        GNContactId = contact.Id,
                        Organization = Organization,
                        OrganizationId = Organization.Id,
                        TeamLead = contact
                    };
                    db.GNTeams.Add(newTeam);
                    db.SaveChanges();
                    System.Console.WriteLine("***\n  New newTeam Created: " + newTeam.Id);

                    newProject = new GNProject
                    {
                        Id = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                        CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                        ProjectLead = contact,
                        ProjectLeadId = contact.Id.ToString(),
                        Name = "ProjectBatch-" + newSampleBatch.BatchId.Substring(0, 15),
                        TeamId = newTeam.Id.ToString(),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(30),
                        Description = "Created automatically from the Sample Batch Process"
                    };

                    System.Console.WriteLine("***\n  New newProject Created: " + newProject.Id);

                    newTeam.Projects.Add(newProject);
                    newProject.Teams.Add(newTeam);
                    db.GNProjects.Add(newProject);

                    db.SaveChanges();

                    if (newSampleMessage.qualifier.Equals("TUMOR"))
                    {
                        AnalysisCode = "TUMORNORMAL";
                    }
                    analysisType = db.GNAnalysisTypes.Where(a => a.Name.Equals(newSampleMessage.type.ToUpper())).FirstOrDefault();
                }

                foreach (String sampleName in newSampleMessage.samples)
                {
                    String newSampleName = sampleName;
                    GNSample sampleExist = db.GNSamples.Where(a => a.Name.Equals(sampleName) && a.GNOrganizationId.Equals(Organization.Id)).FirstOrDefault();
                    bool createNewSample = false;
                    if (sampleExist != null)
                    {
                        if (sampleExist.IsReady)
                        {
                            //the sample is ready, let's create a new one with a suffix of (1)
                            newSampleName = sampleName + " (1)";
                            createNewSample = true;
                        }
                        else
                        {
                            //the sample is in the middle of being created, so ignore the message
                            createNewSample = false;
                        }
                    }
                    else
                    {
                        createNewSample = true;
                    }

                    try
                    {
                        if (createNewSample)
                        {
                            
                            GNSample newSample = new GNSample
                            {
                                Id = Guid.NewGuid(),
                                GNOrganizationId = Organization.Id,
                                Name = newSampleName,
                                Gender = newSampleMessage.gender,
                                GNSampleTypeId = type.Id,
                                GNSampleQualifierCode = newSampleMessage.qualifier,
                                IsReady = false,
                                IsPairEnded = (newSampleMessage.read == "paired-end"),
                                CreateDateTime = DateTime.Now,
                                CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784")
                            };

                            db.GNSamples.Add(newSample);
                            
                            db.SaveChanges();
                            System.Console.WriteLine("***\n  New Sample Created: " + newSample.Id);

                            GNNewSampleBatchSamples batchSample = new GNNewSampleBatchSamples { 
                                                                    Id = Guid.NewGuid(),
                                                                    GNSample = newSample,                                                                    
                                                                    CreateDateTime = DateTime.Now,
                                                                    GNNewSampleBatch = newSampleBatch,
                                                                    GNNewSampleBatchId = newSampleBatch.Id,
                                                                    Name = sampleName
                                                           };
                            db.GNNewSampleBatchSamples.Add(batchSample);

                            db.SaveChanges();
                            System.Console.WriteLine("***\n  New batchSample Created: " + batchSample.Id);

                            GNNewSampleBatchStatus batchStatus = new GNNewSampleBatchStatus {
                                                                    Id = Guid.NewGuid(),
                                                                    CreateDateTime = DateTime.Now,
                                                                    GNNewSampleBatch = newSampleBatch,
                                                                    GNNewSampleBatchId = newSampleBatch.Id,
                                                                    IsError = false,
                                                                    PercentComplete = 0,
                                                                    Status = "STARTING PROCESS",
                                                                    RepositoryId = newSampleMessage.repositoryId
                                                            };
                            db.GNNewSampleBatchStatus.Add(batchStatus);
                            db.SaveChanges();
                            System.Console.WriteLine("***\n  New batchStatus Created: " + batchStatus.Id);


                            if(newSampleBatch.CreateAnalysisPerSample)
                            {
                                GNAnalysisRequest newAnalysis = new GNAnalysisRequest {
                                                                Id = Guid.NewGuid(),
                                                                Project = newProject,
                                                                GNProjectId = newProject.Id,
                                                                CreateDateTime = DateTime.Now,
                                                                CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                                                                AnalysisType = analysisType,
                                                                RequestProgress = 0, RequestDateTime = DateTime.Now,
                                                                GNAnalysisRequestTypeCode = AnalysisCode,
                                                                Description = newSampleName,
                                                                GNAnalysisAdaptorCode = "NONE",
                                                                AnalysisTypeId = analysisType.Id.ToString(),
                                                                AutoStart = newSampleBatch.AutoStartAnalysis,
                                                                AWSRegionSystemName = db.AWSRegions.FirstOrDefault().AWSRegionSystemName
                                                            };
                                db.GNAnalysisRequests.Add(newAnalysis);

                                db.SaveChanges();
                                System.Console.WriteLine("***\n  New newAnalysis Created: " + newAnalysis.Id);


                                GNAnalysisRequestGNSample newAnalysisSample = new GNAnalysisRequestGNSample { 
                                                                 AffectedIndicator = "N",
                                                                 TargetIndicator = "N",
                                                                 GNAnalysisRequest = newAnalysis,
                                                                 GNSample = newSample,
                                                                 GNAnalysisRequestId = newAnalysis.Id,
                                                                 GNSampleId = newSample.Id
                                                            };
                                db.GNAnalysisRequestGNSamples.Add(newAnalysisSample);
                            } //end of "if(newSampleBatch.CreateAnalysisPerSample)"

                            System.Console.WriteLine("***\n  Sample Name: " + sampleName);

                            //Notify BCL2FASTQ service
                            sampleResponseService.NotifyBcl2FastqSystem(newSample, newSampleBatch.RepositoryId);

                        }

                        db.SaveChanges();

                    }
                    catch (Exception e1)
                    {

                        System.Console.WriteLine("***\n  Exception: " + e1.Message + e1.InnerException);
                    }
                }

                System.Console.WriteLine("***\n  EVERYTHING WORKED!");

                //NOTIFY USER
                bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            "SAMPLE_CREATION",
                            "telma.frege@genomenext.com",
                            "Sample Creation",
                            new Dictionary<string, string>
                                    {
                                        {"BatchId", newSampleMessage.batchId},     
                                        {"TotalSamples", newSampleBatch.TotalSamples.ToString()},                                       
                                        {"CreatorName", Organization.OrgMainContact.FullName},
                                        {"CreateDateTime",DateTime.Now.ToString()}
                                    });
                success = true;
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to Create New Sample.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }




    }



    public class SampleResponseService : GNCloudMessageService<NewSampleResponse>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_RESPONSE";

        public SampleResponseService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SampleResponseService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }


        public bool NotifyBcl2FastqSystem(GNSample sample, string s3Path)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            try
            {
                NewSampleResponse sampleResponse = new NewSampleResponse
                {
                    id = sample.Id.ToString(),
                    sample_name = sample.Name,
                    s3_path = s3Path
                };

                this.SendMessage(sampleResponse);
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to send notification to queue.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                return false;
            }

            return true;
        }

    }


    public class SampleRequestBatchStatusService : GNCloudMessageService<NewSampleBatchStatus>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_STATUS";

        public SampleRequestBatchStatusService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SampleRequestBatchStatusService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(NewSampleBatchStatus newSampleMessageStatus, object queueMessage)
        {
            System.Console.WriteLine("***\n newSampleMessageStatus Batch Name: " + newSampleMessageStatus.batchId);

            if (newSampleMessageStatus.batchId.Equals("NO_SAMPLE_FOUND"))
            {
                //processSpecialCases();
            }

            if(newSampleMessageStatus.status.Equals("QC-COMPLETED"))
            {
               // ProcessQCMessage(newSampleMessageStatus);
                return true;
            }

            bool success = false;
            try
            {
                GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessageStatus.repositoryId)).FirstOrDefault();
                GNNewSampleBatch Batch = db.GNNewSampleBatches.Where(a => a.BatchId.Equals(newSampleMessageStatus.batchId)).FirstOrDefault();
                
                bool isError = false;
                if (newSampleMessageStatus.isError.Equals("true"))
                {
                    isError = true;
                }
                

                GNNewSampleBatchStatus batchStatus = new GNNewSampleBatchStatus
                {
                    Id = Guid.NewGuid(),
                    CreateDateTime = DateTime.Now,
                    GNNewSampleBatch = Batch,
                    GNNewSampleBatchId = Batch.Id,
                    IsError = isError,
                    PercentComplete = newSampleMessageStatus.percentComplete,
                    Status = newSampleMessageStatus.status,
                    RepositoryId = newSampleMessageStatus.repositoryId,
                    FilesBucket = newSampleMessageStatus.filesBucket,
                    Message = newSampleMessageStatus.message
                };
                db.GNNewSampleBatchStatus.Add(batchStatus);
                db.SaveChanges();


                /*
                var tx = db.Database.BeginTransaction();

                string insertSQL =
                    "INSERT INTO [gn].[GNSampleStatus]" +
                    "([Id],[GNSampleId],[SampleName],[Repository],[Message],[PercentComplete],[IsError],[CreatedBy],[CreateDateTime])" +
                    "VALUES " +
                    "(@Id, @GNSampleId, @SampleName, @Repository, @Message, @PercentComplete, @IsError, @CreatedBy, @CreateDateTime)";

                db.Database.ExecuteSqlCommand(
                    insertSQL,
                    new SqlParameter("@Id", Guid.NewGuid()),
                    new SqlParameter("@GNSampleId", sample.Id),
                    new SqlParameter("@SampleName", newSampleMessageStatus.name),
                    new SqlParameter("@Repository", newSampleMessageStatus.repositoryId),
                    new SqlParameter("@Message", newSampleMessageStatus.message),
                    new SqlParameter("@PercentComplete", newSampleMessageStatus.percentComplete),
                    new SqlParameter("@IsError", isError),
                    new SqlParameter("@CreatedBy", Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784")),
                    new SqlParameter("@CreateDateTime", DateTime.Now));

                LogUtil.Warn(logger, insertSQL);
                tx.Commit();
                */





                if (newSampleMessageStatus.filesBucket != null && newSampleMessageStatus.filesBucket != "")
                {
                    System.Console.WriteLine("&&&&& \n Will copy files &&&&&&& ");

                    try
                    {
                        
                        /*
                        IQueryable<GNSample> listOfSamples = db.GNSamples.Where(a => a.GNNewSampleBatchSample.GNNewSampleBatchId.Equals(Batch.Id));

                        GNCloudStorageService st = new GNCloudStorageService();
                        string originBucket = newSampleMessageStatus.filesBucket;
                        originBucket = "tfrege-test-20140711"; //testing purposes
                        Dictionary<String, long> listOfFiles = st.ListingObjects(originBucket);

                        string s3BucketName = st.FetchAWSS3Bucket().ARN;

                        /*
                        //string volume = newSampleMessageStatus.filesBucket.Substring(newSampleMessageStatus.filesBucket.IndexOf("/")+1, )
                        string folderPath1 = newSampleMessageStatus.filesBucket.Replace("http://", "");
                        folderPath1 = folderPath1.Replace("https://", "");
                        int idxSlash = folderPath1.IndexOf("/");
                        int strLen = folderPath1.Length;
                        
                        //string volume = folderPath1.Substring(0, idxSlash);
                        //string folderPath = folderPath1.Substring(idxSlash + 1, strLen - idxSlash - 1);
                        GNCloudFileCategory fileCategory = db.GNCloudFileCategories.Where(a => a.Id == 1).FirstOrDefault();


                        string volume = s3BucketName;

                        foreach (GNSample sample in listOfSamples)
                        {
                            Guid fileId = Guid.NewGuid();

                            //Create new folder in S3
                            st.PutObjectOnBucket(s3BucketName + "fastq/", sample.Id.ToString(), sample.Id.ToString());
                            st.PutObjectOnBucket(s3BucketName + "fastq/", sample.Id.ToString() + "/" + fileId.ToString(), sample.Id.ToString() + "/" + fileId.ToString());

                            string folderPath = "fastq/" + sample.Id.ToString() + "/" + fileId.ToString();
                            

                            var subList = listOfFiles.Where(a => a.Key.Contains(sample.Name));
                            foreach (var file in subList)
                            {
                                System.Console.WriteLine("&&&&& \n Will copy files &&&&&&& "+file.Key);

                                string fileURL = "https://dev-gn-s3-01.s3.amazonaws.com/" + folderPath + file.Key;

                                GNCloudFile newFile = new GNCloudFile
                                {
                                    Id = fileId,
                                    GNCloudFileCategoryId = fileCategory.Id, //FASTQ
                                    CloudFileCategory = fileCategory,
                                    FileName = folderPath + file.Key,
                                    FileURL = fileURL,
                                    FolderPath = folderPath,
                                    Volume = volume,
                                    Description = file.Key,
                                    FileSize = file.Value,
                                    AWSRegionSystemName = db.AWSRegions.FirstOrDefault().AWSRegionSystemName,
                                    CreateDateTime = DateTime.Now,
                                    CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                                    SampleId = sample.Id.ToString()
                                };
                                db.GNCloudFiles.Add(newFile);

                             //   st.CopyS3Object(originBucket, fileURL, file.Key);
                            }
                            db.SaveChanges()
                         
                        }*/

                    }
                    catch (Exception filesUpdateExc)
                    {

                        System.Console.WriteLine("***\n Unable to Read the Files from S3: " + newSampleMessageStatus.batchId + "\n" + filesUpdateExc.Message + filesUpdateExc.InnerException + filesUpdateExc.StackTrace);

                        Exception e2 = new Exception("Unable to Read the Files from S3 for this batch New Sample: ", filesUpdateExc);
                        LogUtil.Warn(logger, filesUpdateExc.Message, filesUpdateExc);
                        success = false;
                    }

                    success = true;
                }
                ////////////////////////////END OF READING FILES FROM S3 AND UPDATING RDS //////////////







                if (isError)
                {
                    Batch.GNSequencerJob.Status = "ERROR";
                    db.SaveChanges();

                    //NOTIFY USER
                    bool notifySuccess =
                            new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                "SAMPLE_STATUS_UPDATE_ERROR",
                                "telma.frege@genomenext.com",
                                "Sample Status Update",
                                new Dictionary<string, string>
                                        {
                                            {"BatchId", newSampleMessageStatus.batchId},
                                            {"CreatorName", Organization.OrgMainContact.FullName},
                                            {"ErrorMessage", newSampleMessageStatus.message},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });
                }

                if (newSampleMessageStatus.percentComplete == 100 || newSampleMessageStatus.percentComplete.Equals("100"))
                {
                    Batch.GNSequencerJob.Status = "COMPLETED";
                    db.SaveChanges();


                    if(Batch.AutoStartAnalysis)
                    {
                        //loop through all the samples, look for their analyses, and start them.
                        List<GNNewSampleBatchSamples> ListOfSamples = Batch.GNNewSampleBatchSamples.ToList();

                        foreach (GNNewSampleBatchSamples batchSample in ListOfSamples)
	                    {
		                    GNSample sample = batchSample.GNSample;
                            var result = this.AutostartAnalysis(sample);
	                    }
                    }


                    try
                    {
                        //NOTIFY USER
                        bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "SAMPLE_STATUS_UPDATE_COMPLETE",
                                    "telma.frege@genomenext.com",
                                    "Sample Status Update",
                                    new Dictionary<string, string>
                                        {
                                            {"BatchId", Batch.Id.ToString()},
                                            {"TotalSamples", Batch.TotalSamples.ToString()},
                                            {"CreatorName", Organization.OrgMainContact.FullName},
                                            {"ErrorMessage", newSampleMessageStatus.message},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });
                    }
                    catch (Exception e2)
                    {
                        System.Console.WriteLine("***Message " + e2.Message + e2.StackTrace + " **********************************");

                    }

                }

            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to Update Status of New Sample.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }


        public async Task<bool> AutostartAnalysis(GNSample sample)
        {
            GNAnalysisRequest analysis = sample.GNAnalysisRequestGNSamples.FirstOrDefault().GNAnalysisRequest;
            AnalysisRequestService analysisService = new AnalysisRequestService(db);
            GNContact userContact = db.GNContacts.Find(Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"));
            await analysisService.StartAnalysis(userContact, analysis.Id);
            return true;
        }


        public async Task<bool> ProcessQCMessage(NewSampleBatchStatus newSampleMessageStatus)
        {
            System.Console.WriteLine("QCQCQC " + newSampleMessageStatus.name);

            try
            {
                GNCloudFileCategory fileCategory = db.GNCloudFileCategories.Where(a => a.Id == 1).FirstOrDefault();

                string sampleName1 = newSampleMessageStatus.name;
                string sampleName = sampleName1.Substring(0, sampleName1.IndexOf("_"));
                //s3://sanford-fastq/151204_SN7001383_0164_AC7Y91ACXX/FASTQ/fastqc/AAA076960/AAA076960_S6_R2_001/

                GNSample sample = db.GNSamples.Where(a => a.Name.Equals(sampleName) && a.IsReady == false).FirstOrDefault();

                GNCloudStorageService st = new GNCloudStorageService();                
                string volume = "dev-gn-s3-01";

                Guid fileId = Guid.NewGuid();
                string folderPath = "fastq/" + sample.Id + "/" + fileId + "/";
                string fastqFileName = folderPath + newSampleMessageStatus.name + ".fastq.gz";
                string fileURL = "https://dev-gn-s3-01.s3.amazonaws.com/" + fastqFileName;

                string originBucket = newSampleMessageStatus.filesBucket;
                originBucket = originBucket.Replace("s3://", "");
                originBucket = originBucket.Substring(0, originBucket.IndexOf("/")); //"sanford-fastq";

                string QcOriginKey = newSampleMessageStatus.filesBucket;
                QcOriginKey = QcOriginKey.Replace("s3://", "");
                QcOriginKey = QcOriginKey.Substring(originBucket.Length + 1);

                string FileDescription = newSampleMessageStatus.name + ".fastq.gz";
                string fastqOriginKey = QcOriginKey.Substring(0, QcOriginKey.IndexOf("FASTQ/") + 5);
                fastqOriginKey = fastqOriginKey + "/" + FileDescription;

                QcOriginKey = QcOriginKey + "fastqc_report.html";


                string destinationBucket = "dev-gn-s3-01/" + folderPath;
                destinationBucket = "dev-gn-s3-01";
                string QcFileName = folderPath + "fastqc_report.html";
                string QcDestinationURL = "https://dev-gn-s3-01.s3.amazonaws.com/" + QcFileName;

                st.CopyS3Object(originBucket, fastqOriginKey, destinationBucket, fastqFileName);
                st.CopyS3Object(originBucket, QcOriginKey, destinationBucket, QcFileName);


                GNCloudFile cloudFile = new GNCloudFile
                {
                    Id = fileId,
                    GNCloudFileCategoryId = fileCategory.Id, //FASTQ
                    CloudFileCategory = fileCategory,
                    FileName = folderPath + fastqFileName,
                    FileURL = fileURL,
                    FolderPath = folderPath,
                    Volume = volume,
                    Description = fastqFileName,
                    FileSize = st.GetObjectSize(originBucket, fastqOriginKey),
                    AWSRegionSystemName = db.AWSRegions.FirstOrDefault().AWSRegionSystemName,
                    CreateDateTime = DateTime.Now,
                    CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"),
                    SampleId = sample.Id.ToString(),
                    PassedQC = (newSampleMessageStatus.isError.Equals("false")),
                    QcReportLocation = QcDestinationURL
                };
                db.GNCloudFiles.Add(cloudFile);
                sample.CloudFiles.Add(cloudFile);
                
                if(sample.CloudFiles.Count() == 2 && sample.CloudFiles.Where(a => a.PassedQC == false).Count() == 0)
                {
                    sample.IsReady = true;

                    if(sample.GNNewSampleBatchSample.GNNewSampleBatch.AutoStartAnalysis)
                    {
                        GNAnalysisRequest analysis = sample.GNAnalysisRequestGNSamples.FirstOrDefault().GNAnalysisRequest;
                        AnalysisRequestService analysisService = new AnalysisRequestService(db);
                        GNContact userContact = db.GNContacts.Find(Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"));
                        await analysisService.StartAnalysis(userContact, analysis.Id);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {                
                System.Console.WriteLine("***Unable to Update QC-COMPLETED status. " + e.Message + e.StackTrace + " **********************************");
                Exception e2 = new Exception("Unable to Update QC-COMPLETED status.", e);
                LogUtil.Warn(logger, e2.Message, e2);
                return false;
            }
            

            return true;
        }
    }


}
