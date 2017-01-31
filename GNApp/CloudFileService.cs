using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using GenomeNext.Cloud.Storage;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;
using GenomeNext.Billing;

namespace GenomeNext.App
{
    public class CloudFileService : GNEntityService<GNCloudFile>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AspNetRoleService aspNetRoleService { get; set; }
        public GNCloudStorageService cloudStorageService { get; set; }
        public TransactionService transactionService { get; set; }

        public CloudFileService(GNEntityModelContainer db, IdentityModelContainer identityDB)
            : base(db)
        {
            base.db = db;
            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.transactionService = new TransactionService(db);
        }

        public void InitCloudServices(Guid AWSConfigId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if(cloudStorageService == null)
            {
                cloudStorageService = new GNCloudStorageService();
                cloudStorageService.AWSConfigId = AWSConfigId;
                cloudStorageService.ConnectToCloudStorage();
            }
        }

        public override async Task<List<GNCloudFile>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNCloudFile> entities = null;

            if(filters != null && filters.ContainsKey("CloudFileCategory.Name"))
            {
                var filterVal = (string)filters["CloudFileCategory.Name"];

                entities =
                    await db.GNCloudFiles
                        .Include(f => f.CloudFileCategory)
                        .Where(f => f.CloudFileCategory.Name == filterVal)
                        .OrderByDescending(f => f.CreateDateTime)
                        .ToListAsync();
            }
            else
	        {
                entities =
                    await db.GNCloudFiles
                        .Include(f => f.CloudFileCategory)
                        .OrderByDescending(f => f.CreateDateTime)
                        .ToListAsync();
	        }

