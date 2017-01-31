using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using GenomeNext.Cloud.CloudNoSQL;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;
using GenomeNext.Billing;
using GenomeNext.Data.Metadata;

namespace GenomeNext.App
{
    public class SampleService : GNEntityService<GNSample>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "SAMPLE";

        public AspNetRoleService aspNetRoleService { get; set; }
        public TeamService teamService { get; set; }
        public CloudFileService cloudFileService { get; set; }

        public SampleService(GNEntityModelContainer db, IdentityModelContainer identityDB)
            : base(db)
        {
            base.db = db;
            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.teamService = new TeamService(db,identityDB);
            this.cloudFileService = new CloudFileService(db,identityDB);
        }

        public override async Task<List<GNSample>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSample> samples = null;
            IQueryable<GNSample> samplesResult = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                samples = db.GNSamples
                    .Include(s => s.Organization);
            }
            //ORG_MANAGER, TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                samples = db.GNSamples
                    .Include(s => s.Organization)
                    .Where(s => s.GNOrganizationId == userContact.GNOrganizationId);
            }
            samplesResult = samples;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    samplesResult = samplesResult.Where(t => t.Organization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Sample Name"))
                {
                    filterVal = (string)filters["Sample Name"];
                    samplesResult = samplesResult.Where(t => t.Name.ToUpper().Contains(filterVal.ToUpper()));
                }

                if (filters.ContainsKey("Sample Type"))
                {
                    filterVal = (string)filters["Sample Type"];
                    samplesResult = samplesResult.Where(t => t.SampleType.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Gender"))
                {
                    filterVal = (string)filters["Gender"];
                    samplesResult = samplesResult.Where(t => t.Gender.Equals(filterVal));
                }

                if (filters.ContainsKey("GNSampleTypeId"))
                {
                    var intFilter = Convert.ToInt32(filters["GNSampleTypeId"]);
                    samplesResult = samplesResult.Where(t => t.SampleType.Id.Equals(intFilter));
                }

                if (filters.ContainsKey("GNSampleQualifierCode"))
                {
                    filterVal = (string)filters["GNSampleQualifierCode"];
                    samplesResult = samplesResult.Where(t => t.GNSampleQualifierCode.Equals(filterVal));
                }

                if (filters.ContainsKey("GroupId"))
                {
                    var guidFilter = Guid.Parse(filters["GroupId"].ToString());
                    GNAnalysisRequestGroup Group = db.GNAnalysisRequestGroups.Where(a => a.Id.Equals(guidFilter)).FirstOrDefault();
                    samplesResult = samplesResult.Where(t => t.GNAnalysisRequestGroups.Contains(Group));
                }

                if (filters.ContainsKey("CreatedBy"))
                {
                    var guidFilter = Guid.Parse(filters["CreatedBy"].ToString());
                    samplesResult = samplesResult.Where(t => t.CreatedBy == guidFilter);
                }

                if (filters.ContainsKey("IsReady"))
                {
                    bool boolFilter = true;
                    filterVal = (string)filters["IsReady"];
                    if (filterVal == "N")
                    {
                        boolFilter = false;
                    }

                    samplesResult = samplesResult.Where(t => t.IsReady == boolFilter);
                }

                if (filters.ContainsKey("GNOrganizationId"))
                {
                    var guidFilter = Guid.Parse((string)filters["GNOrganizationId"]);
                    samplesResult = samplesResult.Where(t => t.Organization.Id.Equals(guidFilter));
                }

                if (filters.ContainsKey("GNLeftSampleId"))
                {
                    var guidFilter = Guid.Parse((string)filters["GNLeftSampleId"]);
                    samplesResult = samplesResult.Where(t => t.Id.Equals(guidFilter) == false); // && !this.FindNeedleInHaystack(t));
                }

               
                 
                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    samplesResult = samplesResult
                        .Where(t => t.Name.Contains(filterVal)
                            || t.Organization.Name.Contains(filterVal)
                            || t.SampleType.Name.Contains(filterVal));
                }
            }

            //Order By Results
            samplesResult = samplesResult
                .OrderBy(t => t.SampleType.Name)
                .OrderBy(t => t.Name)
                .OrderBy(t => t.Organization.Name)
                .OrderByDescending(t => t.CreateDateTime);

            //Limit Result Size
            samplesResult = samplesResult.Skip(start).Take(end - start);


            return EvalEntityListSecurity(userContact, await samplesResult.ToListAsync());
        }

        public bool FindNeedleInHaystack(GNSample Needle)
        {
            Dictionary<string, object> rightFilters = new Dictionary<string, object>();
            rightFilters.Add("GNLeftSampleId", Needle.Id);
            var RightSamples = db.GNSampleRelationships.Find(rightFilters).GNLeftSample.GNSampleRightRelationships;

            foreach(GNSampleRelationship Item in RightSamples)
            {
                if(Item.GNLeftSample.Id.Equals(Needle.Id))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<GNSample>> FindByOrganizationId(GNContact userContact, Guid organizationId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNSample> entities =
                await db.GNSamples
                .Include(g => g.Organization)
                .Where(s => s.GNOrganizationId == organizationId)
                //.OrderByDescending(s => s.CreateDateTime)
                .ToListAsync();

            return EvalEntityListSecurity(userContact, entities);
        }

        public override async Task<GNSample> Find(GNContact userContact, params object[] keys)
        {
            await this.cloudFileService.InitStorageTransactionLogging(userContact);
            return await base.Find(userContact, keys);
        }

        public override async Task<GNSample> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await db.GNSamples.FindAsync(keys);
        }

        public override async Task<GNSample> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNSample sample = (GNSample)entity;

            try
            {
                sample.CreateDateTime = DateTime.Now;

                sample = await base.Insert(sample);

                Guid analysisRequestId = Guid.Empty;
                Guid.TryParse(sample.CurrentAnalysisRequestId, out analysisRequestId);
                AssociateSampleToAnalysisRequest(sample.Id, analysisRequestId);
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to create sample.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            bool auditResult = new GNCloudNoSQLService().LogEvent(sample.CreatedByContact, sample.Id, this.ENTITY, "N/A", "EVENT_INSERT");
 
            return sample;
        }

        public void AssociateSampleToAnalysisRequest(Guid sampleId, Guid analysisRequestId)
        {
            if (analysisRequestId != Guid.Empty)
            {
                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNAnalysisRequestGNSamples]([GNAnalysisRequestId],[GNSampleId]) " +
                    "VALUES(@analysisRequestId, @sampleId)",
                    new SqlParameter("@analysisRequestId", analysisRequestId),
                    new SqlParameter("@sampleId", sampleId));

                tx.Commit();
            }
        }

        public async Task<int> AddSampleToAnalysisRequest(Guid? sampleId, string analysisRequestId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (sampleId != null && !string.IsNullOrEmpty(analysisRequestId))
                {
                    var tx = db.Database.BeginTransaction();

                    result = db.Database.ExecuteSqlCommand(
                        "INSERT INTO [gn].[GNAnalysisRequestGNSamples]([GNAnalysisRequestId],[GNSampleId]) " +
                        "VALUES(@analysisRequestId, @sampleId)",
                        new SqlParameter("@analysisRequestId", Guid.Parse(analysisRequestId)),
                        new SqlParameter("@sampleId", sampleId));

                    tx.Commit();
                }

                return result;            
            });
        }

        public async Task<int> UpdateSampleAttributesOnAnalysisRequest(Guid? sampleId, string analysisRequestId, string AffectedIndicator, string TargetIndicator)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (sampleId != null && !string.IsNullOrEmpty(analysisRequestId))
                {
                    var tx = db.Database.BeginTransaction();

                    result = db.Database.ExecuteSqlCommand(
                        "UPDATE [gn].[GNAnalysisRequestGNSamples]"+
                        "SET  AffectedIndicator = '"+ AffectedIndicator + "',"+ 
                        "     TargetIndicator = '"+ TargetIndicator + "' "+
                        "WHERE GNAnalysisRequestId = '"+ analysisRequestId + "'"+
                        "AND GNSampleId = '"+ sampleId + "'");

                    tx.Commit();
                }

                return result;
            });
        }

        public async Task<int> RemoveSampleFromAnalysisRequest(Guid? sampleId, string analysisRequestId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (sampleId != null && !string.IsNullOrEmpty(analysisRequestId))
                {
                    var tx = db.Database.BeginTransaction();

                    db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNAnalysisRequestGNSamples] " +
                        "WHERE [GNAnalysisRequestId] = @analysisRequestId " +
                        "AND [GNSampleId] = @sampleId",
                        new SqlParameter("@analysisRequestId", Guid.Parse(analysisRequestId)),
                        new SqlParameter("@sampleId", sampleId));

                    tx.Commit();
                }

                return result;            
            });
        }

        public async Task<int> AddSampleToAnalysisRequestGroup(Guid sampleId, Guid groupId)
        {
            int result = 0;

            if (sampleId != Guid.Empty && groupId != Guid.Empty)
            {
                var tx = db.Database.BeginTransaction();

                result = db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNAnalysisRequestGroupGNSample]([GNAnalysisRequestGroups_Id],[GNSamples_Id]) " +
                    "VALUES(@groupId, @sampleId)",
                    new SqlParameter("@groupId", groupId),
                    new SqlParameter("@sampleId", sampleId));

                tx.Commit();
            }
            return result;
        }

        public async Task<int> RemoveSampleFromAnalysisRequestGroup(Guid? sampleId, Guid groupId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (sampleId != null && groupId != null)
                {
                    var tx = db.Database.BeginTransaction();

                    db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNAnalysisRequestGroupGNSample] " +
                        "WHERE [GNAnalysisRequestGroups_Id] = @groupId " +
                        "AND [GNSamples_Id] = @sampleId",
                        new SqlParameter("@groupId", groupId),
                        new SqlParameter("@sampleId", sampleId));

                    tx.Commit();
                }

                return result;
            });
        }

        public bool IsValidSingleEnded(GNSample sample)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            bool isValid = false;

            if (sample != null)
            {
                if (sample.IsPairEnded)
                {
                    isValid = true;
                }
                else
                {
                    int totalFileCount = sample.CloudFiles.Count;
                    int r1Count = sample.CloudFiles.Where(f => f.Description.ToUpper().Contains("_R1")).Count();
                    
                    if(totalFileCount == r1Count)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public bool IsValidPairEnded(GNSample sample)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            bool isValid = false;

            if (sample != null)
            {
                if (!sample.IsPairEnded)
                {
                    isValid = true;
                }
                else
                {
                    int uniqueFileCount = 0;
                    int totalFileCount = sample.CloudFiles.Count;
                    int r1Count = sample.CloudFiles.Where(f => f.Description.ToUpper().Contains("_R1")).Count();
                    int r2Count = sample.CloudFiles.Where(f => f.Description.ToUpper().Contains("_R2")).Count();

                    IEnumerable<string> allFileNames = sample.CloudFiles.Select(f => f.Description);
                    List<string> uniqueFileNames = new List<string>();
                    foreach (var fileName in allFileNames)
                    {
                        var fileNameClean = fileName.ToUpper().Replace("_R1", "").Replace("_R2", "");
                        if (!uniqueFileNames.Contains(fileNameClean))
                        {
                            uniqueFileNames.Add(fileNameClean);
                        }
                    }
                    uniqueFileCount = uniqueFileNames.Count;

                    if (uniqueFileCount == r1Count && uniqueFileCount == r2Count)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public override GNSample EvalEntitySecurity(GNContact userContact, GNSample sample)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (sample != null)
            {
                bool isSampleOwnedByOrganization = IsSampleOwnedByOrganization(userContact, sample);

                //set to false by default
                sample.CanCreate = false;
                sample.CanView = false;
                sample.CanEdit = false;
                sample.CanDelete = false;

                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    sample.CanCreate = true;
                    sample.CanView = true;
                    sample.CanEdit = true;
                    sample.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    sample.CanCreate = true;

                    if (isSampleOwnedByOrganization)
                    {
                        sample.CanView = true;
                        sample.CanEdit = true;
                        sample.CanDelete = true;
                    }
                }
                //COLLABORATOR
                else if (userContact.IsInRole("COLLABORATOR"))
                {
                    if (isSampleOwnedByOrganization)
                    {
                        sample.CanView = true;
                        sample.CanCreate = false;
                        sample.CanEdit = false;
                        sample.CanDelete = false;
                    }
                }
                //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
                else if (isSampleOwnedByOrganization)
                {
                    bool isCreatorOfSample = sample.CreatedBy != null && sample.CreatedBy != Guid.Empty & sample.CreatedBy == userContact.Id;

                    if (userContact.IsInRole("TEAM_MANAGER") 
                        || userContact.IsInRole("PROJECT_MANAGER") 
                        || userContact.IsInRole("TEAM_MEMBER"))
                    {
                        sample.CanCreate = true;
                        sample.CanView = true;

                        if (isCreatorOfSample)
                        {
                            sample.CanEdit = true;
                            sample.CanDelete = true;
                        }
                    }
                }

                //Prevent Deletion of samples with associated entities
                if (sample.CanDelete
                    && (sample.GNAnalysisRequestGNSamples == null || sample.GNAnalysisRequestGNSamples.Count == 0)
                    && (sample.CloudFiles == null || sample.CloudFiles.Count == 0)
                    && sample.GNSampleRightRelationships.Count == 0
                    && sample.GNSampleLeftRelationships.Count == 0)
                {
                    sample.CanDelete = true;
                }
                else
                {
                    sample.CanDelete = false;
                }
            }

            return sample;
        }

        private bool IsSampleOwnedByOrganization(GNContact userContact, GNSample sample)
        {
            return userContact.GNOrganizationId == sample.GNOrganizationId;
        }

        public async Task<List<GNSample>> FindPedigreeCandidates(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSample> samples = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                samples = db.GNSamples
                    .Include(s => s.Organization);
            }
            //ORG_MANAGER, TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                samples = db.GNSamples
                    .Include(s => s.Organization)
                    .Where(s => s.GNOrganizationId == userContact.GNOrganizationId);
            }

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    samples = samples.Where(t => t.Organization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("GNOrganizationId"))
                {
                    filterVal = (string)filters["GNOrganizationId"];
                    samples = samples.Where(t => t.Organization.Id.Equals(filterVal));
                }

                if (filters.ContainsKey("Sample Name"))
                {
                    filterVal = (string)filters["Sample Name"];
                    samples = samples.Where(t => t.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("GNSampleTypeId"))
                {
                    filterVal = (string)filters["GNSampleTypeId"];
                    samples = samples.Where(t => t.SampleType.Id.Equals(filterVal));
                }

                if (filters.ContainsKey("Gender"))
                {
                    filterVal = (string)filters["Gender"];
                    samples = samples.Where(t => t.Gender.Equals(filterVal));
                }


                if (filters.ContainsKey("GNLeftSampleId"))
                {
                    filterVal = (string)filters["GNLeftSampleId"];
                    samples = samples.Where(t => !t.Id.Equals(filterVal));

                    var filterGuid = Guid.Parse(filterVal);
                    samples = samples.Where(t => t.GNSampleRightRelationships.Count(tr => tr.GNLeftSampleId == filterGuid)==0);
                }
                

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    samples = samples
                        .Where(t => t.Name.Contains(filterVal)
                            || t.Organization.Name.Contains(filterVal)
                            || t.SampleType.Name.Contains(filterVal));
                }
            }

            //Order By Results
            samples = samples
                .OrderBy(t => t.SampleType.Name)
                .OrderBy(t => t.Name)
                .OrderBy(t => t.Organization.Name)
                .OrderByDescending(t => t.CreateDateTime);

            //Limit Result Size
            samples = samples.Skip(start).Take(end - start);

            return EvalEntityListSecurity(userContact, await samples.ToListAsync());
        }

        public async Task<int> AddRelationship(string leftSampleId, string rigthSampleId, string sampleRelationshipTypeId, GNContact userContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (leftSampleId != null && rigthSampleId != null && sampleRelationshipTypeId != null)
                {
                    var tx = db.Database.BeginTransaction();

                    result = db.Database.ExecuteSqlCommand(
                        "INSERT INTO [gn].[GNSampleRelationships]([GNLeftSampleId],[GNRightSampleId],[GNSampleRelationshipTypeId],[CreatedBy],[CreateDateTime]) " +
                        "VALUES(@leftSampleId, @rigthSampleId, @sampleRelationshipTypeId, @CreatedBy, @CreateDateTime)",
                        new SqlParameter("@leftSampleId", Guid.Parse(leftSampleId)),
                        new SqlParameter("@rigthSampleId", Guid.Parse(rigthSampleId)),
                        new SqlParameter("@sampleRelationshipTypeId", sampleRelationshipTypeId),
                        new SqlParameter("@CreatedBy", userContact.AspNetUserId),
                        new SqlParameter("@CreateDateTime", DateTime.Now));



                    //insert also the other-way relationship using the following mapping:
                    Guid guidLeftSampleId = Guid.Parse(leftSampleId);
                    int relationshipTypeId = Int32.Parse(sampleRelationshipTypeId);
                    string leftSampleGender = db.GNSamples.Where(a => a.Id.Equals(guidLeftSampleId)).FirstOrDefault().Gender;
                    string nameLeftSampleRel = db.GNSampleRelationshipTypes.Where(a => a.Id == relationshipTypeId).FirstOrDefault().Name;
                    string nameRightSampleRel = db.GNSampleRelationshipTypeMappings.Where(a => a.NameLeftRelationship.Equals(nameLeftSampleRel) && a.GenderRightRelationship == leftSampleGender).FirstOrDefault().NameRightRelationship;
                    int rightSampleRelationshipTypeId = db.GNSampleRelationshipTypes.Where(a => a.Name.Equals(nameRightSampleRel)).FirstOrDefault().Id;

                    result = db.Database.ExecuteSqlCommand(
                        "INSERT INTO [gn].[GNSampleRelationships]([GNLeftSampleId],[GNRightSampleId],[GNSampleRelationshipTypeId],[CreatedBy],[CreateDateTime]) " +
                        "VALUES(@leftSampleId, @rigthSampleId, @sampleRelationshipTypeId, @CreatedBy, @CreateDateTime)",
                        new SqlParameter("@leftSampleId", Guid.Parse(rigthSampleId)),
                        new SqlParameter("@rigthSampleId", Guid.Parse(leftSampleId)),
                        new SqlParameter("@sampleRelationshipTypeId", rightSampleRelationshipTypeId),
                        new SqlParameter("@CreatedBy", userContact.AspNetUserId),
                        new SqlParameter("@CreateDateTime", DateTime.Now));

                    tx.Commit();
                }

                bool auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(leftSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");
                auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(rigthSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");

                return result;
            });
        }

        public async Task<int> RemoveRelationship(int relId, Guid leftSampleId, Guid rightSampleId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (leftSampleId != null && rightSampleId != null && relId > 0)
                {
                    var tx = db.Database.BeginTransaction();

                    db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNSampleRelationships] " +
                        "WHERE [Id] = @relId " +
                        "AND [GNLeftSampleId] = @leftSampleId " +
                        "AND [GNRightSampleId] = @rightSampleId",
                        new SqlParameter("@relId", relId),
                        new SqlParameter("@leftSampleId", leftSampleId),
                        new SqlParameter("@rightSampleId", rightSampleId));


                    //remove also the other-way relationship using the following mapping:
                    db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNSampleRelationships] " +
                        "WHERE [GNLeftSampleId] = @rightSampleId " +
                        "AND [GNRightSampleId] = @leftSampleId",
                        new SqlParameter("@leftSampleId", leftSampleId),
                        new SqlParameter("@rightSampleId", rightSampleId));

                    tx.Commit();
                }

                return result;
            });
        }
    }

    public class SampleTypeService : GNEntityService<GNSampleType>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleTypeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleType>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSampleType> entities =
                await db.GNSampleTypes
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSampleType> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleTypes.FindAsync(keys);
        }
    }

    public class SampleQualifierService : GNEntityService<GNSampleQualifier>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleQualifierService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleQualifier>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSampleQualifier> entities =
                await db.GNSampleQualifiers
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSampleQualifier> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleQualifiers.FindAsync(keys);
        }
    }

    public class SampleQualifierGroupService : GNEntityService<GNSampleQualifierGroup>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleQualifierGroupService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleQualifierGroup>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSampleQualifierGroup> entities =
                await db.GNSampleQualifierGroups
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSampleQualifierGroup> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleQualifierGroups.FindAsync(keys);
        }
    }
   
    public class SampleRelationshipTypeService : GNEntityService<GNSampleRelationshipType>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleRelationshipTypeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleRelationshipType>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSampleRelationshipType> entities =
                await db.GNSampleRelationshipTypes
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSampleRelationshipType> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleRelationshipTypes.FindAsync(keys);
        }


    }


    public class SampleRelationshipTypeMappingService : GNEntityService<GNSampleRelationshipTypeMapping>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleRelationshipTypeMappingService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleRelationshipTypeMapping>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNSampleRelationshipTypeMapping> entities =
                await db.GNSampleRelationshipTypeMappings
                .ToListAsync();

            return entities;
        }

        public override async Task<GNSampleRelationshipTypeMapping> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleRelationshipTypeMappings.FindAsync(keys);
        }
    }
    
    public class SampleRelationshipService : GNEntityService<GNSampleRelationship>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleRelationshipService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSampleRelationship>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSampleRelationship> relationships = db.GNSampleRelationships;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("LeftSampleId"))
                {
                    filterVal = (string)filters["LeftSampleId"];
                    var filterGuid = Guid.Parse(filterVal);
                    relationships = relationships.Where(r => r.GNLeftSampleId == filterGuid);
                }

            }

            //Order By Results
            relationships = relationships
                .OrderBy(r => r.GNSampleRelationshipType.Name)
                .OrderByDescending(t => t.CreateDateTime);

            return await relationships.ToListAsync();
        }

        public override async Task<GNSampleRelationship> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSampleRelationships.FindAsync(keys);
        }
    }

    //public class ReplicateService : GNEntityService<GNReplicate>
    //{
    //    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    //    public ReplicateService(GNEntityModelContainer db)
    //        : base(db)
    //    {
    //        base.db = db;
    //    }

    //    public override async Task<List<GNReplicate>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
    //    {
    //        LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

    //        IQueryable<GNReplicate> replicates = db.GNReplicates;


    //        //Order By Results
    //        replicates = replicates
    //            .OrderByDescending(t => t.CreateDateTime);

    //        return await replicates.ToListAsync();
    //    }
    //}

}
