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

    public class SampleBatchRequestService : GNCloudMessageService<NewSampleBatch>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SAMPLE_REQUEST";
        private Guid CreatedBy = Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784");

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
            System.Console.WriteLine("***\n ****** Sample repository: " + newSampleMessage.repository);

            SampleResponseService sampleResponseService = new SampleResponseService();

            List<GNSample> listOfSamples = new List<GNSample>();
            List<GNNewSampleBatchSamples> listOfBatchSamples = new List<GNNewSampleBatchSamples>();
            List<String> listOfAnalysisNames = new List<String>();

            GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessage.repository)).FirstOrDefault();
            System.Console.WriteLine("***\n  Organization " + Organization.Name);
            System.Console.WriteLine("***\n  Organization Id " + Organization.Id);
            System.Console.WriteLine("***\n  Project " + newSampleMessage.project);


            bool success = false;
            
            try
            {

                System.Console.WriteLine("***\n  Searching sequencerJob ");

                //Find sequencer job
                GNSequencerJob sequencerJob = db.GNSequencerJobs.Where(a => a.GNOrganizationId.Equals(Organization.Id) && a.Project.Equals(newSampleMessage.project)).FirstOrDefault();
                if(sequencerJob == null)
                {
                    throw new Exception("Unable to find Sequencer Job!");
                }

                sequencerJob.Status = "PROCESSING SAMPLES";

                System.Console.WriteLine("***\n  Starting.  sequencerJob project: " + sequencerJob.Project);

                GNProject newProject = sequencerJob.GNProject;
                GNTeam newTeam = newProject.Teams.FirstOrDefault();

                GNNewSampleBatch newSampleBatch = new GNNewSampleBatch
                {
                    Id = Guid.NewGuid(),
                    BatchId = newSampleMessage.batchId,
                    GNSequencerJobId = sequencerJob.Id,
                    GNSequencerJob = sequencerJob,
                    Project = newSampleMessage.project,
                    AutoStartAnalysis = (newSampleMessage.autoStartAnalysis.ToLower().Equals("true")),
                    CreateAnalysisPerSample = (newSampleMessage.createAnalysisPerSample.ToLower().Equals("true")),
                    Qualifier = newSampleMessage.qualifier,
                    Type = newSampleMessage.type,
                    Repository = newSampleMessage.repository,
                    ReadType = newSampleMessage.read,
                    TotalSamples = newSampleMessage.samples.Count(),
                    TotalSamplesCompleted = 0,
                    TotalNumberOfFastqFiles = 0,
                    CreateDateTime = DateTime.Now
                };

                db.GNNewSampleBatches.Add(newSampleBatch);
                
                GNSampleType type = db.GNSampleTypes.Where(a => a.Name.Equals(newSampleMessage.type.ToUpper())).FirstOrDefault();
                System.Console.WriteLine("***\n  type " + type.Name);

                string datetimeString = DateTime.Now.ToString("MM-dd 0:HH:mm:ss");

                GNAnalysisType analysisType = null;
                String AnalysisCode = newSampleMessage.qualifier;
                if (newSampleBatch.CreateAnalysisPerSample)
                {
                    /**
                     * 1. Create a Team
                     * 2. Create a Project
                     */
                    GNContact contact = db.GNContacts.Find(Organization.GNContactId);
                    
                    if (newSampleMessage.qualifier.Equals("TUMOR"))
                    {
                        AnalysisCode = "TUMORNORMAL";
                    }
                    analysisType = db.GNAnalysisTypes.Where(a => a.Name.Equals(newSampleMessage.type.ToUpper())).FirstOrDefault();
                }

                int i = 0;
                GNAnalysisRequest newAnalysis = null;
                /**
                 * Loop samples
                 */

                GNNewSampleBatchStatus batchStatus = new GNNewSampleBatchStatus
                {
                    Id = Guid.NewGuid(),
                    CreateDateTime = DateTime.Now,
                    GNNewSampleBatch = newSampleBatch,
                    GNNewSampleBatchId = newSampleBatch.Id,
                    IsError = false,
                    PercentComplete = 0,
                    Status = "STARTING PROCESS",
                    Repository = newSampleMessage.repository
                };
                db.GNNewSampleBatchStatus.Add(batchStatus);
                System.Console.WriteLine("***\n  New batchStatus Created: " + batchStatus.Id);

                foreach (SampleBatch sampleBatch in newSampleMessage.samples)
                {
                    String newSampleName = sampleBatch.name;
                    //tfrege 2016.12.07 do not append family id to sample name anymore
                    /*
                    if(sampleBatch.familyId != "")
                    {
                        newSampleName = newSampleName + " (" + sampleBatch.familyId + ")";
                        System.Console.WriteLine("***\n ------------->  newSampleName: " + newSampleName);
                    }
                     */

                    GNSample sampleExist = db.GNSamples.Where(a => a.Name.Equals(sampleBatch.name) && a.GNOrganizationId.Equals(Organization.Id)).FirstOrDefault();
                    bool createNewSample = false;


                    //Since BCL2FASTQ is not passing gender for now, figure it out based on the family relation
                    String sampleBatchgender = "U";
                    switch(sampleBatch.familyRelation)
                    {
                        case "F":
                        case "S":
                            sampleBatchgender = "M";
                            break;
                            
                        case "M":
                        case "D":
                            sampleBatchgender = "F";
                            break;

                        default:
                            sampleBatchgender = "F";
                            break;
                    }

                    if (sampleExist != null)
                    {
                        if (sampleExist.IsReady)
                        {
                            //the sample is ready, let's create a new one with a suffix of (1)
                            newSampleName = sampleBatch.name + " (1)";
                            createNewSample = true;
                        }
                        else
                        {
                            //the sample is in the middle of being created, so ignore the message
                            createNewSample = false;
                            //tfrege remove this control just for testing purposes, create new sample always
                            newSampleName = sampleBatch.name + " (1)"; //remove this after validation is put back in
                            createNewSample = true;
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
                            GNReplicate replicate = db.GNReplicates.Where(a => a.Name.Equals("NO")).FirstOrDefault();

                            GNSample newSample = new GNSample
                            {
                                Id = Guid.NewGuid(),
                                GNOrganizationId = Organization.Id,
                                Name = newSampleName,
                                Gender = sampleBatchgender,
                                GNSampleTypeId = type.Id,
                                GNSampleQualifierCode = newSampleMessage.qualifier,
                                IsReady = false,
                                IsPairEnded = (newSampleMessage.read == "paired-end"),
                                GNReplicateCode = "0",
                                GNReplicate = replicate,
                                CreateDateTime = DateTime.Now,
                                CreatedBy = CreatedBy
                            };
                           
                            db.GNSamples.Add(newSample);
                            System.Console.WriteLine("***\n  New Sample Created: " + newSample.Id);


                            string analysisName = sampleBatch.name;
                            if(sampleBatch.familyId != null)
                            {
                                analysisName = sampleBatch.familyId;
                            }

                            GNNewSampleBatchSamples batchSample = new GNNewSampleBatchSamples { 
                                                                    Id = Guid.NewGuid(),
                                                                    GNSample = newSample,                                                                    
                                                                    CreateDateTime = DateTime.Now,
                                                                    GNNewSampleBatch = newSampleBatch,
                                                                    GNNewSampleBatchId = newSampleBatch.Id,
                                                                    Name = sampleBatch.name, 
                                                                    Affected = sampleBatch.affected,
                                                                    Proband = sampleBatch.proband,
                                                                    FamilyId = sampleBatch.familyId,
                                                                    RelationId = sampleBatch.familyRelation,
                                                                    Gender = sampleBatchgender, 
                                                                    AnalysisName = analysisName
                                                           };
                            db.GNNewSampleBatchSamples.Add(batchSample);
                            System.Console.WriteLine("***\n  New batchSample Created: " + batchSample.Id);
                            System.Console.WriteLine("***\n  ====>>>>> Adding to listOfAnalysisNames: " + analysisName);

                            if (!listOfAnalysisNames.Contains(analysisName))
                            {
                                listOfAnalysisNames.Add(analysisName);
                            }

                            db.SaveChanges();
                            System.Console.WriteLine("***\n  DB CHANGES SAVED.");
                            
                            System.Console.WriteLine("***\n  Sample Name: " + sampleBatch.name);

                            //Build list to notify the BCL2FASTQ service later
                            listOfSamples.Add(newSample);
                            listOfBatchSamples.Add(batchSample);
                        }
                    }
                    catch (Exception e1)
                    {
                        System.Console.WriteLine("***\n  Exception: " + e1.Message + e1.StackTrace + e1.InnerException);
                    }
                    i++;
                }
                
                /**
                 * Update pedigrees, if applies
                 */
                int maxPedigreeId = db.GNSampleRelationships.Max(a => a.Id);
                System.Console.WriteLine("***\n  Updating pedigrees for families ");

                foreach (GNNewSampleBatchSamples pedigreeSample in listOfBatchSamples.Where(a => a.Gender != "U" && a.RelationId != ""))
                {
                    switch (pedigreeSample.RelationId)
                    {
                        case "F":
                        case "M":
                            {
                                GNSampleRelationshipType relationshipType = db.GNSampleRelationshipTypes.Where(a => a.Name.Equals("SON")).FirstOrDefault();

                                //find sons and daughters
                                List<GNNewSampleBatchSamples> sons = listOfBatchSamples.Where(a => a.FamilyId.Equals(pedigreeSample.FamilyId) && a.RelationId.Equals("S")).ToList();
                                foreach (GNNewSampleBatchSamples son in sons)
                                {
                                    GNSampleRelationship sampleRelationship = new GNSampleRelationship
                                    {
                                        Id = maxPedigreeId++,
                                        GNLeftSample = pedigreeSample.GNSample,
                                        GNLeftSampleId = pedigreeSample.GNSample.Id,
                                        GNRightSample = son.GNSample,
                                        GNRightSampleId = son.GNSample.Id,
                                        GNSampleRelationshipType = relationshipType,
                                        GNSampleRelationshipTypeId = relationshipType.Id,
                                        CreateDateTime = DateTime.Now,
                                        CreatedBy = CreatedBy
                                    };
                                    db.GNSampleRelationships.Add(sampleRelationship);
                                    System.Console.WriteLine("***\n  Added Son " + son.GNSample.Id + " to Parent Sample : " + pedigreeSample.GNSample.Id);
                                }

                                relationshipType = db.GNSampleRelationshipTypes.Where(a => a.Name.Equals("DAUGHTER")).FirstOrDefault();

                                //find sons and daughters
                                List<GNNewSampleBatchSamples> daughters = listOfBatchSamples.Where(a => a.FamilyId.Equals(pedigreeSample.FamilyId) && a.RelationId.Equals("D")).ToList();
                                foreach (GNNewSampleBatchSamples daughter in daughters)
                                {
                                    GNSampleRelationship sampleRelationship = new GNSampleRelationship
                                    {
                                        Id = maxPedigreeId++,
                                        GNLeftSample = pedigreeSample.GNSample,
                                        GNLeftSampleId = pedigreeSample.GNSample.Id,
                                        GNRightSample = daughter.GNSample,
                                        GNRightSampleId = daughter.GNSample.Id,
                                        GNSampleRelationshipType = relationshipType,
                                        GNSampleRelationshipTypeId = relationshipType.Id,
                                        CreateDateTime = DateTime.Now,
                                        CreatedBy = CreatedBy
                                    };
                                    db.GNSampleRelationships.Add(sampleRelationship);
                                    System.Console.WriteLine("***\n  Added Daughter " + daughter.GNSample.Id + " to Parent Sample : " + pedigreeSample.GNSample.Id);
                                }

                            } break;

                        case "S":
                        case "D":
                            {
                                GNSampleRelationshipType relationshipType = db.GNSampleRelationshipTypes.Where(a => a.Name.Equals("FATHER")).FirstOrDefault();

                                //find sons and daughters
                                List<GNNewSampleBatchSamples> fathers = listOfBatchSamples.Where(a => a.FamilyId.Equals(pedigreeSample.FamilyId) && a.RelationId.Equals("F")).ToList();
                                foreach (GNNewSampleBatchSamples dad in fathers)
                                {
                                    GNSampleRelationship sampleRelationship = new GNSampleRelationship
                                    {
                                        Id = maxPedigreeId++,
                                        GNLeftSample = pedigreeSample.GNSample,                                        
                                        GNLeftSampleId = pedigreeSample.GNSample.Id,
                                        GNRightSample = dad.GNSample,
                                        GNRightSampleId = dad.GNSample.Id,
                                        GNSampleRelationshipType = relationshipType,
                                        GNSampleRelationshipTypeId = relationshipType.Id,
                                        CreateDateTime = DateTime.Now,
                                        CreatedBy = CreatedBy
                                    };
                                    db.GNSampleRelationships.Add(sampleRelationship);
                                    System.Console.WriteLine("***\n  Added Dad " + dad.GNSample.Id + " to Child Sample : " + pedigreeSample.GNSample.Id);
                                }

                                relationshipType = db.GNSampleRelationshipTypes.Where(a => a.Name.Equals("MOTHER")).FirstOrDefault();

                                //find sons and daughters
                                List<GNNewSampleBatchSamples> mothers = listOfBatchSamples.Where(a => a.FamilyId.Equals(pedigreeSample.FamilyId) && a.RelationId.Equals("M")).ToList();
                                foreach (GNNewSampleBatchSamples mom in mothers)
                                {
                                    GNSampleRelationship sampleRelationship = new GNSampleRelationship
                                    {
                                        Id = maxPedigreeId++,
                                        GNLeftSample = pedigreeSample.GNSample,
                                        GNLeftSampleId = pedigreeSample.GNSample.Id,
                                        GNRightSample = mom.GNSample,
                                        GNRightSampleId = mom.GNSample.Id,
                                        GNSampleRelationshipType = relationshipType,
                                        GNSampleRelationshipTypeId = relationshipType.Id,
                                        CreateDateTime = DateTime.Now,
                                        CreatedBy = CreatedBy
                                    };
                                    db.GNSampleRelationships.Add(sampleRelationship);
                                    System.Console.WriteLine("***\n  Added Mom " + mom.GNSample.Id + " to Child Sample : " + pedigreeSample.GNSample.Id);
                                }
                            } break;
                        case "U":
                        default:
                            //don't do a thing.
                            System.Console.WriteLine("***\n  Relation is undefined, do not add anything.");
                            break;
                    }
                }

                /******************************************************************************************************/
                //SAVE ALL CHANGES (new team, new project, new samples, new analyses, new batch records, new pedigrees)
                /******************************************************************************************************/

                try
                {
                    db.SaveChanges();
                }
                catch (Exception eRDS)
                {
                    System.Console.WriteLine("***EXCEPTION DB!!! "+eRDS.Message + " " + eRDS.StackTrace + " " + eRDS.InnerException);
                }


                try
                {
                    if (newSampleMessage.autoStartAnalysis.ToLower().Equals("true"))
                    {
                        System.Console.WriteLine("***\n  Elements: " + listOfAnalysisNames.Count());

                        foreach (String anName in listOfAnalysisNames)
                        {
                            List<GNNewSampleBatchSamples> samplesForAnalysis = new List<GNNewSampleBatchSamples>();

                            newAnalysis = new GNAnalysisRequest
                            {
                                Id = Guid.NewGuid(),
                                Project = newProject,
                                GNProjectId = newProject.Id,
                                CreateDateTime = DateTime.Now,
                                CreatedBy = CreatedBy,
                                AnalysisType = analysisType,
                                RequestProgress = 0,
                                RequestDateTime = DateTime.Now,
                                GNAnalysisRequestTypeCode = AnalysisCode,
                                Description = anName,
                                GNAnalysisAdaptorCode = "NONE",
                                AnalysisTypeId = analysisType.Id.ToString(),
                                AutoStart = (newSampleMessage.autoStartAnalysis.ToLower().Equals("true")),
                                AWSRegionSystemName = db.AWSRegions.FirstOrDefault().AWSRegionSystemName
                            };
                            db.GNAnalysisRequests.Add(newAnalysis);
                            System.Console.WriteLine("***\n  ========================\n New newAnalysis Created: " + newAnalysis.Id);

                            samplesForAnalysis = db.GNNewSampleBatchSamples.Where(a => a.GNNewSampleBatchId.Equals(newSampleBatch.Id) && a.AnalysisName.Equals(anName)).ToList();
                            foreach (GNNewSampleBatchSamples sample in samplesForAnalysis)
                            {
                                GNAnalysisRequestGNSample newAnalysisSample = new GNAnalysisRequestGNSample
                                {
                                    AffectedIndicator = sample.Affected,
                                    TargetIndicator = sample.Proband,
                                    GNAnalysisRequest = newAnalysis,
                                    GNSample = sample.GNSample,
                                    GNAnalysisRequestId = newAnalysis.Id,
                                    GNSampleId = sample.GNSample.Id
                                };
                                db.GNAnalysisRequestGNSamples.Add(newAnalysisSample);
                                System.Console.WriteLine("***\n  Sample Added to New newAnalysis: " + sample.GNSample.Name);
                            }

                        }

                    } //end of "if(newSampleBatch.CreateAnalysisPerSample)"

                }
                catch (Exception eAnalysis)
                {

                    System.Console.WriteLine("***\n  Exception: " + eAnalysis.Message + eAnalysis.StackTrace + eAnalysis.InnerException);
                }


                db.SaveChanges();

                //Notify BCL2FASTQ service
                sampleResponseService.NotifyBcl2FastqSystem(listOfBatchSamples);

                System.Console.WriteLine("***\n  EVERYTHING WORKED!");

                //NOTIFY USER
                bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            "SAMPLE_CREATION",
                            Organization.OrgMainContact.Email,
                            "Sample Creation",
                            new Dictionary<string, string>
                                    {
                                        {"BatchId", newSampleMessage.batchId},     
                                        {"TotalSamples", newSampleBatch.TotalSamples.ToString()},                                       
                                        {"CreatorName", Organization.OrgMainContact.FullName},                                     
                                        {"JobId", sequencerJob.Id.ToString()},                                     
                                        {"ProjectName", sequencerJob.Project},
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


        /**
         * For this batch:
         *  - Add a status of 90%: Running quality control
         *  - Check that each sample:
         *      - Has at least one file
         *      - If the sample is pair-ended, it has an even number of files
         *      - If so, change to "is ready"
         *  If the batch has the "start analysis" in true, start analyses
         *  Log errors if apply
         *  Change status to 100% if no errors were found
         */
        public void QualityControl(GNNewSampleBatch Batch)
        {
            bool result = true;
            GNNewSampleBatchStatus batchStatus = new GNNewSampleBatchStatus
            {
                Id = Guid.NewGuid(),
                CreateDateTime = DateTime.Now,
                GNNewSampleBatch = Batch,
                GNNewSampleBatchId = Batch.Id,
                IsError = false,
                PercentComplete = 90,
                Status = "RUNNING QUALITY CONTROL",
                Repository = Batch.Repository,
                Message = ""
            };
            db.GNNewSampleBatchStatus.Add(batchStatus);

            string errorMessage = "";
            foreach (GNNewSampleBatchSamples BatchSample in Batch.GNNewSampleBatchSamples)
            {
                GNSample sample = BatchSample.GNSample;
                if(sample.IsPairEnded && sample.CloudFiles.Count() > 0 && sample.CloudFiles.Count() % 2 == 0)
                {
                    sample.IsReady = true;
                }
                else if (!sample.IsPairEnded && sample.CloudFiles.Count() > 0 && sample.CloudFiles.Count() % 2 == 1)
                {
                    sample.IsReady = true;
                }
                else 
                {
                    errorMessage += "Sample " + sample.Id.ToString() + "doesn't have the right number of files.\n";
                    result = false;
                }
            }

            if (result && Batch.AutoStartAnalysis)
            {
                GNNewSampleBatchStatus batchStatus2 = new GNNewSampleBatchStatus
                {
                    Id = Guid.NewGuid(),
                    CreateDateTime = DateTime.Now,
                    GNNewSampleBatch = Batch,
                    GNNewSampleBatchId = Batch.Id,
                    IsError = false,
                    PercentComplete = 95,
                    Status = "STARTING ANALYSIS",
                    Repository = Batch.Repository,
                    Message = ""
                };
                db.GNNewSampleBatchStatus.Add(batchStatus2);

                List<GNAnalysisRequest> analysisRequestsToStart = new List<GNAnalysisRequest>();
                foreach (GNNewSampleBatchSamples BatchSample in Batch.GNNewSampleBatchSamples)
                {
                    GNSample sample = BatchSample.GNSample;
                    GNAnalysisRequest analysis = sample.GNAnalysisRequestGNSamples.FirstOrDefault().GNAnalysisRequest;
                    if (!analysisRequestsToStart.Contains(analysis))
                    {
                        analysisRequestsToStart.Add(analysis);
                    }
                }

                foreach (GNAnalysisRequest analysis in analysisRequestsToStart)
                {
                    this.AutostartAnalysis(analysis);
                }

                GNNewSampleBatchStatus batchStatus3 = new GNNewSampleBatchStatus
                {
                    Id = Guid.NewGuid(),
                    CreateDateTime = DateTime.Now,
                    GNNewSampleBatch = Batch,
                    GNNewSampleBatchId = Batch.Id,
                    IsError = false,
                    PercentComplete = 100,
                    Status = "COMPLETED",
                    Repository = Batch.Repository,
                    Message = ""
                };
                db.GNNewSampleBatchStatus.Add(batchStatus3);

                this.NotifyEndOfBatch(Batch);
            }

            if(!result)
            {
                GNNewSampleBatchStatus batchStatus4 = new GNNewSampleBatchStatus
                {
                    Id = Guid.NewGuid(),
                    CreateDateTime = DateTime.Now,
                    GNNewSampleBatch = Batch,
                    GNNewSampleBatchId = Batch.Id,
                    IsError = true,
                    PercentComplete = 92,
                    Status = "ERRORS DETECTED",
                    Repository = Batch.Repository,
                    Message = ""
                };
                db.GNNewSampleBatchStatus.Add(batchStatus4);
                this.NotifyErrorAtEndOfBatch(Batch, errorMessage);
            }

            db.SaveChanges();
        }



        public async Task<bool> AutostartAnalysis(GNAnalysisRequest analysis)
        {
            AnalysisRequestService analysisService = new AnalysisRequestService(db);
            GNContact userContact = db.GNContacts.Find(Guid.Parse("0750f896-e7d6-48d4-a1b9-007059f62784"));
            await analysisService.StartAnalysis(userContact, analysis.Id);
            return true;
        }


        public bool NotifyEndOfBatch(GNNewSampleBatch Batch)
        {
            bool notifySuccess = true;
            try
            {
                String notifyCreateAnalysis = "";
                if (Batch.CreateAnalysisPerSample)
                {
                    notifyCreateAnalysis = "Each sample had one analysis created automatically.";
                }

                String notifyAutostartAnalysis = "";
                if (Batch.AutoStartAnalysis)
                {
                    notifyAutostartAnalysis = "Each analysis create was autostarted as soon as the samples were completed. New notifications will be sent to your inbox -with links to each analysis- as these move along.";
                }

                //NOTIFY USER
                notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            "SAMPLE_STATUS_UPDATE_COMPLETE",
                            Batch.GNSequencerJob.GNOrganization.OrgMainContact.Email,
                            "Sample Status Update",
                            new Dictionary<string, string>
                                {
                                    {"ProjectName", Batch.GNSequencerJob.Project},
                                    {"TotalSamples", Batch.TotalSamples.ToString()},                                            
                                    {"CreateAnalysisPerSample", notifyCreateAnalysis},
                                    {"AutoStartAnalysis", notifyAutostartAnalysis},
                                    {"ErrorMessage", ""},
                                    {"CreateDateTime",DateTime.Now.ToString()}
                                });
            }
            catch (Exception e2)
            {
                System.Console.WriteLine("***Message " + e2.Message + e2.StackTrace + " **********************************");

            }

            return notifySuccess;
        }

        public bool NotifyErrorAtEndOfBatch(GNNewSampleBatch Batch, string errorMessage)
        {            
            //NOTIFY USER
            bool notifySuccess =
                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                        "SAMPLE_STATUS_UPDATE_ERROR",
                            Batch.GNSequencerJob.GNOrganization.OrgMainContact.Email,
                        "Sample Status Update",
                        new Dictionary<string, string>
                                {
                                    {"BatchId", Batch.Id.ToString()},
                                    {"CreatorName", Batch.GNSequencerJob.GNOrganization.OrgMainContact.FullName},
                                    {"ErrorMessage", errorMessage},
                                    {"CreateDateTime",DateTime.Now.ToString()}
                                });

            return notifySuccess;
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

        //This can be optimized by sending 1 message to the Queue, that can send the entire list of samples.
        public bool NotifyBcl2FastqSystem(List<GNNewSampleBatchSamples> listOfSamples)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            System.Console.WriteLine("***\n  NOTIFY BLC2FASTQ! SIZE OF LIST: " + listOfSamples.Count());

            try
            {
                String sampleS3Path = "";

                foreach (GNNewSampleBatchSamples sample in listOfSamples)
                {
                    sampleS3Path = "s3://dev-gn-s3-01/fastq/" + sample.GNSample.Id.ToString() + "/";

                    //sample.GNNewSampleBatch.Repository.Replace("https://s3.amazonaws.com/", "s3://")
                    NewSampleResponse sampleResponse = new NewSampleResponse
                    {
                        id = sample.GNSample.Id.ToString(), 
                        sample_name = sample.Name,
                        familyId = sample.FamilyId,
                        s3_path = sampleS3Path
                    };

                    this.SendMessage(sampleResponse);
                }
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to send notification to GN_SAMPLE_RESPONSE.", e1);
                System.Console.WriteLine("***\n  Exception: " + e2.Message + e2.StackTrace + e2.InnerException);
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

            if (newSampleMessageStatus.status.Equals("QC-COMPLETED"))
            {
                // ProcessQCMessage(newSampleMessageStatus);
                return true;
            }

            bool success = false;
            try
            {
                GNOrganization Organization = db.GNOrganizations.Where(a => a.Repository.Equals(newSampleMessageStatus.repository)).FirstOrDefault();
                GNNewSampleBatch Batch = db.GNNewSampleBatches.Where(a => a.BatchId.Trim().Equals(newSampleMessageStatus.batchId)).FirstOrDefault();

                if (Batch == null)
                {
                    throw new Exception("Batch not found!");
                }

                System.Console.WriteLine("\n --==========================---------> Batch " + Batch.Id);

                try
                {
                    Batch.TotalNumberOfFastqFiles = Int32.Parse(newSampleMessageStatus.numberOfFastqFiles);                    
                }
                catch (Exception numberNull)
                {
                    Batch.TotalNumberOfFastqFiles = 0;
                }                

                System.Console.WriteLine("\n -----------> Organization " + Organization.Name);


                bool isError = false;
                if (newSampleMessageStatus.isError.Equals("true"))
                {
                    isError = true;
                }

                string status = newSampleMessageStatus.status;
                if(newSampleMessageStatus.percentComplete > 89)
                {
                    //higher percentages will be dealt by the Portal during the Quality Control step
                    return true;
                }


                //check if the status has been already recorded
                int alreadyExists = db.GNNewSampleBatchStatus.Where(a => a.GNNewSampleBatch.BatchId.Equals(newSampleMessageStatus.batchId) && a.PercentComplete == newSampleMessageStatus.percentComplete && a.Status.Equals(status)).Count();

                if (alreadyExists == 0)
                {
                    GNNewSampleBatchStatus batchStatus = new GNNewSampleBatchStatus
                    {
                        Id = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                        GNNewSampleBatch = Batch,
                        GNNewSampleBatchId = Batch.Id,
                        IsError = isError,
                        PercentComplete = newSampleMessageStatus.percentComplete,
                        Status = status,
                        Repository = newSampleMessageStatus.repository,
                        Message = newSampleMessageStatus.message
                    };
                    db.GNNewSampleBatchStatus.Add(batchStatus);
                    db.SaveChanges();

                    System.Console.WriteLine("\n -----------> Added status " + newSampleMessageStatus.status + " " + newSampleMessageStatus.percentComplete + "%");

                    if (isError)
                    {
                        Batch.GNSequencerJob.Status = "ERROR";
                        db.SaveChanges();

                        //NOTIFY USER
                        bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "SAMPLE_STATUS_UPDATE_ERROR",
                                    Organization.OrgMainContact.Email,
                                    "Sample Status Update",
                                    new Dictionary<string, string>
                                            {
                                                {"BatchId", newSampleMessageStatus.batchId},
                                                {"CreatorName", Organization.OrgMainContact.FullName},
                                                {"ErrorMessage", newSampleMessageStatus.message},
                                                {"CreateDateTime",DateTime.Now.ToString()}
                                            });
                    }

                    /*
                    if (newSampleMessageStatus.percentComplete == 100 || newSampleMessageStatus.percentComplete.Equals("100"))
                    {
                        Batch.GNSequencerJob.Status = "COMPLETED";


                        //loop through all the samples, look for their analyses, and start them.
                        foreach (GNNewSampleBatchSamples sampleBatch in Batch.GNNewSampleBatchSamples)
                        {
                            sampleBatch.GNSample.IsReady = true;
                        }

                        db.SaveChanges();


                        if (Batch.AutoStartAnalysis)
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
                            String notifyCreateAnalysis = "";
                            if (Batch.CreateAnalysisPerSample)
                            {
                                notifyCreateAnalysis = "Each sample had one analysis created automatically.";
                            }

                            String notifyAutostartAnalysis = "";
                            if (Batch.AutoStartAnalysis)
                            {
                                notifyAutostartAnalysis = "Each analysis create was autostarted as soon as the samples were completed. New notifications will be sent to your inbox -with links to each analysis- as these move along.";
                            }

                            //NOTIFY USER
                            bool notifySuccess =
                                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                        "SAMPLE_STATUS_UPDATE_COMPLETE",
                                        "telma.frege@genomenext.com", //Organization.OrgMainContact.Email,
                                        "Sample Status Update",
                                        new Dictionary<string, string>
                                            {
                                                {"ProjectName", Batch.GNSequencerJob.Project},
                                                {"TotalSamples", Batch.TotalSamples.ToString()},                                            
                                                {"CreateAnalysisPerSample", notifyCreateAnalysis},
                                                {"AutoStartAnalysis", notifyAutostartAnalysis},
                                                {"ErrorMessage", newSampleMessageStatus.message},
                                                {"CreateDateTime",DateTime.Now.ToString()}
                                            });
                        }
                        catch (Exception e2)
                        {
                            System.Console.WriteLine("***Message " + e2.Message + e2.StackTrace + " **********************************");

                        }

                    }
                    */
                }
                    
                
                success = true;
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to Update Status of New Sample.", e1);
                System.Console.WriteLine("***\n  Exception: " + e1.Message + e1.StackTrace + e1.InnerException);
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
    }
}
