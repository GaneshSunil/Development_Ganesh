using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data;
using GenomeNext.Data.Metadata.Audit;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using GenomeNext.Cloud.CloudNoSQL;

namespace GenomeNext.App
{
    public class AuditService : GNEntityService<GNAudit>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AmazonDynamoDBClient dynamoClient;

        public AuditService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;

            AWSConfig AWSConfigEntity = db.AWSConfigs.FirstOrDefault();
            
            Amazon.RegionEndpoint regionEndpoint =
                        Amazon.RegionEndpoint.GetBySystemName(AWSConfigEntity.AWSRegionSystemName);

            dynamoClient = new AmazonDynamoDBClient(
                                                           AWSConfigEntity.AWSAccessKeyId,
                                                           AWSConfigEntity.AWSSecretAccessKey,
                                                           regionEndpoint);
        }

        public override async Task<List<GNAudit>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            await Task.Delay(1);

            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNAudit> results = new List<GNAudit>();

            try
            {
                GNCloudNoSQLService noSQL = new GNCloudNoSQLService();

                List<Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>> logs = noSQL.ScanGNEvents(filters);

                foreach (var item in logs)
                {
                    try
                    {

                        GNAudit audit = new GNAudit();
                        foreach (KeyValuePair<string, Amazon.DynamoDBv2.Model.AttributeValue> entry in item)
                        {
                            string entryKey = entry.Key;
                            switch (entry.Key)
                            {
                                case "Id":
                                    audit.Id = entry.Value.S.ToString();
                                    break;
                                case "OrganizationId":
                                    audit.OrganizationId = Guid.Parse(entry.Value.S.ToString().Trim());
                                    audit.ActorOrganization = db.GNOrganizations.Where(a => a.Id.Equals(audit.OrganizationId)).FirstOrDefault();
                                    if(audit.ActorOrganization != null)
                                    {
                                        audit.OrganizationName = audit.ActorOrganization.Name;
                                    }                                    
                                    break;
                                case "Actor":
                                    if(entry.Value.SS.Count() > 0)
                                    {
                                        audit.ActorId = Guid.Parse(entry.Value.SS.First().ToString());
                                        audit.ActorEmail = entry.Value.SS.Last().ToString();
                                        audit.Actor = db.GNContacts.Where(a => a.Id.Equals(audit.ActorId)).FirstOrDefault();
                                        if(audit.Actor != null)
                                        {
                                            audit.ActorName = audit.Actor.FullName;
                                        }                                       
                                    }
                                    break;
                                case "ActorEmail":
                                    if (entry.Value.S.ToString().Trim() != "")
                                    {
                                        audit.ActorEmail = entry.Value.S.ToString().Trim();
                                    }
                                    break;
                                case "ActorId":
                                    if (entry.Value.S.ToString().Trim() != "")
                                    {
                                        audit.ActorId = Guid.Parse(entry.Value.S.ToString().Trim());
                                        audit.Actor = db.GNContacts.Where(a => a.Id.Equals(audit.ActorId)).FirstOrDefault();
                                        if (audit.Actor != null)
                                        {
                                            audit.ActorName = audit.Actor.FullName;
                                        }                                        
                                    }                                    
                                    break;
                                case "Action":
                                case "ActionExecuted":
                                    audit.Action = entry.Value.S.ToString();
                                    break;
                                case "EntityId":
                                    audit.EntityId = Guid.Parse(entry.Value.S.ToString());
                                    break;
                                case "EntityType":
                                    audit.EntityType = entry.Value.S.ToString();
                                    break;
                                case "IP":
                                    audit.IP = entry.Value.S.ToString();
                                    break;
                                case "Timestamp":
                                    audit.Timestamp = DateTime.Parse(entry.Value.S.ToString());
                                    break;
                                case "TimestampNumeric":
                                    audit.TimestampNumeric = Int64.Parse(entry.Value.S.ToString());
                                    break;
                            }
                        }
                        string auditEntityType = audit.EntityType;
                        switch (audit.EntityType)
                        {
                            case "CONTACT":
                                audit.Contact = db.GNContacts.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "ORGANIZATION":
                                audit.Organization = db.GNOrganizations.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "TEAM":
                                audit.Team = db.GNTeams.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "PROJECT":
                                audit.Project = db.GNProjects.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "SAMPLE":
                                audit.Sample = db.GNSamples.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "SAMPLE_RELATIONSHIP":
                                audit.Sample = db.GNSamples.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "ANALYSIS_REQUEST":
                                audit.Analysis = db.GNAnalysisRequests.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "SETTINGS_TEMPLATE":
                                audit.SettingsTemplate = db.GNSettingsTemplates.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "SETTINGS_TEMPLATE_CONFIG":
                                audit.SettingsTemplateConfig = db.GNSettingsTemplateConfigs.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "ACCOUNT":
                                audit.Account = db.GNAccounts.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "PRODUCT":
                                audit.Product = db.GNProducts.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "CLOUD_FILE":
                                audit.CloudFile = db.GNCloudFiles.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "INVOICE":
                                audit.Invoice = db.GNInvoices.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "INVOICE_DETAIL":
                                audit.InvoiceDetail = db.GNInvoiceDetails.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "PAYMENT":
                                audit.Payment = db.GNPayments.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "PURCHASE_ORDER":
                                audit.PurchaseOrder = db.GNPurchaseOrders.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                            case "USER":
                                //audit.User = db.GNAnalysisRequests.Where(a => a.Id.Equals(audit.EntityId)).FirstOrDefault();
                                break;
                        }

                        results.Add(audit);
                    }

                    catch (Exception e1)
                    {
                        logger.Error("Error while loading records from Audit table GNEvents " + e1.Message);
                    }

                }
            }
            catch (Exception e2)
            {
                logger.Error("Error while loading records from Audit table GNEvents " + e2.Message);
            }

            logger.Error("Returning  " + results.Count() + " results");
            return results;
        }

    }
}
