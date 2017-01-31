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

namespace GenomeNext.App
{
    /// <summary>
    /// Message Consumer for 'GN_NOTIFICATION' queue
    /// </summary>
    public class NotificationCloudMessageService : GNCloudMessageService<NotificationMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_NOTIFICATION";

        public NotificationCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public NotificationCloudMessageService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }
        
        public bool NotifyGNContact(String notificationTopic, String email, String source, Dictionary<string, string> values)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            
            try 
	        {	        		        
                NotificationMessage notificationMsg = new NotificationMessage();
                notificationMsg.notificationTopic = notificationTopic;
                notificationMsg.email = email;
                notificationMsg.source = source;
                notificationMsg.valuesDictionary = values;

                this.SendMessage(notificationMsg);
	        }
	        catch (Exception e1)
	        {		
                Exception e2 = new Exception("Unable to send notification to queue.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                return false;
	        }

            return true;
        }

        public override bool ProcessMessage(NotificationMessage notificationMessage, object queueMessage)
        {
            bool success = false;
            try
            {
                if (EvalCanSendMessage(notificationMessage.notificationTopic, notificationMessage.source))
                {
                    success = new GenomeNext.Notification.NotificationService().SendNotification(
                                    notificationMessage.notificationTopic,
                                    notificationMessage.email,
                                    notificationMessage.source,
                                    notificationMessage.valuesDictionary
                                );
                }                
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to send notification.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
	        }
            return success;
        }

        private bool EvalCanSendMessage(string topic, string source) 
        {
            switch (topic)
            {
                case "ANALYSIS_RESULT_RECEIVED":
                    //add logic or call to another method here.
                    return true;
                default:
                    return true;
            }
        }
    }






    public class NotificationSesBounceMessageService : GNCloudMessageService<AmazonSqsNotification>
    {        
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SES_BOUNCE_NOTIFICATION";

        public NotificationSesBounceMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public NotificationSesBounceMessageService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }
    
        public override bool ProcessMessage(AmazonSqsNotification notificationMessage, object queueMessage)
        {
            bool success = false;
            try
            {
                //parse again, to get the SES Bounce from the SQS object
                var bounce = Newtonsoft.Json.JsonConvert.DeserializeObject<AmazonSesBounceNotification>(notificationMessage.Message);
                if(!bounce.Bounce.BounceType.ToUpper().Equals("TRANSIENT"))   //Ignore transients, keep notifying those emails
                {
                    GNNotificationSuppressionList suppressionListItem = new GNNotificationSuppressionList();
                    suppressionListItem.CreateDateTime = bounce.Bounce.Timestamp;
                    suppressionListItem.Category = "BOUNCE";
                    suppressionListItem.Type = bounce.Bounce.BounceType;
                    suppressionListItem.Subtype = bounce.Bounce.BounceSubType;
                    //suppressionListItem.CreatedBy = ;

                    //Store message in DB
                    foreach (var recipient in bounce.Bounce.BouncedRecipients)
                    {
                        suppressionListItem.Email = recipient.EmailAddress;
                        db.GNNotificationSuppressionLists.Add(suppressionListItem);
                        db.SaveChanges();
                    }
                }
                success = true;
             
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to save bounce.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }
    }    

    
    public class NotificationSesComplaintMessageService : GNCloudMessageService<AmazonSqsNotification>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_SES_COMPLAINT_NOTIFICATION";

        public NotificationSesComplaintMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public NotificationSesComplaintMessageService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(AmazonSqsNotification notificationMessage, object queueMessage)
        {
            bool success = false;
            try
            {
                //parse again, to get the SES Complaint from the SQS object
                var complaint = Newtonsoft.Json.JsonConvert.DeserializeObject<AmazonSesComplaintNotification>(notificationMessage.Message);
                GNNotificationSuppressionList suppressionListItem = new GNNotificationSuppressionList();
                suppressionListItem.CreateDateTime = complaint.Complaint.Timestamp;
                suppressionListItem.Category = "COMPLAINT";
                suppressionListItem.Type = complaint.Complaint.ComplaintFeedbackType;
 
                //Store message in DB
                foreach (var recipient in complaint.Complaint.ComplainedRecipients)
                {                    
                    suppressionListItem.Email = recipient.EmailAddress;                    
                    db.GNNotificationSuppressionLists.Add(suppressionListItem);
                    db.SaveChanges();
                }
                success = true;
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to save complaint.", e1);
                LogUtil.Warn(logger, e2.Message, e2);
                success = false;
            }
            return success;
        }
    }    

}
