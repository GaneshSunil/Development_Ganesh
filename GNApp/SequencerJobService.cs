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
    public class SequencerService : GNCloudMessageService<Sequencer>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SEQUENCER";

        public SequencerService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public SequencerService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(Sequencer sequencerMsg, object queueMessage)
        {
            System.Console.WriteLine("***\n ****** bucket Name: " + sequencerMsg.bucket);
            System.Console.WriteLine("***\n ****** project name: " + sequencerMsg.project_name);
            bool success = false;
            try
            {
                System.Console.WriteLine("***\n  Searching for Org " + sequencerMsg.bucket);
                GNOrganization organization = db.GNOrganizations.Where(a => a.Repository.Equals(sequencerMsg.bucket)).FirstOrDefault();
                System.Console.WriteLine("***\n ****** organization: " + organization.Name);

                if (organization == null)
                {
                    System.Console.WriteLine("***\n  NO ORG FOUND!!!!" );
                    return success;
                }

                System.Console.WriteLine("***\n  organization: " + organization.Id);
                if(sequencerMsg.bucket.Equals("ERROR") && sequencerMsg.project_name.Equals("UNDEFINED"))
                {
                    this.NotifyError(sequencerMsg);
                    return true;
                }


                //check if a project is already undergoing for the same Org and same name (repeated message)
                int seqJobsRunning = db.GNSequencerJobs.Where(a => a.GNOrganizationId.Equals(organization.Id) && a.Project.Equals(sequencerMsg.project_name.Trim())).Count();
                //if none exists, create
                if(seqJobsRunning == 0)
                {
                    string datetimeString = DateTime.Now.ToString("MM-dd 0:HH:mm:ss");

                    /**
                     * 1. Create a Team
                     * 2. Create a Project
                     */
                    GNContact contact = db.GNContacts.Find(organization.GNContactId);

                    GNTeam newTeam = new GNTeam
                    {
                        Id = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                        CreatedBy = organization.GNContactId,
                        Name = "Batch " + datetimeString,
                        GNContactId = contact.Id,
                        Organization = organization,
                        OrganizationId = organization.Id,
                        TeamLead = contact
                    };
                                
                    System.Console.WriteLine("***\n  New newTeam Created: " + newTeam.Id);

                    Guid newProjectId = Guid.NewGuid();

                    System.Console.WriteLine("*********************************\n  Contact for project: " + contact.FullName);
                    System.Console.WriteLine("*********************************\n  Contact for project: " + newProjectId);

                    GNProject newProject = new GNProject
                    {
                        Id = newProjectId,
                        CreateDateTime = DateTime.Now,
                        CreatedBy = organization.GNContactId,
                        ProjectLead = contact,
                        ProjectLeadId = contact.Id.ToString(),
                        Name = sequencerMsg.project_name,   //name assigned to the project
                        TeamId = newTeam.Id.ToString(),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(30),
                        Description = "Created automatically from the Sample Batch Process"
                    };

                    newTeam.Projects.Add(newProject);
                    newProject.Teams.Add(newTeam);
                    db.GNTeams.Add(newTeam);
                    db.GNProjects.Add(newProject);
                    
                    System.Console.WriteLine("*********************************\n  Contact for project: " + newProjectId);
                    GNSequencerJob sequencerJob = new GNSequencerJob
                    {
                        Id = Guid.NewGuid(),
                        CreateDateTime = DateTime.Now,
                        Project = sequencerMsg.project_name,
                        Status = "STARTED",
                        GNOrganization = organization,
                        GNOrganizationId = organization.Id,
                        GNProject = newProject
                    };

                    db.GNSequencerJobs.Add(sequencerJob);
                    System.Console.WriteLine("***\n  New sequencerJob Created: " + sequencerJob.Id);

                    try
                    {
                        db.SaveChanges();
                        //NOTIFY USER
                        bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "SEQUENCER_JOB_STARTED",
                                    "telma.frege@genomenext.com",
                                    "SequencerJob:" + sequencerJob.Id.ToString(),
                                    new Dictionary<string, string>
                                            {
                                                {"JobId", sequencerJob.Id.ToString()},
                                                {"ProjectName", sequencerJob.Project},
                                                {"CreateDateTime", DateTime.Now.ToString()}
                                            });
                        success = true;
                    }
                    catch (Exception eRDS)
                    {
                        System.Console.WriteLine("***\n  EXCEPCION!!! " + eRDS.Message + eRDS.StackTrace + eRDS.InnerException);
                        
                        success = false;
                    }
                    

                }

            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to process Sequencer Job Message.", e1);
                System.Console.WriteLine("***\n  EXCEPCION!!! " + e1.Message + e1.StackTrace);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }

        public void NotifyError(Sequencer sequencerMsg)
        {
            Guid OrgId = Guid.Parse("82ff6451-dac6-4a88-a967-cdaf5f9a3599");
            GNOrganization Organization = db.GNOrganizations.Find(OrgId);

            //NOTIFY USER
            bool notifyError =
                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                        "SEQUENCER_JOB_ERROR",
                        "devops@genomenext.com",
                        "SequencerJobError",
                        new Dictionary<string, string>
                                    {
                                        {"Message", sequencerMsg.message},
                                        {"CreateDateTime", DateTime.Now.ToString()}
                                    });
        }

    }

    public class SequencerJobService : GNEntityService<GNSequencerJob>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SequencerJobService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSequencerJob>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSequencerJob> entities =
                await db.GNSequencerJobs
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSequencerJob> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSequencerJobs.FindAsync(keys);
        }

    }

}