            return EvalEntityListSecurity(userContact, entities);
        }

        public override async Task<GNCloudFile> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNCloudFiles.FindAsync(keys);
        }

        public override async Task<GNCloudFile> Insert(GNContact userContact, object entity)
        {
            bool isExternallyHosted = ((GNCloudFile)entity).IsExternallyHosted;

            GNCloudFile cloudFile = await base.Insert(userContact, entity);

            if (cloudFile == null && entity != null)
            {
                cloudFile = (GNCloudFile)entity;
            }

            //record transaction
            if (cloudFile != null && !isExternallyHosted)
            {
                string txnTypeKey = "STORAGE_S3_UPLOAD";
                string description = cloudFile.Description;
                double valueUsed = ((double)cloudFile.FileSize / (double)(1024 * 1024 * 1024));
                string valueUnits = "GB";

                GNTransaction txn = 
                    await this.transactionService.CreateTransaction(
                        userContact, txnTypeKey, description, valueUsed, valueUnits);

                if (txn != null)
                {
                    bool success = await this.transactionService.ApplyTransactionTotalToBestPaymentMethod(
                        userContact, txn, userContact.GNOrganization.Account.BillingMode);
                }
            }
            else
            {
                LogUtil.Error(logger, "Unable to record transaction for File Upload due to a NULL Cloud File / Entity object.");
            }

            return cloudFile;
        }

        public override async Task<GNCloudFile> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNCloudFile cloudFile = null;

            try
            {
                cloudFile = (GNCloudFile)entity;

                cloudFile.Id = Guid.NewGuid();
                cloudFile.FolderPath = cloudFile.FileName.Replace(cloudFile.Description, "");
                cloudFile.FileURL = GNCloudStorageService.GetObjectUrl(cloudFile.Volume, cloudFile.FileName);
                cloudFile.CreateDateTime = DateTime.Now;

                var tx = db.Database.BeginTransaction();

                string sql = "INSERT INTO [gn].[GNCloudFiles] " +
                    "([Id],[FileURL],[Volume],[FileName],[FolderPath],[FileSize],[Description],[GNCloudFileCategoryId],[AWSRegionSystemName], " +
                    "[CreatedBy],[CreateDateTime]) " +
                    "VALUES " +
                    "(@Id, " +
                    "@FileURL, " +
                    "@Volume, " +
                    "@FileName, " +
                    "@FolderPath, " +
                    "@FileSize, " +
                    "@Description, " +
                    "@GNCloudFileCategoryId, " +
                    "@AWSRegionSystemName, " +
                    "@CreatedBy, " +
                    "@CreateDateTime)";

                db.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@Id", cloudFile.Id),
                    new SqlParameter("@FileURL", cloudFile.FileURL),
                    new SqlParameter("@Volume", cloudFile.Volume),
                    new SqlParameter("@FileName", cloudFile.FileName),
                    new SqlParameter("@FolderPath", cloudFile.FolderPath),
                    new SqlParameter("@FileSize", cloudFile.FileSize),
                    new SqlParameter("@Description", cloudFile.Description),
                    new SqlParameter("@GNCloudFileCategoryId", cloudFile.GNCloudFileCategoryId),
                    new SqlParameter("@AWSRegionSystemName", cloudFile.AWSRegionSystemName),
                    new SqlParameter("@CreatedBy", cloudFile.CreatedBy),
                    new SqlParameter("@CreateDateTime", cloudFile.CreateDateTime.Value));

                tx.Commit();

                Guid sampleId = Guid.Empty;
                Guid.TryParse(cloudFile.SampleId, out sampleId);
                AssociateCloudFileToSample(cloudFile.Id, sampleId);
            }
            catch (Exception ex)
            {
                LogUtil.Error(logger, "Unable to insert Cloud File into Database.", ex);
            }

            return cloudFile;
        }

        public void AssociateCloudFileToSample(Guid cloudFileId, Guid sampleId)
        {
            var tx = db.Database.BeginTransaction();

            if (cloudFileId != Guid.Empty && sampleId != Guid.Empty)
            {
                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNSampleGNCloudFile]([GNSampleGNCloudFile_GNCloudFile_Id],[CloudFiles_Id]) " +
                    "VALUES(@sampleId, @cloudFileId)",
                    new SqlParameter("@sampleId", sampleId),
                    new SqlParameter("@cloudFileId", cloudFileId));
            }

            tx.Commit();
        }

        public async Task<int> Delete(GNContact userContact, string sampleId, params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            int result = 0;
                
            GNCloudFile cloudFile = db.GNCloudFiles.Find(keys);

            if (cloudFile != null)
            {
                int s3BucketMatchCount = userContact.GNOrganization.AWSConfig.AWSResources.Count(r => r.ARN == cloudFile.Volume);

                bool s3DeleteSuccess = true;
                
                //Perform Physical Delete of File on S3, if in bucket owned by GenomeNext
                if (s3BucketMatchCount != 0)
                {
                    s3DeleteSuccess = cloudStorageService.DeleteS3File(cloudFile.Volume, cloudFile.FileName);

                    if (s3DeleteSuccess)
                    {
                        //record transaction
                        if (cloudFile != null)
                        {
                            string txnTypeKey = "STORAGE_S3_DELETE";
                            string description = cloudFile.Description;
                            double valueUsed = -1.0 * ((double)cloudFile.FileSize / (double)(1024 * 1024 * 1024));
                            string valueUnits = "GB";

                            GNTransaction txn =
                                await this.transactionService.CreateTransaction(
                                    userContact, txnTypeKey, description, valueUsed, valueUnits);
                        }
                        else
                        {
                            LogUtil.Error(logger, "Unable to record transaction for File Delete due to a NULL Cloud File / Entity object.");
                        }
                    }
                }

                //Perform Logical Delete of File in DB
                if (s3DeleteSuccess && !string.IsNullOrEmpty(sampleId))
                {
                    var tx = db.Database.BeginTransaction();

                    result = db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNSampleGNCloudFile] " +
                        "WHERE [GNSampleGNCloudFile_GNCloudFile_Id] = @sampleId " +
                        "AND [CloudFiles_Id] = @cloudFileId",
                        new SqlParameter("@sampleId", Guid.Parse(sampleId)),
                        new SqlParameter("@cloudFileId", cloudFile.Id));

                    if (result != 0)
                    {
                        result = db.Database.ExecuteSqlCommand(
                            "DELETE FROM [gn].[GNCloudFiles] " +
                            "WHERE [Id] = @cloudFileId",
                            new SqlParameter("@cloudFileId", cloudFile.Id));
                    }

                    tx.Commit();
                }
                else
                {
                    Exception e = new Exception("S3 Delete Object Failed and/or Sample ID is empty");
                    LogUtil.Error(logger, e.Message, e);
                    throw e;
                }
            }

            return result;
        }

        public async Task InitStorageTransactionLogging(GNContact userContact)
        {
            List<string> txnTypeKeys = await this.db.GNTransactionTypes
                .Where(tt => tt.Name.Contains("STORAGE")).Select(tt => tt.Name).ToListAsync();

            foreach (var txnTypeKey in txnTypeKeys)
            {
                await CreateStorageTransactionForCurrentMonth(userContact, txnTypeKey);
            }
        }

        private async Task CreateStorageTransactionForCurrentMonth(GNContact userContact, string txnTypeKey)
        {
            await this.transactionService.CreateTransaction(
                userContact, txnTypeKey,
                this.transactionService.INITIALIZE_TXN_DESCRIPTION,
                0.0, "GB");
        }


        public override GNCloudFile EvalEntitySecurity(GNContact userContact, GNCloudFile cloudFile)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (cloudFile != null)
            {
                bool isCloudFileOwnedByOrganization = IsCloudFileOwnedByOrganization(userContact, cloudFile);

                cloudFile.CanCreate = true;

                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    cloudFile.CanView = true;
                    cloudFile.CanEdit = true;
                    cloudFile.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    if (isCloudFileOwnedByOrganization)
                    {
                        cloudFile.CanView = true;
                        cloudFile.CanEdit = true;
                        cloudFile.CanDelete = true;
                    }
                }
                //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
                else if (isCloudFileOwnedByOrganization)
                {
                    bool isCreatorOfSampleForCloudFile = IsCreatorOfSampleForCloudFile(userContact, cloudFile);
                    bool isCreatorOfCloudFile = cloudFile.CreatedBy != null && cloudFile.CreatedBy != Guid.Empty && cloudFile.CreatedBy == userContact.Id;

                    if (userContact.IsInRole("TEAM_MANAGER")
                        || userContact.IsInRole("PROJECT_MANAGER")
                        || userContact.IsInRole("TEAM_MEMBER"))
                    {
                        cloudFile.CanView = true;

                        if (isCreatorOfSampleForCloudFile || isCreatorOfCloudFile)
                        {
                            cloudFile.CanEdit = true;
                            cloudFile.CanDelete = true;
                        }
                        else
                        {
                            cloudFile.CanEdit = false;
                            cloudFile.CanDelete = false;
                        }
                    }
                }

                //Prevent Deletion of cloudFiles with associated entities
                if (cloudFile.CanDelete
                    && (cloudFile.SampleId == null))
                {
                    cloudFile.CanDelete = true;
                }
                else
                {
                    cloudFile.CanDelete = false;
                }
            }

            return cloudFile;
        }

        public string GetCloudFileName(GNCloudFile cloudFile)
        {
            string cloudFileName = cloudFile.Description;

            try
            {
                if (!string.IsNullOrEmpty(cloudFileName))
                {
                    if (cloudFileName.Contains(":"))
                    {
                        cloudFileName = cloudFileName.Split(':').Last();
                    }

                    if (cloudFileName.Contains("/"))
                    {
                        cloudFileName = cloudFileName.Split('/').Last();
                    }

                    cloudFileName = cloudFileName.Trim();
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to parse out cloud file name for " + cloudFile.Description, e);
            }

            return cloudFileName;
        }

        public string GetQcCloudFileName(GNCloudFile cloudFile)
        {
            string cloudFileName = cloudFile.QcStatsReportLocation.Trim();

            try
            {
                if (!string.IsNullOrEmpty(cloudFileName))
                {
                    if (cloudFileName.Contains(":"))
                    {
                        cloudFileName = cloudFileName.Split(':').Last();
                    }

                    if (cloudFileName.Contains("/"))
                    {
                        cloudFileName = cloudFileName.Split('/').Last();
                    }

                    cloudFileName = cloudFileName.Trim();
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to parse out cloud file name for " + cloudFile.Description, e);
            }

            return cloudFileName;
        }

        private bool IsCreatorOfSampleForCloudFile(GNContact userContact, GNCloudFile cloudFile)
        {
            bool isCreatorOfSample = false;

            foreach (var sample in this.db.GNSamples.Where(s => s.CloudFiles.Select(f => f.Id).Contains(cloudFile.Id)))
            {
                if (sample.CreatedBy != null && sample.CreatedBy != Guid.Empty && userContact.Id == sample.CreatedBy)
                {
                    isCreatorOfSample = true;
                    break;
                }
            }

            return isCreatorOfSample;
        }

        private bool IsCloudFileOwnedByOrganization(GNContact userContact, GNCloudFile cloudFile)
        {
            bool isOwnedByOrg = false;

            foreach (var sample in this.db.GNSamples.Where(s => s.CloudFiles.Select(f => f.Id).Contains(cloudFile.Id)))
            {
                if(userContact.GNOrganizationId == sample.GNOrganizationId)
                {
                    isOwnedByOrg = true;
                    break;
                }
            }

            return isOwnedByOrg;
        }

    }

    public class CloudFileCategoryService : GNEntityService<GNCloudFileCategory>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CloudFileCategoryService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNCloudFileCategory>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNCloudFileCategory> entities =
                await db.GNCloudFileCategories
                .ToListAsync();

            return entities;
        }

        public override async Task<GNCloudFileCategory> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNCloudFileCategories.FindAsync(keys);
        }
    }
}
