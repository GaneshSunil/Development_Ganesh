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
using GenomeNext.Cloud.CloudNoSQL;
using System.Data.SqlClient;

namespace GenomeNext.App
{
    public class OrganizationService : GNEntityService<GNOrganization>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "ORGANIZATION";

        public OrganizationService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNOrganization>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());


            IQueryable<GNOrganization> Organizations = db.GNOrganizations;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;
                
                if (filters.ContainsKey("Name"))
                {
                    filterVal = filters["Name"].ToString().Trim().ToUpper();
                    Organizations = Organizations.Where(t => t.Name.ToUpper().Contains(filterVal));
                }
            }

            //Order By Results
            Organizations = Organizations
                .OrderBy(t => t.Name);

            //Limit Result Size
            Organizations = Organizations.Skip(start).Take(end - start);

            return Organizations.ToList();
        }

        public override async Task<GNOrganization> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await db.GNOrganizations.FindAsync(keys);
        }

        public override async Task<GNOrganization> Insert(object entity)
        {
            GNOrganization org = null;

            org = SetUTCOffset(entity);
            if(((GNOrganization)entity).GNSettingsTemplateId == null)
            {
                org = SetDefaultSettingsTemplate(entity);
            }            

            return await base.Insert(org);
        }

        public override async Task<GNOrganization> Update(object entity)
        {
            GNOrganization org = null;

            org = SetUTCOffset(entity);

            bool auditResult = new GNCloudNoSQLService().LogEvent(org.CreatedByContact, org.Id, this.ENTITY, "N/A", "EVENT_UPDATE");

            return await base.Update(org);
        }

        private GNOrganization SetUTCOffset(object entity)
        {
            GNOrganization org = null;

            if (entity != null)
            {
                org = (GNOrganization)entity;
                if (!string.IsNullOrEmpty(org.UTCOffsetDescription))
                {
                    org.UTCOffset = org.UTCOffsetDescription.Substring(4, 6);
                    org.UTCOffsetDescription = org.UTCOffsetDescription.Trim();
                }

                if (string.IsNullOrEmpty(org.UTCOffset))
                {
                    org.UTCOffset = "00:00";
                }
            }
            return org;
        }

        private GNOrganization SetDefaultSettingsTemplate(object entity)
        {
            GNOrganization org = null;

            if (entity != null)
            {
                org = (GNOrganization)entity;
                org.GNSettingsTemplateId = 1;
            }

            return org;
        }
    }


    public class SharedPurchaseOrderOrganizationService : GNEntityService<GNSharedPurchaseOrderOrganization>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SharedPurchaseOrderOrganizationService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSharedPurchaseOrderOrganization>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSharedPurchaseOrderOrganization> entities =
                await db.GNSharedPurchaseOrderOrganizations
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSharedPurchaseOrderOrganization> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSharedPurchaseOrderOrganizations.FindAsync(keys);
        }


        public override async Task<GNSharedPurchaseOrderOrganization> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNSharedPurchaseOrderOrganization sharedPOOrg = (GNSharedPurchaseOrderOrganization)entity;

            try
            {
                sharedPOOrg.Id = Guid.NewGuid();
                sharedPOOrg.CreateDateTime = DateTime.Now;

                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNSharedPurchaseOrderOrganizations] " +
                    "([Id],[AmountBilledOnInvoice],[GNPurchaseOrderId],[GNInvoiceId],[CreatedBy],[CreateDateTime],[Notes]) " +
                    "VALUES " +
                    "(@id, " +
                    "@AmountBilled, " +
                    "@GNPurchaseOrderId, " +
                    "@GNInvoiceId, " +
                    "@projectCreatedBy, " +
                    "@projectCreateDateTime, " +
                    "@Notes)",
                    new SqlParameter("@id", sharedPOOrg.Id),
                    new SqlParameter("@AmountBilled", sharedPOOrg.AmountBilledOnInvoice),
                    new SqlParameter("@GNPurchaseOrderId", sharedPOOrg.GNPurchaseOrderId),
                    new SqlParameter("@GNInvoiceId", sharedPOOrg.GNInvoiceId),
                    new SqlParameter("@projectCreatedBy", sharedPOOrg.CreatedBy),
                    new SqlParameter("@projectCreateDateTime", sharedPOOrg.CreateDateTime),
                    new SqlParameter("@Notes", sharedPOOrg.Notes));
                
                tx.Commit();
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to create entity.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            return sharedPOOrg;
        }


        public override async Task<GNSharedPurchaseOrderOrganization> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNSharedPurchaseOrderOrganization sharedPOOrg = (GNSharedPurchaseOrderOrganization)entity;

            try
            {
                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "UPDATE [gn].[GNSharedPurchaseOrderOrganizations] "+
                    "SET [AmountBilledOnInvoice] = @AmountBilledOnInvoice " +
                    ", [GNPurchaseOrderId] = @GNPurchaseOrderId " +
                    ", [GNInvoiceId] = @GNInvoiceId " +
                    ", [Notes] = @Notes " +
                    "WHERE [Id] = @id",
                    new SqlParameter("@AmountBilledOnInvoice", sharedPOOrg.AmountBilledOnInvoice),
                    new SqlParameter("@GNPurchaseOrderId", sharedPOOrg.GNPurchaseOrderId),
                    new SqlParameter("@GNInvoiceId", sharedPOOrg.GNInvoiceId),
                    new SqlParameter("@Notes", sharedPOOrg.Notes),
                    new SqlParameter("@id", sharedPOOrg.Id));

                tx.Commit();
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to update entity.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            return sharedPOOrg;
        }
    }

}
