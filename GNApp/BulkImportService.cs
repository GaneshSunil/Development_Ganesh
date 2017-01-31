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
using System.Data.OleDb;
using System.Xml;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Cloud.Messaging;
using Newtonsoft.Json;
using GenomeNext.Cloud.Messaging.Model.S3;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace GenomeNext.App
{
    public class BulkImportService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GNEntityModelContainer db { get; set; }

        public BulkImportStatusService bulkImportStatusService { get; set; }
        public BulkImportLogService bulkImportLogService { get; set; }

        public TeamService teamService { get; set; }
        public ProjectService projectService { get; set; }
        public AnalysisRequestService analysisRequestService { get; set; }
        public SampleService sampleService { get; set; }
        public CloudFileService cloudFileService { get; set; }

        public BulkImportService(Guid AWSConfigId = default(Guid), bool forBatch = false)
        {
            InitServicesAndDBContext(AWSConfigId, forBatch);
        }

        protected void InitServicesAndDBContext(Guid AWSConfigId, bool forBatch = false)
        {
            this.db = new GNEntityModelContainer();
            var identityDB = new IdentityModelContainer();

            this.bulkImportStatusService = new BulkImportStatusService(db);
            this.bulkImportLogService = new BulkImportLogService(db);

            this.teamService = new TeamService(this.db, identityDB);
            this.projectService = new ProjectService(this.db, identityDB);
            this.analysisRequestService = new AnalysisRequestService(this.db, identityDB);
            this.sampleService = new SampleService(this.db, identityDB);
            this.cloudFileService = new CloudFileService(this.db, identityDB);

            if (forBatch)
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
            }
        }

        public async Task<GNBulkImportStatus> PersistBulkImport(string bucket, string key, string name, string uuid, GNContact userContact)
        {
            if (this.cloudFileService.cloudStorageService == null)
            {
                this.cloudFileService.InitCloudServices(userContact.GNOrganization.AWSConfigId);
            }

            var fileMetadata = this.cloudFileService.cloudStorageService.GetS3FileMetadata(bucket, key);

            //Insert bulk import status record into db
            GNBulkImportStatus bulkImportStatus = new GNBulkImportStatus
            {
                Id = Guid.Parse(uuid),
                FileURI = bucket+"/"+key,
                FileURIType = "S3",
                FileExtension = System.IO.Path.GetExtension(name),
                CreatedBy = userContact.Id,
                CreateDateTime = DateTime.Now
            };

            bulkImportStatus = await this.bulkImportStatusService.Insert(userContact, bulkImportStatus);

            return bulkImportStatus;
        }

        public AWSResource FetchAWSS3BucketForBulkImport()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            var s3Buckets = db.AWSResources.Where(r => r.AWSResourceType.Name == "S3 Bucket for Bulk Import").ToList();

            int randomS3BucketIdx = new Random().Next(s3Buckets.Count);

            return s3Buckets[randomS3BucketIdx];
        }

        public async Task<bool> DoImport(GNBulkImportStatus bulkImportStatus)
        {
            bool success = false;

            try
            {
                //check if data is already staged
                long totalRecordCountInStaging = db.GNBulkImportStaging.Count(s => s.GNBulkImportStatusId == bulkImportStatus.Id);
                long totalRecordCountInGroupStaging = db.GNBulkImportGroupStagings.Count(s => s.GNBulkImportStatusId == bulkImportStatus.Id);
                
                //if not fully staged, reload from file
                if (totalRecordCountInStaging == 0 || bulkImportStatus.TotalRecordCount != totalRecordCountInStaging)
                {
                    bool loadSuccess = false;

                    //load xls file
                    if (bulkImportStatus.FileExtension == ".xls" || bulkImportStatus.FileExtension == ".xlsx")
                    {
                        System.Console.WriteLine("--------Importing SAMPLES " + bulkImportStatus.FileURI);
                        loadSuccess = LoadExcelDataSet(bulkImportStatus);
                        System.Console.WriteLine("--------Importing GROUPS " + bulkImportStatus.FileURI);
                           
                        loadSuccess = LoadGroupsExcelDataSet(bulkImportStatus);
                    }
                    else
                    {
                        throw new Exception("Unsupported file format/extenstion: " + bulkImportStatus.FileExtension);
                    }

                    if(loadSuccess)
                    {
                        //get staged record count
                        totalRecordCountInStaging = db.GNBulkImportStaging.Count(s => s.GNBulkImportStatusId == bulkImportStatus.Id);
                        totalRecordCountInGroupStaging = db.GNBulkImportStaging.Count(s => s.GNBulkImportStatusId == bulkImportStatus.Id);
                    }
                    else
                    {
                        throw new Exception("An unknown error occurred staging data from the sample index file.");
                    }
                }

                if (totalRecordCountInStaging > 0)
                {
                    //reset bulk import status stats
                    ResetBulkImportStatusStats(bulkImportStatus.Id, totalRecordCountInStaging);

                    //load data set into db
                    Dictionary<string, object> lastSampleFileRecord = new Dictionary<string, object> 
                    {
                        {"TEAM",new GNTeam()},
                        {"PROJECT",new GNProject()},
                        {"ANALYSIS",new GNAnalysisRequest()},
                        {"SAMPLE",new GNSample()}     
                    };

                    long batchTotalTime = 0;
                    double batchRecordAvgTime = 0.0;

                    int BATCH_START_IDX = 0;
                    int BATCH_MAX_SIZE = 1000;
                    int BATCH_ROW_IDX = 0;

                    while (BATCH_ROW_IDX < totalRecordCountInStaging)
                    {

                        System.Console.WriteLine("----------------------\nSTARTING BATCH of [" + BATCH_MAX_SIZE + "]");
                        System.Console.WriteLine("----------------------\nBATCH_START_IDX [" + BATCH_START_IDX + "]");
                        System.Console.WriteLine("----------------------\n------------------------------------------");
                    
                        foreach (GNBulkImportStaging dataRow in db.GNBulkImportStaging
                            .Where(s => s.GNBulkImportStatusId == bulkImportStatus.Id)
                            .OrderBy(s => s.ROW_IDX)
                            .Skip(BATCH_START_IDX)
                            .Take(BATCH_MAX_SIZE).ToList())
                        {
                            System.Console.WriteLine("----------------------\nImporting Record [" + BATCH_ROW_IDX + "]");
                            long insertImportRecordDataStartTime = DateTime.Now.ToFileTime();

                            lastSampleFileRecord = await InsertImportRecordData(
                                dataRow, BATCH_ROW_IDX, bulkImportStatus.CreatedByContact, bulkImportStatus.Id, lastSampleFileRecord);

                            long insertImportRecordDataEndTime = DateTime.Now.ToFileTime();
                            long insertImportRecordDataTotalTime = insertImportRecordDataEndTime - insertImportRecordDataStartTime;

                            batchTotalTime += insertImportRecordDataTotalTime;
                            batchRecordAvgTime = (batchTotalTime / (BATCH_ROW_IDX + 1));

                            System.Console.WriteLine("Insert Record Total Time = " + (insertImportRecordDataTotalTime / 1000.0));
                            System.Console.WriteLine("Batch Total Time = " + (batchTotalTime / 1000.0));
                            System.Console.WriteLine("Batch Record Avg Time = " + (batchRecordAvgTime / 1000.0));
                            System.Console.WriteLine("\n----------------------");

                            BATCH_ROW_IDX++;
                        }

                        BATCH_START_IDX = BATCH_ROW_IDX;
                    }

                    //auto-start last analysis
                    //TFREGE FIX THIS STEP FOR RNA: IT SHOULD BE AFTER CREATING GROUPS
                    await AutoStartLastAnalysis(null, bulkImportStatus.CreatedByContact, bulkImportStatus.Id, lastSampleFileRecord);

                    //save end time
                    UpdateBulkImportStatusField(bulkImportStatus.Id, "ImportEndDateTime", DateTime.Now);
                }

                success = true;
            }
            catch (Exception e)
            {
                this.bulkImportLogService.LogError(bulkImportStatus.CreatedByContact, bulkImportStatus.Id, e.Message);
            }

            return success;
        }

        private void ResetBulkImportStatusStats(Guid bulkImportStatusId, long totalRecordCount)
        {
            try
            {
                string sql = "UPDATE [gn].[GNBulkImportStatus] " +
                    "SET [TotalRecordCount] = @totalRecordCount, " +
                    "[DuplicateRecordCount] = 0, " +
                    "[FailedRecordCount] = 0, " +
                    "[ImportedRecordCount] = 0, " +
                    "[TeamsCreatedCount] = 0, " +
                    "[ProjectsCreatedCount] = 0, " +
                    "[AnalysesCreatedCount] = 0, " +
                    "[SamplesCreatedCount] = 0, " +
                    "[FilesCreatedCount] = 0, " +
                    "[ImportStartDateTime] = @importStartDateTime " +
                    "WHERE [Id] = @bulkImportStatusId";

                var txn = db.Database.BeginTransaction();
                db.Database.ExecuteSqlCommand(
                    sql,
                    new SqlParameter("@totalRecordCount", totalRecordCount),
                    new SqlParameter("@importStartDateTime", DateTime.Now),
                    new SqlParameter("@bulkImportStatusId", bulkImportStatusId));
                txn.Commit();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void UpdateBulkImportStatusField(Guid bulkImportStatusId, string fieldName, object fieldVal)
        {
            try
            {
                string sql = "UPDATE [gn].[GNBulkImportStatus] " +
                    "SET [" + fieldName + "] = @" + fieldName + " " +
                    "WHERE [Id] = @BulkImportStatusId";

                var txn = db.Database.BeginTransaction();
                db.Database.ExecuteSqlCommand(
                    sql,
                    new SqlParameter("@" + fieldName, fieldVal),
                    new SqlParameter("@BulkImportStatusId", bulkImportStatusId));
                txn.Commit();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private async Task<Dictionary<string, object>> InsertImportRecordData(GNBulkImportStaging dataRow, int dataRowIdx,
            GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            try
            {
                GNTeam team = await InsertTeam(dataRow, dataRowIdx, userContact, bulkImportStatusId, lastSampleFileRecord);
                GNProject project = await InsertProject(dataRow, dataRowIdx, team, userContact, bulkImportStatusId, lastSampleFileRecord);
                GNAnalysisRequest analysis = await InsertAnalysis(dataRow, dataRowIdx, team, project, userContact, bulkImportStatusId, lastSampleFileRecord);
                
                try
                {
                    GNSample sample = await InsertSample(dataRow, dataRowIdx, analysis, userContact, bulkImportStatusId, lastSampleFileRecord);



                    if(dataRow.ANALYSIS_GROUP_NAME != null)
                    {
                        GNAnalysisRequestGroup group = await InsertGroup(dataRow, dataRowIdx, analysis, sample, userContact, bulkImportStatusId, lastSampleFileRecord);
                    }
                    

                    try
                    {
                        GNCloudFile cloudFile = await InsertCloudFile(dataRow, dataRowIdx, sample, userContact, bulkImportStatusId, true);

                        try
                        {
                            //this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Record imported: sample " + sample.Id + " and analysis " + analysis.Id + " and user " + userContact.FullName);
                            if (analysis.AutoStart)
                            {
                                await analysisRequestService.StartAnalysis(userContact, analysis.Id);  //telma debugging
                            }

                            /*
                            lastSampleFileRecord = new Dictionary<string, object> 
                            {
                                {"TEAM",team},
                                {"PROJECT",project},
                                {"ANALYSIS",analysis},
                                {"SAMPLE",sample}            
                            };

                            await AutoStartLastAnalysis(analysis, userContact, bulkImportStatusId, lastSampleFileRecord);
                             * */
                        }
                        catch (Exception launchEx)
                        {
                            this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Error launching analysis " + sample.Id + " and analysis " + analysis.Id + " and user " + userContact.FullName + launchEx.Message + launchEx.StackTrace);
                        }

                    }
                    catch (Exception exFile)
                    {
                        this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Error creating the file record for sample " + sample.Id + " and analysis " + analysis.Id + " and user " + userContact.FullName + exFile.Message);


                    }

                }
                catch (Exception e2)
                {
                    this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Error creating the sample  " + e2.Message);

                }

                //tfrege add groups here 
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error inserting sample file at row idx = " + dataRowIdx, e);
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Error inserting sample file:" + e.Message, dataRowIdx + "");
            }

            return lastSampleFileRecord;
        }
        
        private async Task AutoStartLastAnalysis(GNAnalysisRequest analysis, GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNAnalysisRequest lastAnalysis = (GNAnalysisRequest)lastSampleFileRecord["ANALYSIS"];

            if (lastAnalysis != null && lastAnalysis.Id != Guid.Empty
                && ((analysis != null && analysis.Id != lastAnalysis.Id) || analysis == null))
            {
                if (lastAnalysis.AutoStart)
                {

                    this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Autostarting  " + lastAnalysis.Id);
                    
                    //fetch analysis request from db
                    lastAnalysis = this.db.GNAnalysisRequests
                        .Include(ar => ar.GNAnalysisRequestGNSamples)
                        .Include(ar => ar.GNAnalysisRequestGNSamples.Select(s => s.GNSample).Select(s => s.CloudFiles))
                        .Include(ar => ar.AnalysisStatus)
                        .Include(ar => ar.AnalysisResult)
                        .Where(ar => ar.Id == lastAnalysis.Id)
                        .FirstOrDefault();

                    //determine if analysis has valid sample set
                    lastAnalysis = analysisRequestService.IsValidSampleSet(lastAnalysis);

                    //determine if analysis "normal" start is allowed
                    lastAnalysis.CanStartAnalysis = analysisRequestService.IsAnalysisStartAllowed(lastAnalysis);

                    //determine if analysis re-start is allowed
                    lastAnalysis.CanReStartAnalysis = analysisRequestService.IsAnalysisRestartAllowed(lastAnalysis);

                    if(lastAnalysis.CanStartAnalysis || lastAnalysis.CanReStartAnalysis)
                    {
                        await analysisRequestService.StartAnalysis(userContact, lastAnalysis.Id);
                    }
                }
            }
        }

        private GNCloudFileCategory cloudFileCat = null;

        private async Task<GNCloudFile> InsertCloudFile(GNBulkImportStaging dataRow, int dataRowIdx, GNSample sample,
            GNContact userContact, Guid bulkImportStatusId, bool checkForDuplicates = true) 
        {
            GNCloudFile cloudFile = null;
      
            
            string fileBucket = dataRow.FILE_BUCKET.ToString();
            string fileKey = dataRow.FILE_KEY.ToString();

            if (sample != null
                && !string.IsNullOrEmpty(fileBucket)
                && !string.IsNullOrEmpty(fileKey))
            {
                //clean up bucket str
                fileBucket = fileBucket.Replace("s3://", "");

                if (checkForDuplicates)
                {
                    //get cloud file by name
                    cloudFile = this.db.GNCloudFiles
                        .Where(f => (
                            f.Volume.ToLower() == fileBucket.ToLower()
                            && f.FileName.ToLower() == fileKey.ToLower()
                            && (this.db.GNContacts.Where(c => c.Id == f.CreatedBy.Value)
                                .Select(c => c.GNOrganizationId).FirstOrDefault() == userContact.GNOrganizationId)
                            )
                        )
                        .FirstOrDefault();
                }

                if (!checkForDuplicates || (checkForDuplicates && cloudFile == null))
                {
                    if(cloudFileCat == null)
                    {
                        cloudFileCat = this.db.GNCloudFileCategories
                            .Where(cf => (cf.Name == "FASTQ File" || cf.FolderPath == "fastq"))
                            .FirstOrDefault();
                    }

                    if (cloudFileCat != null)
                    {
                        Int64 fileSize = 0;
                        string fileSizeStr = dataRow.FILE_SIZE.ToString();
                        if (!string.IsNullOrEmpty(fileSizeStr))
                        {
                            Int64.TryParse(fileSizeStr, out fileSize);
                        }

                        cloudFile = new GNCloudFile
                        {
                            GNCloudFileCategoryId = cloudFileCat.Id,
                            Description = fileKey,
                            Volume = fileBucket,
                            FileName = fileKey,
                            FileSize = fileSize,
                            SampleId = sample.Id.ToString(),
                            AWSRegionSystemName = userContact.GNOrganization.AWSConfig.AWSRegionSystemName,
                            CreatedBy = userContact.Id,
                            CreateDateTime = DateTime.Now,
                            IsExternallyHosted = true,
                            CanCreate = true
                        };

                        cloudFile.Description = this.cloudFileService.GetCloudFileName(cloudFile);

                        cloudFile = await this.cloudFileService.Insert(userContact, cloudFile);

                        this.bulkImportLogService.LogSuccess(userContact, bulkImportStatusId, "File '" + fileBucket + "/" + fileKey + "' created.", dataRowIdx + "");
                    }
                }
                else
                {
                    //check if cloud file is linked to sample
                    bool isCloudFileLinkedToSample = 
                        db.GNSamples
                        .Include(s => s.CloudFiles)
                        .Where(s => 
                            (
                            s.Id == sample.Id 
                            && s.CloudFiles.Count(c => c.Id == cloudFile.Id) == 0
                            )
                        )
                        .FirstOrDefault() != null 
                        ? false : true;

                    //if no linkage between file and sample exists, then create one
                    if (!isCloudFileLinkedToSample)
                    {
                        this.cloudFileService.AssociateCloudFileToSample(cloudFile.Id, sample.Id);
                    }

                    this.bulkImportLogService.LogDuplicate(userContact, bulkImportStatusId, "File '" + fileBucket + "/" + fileKey + "' is a duplicate. File not created.", dataRowIdx + "");
                }
            }

            if (cloudFile == null)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "File '" + fileBucket + "/" + fileKey + "' not created due to unknown error.", dataRowIdx + "");
            }

            return cloudFile;
        }

        private async Task<GNSample> InsertSample(GNBulkImportStaging dataRow, int dataRowIdx, GNAnalysisRequest analysis,
            GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNSample sample = null;

            string sampleName = dataRow.SAMPLE_NAME;
            if (sampleName.Length > 30)
            {
                sampleName = dataRow.SAMPLE_NAME.Substring(0, 30);
            }

            if (analysis != null && !string.IsNullOrEmpty(sampleName))
            {
                GNSample lastSample = (GNSample)lastSampleFileRecord["SAMPLE"];

                if (lastSample != null && lastSample.Name == sampleName)
                {
                    sample = lastSample;
                }
                else
                {
                    //get sample by name
                    sample = this.db.GNSamples
                        .Where(s => (
                            s.Name.ToLower() == sampleName.ToLower()
                            && s.GNOrganizationId == userContact.GNOrganizationId
                            )
                        )
                        .FirstOrDefault();

                    if (sample == null)
                    {
                        try
                        {
                            GNSampleType sampleType = null;
                            string sampleTypeName = dataRow.SAMPLE_TYPE.ToString();

                            if (!string.IsNullOrEmpty(sampleTypeName))
                            {
                                sampleType = this.db.GNSampleTypes
                                    .Where(st => st.Name.ToLower() == sampleTypeName.ToLower())
                                    .FirstOrDefault();
                            }

                            if (sampleType != null)
                            {
                                string sampleGender = dataRow.SAMPLE_GENDER.ToString();

                                switch (sampleGender)
                                {
                                    case "female": sampleGender = "F"; break;
                                    case "male": sampleGender = "M"; break;
                                    default: sampleGender = "U"; break;
                                }

                                bool isPairEnded = false;
                                string sampleReadType = dataRow.SAMPLE_READ_TYPE.ToString();
                                switch (sampleReadType)
                                {
                                    case "PE": isPairEnded = true; break;
                                    default: isPairEnded = false; break;
                                }

                                /**
                                 * tfrege August 2016. Changing to support RNA and TumorNormal imports
                                 */
                                //RNA, DNA or TUMOR
                                string sampleQualifierCode = dataRow.SAMPLE_QUALIFIER.ToString().Trim().ToUpper();

                                //GNSampleQualifierCode

                                try
                                {
                                    sample = new GNSample
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = sampleName,
                                        GNSampleTypeId = sampleType.Id,
                                        Gender = sampleGender,
                                        IsPairEnded = isPairEnded,
                                        IsReady = true,
                                        CurrentAnalysisRequestId = analysis.Id.ToString(),
                                        GNOrganizationId = userContact.GNOrganizationId,
                                        CreatedBy = userContact.Id,
                                        CreateDateTime = DateTime.Now,
                                        CanCreate = true,
                                        GNSampleQualifierCode = sampleQualifierCode
                                    };

                                    sample = await this.sampleService.Insert(userContact, sample);

                                    this.bulkImportLogService.LogSuccess(userContact, bulkImportStatusId, "Sample '" + sampleName + "' created.", dataRowIdx + "");
                                }
                                catch (Exception e3)
                                {
                                    string m3 = e3.Message;
                                }
                                
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Error(e.Message);
                        }
                        
                    }
                    else
                    {
                        //if no linkage between sample and analysis exists, then create one
                        if(db.GNAnalysisRequestGNSamples.Count(ars=> (ars.GNSampleId == sample.Id && ars.GNAnalysisRequestId == analysis.Id)) == 0)
                        {
                            this.sampleService.AssociateSampleToAnalysisRequest(sample.Id, analysis.Id);
                        }
                    }
                }
            }

            if (sample == null)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Sample '" + sampleName + "' not created due to unknown error.", dataRowIdx + "");
            }

            return sample;
        }

        private async Task<GNAnalysisRequest> InsertAnalysis(GNBulkImportStaging dataRow, int dataRowIdx, GNTeam team, GNProject project,
            GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNAnalysisRequest analysisRequest = null;

            string analysisName = dataRow.ANALYSIS_NAME;
            if (analysisName.Length > 30)
            {
                analysisName = dataRow.ANALYSIS_NAME.ToString().Substring(0, 30);
            }

            if (team != null && project != null && !string.IsNullOrEmpty(analysisName))
            {
                GNAnalysisRequest lastAnalysis = (GNAnalysisRequest)lastSampleFileRecord["ANALYSIS"];

                if (lastAnalysis != null && lastAnalysis.Description == analysisName)
                {
                    analysisRequest = lastAnalysis;
                }
                else
                {
                    analysisRequest = this.db.GNAnalysisRequests
                        .Where(a => (
                            a.Description.ToLower() == analysisName.ToLower()
                            && a.Project.Teams.Select(t => t.OrganizationId).FirstOrDefault() == userContact.GNOrganizationId
                            )
                        )
                        .FirstOrDefault();

                    if (analysisRequest == null)
                    {
                        try
                        {
                            GNAnalysisType analysisType = null;
                            string analysisTypeName = dataRow.SAMPLE_TYPE.ToString();

                            if (!string.IsNullOrEmpty(analysisTypeName))
                            {
                                analysisType = this.db.GNAnalysisTypes
                                    .Where(at => at.Name.ToLower() == analysisTypeName.ToLower())
                                    .FirstOrDefault();
                            }

                            /**
                             * tfrege August 2016. Changing to support RNA and TumorNormal imports
                             */
                            string analysisQualifier = dataRow.ANALYSIS_TYPE.ToString().Trim().ToUpper();
                            string analysisAdaptor = dataRow.ANALYSIS_ADAPTOR.ToString().Trim().ToUpper();
                            
                            if (analysisType != null)
                            {
                                try
                                {
                                    analysisRequest = new GNAnalysisRequest
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = analysisName,
                                        AllSamplesAreReady = true,
                                        AnalysisType = analysisType,
                                        AnalysisTypeId = analysisType.Name,
                                        AWSRegionSystemName = userContact.GNOrganization.AWSConfig.AWSRegionSystemName,
                                        GNProjectId = project.Id,
                                        CreatedBy = userContact.Id,
                                        CreateDateTime = DateTime.Now,
                                        CanCreate = true,
                                        GNAnalysisRequestTypeCode = analysisQualifier,
                                        GNAnalysisAdaptorCode = analysisAdaptor
                                    };

                                    analysisRequest = await this.analysisRequestService.Insert(userContact, analysisRequest);

                                    this.bulkImportLogService.LogSuccess(userContact, bulkImportStatusId, "Analysis '" + analysisName + "' created.", dataRowIdx + "");
                                }
                                catch (Exception e3)
                                {
                                    string m3 = e3.Message;
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            
                            logger.Error(e.Message);
                        }
                        
                    }
                }

                if (analysisRequest != null)
                {
                    if (!string.IsNullOrEmpty(dataRow.AUTO_START)
                        && (dataRow.AUTO_START.Trim().ToUpper() == "Y" || dataRow.AUTO_START.Trim().ToUpper() == "YES"
                         || dataRow.AUTO_START.Trim().ToUpper() == "TRUE"))
                    {
                        analysisRequest.AutoStart = true;
                    }
                }
            }

            if (analysisRequest == null)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Analysis '" + analysisName + "' not created due to unknown error.", dataRowIdx + "");
            }

            return analysisRequest;
        }

        private async Task<GNProject> InsertProject(GNBulkImportStaging dataRow, int dataRowIdx, GNTeam team, GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNProject project = null;

            string projectName = dataRow.PROJECT_NAME.ToString();

            if (team != null && !string.IsNullOrEmpty(projectName))
            {
                GNProject lastProject = (GNProject)lastSampleFileRecord["PROJECT"];

                if (lastProject != null && lastProject.Name == projectName)
                {
                    project = lastProject;
                }
                else
                {
                    try
                    {
                        project = this.db.GNProjects
                        .Where(p => (
                            p.Name.ToLower() == projectName.ToLower()
                            && p.Teams.Select(t => t.OrganizationId).FirstOrDefault() == userContact.GNOrganizationId
                            )
                         )
                        .FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        string m = e.Message;
                    }
                    

                    if (project == null)
                    {
                        project = new GNProject
                        {
                            Id = Guid.NewGuid(),
                            Name = projectName,
                            Description = projectName,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(30),
                            ProjectLeadId = userContact.Id.ToString(),
                            TeamId = team.Id.ToString(),
                            CreatedBy = userContact.Id,
                            CreateDateTime = DateTime.Now,
                            CanCreate = true
                        };

                        project = await this.projectService.Insert(userContact, project);

                        this.bulkImportLogService.LogSuccess(userContact, bulkImportStatusId, "Project '" + projectName + "' created.", dataRowIdx + "");
                    }
                }
            }

            if (project == null)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Project '" + projectName + "' not created due to unknown error.", dataRowIdx + "");
            }

            return project;
        }

        private async Task<GNAnalysisRequestGroup> InsertGroup(GNBulkImportStaging dataRow, int dataRowIdx, GNAnalysisRequest analysis, GNSample sample, GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNAnalysisRequestGroup group = null;

            string groupName = dataRow.ANALYSIS_GROUP_NAME.ToString();

            GNBulkImportGroupStaging stagingRecord = db.GNBulkImportGroupStagings.Where(a => a.ANALYSIS_NAME.Equals(analysis.Description) && a.CONTROL_GROUP_NAME.Equals(groupName) && a.GNBulkImportStatusId.Equals(bulkImportStatusId)).FirstOrDefault();


            // insert sample to control group in analysis
            System.Console.WriteLine("------>>>> STAGING RECORD:::::: " + stagingRecord.COMPARISON_GROUP_NAME);

            //Check if comparison group exists
            GNAnalysisRequestGroup comparisonGroup = db.GNAnalysisRequestGroups.Where(a => a.Name.Equals(stagingRecord.COMPARISON_GROUP_NAME) && a.GNAnalysisRequestId.Equals(analysis.Id)).FirstOrDefault();


            try
            {
                //Check if group exists
                group = db.GNAnalysisRequestGroups.Where(a => a.Name.Equals(groupName) && a.GNAnalysisRequestId.Equals(analysis.Id)).FirstOrDefault();

                if (group == null)
                {
                    group = new GNAnalysisRequestGroup
                    {
                        Id = Guid.NewGuid(),
                        Name = groupName,
                        GNAnalysisRequestId = analysis.Id,
                        GNAnalysisRequest = analysis,
                        CreatedBy = userContact.Id,
                        CreateDateTime = DateTime.Now
                    };

                    db.GNAnalysisRequestGroups.Add(group);
                    db.SaveChanges();
                }


                // insert sample to control group in analysis
                System.Console.WriteLine("------>>>> Group " + group.Name + " and Sample " + sample.Name);

                string sql = "INSERT INTO [gn].[GNAnalysisRequestGroupGNSample]" +
                   "([GNAnalysisRequestGroups_Id],[GNSamples_Id])";

                var txn = db.Database.BeginTransaction();
                db.Database.ExecuteSqlCommand(
                    sql,
                    new SqlParameter("@GNAnalysisRequestGroups_Id", group.Id),
                    new SqlParameter("@GNSamples_Id", sample.Id));

                txn.Commit();

            }
            catch (Exception controlGroupEx)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Control Group '" + groupName + "' not created due to unknown error.", dataRowIdx + " " + controlGroupEx.Message);
            }


            try
            {
                if (comparisonGroup == null)
                {
                    comparisonGroup = new GNAnalysisRequestGroup
                    {
                        Id = Guid.NewGuid(),
                        Name = groupName,
                        GNAnalysisRequestId = analysis.Id,
                        GNAnalysisRequest = analysis,
                        CreatedBy = userContact.Id,
                        CreateDateTime = DateTime.Now
                    };

                    db.GNAnalysisRequestGroups.Add(comparisonGroup);
                    db.SaveChanges();
                }

                // create the comparison set between both groups
                this.analysisRequestService.CreateAnalysisRequestGroupAssociation(analysis.Id, group.Id, comparisonGroup.Id);

            }
            catch (Exception comparisonGroupEx)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Comparison Group for control group '" + groupName + "' not created due to unknown error.", dataRowIdx + " " + comparisonGroupEx.Message);
            }

            try
            {
                // insert sample to control group in analysis
                System.Console.WriteLine("------>>>> Group " + group.Name + " and Sample " + sample.Name);

                string sql = "INSERT INTO [gn].[GNAnalysisRequestGroupGNSample]" +
                   "([GNAnalysisRequestGroups_Id],[GNSamples_Id])";

                var txn = db.Database.BeginTransaction();
                db.Database.ExecuteSqlCommand(
                    sql,
                    new SqlParameter("@GNAnalysisRequestGroups_Id", group.Id),
                    new SqlParameter("@GNSamples_Id", sample.Id));

                txn.Commit();
            }
            catch (Exception sampleGroupEx)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Comparison Set for Analysis " + analysis.Description + " " + analysis.Id + " and groups " + groupName + " , " + comparisonGroup.Name + " could not be created :"+ dataRowIdx + " " + sampleGroupEx.Message);
            }

            return group;
        }



        private async Task<GNTeam> InsertTeam(GNBulkImportStaging dataRow, int dataRowIdx, GNContact userContact, Guid bulkImportStatusId, Dictionary<string, object> lastSampleFileRecord)
        {
            GNTeam team = null;

            string teamName = dataRow.TEAM_NAME.ToString();

            if (!string.IsNullOrEmpty(teamName))
            {
                GNTeam lastTeam = (GNTeam)lastSampleFileRecord["TEAM"];
                
                if(lastTeam != null && lastTeam.Name == teamName)
                {
                    team = lastTeam;
                }
                else
                {
                    try
                    {
                        team = this.db.GNTeams
                        .Where(t => (
                            t.Name.ToLower() == teamName.ToLower()
                            && t.OrganizationId == userContact.GNOrganizationId
                            )
                        )
                        .FirstOrDefault();

                    }
                    catch (Exception e)
                    {
                        string ms = e.Message;
                        
                    }
                    
                    if (team == null)
                    {
                        try
                        {
                            team = new GNTeam
                            {
                                Id = Guid.NewGuid(),
                                Name = teamName,
                                OrganizationId = userContact.GNOrganizationId,
                                GNContactId = userContact.Id,
                                CreatedBy = userContact.Id,
                                CreateDateTime = DateTime.Now
                            };

                            team.CanCreate = true;

                            team = await this.teamService.Insert(userContact, team);

                            this.bulkImportLogService.LogSuccess(userContact, bulkImportStatusId, "Team '" + teamName + "' created.", dataRowIdx + "");
                        }
                        catch (Exception e2)
                        {

                            string m2 = e2.Message;
                        }
                        
                    }
                }
            }

            if(team == null)
            {
                this.bulkImportLogService.LogError(userContact, bulkImportStatusId, "Team '" + teamName + "' not created due to unknown error.", dataRowIdx + "");
            }

            return team;
        }

        private bool LoadExcelDataSet(GNBulkImportStatus bulkImportStatus)
        {
            bool success = false;
            OleDbConnection conn = null;

            System.Console.WriteLine("------" + bulkImportStatus.FileExtension);

            System.Console.WriteLine("------" + bulkImportStatus.FileURI);


            try
            {
                string connStr = null;

                //load file from cloud
                string fileLocation = LoadFile(bulkImportStatus);

                //load xls file
                if (bulkImportStatus.FileExtension == ".xls")
                {
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //load xlsx file
                else if (bulkImportStatus.FileExtension == ".xlsx")
                {
                    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                //read data from excel file
                DataTable dtExcelData = new DataTable();
                DataTable tableColumns = null;
                using (OleDbConnection excelCon = new OleDbConnection(connStr))
                {
                    excelCon.Open();

                    string excelSelectSQL = "SELECT ";

                    string tableName = excelCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[1]["TABLE_NAME"].ToString();
                    tableColumns = excelCon.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                    foreach (DataRow row in tableColumns.Rows)
                    {
                        string columnNameColumn = (string)row["COLUMN_NAME"];

                        excelSelectSQL += columnNameColumn + ", ";
                        dtExcelData.Columns.Add(new DataColumn(columnNameColumn, typeof(string)));
                    }

                    excelSelectSQL += 
                        " '" + bulkImportStatus.Id + "' as GNBulkImportStatusId " +
                        "FROM [" + tableName + "]";
                    dtExcelData.Columns.Add(new DataColumn("GNBulkImportStatusId", typeof(Guid)));


                    System.Console.WriteLine("----------------------\n SQL: [" + excelSelectSQL + "]");

                    using (OleDbDataAdapter oda = new OleDbDataAdapter(excelSelectSQL, excelCon))
                    {
                        oda.Fill(dtExcelData);
                    }

                    excelCon.Close();
                }

                //import data into staging table
                string dbConStr = ConfigurationManager.ConnectionStrings["GN_DB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(dbConStr))
                {
                    con.Open();

                    string cleanStagingSql = "DELETE FROM gn.GNBulkImportStaging WHERE GNBulkImportStatusId = '" + bulkImportStatus.Id + "'";
                    SqlCommand cleanStagingCmd = new SqlCommand(cleanStagingSql, con);
                    int result = cleanStagingCmd.ExecuteNonQuery();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "gn.GNBulkImportStaging";

                        //Map the Excel columns with that of the database table
                        foreach (DataRow row in tableColumns.Rows)
                        {
                            string columnNameColumn = (string)row["COLUMN_NAME"];

                            sqlBulkCopy.ColumnMappings.Add(columnNameColumn, columnNameColumn);
                        }

                        sqlBulkCopy.ColumnMappings.Add("GNBulkImportStatusId", "GNBulkImportStatusId");

                        sqlBulkCopy.WriteToServer(dtExcelData);
                    }

                    con.Close();
                }

                success = true;
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error loading excel data set!", e);
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return success;
        }

        /**
         * tfrege August 2016
         */
        private bool LoadGroupsExcelDataSet(GNBulkImportStatus bulkImportStatus)
        {
            bool success = false;
            OleDbConnection conn = null;

            try
            {
                string connStr = null;

                //load file from cloud
                string fileLocation = LoadFile(bulkImportStatus);

                //load xls file
                if (bulkImportStatus.FileExtension == ".xls")
                {
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //load xlsx file
                else if (bulkImportStatus.FileExtension == ".xlsx")
                {
                    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                //read data from the 2nd sheet of the excel file (GROUPS COMPARISON)
                DataTable dtExcelData = new DataTable();
                DataTable tableColumns = null;
                using (OleDbConnection excelCon = new OleDbConnection(connStr))
                {
                    excelCon.Open();

                    string excelSelectSQL = "SELECT ";

                    // tfrege: Rows[0] indicates we want the 2nd sheet of the Excel file, which is the one with the groups comparisons
                    string tableName = excelCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    tableColumns = excelCon.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                    foreach (DataRow row in tableColumns.Rows)
                    {
                        string columnNameColumn = (string)row["COLUMN_NAME"];

                        excelSelectSQL += columnNameColumn + ", ";
                        dtExcelData.Columns.Add(new DataColumn(columnNameColumn, typeof(string)));
                    }

                    excelSelectSQL +=
                        " '" + bulkImportStatus.Id + "' as GNBulkImportStatusId " +
                        "FROM [" + tableName + "]";
                    dtExcelData.Columns.Add(new DataColumn("GNBulkImportStatusId", typeof(Guid)));

                    System.Console.WriteLine("----------------------\n SQL: [" + excelSelectSQL + "]");

                    using (OleDbDataAdapter oda = new OleDbDataAdapter(excelSelectSQL, excelCon))
                    {
                        oda.Fill(dtExcelData);
                    }

                    excelCon.Close();
                }

                //import data into staging table
                string dbConStr = ConfigurationManager.ConnectionStrings["GN_DB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(dbConStr))
                {
                    con.Open();

                    string cleanStagingSql = "DELETE FROM gn.GNBulkImportGroupStagings WHERE GNBulkImportStatusId = '" + bulkImportStatus.Id + "'";
                    SqlCommand cleanStagingCmd = new SqlCommand(cleanStagingSql, con);
                    int result = cleanStagingCmd.ExecuteNonQuery();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "gn.GNBulkImportGroupStagings";

                        //Map the Excel columns with that of the database table
                        foreach (DataRow row in tableColumns.Rows)
                        {
                            string columnNameColumn = (string)row["COLUMN_NAME"];

                            System.Console.WriteLine("----------------------\n columnNameColumn: [" + columnNameColumn + "]");

                            sqlBulkCopy.ColumnMappings.Add(columnNameColumn, columnNameColumn);
                        }

                        sqlBulkCopy.ColumnMappings.Add("GNBulkImportStatusId", "GNBulkImportStatusId");

                        sqlBulkCopy.WriteToServer(dtExcelData);
                    }

                    con.Close();
                }

                try
                {
                    //delete file
                    bool deleteSuccess = DeleteFile(bulkImportStatus);

                    if (!deleteSuccess)
                    {
                        throw new Exception("Delete failed!");
                    }
                }
                catch (Exception e)
                {
                    LogUtil.Error(logger, "Error deleting physical file for bulk import id :" + bulkImportStatus.Id, e);
                }

                success = true;
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error loading excel data set!", e);
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return success;
        }

        private string LoadFile(GNBulkImportStatus bulkImportStatus)
        {
            string fileLocation = null;

            if (bulkImportStatus.FileURIType == "S3")
            {
                //fetch file from S3
                Guid awsConfigId = this.db.GNOrganizations
                    .Where(o => o.Id == bulkImportStatus.CreatedByContact.GNOrganizationId)
                    .Select(o => o.AWSConfigId)
                    .FirstOrDefault();

                this.cloudFileService.InitCloudServices(awsConfigId);

                string bucket = bulkImportStatus.FileURI.Split('/')[0];
                string key = bulkImportStatus.FileURI.Replace(bucket + "/", "");
                fileLocation = System.IO.Path.GetTempPath() + bulkImportStatus.FileURI.Replace('/', '\\');
                this.cloudFileService.cloudStorageService.WriteObjectToFile(bucket, key, fileLocation);
            }
            else if (bulkImportStatus.FileURIType == "LOCAL")
            {
                fileLocation = bulkImportStatus.FileURI;
            }

            return fileLocation;
        }

        private bool DeleteFile(GNBulkImportStatus bulkImportStatus)
        {
            bool success = false;

            if (bulkImportStatus.FileURIType == "S3")
            {
                //fetch file from S3
                Guid awsConfigId = this.db.GNOrganizations
                    .Where(o => o.Id == bulkImportStatus.CreatedByContact.GNOrganizationId)
                    .Select(o => o.AWSConfigId)
                    .FirstOrDefault();

                this.cloudFileService.InitCloudServices(awsConfigId);

                string bucket = bulkImportStatus.FileURI.Split('/')[0];
                string key = bulkImportStatus.FileURI.Replace(bucket + "/", "");
                success = this.cloudFileService.cloudStorageService.DeleteS3File(bucket, key);
            }
            else if (bulkImportStatus.FileURIType == "LOCAL")
            {
                File.Delete(bulkImportStatus.FileURI);
                success = true;
            }

            return success;
        }

        private String[] GetExcelWorksheetNames(string excelConnectionString)
        {
            String[] excelSheets = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(excelConnectionString);

                if (conn != null)
                {
                    conn.Open();

                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dt != null)
                    {
                        excelSheets = new String[dt.Rows.Count];

                        int t = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error getting excel worksheet names!", e);
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return excelSheets;
        }
    }

    public class BulkImportStatusService : GNEntityService<GNBulkImportStatus>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BulkImportStatusService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNBulkImportStatus>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNBulkImportStatus> entities =
                await db.GNBulkImportStatus
                .OrderByDescending(i=>i.CreateDateTime)
                .ToListAsync();

            return entities;
        }

        public async Task<List<GNBulkImportStatus>> FindMyImports(GNContact userContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNBulkImportStatus> entities =
                await db.GNBulkImportStatus
                .Where(i=>i.CreatedBy == userContact.Id)
                .OrderByDescending(i => i.CreateDateTime)
                .ToListAsync();

            return entities;
        }

        public override async Task<GNBulkImportStatus> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNBulkImportStatus.FindAsync(keys);
        }
    }

    public class BulkImportLogService : GNEntityService<GNBulkImportLog>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BulkImportLogService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNBulkImportLog>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNBulkImportLog> entities =
                await db.GNBulkImportLogs
                .ToListAsync();

            return entities;
        }

        public override async Task<GNBulkImportLog> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNBulkImportLogs.FindAsync(keys);
        }

        public void LogSuccess(GNContact userContact, Guid bulkImportStatusId, string message, string recordRowId = "")
        {
            LogMessage(userContact, bulkImportStatusId, message, false, false, recordRowId);

            if(!string.IsNullOrEmpty(recordRowId))
            {
                if(message.ToLower().StartsWith("team"))
                {
                    IncrementBulkImportStatusField(bulkImportStatusId, "TeamsCreatedCount");
                }
                else if (message.ToLower().StartsWith("project"))
                {
                    IncrementBulkImportStatusField(bulkImportStatusId, "ProjectsCreatedCount");
                }
                else if (message.ToLower().StartsWith("analysis"))
                {
                    IncrementBulkImportStatusField(bulkImportStatusId, "AnalysesCreatedCount");
                }
                else if (message.ToLower().StartsWith("sample"))
                {
                    IncrementBulkImportStatusField(bulkImportStatusId, "SamplesCreatedCount");
                }
                else if (message.ToLower().StartsWith("file"))
                {
                    IncrementBulkImportStatusField(bulkImportStatusId, "FilesCreatedCount");
                    IncrementBulkImportStatusField(bulkImportStatusId, "ImportedRecordCount");
                }
            }
        }

        public void LogError(GNContact userContact, Guid bulkImportStatusId, string message, string recordRowId = "")
        {
            LogMessage(userContact, bulkImportStatusId, message, true, false, recordRowId);

            if (!string.IsNullOrEmpty(recordRowId) && message.ToLower().StartsWith("file"))
            {
                IncrementBulkImportStatusField(bulkImportStatusId, "FailedRecordCount");
            }
        }

        public void LogDuplicate(GNContact userContact, Guid bulkImportStatusId, string message, string recordRowId = "")
        {
            LogMessage(userContact, bulkImportStatusId, message, false, true, recordRowId);

            if (!string.IsNullOrEmpty(recordRowId) && message.ToLower().StartsWith("file"))
            {
                IncrementBulkImportStatusField(bulkImportStatusId, "DuplicateRecordCount");
            }
        }

        public void LogMessage(GNContact userContact, Guid bulkImportStatusId, string message,
            bool isError, bool isDuplicate, string recordRowId = "")
        {
            string sql = "INSERT INTO [gn].[GNBulkImportLogs]" +
                "([Id],[RecordRowId],[Message],[IsError],[IsDuplicate],[CreatedBy],[CreateDateTime],[GNBulkImportStatusId]) "+
                "VALUES(@id,@recordRowId,@message,@isError,@isDuplicate,@createdBy,@createDateTime,@bulkImportStatusId)";

            var txn = db.Database.BeginTransaction();
            db.Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@id", Guid.NewGuid()),
                new SqlParameter("@recordRowId", recordRowId),
                new SqlParameter("@message", message),
                new SqlParameter("@isError", isError),
                new SqlParameter("@isDuplicate", isDuplicate),
                new SqlParameter("@createdBy", userContact.Id),
                new SqlParameter("@createDateTime", DateTime.Now),
                new SqlParameter("@bulkImportStatusId", bulkImportStatusId));
            txn.Commit();
        }

        private void IncrementBulkImportStatusField(Guid bulkImportStatusId, string fieldName)
        {
            string sql = "UPDATE [gn].[GNBulkImportStatus] " +
                "SET [" + fieldName + "] = ([" + fieldName + "] + 1)  " +
                "WHERE [Id] = @BulkImportStatusId";

            var txn = db.Database.BeginTransaction();
            db.Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@BulkImportStatusId", bulkImportStatusId));
            txn.Commit();
        }
    }

    public class BulkImportCloudMessageService : GNCloudMessageService<BulkImportMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_BULK_IMPORT";

        public BulkImportService bulkImportService { get; set; }


        public BulkImportCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
            this.bulkImportService = new BulkImportService(AWSConfigId,forBatch:true);
        }

        public override bool ProcessMessage(BulkImportMessage messageObj, object queueMessage)
        {
            bool success = false;

            GNBulkImportStatus bulkImportStatus = null;

            try
            {
                string bucket = messageObj.Records.FirstOrDefault().s3.s3BucketInfo.name;
                string key = messageObj.Records.FirstOrDefault().s3.s3ObjectInfo.key;

                string[] keyParts = key.Split('/');
                Guid bulkImportStatusId = Guid.Parse(keyParts[1]);

                var t = Task.Run(async delegate
                {
                    return await this.bulkImportService.bulkImportStatusService.Find(bulkImportStatusId);
                });

                bulkImportStatus = t.Result;
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Unable to process Bulk Import message", e);
                throw e;
            }

            if (bulkImportStatus == null)
            {
                throw new Exception("Unable to retrieve bulk import status from DB!");
            }

            if (bulkImportStatus != null)
            {
                success = true;

                try
                {
                    if (bulkImportStatus.CreatedByContact == null)
                    {
                        bulkImportStatus.CreatedByContact = this.db.GNContacts.Find(bulkImportStatus.CreatedBy);
                    }

                    var t2 = Task.Run(async delegate
                    {
                        return await this.bulkImportService.DoImport(bulkImportStatus);
                    });
                }
                catch (Exception e)
                {
                    LogUtil.Warn(logger, "Unable to execute Bulk Import ("+bulkImportStatus.Id+"). However, the SQS message was consumed and deleted", e);
                }
            }

            return success;
        }
    }

    public class BulkImportMessage : S3EventMessage
    {
        //nothing to implement
    }
}
