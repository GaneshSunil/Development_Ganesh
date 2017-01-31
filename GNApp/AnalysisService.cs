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
using GenomeNext.Cloud.Storage;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Cloud.Messaging.Model.GN;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;
using GenomeNext.Billing;
using GenomeNext.Notification;
using GenomeNext.Cloud.Compute;
using System.Configuration;
using GenomeNext.Data.EntityModel;

namespace GenomeNext.App
{
    /// <summary>
    /// Analysis Request Service
    /// </summary>
    /// 

    public class tmpAnnotatedTable
    {
        public string TemplateCode { get; set; }
        public string Template { get; set; }
        public int Chr { get; set; }
        public int Start { get; set; }
        public int Stop { get; set; }
        public string Type { get; set; }
        public string CytoBand { get; set; }
        public string Ref { get; set; }
        public string Alt { get; set; }
        public string HB1G0ADXX__E2603_9PM_GT { get; set; }
        public decimal Qual { get; set; }
        public string Filter { get; set; }
        public string Gene { get; set; }
        public string LocInGene { get; set; }
        public string SpliceDist { get; set; }
        public string CodingChange { get; set; }
        public string Exon { get; set; }
        public string Churchill_1kg_MAF { get; set; }
        public string Onekg_MAF { get; set; }
        public decimal MaxFrequency { get; set; }
        public string dbSNP142 { get; set; }
        public string OMIM_number { get; set; }
        public string OMIM_disorder { get; set; }
        public string Clinvar { get; set; }
        public string Cosmic { get; set; }
    }


    public class AnalysisRequestService : GNEntityService<GNAnalysisRequest>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "ANALYSIS_REQUEST";

        public static string[] RESTARTABLE_ERROR_CODES
        {
            get
            {
                return new string[] 
                { 
                    "MaxSpotInstanceCountExceeded",
                    "RequestResourceCountExceeded",
                    "MaxIOPSLimitExceeded",
                    "PrivateIpAddressLimitExceeded",
                    "RequestLimitExceeded",
                    "VolumeLimitExceeded",
                    "ReservedInstancesLimitExceeded",
                    "InstanceLimitExceeded",
                    "AddressLimitExceeded"
                };
            }
        }


        public GNCloudStorageService cloudStorageService { get; set; }
        public AnalysisRequestPendingCloudMessageService analysisRequestPendingCloudMessageService { get; set; }

        public AspNetRoleService aspNetRoleService { get; set; }
        public TeamService teamService { get; set; }
        public SampleService sampleService { get; set; }
        public TransactionService transactionService { get; set; }

        public AnalysisRequestService(GNEntityModelContainer db, IdentityModelContainer identityDB)
            : base(db)
        {
            base.db = db;
            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.teamService = new TeamService(db, identityDB);
            this.sampleService = new SampleService(db, identityDB);
            this.transactionService = new TransactionService(db);
        }

        public AnalysisRequestService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
            this.transactionService = new TransactionService(db);
        }

        public IQueryable<tmpAnnotatedTable> getTmpAnnotatedTable()
        {
            //DataTable table = new DataTable("tmpAnnotatedTable");
            IQueryable<tmpAnnotatedTable> rows = db.Database.SqlQuery<tmpAnnotatedTable>("SELECT d.Code as TemplateCode, d.Name as Template, Chr, Start, Stop, Type, CytoBand, Ref, Alt, HB1G0ADXX__E2603_9PM_GT, Qual, Filter, Gene, LocInGene, SpliceDist, CodingChange, Exon, Churchill_1kg_MAF, 1kg_MAF, MaxFrequency,  dbSNP142, OMIM_number, OMIM_disorder, Clinvar, Cosmic FROM [gn].[tmpAnnotatedTable] a, gn.GNTemplateGenes b, gn.GNAnalysisRequestGNTemplates c, gn.GNTemplates d WHERE b.GNTemplateId = d.Id AND b.GNTemplateId = c.GNTemplateId AND c.GNAnalysisRequestId = '59b1b7c8-b1a3-43d9-8193-408769efa07e&projectId=5e61c1df-4222-4973-8038-812adbad0ef1' AND a.Gene = b.GeneCode").ToList().AsQueryable().Take(1000);
            return rows;
        }


        public void InitCloudServices(Guid AWSConfigId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            cloudStorageService = new GNCloudStorageService();
            cloudStorageService.AWSConfigId = AWSConfigId;
            analysisRequestPendingCloudMessageService = new AnalysisRequestPendingCloudMessageService(
                AWSConfigId, AnalysisRequestPendingCloudMessageService.QUEUE_NAME);

            LogUtil.Info(logger, "Cloud Services Initialized");
        }

        public IQueryable<GNAnalysisRequest> FindMyAnalysisRequests(GNContact userContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNAnalysisRequest> analysisRequests = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                analysisRequests = db.GNAnalysisRequests
                    .Include(ar => ar.Project)
                    .Include(ar => ar.AnalysisResult);
            }
            //ORG_MANAGER
            else if (userContact.IsInRole("ORG_MANAGER"))
            {
                analysisRequests = db.GNAnalysisRequests
                    .Include(ar => ar.Project)
                    .Include(ar => ar.AnalysisResult)
                    .Where(ar => ar.Project.Teams.Select(t => t.OrganizationId).Contains(userContact.GNOrganizationId));
            }
            //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                var myProjects = db.GNTeams
                    .Where(t => (
                        t.OrganizationId == userContact.GNOrganizationId
                        && t.TeamMembers.Count(tm => tm.GNContactId == userContact.Id) != 0
                        )
                    ).SelectMany(t => t.Projects);

                analysisRequests = myProjects.SelectMany(p => p.AnalysisRequests);
            }

            return analysisRequests;
        }

        public override async Task<List<GNAnalysisRequest>> FindAll(GNContact userContact, int start = 0, int end = 10,
            Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNAnalysisRequest> analysisRequests = FindMyAnalysisRequests(userContact);

            LogUtil.Info(logger, "Setting filters");

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("OrganizationId"))
                {
                    filterVal = (string)filters["OrganizationId"];
                    if (filterVal != null)
                    {
                        Guid gOrgId = Guid.Parse(filterVal);
                        analysisRequests = analysisRequests.Where(ar => ar.Project.Teams.FirstOrDefault().OrganizationId.Equals(gOrgId));
                    }
                }

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    analysisRequests = analysisRequests.Where(ar => ar.Project.Teams.FirstOrDefault().Organization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Team Name"))
                {
                    filterVal = (string)filters["Team Name"];
                    analysisRequests = analysisRequests.Where(ar => ar.Project.Teams.FirstOrDefault().Name.Contains(filterVal));
                }

                if (filters.ContainsKey("projectId"))
                {
                    filterVal = (string)filters["projectId"];
                    if (filterVal != null)
                    {
                        Guid gProjectId = Guid.Parse(filterVal);
                        analysisRequests = analysisRequests.Where(ar => ar.Project.Id.Equals(gProjectId));
                    }
                }

                if (filters.ContainsKey("teamId"))
                {
                    filterVal = (string)filters["teamId"];
                    if (filterVal != null)
                    {
                        Guid gTeamId = Guid.Parse(filterVal);
                        analysisRequests = analysisRequests.Where(ar => ar.Project.Teams.FirstOrDefault().Id.Equals(gTeamId));
                    }
                }

                if (filters.ContainsKey("Project Name"))
                {
                    filterVal = (string)filters["Project Name"];
                    analysisRequests = analysisRequests.Where(ar => ar.Project.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Description"))
                {
                    filterVal = (string)filters["Description"];
                    analysisRequests = analysisRequests.Where(ar => ar.Description.Contains(filterVal));
                }

                if (filters.ContainsKey("Type"))
                {
                    filterVal = (string)filters["Type"];
                    analysisRequests = analysisRequests.Where(ar => ar.AnalysisType.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("CreatedBy"))
                {
                    Guid guidFilterVal = Guid.Parse((string)filters["CreatedBy"]);
                    analysisRequests = analysisRequests.Where(ar => ar.CreatedBy == guidFilterVal);
                }

                /*
                if (filters.ContainsKey("Request Date"))
                {
                    filterVal = (string)filters["Request Date"];
                    analysisRequests = analysisRequests.Where(ar => ar.RequestDateTime.Value.ToShortDateString().Equals(filterVal));
                }
                */

                if (filters.ContainsKey("Status"))
                {
                    filterVal = (string)filters["Status"];
                    filterVal = filterVal.ToUpper();

                    if(filterVal == "ERROR")
                    {
                        analysisRequests = analysisRequests
                            .Include(ar => ar.AnalysisStatus)
                            .Where(ar => (ar.AnalysisResult != null && !ar.AnalysisResult.Success)
                            || (ar.AnalysisStatus.Where(s=>s.CreateDateTime == ar.AnalysisStatus.Max(st => st.CreateDateTime.Value))
                                .FirstOrDefault().Status.Contains(filterVal)));
                    }
                    else
                    {
                        analysisRequests = analysisRequests
                            .Include(ar => ar.AnalysisStatus)
                            .Where(ar => (ar.AnalysisStatus.Where(s => s.CreateDateTime == ar.AnalysisStatus.Max(st => st.CreateDateTime.Value))
                                .FirstOrDefault().Status.Contains(filterVal)));
                    }
                }


                if (filters.ContainsKey("Percent Complete"))
                {
                    filterVal = (string)filters["Percent Complete"];
                    int filterValInt = int.Parse(filterVal);
                    analysisRequests = analysisRequests
                        .Where(ar =>
                            ar.AnalysisStatus
                                .Max(s=>s.PercentComplete)
                                >= filterValInt);
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    analysisRequests = analysisRequests
                        .Where(ar => ar.Description.Contains(filterVal)
                            || ar.Project.Teams.FirstOrDefault().Organization.Name.Contains(filterVal)
                            || ar.Project.Teams.FirstOrDefault().Name.Contains(filterVal)
                            || ar.Project.Name.Contains(filterVal)
                            || ar.AnalysisType.Name.Contains(filterVal)
                        //|| ar.RequestDateTime.Value.ToShortDateString().Equals(filterVal)
                        //|| ar.CurrentStatusLong.Contains(filterVal)
                            );
                }
            }

            LogUtil.Info(logger, "Setting order by");

            //Order By Results
            analysisRequests = analysisRequests
                .OrderBy(ar => ar.Description)
                .OrderBy(ar => ar.Project.Name)
                .OrderBy(ar => ar.Project.Teams.FirstOrDefault().Name)
                .OrderBy(ar => ar.Project.Teams.FirstOrDefault().Organization.Name)
                .OrderByDescending(ar => ar.CreateDateTime);

            LogUtil.Info(logger, "Setting limits [" + start + "," + end + "]");

            //Limit Result Size
            analysisRequests = analysisRequests.Skip(start).Take(end - start);

            return EvalEntityListSecurity(userContact, await analysisRequests.ToListAsync());
        }

        public override async Task<GNAnalysisRequest> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            var analysisRequestObj = await db.GNAnalysisRequests.FindAsync(keys);

            try
            {
                if (analysisRequestObj != null && analysisRequestObj.AnalysisResult != null)
                {
                    if (this.cloudStorageService == null)
                    {
                        this.InitCloudServices(analysisRequestObj.Project.Teams.FirstOrDefault().Organization.AWSConfigId);
                    }
                    //loop output files and update size
                    foreach (var item in analysisRequestObj.AnalysisResult.ResultFiles.Where(a => a.FileSize == 0))
                    {
                        item.FileSize = this.cloudStorageService.GetObjectSize(item.Volume, item.FileName);
                    }
                    db.Entry(analysisRequestObj.AnalysisResult).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, e.Message, e);
            }

            return analysisRequestObj;
        }

        public override async Task<GNAnalysisRequest> Find(GNContact userContact, params object[] keys)
        {
            await InitAnalysisTransactionLogging(userContact);
            return await base.Find(userContact, keys);
        }

        public List<GNAnalysisRequest> FetchAnalysesPendingAndNotStarted(GNCloudComputeService cloudComputeService, string awsEnvKey)
        {
            //get all analysis requests with progress less than 100 percent
            var pendingAnalyses = db.GNAnalysisRequests
                .Include(ar => ar.AnalysisStatus)
                .Include(ar => ar.AnalysisResult)
                .Where(ar => (
                    ar.RequestDateTime.HasValue
                    && ar.AnalysisResult == null
                    && ar.AnalysisStatus.Count <= 5
                    && db.GNContacts.Where(con => con.Id == ar.CreatedBy).FirstOrDefault()
                        .GNOrganization.GNSettingsTemplate.GNSettingsTemplateConfigs
                        .Count(c =>
                            c.GNSettingsTemplateField.ConfigType == "ANALYSIS_REQUEST"
                            && c.GNSettingsTemplateField.Id == "AWS_ENV"
                            && c.Value == awsEnvKey) != 0
                    )
                 );

            //AND there are no EC2 instances running for that analysis request
            List<GNAnalysisRequest> pendingAnalysesNotStarted = new List<GNAnalysisRequest>();
            foreach (var analysis in pendingAnalyses)
            {
                if (analysis.CurrentProgress == 0)
                {
                    var analysisMasterInstances = cloudComputeService.GetComputeInstances(
                        new Dictionary<string, List<string>> 
                        { 
                            {"Name", new List<string> { "churchill_"+analysis.Id+"_master" }} 
                        });

                    if (analysisMasterInstances == null || analysisMasterInstances.Count == 0)
                    {
                        pendingAnalysesNotStarted.Add(analysis);
                    }
                }
            }

            return pendingAnalysesNotStarted;
        }

        public IQueryable<GNAnalysisRequest> FetchAnalysesWithErrorsButNotMarkedInError()
        {
            IQueryable<GNAnalysisRequest> analysesWithErrors = db.GNAnalysisRequests
                .Include(ar => ar.AnalysisStatus)
                .Include(ar => ar.AnalysisResult)
                .Where(ar => (ar.AnalysisResult == null || ar.AnalysisResult.Success)
                && (ar.AnalysisStatus.Where(s => s.CreateDateTime == ar.AnalysisStatus.Max(st => st.CreateDateTime.Value))
                    .FirstOrDefault().Status.Contains("ERROR")));

            return analysesWithErrors;
        }

        public IQueryable<GNAnalysisRequest> FetchAnalysesMarkedInError()
        {
            IQueryable<GNAnalysisRequest> analysesWithErrors = db.GNAnalysisRequests
                .Include(ar => ar.AnalysisStatus)
                .Include(ar => ar.AnalysisResult)
                .Where(ar => ar.AnalysisResult != null && !ar.AnalysisResult.Success);

            return analysesWithErrors;
        }
        
        /*
        public IQueryable<GNAnalysisRequest> FetchStaleSubmittedAnalyses()
        {
            //set expire age to 12 hours old, but then try to pull from app.config
            double staleAnalysisTimeoutHrs = 12.0;
            double.TryParse(System.Configuration.ConfigurationManager.AppSettings["StaleAnalysisTimeoutHrs"], out staleAnalysisTimeoutHrs);
            DateTime statusExpireAge = DateTime.Now.AddHours(-1 * staleAnalysisTimeoutHrs);
            //get all analysis requests with progress less than 100 percent
            //and an old/expired last status message (more than 4 hours old)
            IQueryable<GNAnalysisRequest> staleSubmittedAnalyses = db.GNAnalysisRequests
                .Include(ar => ar.AnalysisStatus)
                .Include(ar => ar.AnalysisResult)
                .Where(r =>
                    (
                    r.RequestDateTime.HasValue
                    && (r.AnalysisStatus.Max(s => s.PercentComplete) < 100)
                    && (r.AnalysisStatus.Max(s => s.CreateDateTime.Value).CompareTo(statusExpireAge) <= 0)
                    && (r.AnalysisResult == null || r.AnalysisResult.Success)
                    )
                );
            return staleSubmittedAnalyses;
        }
        */

        public void MarkAnalysisAsFailed(AnalysisRequestService analysisRequestService, GNAnalysisRequest analysis)
        {
            GNAnalysisResult analysisResult = null;

            if (analysis.AnalysisResult != null)
            {
                analysis.AnalysisResult.Success = false;
                analysisRequestService.db.SaveChanges();
                analysisResult = analysis.AnalysisResult;
            }
            else
            {
                analysisResult = new GNAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    Success = false,
                    AWSRegionSystemName = analysis.AWSRegionSystemName,
                    AnalysisStartDateTime = analysis.CreateDateTime.Value,
                    AnalysisEndDateTime = DateTime.Now,
                    CreatedBy = analysis.CreatedBy,
                    CreateDateTime = DateTime.Now
                };

                //insert result
                analysisRequestService.db.GNAnalysisResults.Add(analysisResult);
                analysisRequestService.db.SaveChanges();

                //find result and link to analysis
                analysisResult = analysisRequestService.db.GNAnalysisResults.Find(analysisResult.Id);
                analysisResult.AnalysisRequest = analysis;

                //update result in db with link to analysis
                analysisRequestService.db.Entry(analysisResult).State = EntityState.Modified;
                analysisRequestService.db.SaveChanges();
            }

            //if analysis result is a failure - process refund
            if (analysisResult != null && !analysisResult.Success)
            {
                var t = Task.Run(async delegate
                {
                    TransactionService transactionService = new TransactionService(analysisRequestService.db);
                    await transactionService.ProcessRefundForAnalysisFailure(analysis, analysisResult);
                });
            }

            bool auditResult = new GNCloudNoSQLService().LogEvent(analysis.CreatedByContact, analysis.Id, this.ENTITY, "N/A", "MARK_REQUEST_FAILED");
        }

        public GNAnalysisRequest IsValidSampleSet(GNAnalysisRequest analysisRequest)
        {
            foreach (var sample in analysisRequest.GNAnalysisRequestGNSamples)
            {
                sample.GNSample.IsValidPairEnded = sampleService.IsValidPairEnded(sample.GNSample);
                sample.GNSample.IsValidSingleEnded = sampleService.IsValidSingleEnded(sample.GNSample);
            }

            analysisRequest.IsValidSampleSetPairEndings = !analysisRequest.GNAnalysisRequestGNSamples.Select(s => s.GNSample.IsValidPairEnded).Contains(false);
            analysisRequest.IsValidSampleNumberOfFiles = analysisRequest.GNAnalysisRequestGNSamples.Where(a => a.GNSample.CloudFiles.Count() == 0).Count() == 0;
            analysisRequest.AllSamplesAreReady = analysisRequest.GNAnalysisRequestGNSamples.Where(a => a.GNSample.IsReady == false).Count() == 0;


            analysisRequest.IfTumorNormalIsReady = true;
            if(analysisRequest.GNAnalysisRequestTypeCode == "TUMORNORMAL" && (analysisRequest.GNAnalysisRequestGNSamples.Count() != 2))
            {
                //only switch to false when is TUMOR NORMAL and the count of samples is other than 2.
                analysisRequest.IfTumorNormalIsReady = false;
            }


            analysisRequest.IfRnaIsReady = true;
            if (analysisRequest.GNAnalysisRequestTypeCode == "RNA")
            {
                /* 
                 * More validations are required:=
                 * - No groups exist
                 * - Empty groups exist
                 * - No comparisons have been set up
                 * - Groups are set up with no comparison
                 */
                if(analysisRequest.GNAnalysisRequestGroups.Count() < 2 || analysisRequest.GNAnalysisRequestGroupComparisons.Count() == 0)
                {
                    analysisRequest.IfRnaIsReady = false;
                }
                else
                {
                    foreach (GNAnalysisRequestGroup group in analysisRequest.GNAnalysisRequestGroups)
                    {
                        if(group.GNSamples.Count() == 0 || ((group.GNAnalysisRequestComparisonGroups.Count + group.GNAnalysisRequestControlGroups.Count) == 0 ))
                        {
                            analysisRequest.IfRnaIsReady = false;
                        }
                    }
                }                
            }



            return analysisRequest;
        }

        public bool IsAnalysisStartAllowed(GNAnalysisRequest analysisRequest)
        {
            bool canStartAnalysis = false;

            try
            {
                if ((analysisRequest.GNAnalysisRequestGNSamples != null && analysisRequest.GNAnalysisRequestGNSamples.Count != 0)
                    && analysisRequest.CurrentProgress == 0
                    && (analysisRequest.AnalysisStatus != null && analysisRequest.AnalysisStatus.Count == 0)
                    && !analysisRequest.AnalysisStatus.Select(s => s.Status).Contains("SUBMITTED")
                    && analysisRequest.IsValidSampleSetPairEndings
                    && analysisRequest.IsValidSampleNumberOfFiles
                    && analysisRequest.AllSamplesAreReady
                    && analysisRequest.IfTumorNormalIsReady
                    && analysisRequest.IfRnaIsReady)
                {
                    canStartAnalysis = true;
                }
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Unable to determine if Analysis Start Is Allowed for analysis id: " + analysisRequest.Id, e);
            }

            return canStartAnalysis;
        }

        public bool IsAnalysisRestartAllowed(GNAnalysisRequest analysisRequest, bool forceRestart = false)
        {
            bool canReStartAnalysis = false;

            try
            {
                if (analysisRequest.IsFailedRequest
                    && (analysisRequest.GNAnalysisRequestGNSamples != null && analysisRequest.GNAnalysisRequestGNSamples.Count != 0)
                        && analysisRequest.IsValidSampleSetPairEndings
                        && analysisRequest.IsValidSampleNumberOfFiles
                        && analysisRequest.AllSamplesAreReady
                    )
                {
                    if (forceRestart)
                    {
                        canReStartAnalysis = true;
                    }
                    else
                    {
                        foreach (string errorCode in AnalysisRequestService.RESTARTABLE_ERROR_CODES)
                        {
                            foreach (var analysisStatus in analysisRequest.AnalysisStatus)
                            {
                                if (analysisStatus != null && analysisStatus.Message.Contains(errorCode))
                                {
                                    canReStartAnalysis = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Unable to determine if Analysis Restart Is Allowed for analysis id: " + analysisRequest.Id, e);
            }

            return canReStartAnalysis;
        }

        public async Task<bool> IsAccountAuthToStartAnalysis(Guid analysisRequestId, GNContact userContact)
        {
            bool isAuthAllowedForAccount = false;

            try
            {
                //get analysis request
                GNAnalysisRequest analysisRequest = this.db.GNAnalysisRequests
                    .Include(ar => ar.GNAnalysisRequestGNSamples)
                    .Include(ar => ar.AnalysisType)
                    .Include(a => a.Project)
                    .Where(ar => ar.Id == analysisRequestId)
                    .FirstOrDefault();

                if (analysisRequest != null)
                {
                    var team = this.db.GNTeams.Where(t => t.Projects.Count(p => p.Id == analysisRequest.Project.Id) != 0).FirstOrDefault();
                    var acct = this.db.GNAccounts
                        .Include(a=>a.AccountType)
                        .Where(a => a.Organization.Id == team.OrganizationId).FirstOrDefault();

                    //get related product
                    //look up product by transaction type key AND account type
                    string txnTypeKey = "ANALYSIS_REQUEST_" + analysisRequest.AnalysisType.TypeValue.ToUpper();
                    int arAcctTypeId = acct.AccountType.Id;
                    GNProduct product = db.GNProducts
                        .Where(p => (
                            p.Name == txnTypeKey
                            && p.GNAccountTypeId == arAcctTypeId
                            )
                        )
                        .FirstOrDefault();

                    if (product != null)
                    {
                        double authAmount = product.Price * analysisRequest.GNAnalysisRequestGNSamples.Count;
                        isAuthAllowedForAccount = await (new AccountService(this.db)).IsAuthAllowedForAccount(userContact, authAmount);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Unable to determine if Account is Authorized to Start Analysis for id: " + analysisRequestId, e);
                System.Console.WriteLine("Unable to determine if Account is Authorized to Start Analysis for id: " + analysisRequestId + " : " + e.StackTrace);
            }

            return isAuthAllowedForAccount;
        }

        private async Task InitAnalysisTransactionLogging(GNContact userContact)
        {
            List<string> txnTypeKeys = await this.db.GNTransactionTypes
                .Where(tt => tt.Name.Contains("ANALYSIS_REQUEST")).Select(tt => tt.Name).ToListAsync();

            foreach (var txnTypeKey in txnTypeKeys)
            {
                await CreateTransactionForCurrentMonth(userContact, txnTypeKey);
            }
        }

        private async Task CreateTransactionForCurrentMonth(GNContact userContact, string txnTypeKey)
        {
            await this.transactionService.CreateTransaction(
                userContact, txnTypeKey,
                this.transactionService.INITIALIZE_TXN_DESCRIPTION,
                0.0, "REQUEST_PER_SAMPLE", null, null, null, null);
        }
        
        public override async Task<GNAnalysisRequest> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNAnalysisRequest gNAnalysisRequest = (GNAnalysisRequest)entity;
            try
            {
                db.Entry(gNAnalysisRequest).State = EntityState.Modified;

                //entity = db.GNAnalysisRequests.Attach(entity);

                LogUtil.Info(logger, "Updating analysis request");

                await db.SaveChangesAsync();

                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "UPDATE [gn].[GNAnalysisRequests] " +
                    "SET [AnalysisType_Id] = @analysisTypeId " +
                    "WHERE [Id] = @analysisRequestId",
                    new SqlParameter("@analysisTypeId", int.Parse(gNAnalysisRequest.AnalysisTypeId)),
                    new SqlParameter("@analysisRequestId", gNAnalysisRequest.Id));

                tx.Commit();

                bool auditResult = new GNCloudNoSQLService().LogEvent(gNAnalysisRequest.CreatedByContact, gNAnalysisRequest.Id, this.ENTITY, "N/A", "EVENT_UPDATE");

                LogUtil.Info(logger, "Updated analysis request");
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to update Analysis Request.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            return gNAnalysisRequest;
        }

        public async Task StartAnalysis(GNContact userContact, Guid analysisRequestId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            try
            {

                //check if account is authorized to start analysis
                bool isAuthAllowedForAccount = await IsAccountAuthToStartAnalysis(analysisRequestId, userContact);

                if (isAuthAllowedForAccount)
                {
                    if (this.analysisRequestPendingCloudMessageService == null
                        || this.cloudStorageService == null)
                    {
                        this.InitCloudServices(userContact.GNOrganization.AWSConfigId);
                    }

                    LogUtil.Info(logger, "Finding analysis request [" + analysisRequestId + "]");

                    try
                    {
                        //fetch analysis request from DB
                        GNAnalysisRequest analysisRequest = db.GNAnalysisRequests
                            .Include(a => a.AnalysisType)
                            .Include(a => a.GNAnalysisRequestGNSamples.Select(s => s.GNSample).Select(s => s.CloudFiles))
                            .Include(a => a.AnalysisStatus)
                            .Include(a => a.AnalysisResult)
                            .Include(a => a.Project)
                            .Include(a => a.Project.Teams)
                            .Include(a => a.Project.Teams.Select(t => t.Organization))
                            .Include(a => a.Project.Teams.Select(t => t.Organization.Account))
                            .Where(a => a.Id == analysisRequestId)
                            .FirstOrDefault();

                        if (analysisRequest != null)
                        {
                            AnalysisRequestS3Message analysisRequestS3Message = null;

                            try
                            {
                                LogUtil.Info(logger, "Building Analysis Request Message");
                                System.Console.WriteLine("\nBuilding Analysis Request Message");

                                //build analysis request
                                analysisRequestS3Message = new AnalysisRequestS3Message();
                                analysisRequestS3Message.analysisConfig = BuildAnalysisConfig(userContact);
                                analysisRequestS3Message.analysisRequest = BuildAnalysisRequest(analysisRequest);

                                analysisRequestS3Message.analysisRequest.hasreplicates = "false";

                                if(analysisRequest.GNAnalysisRequestTypeCode == "RNA")
                                {
                                    analysisRequestS3Message.analysisRequest = BuildRnaAnalysisRequest(analysisRequest, analysisRequestS3Message);

                                    if(analysisRequestS3Message.analysisRequest.replicates.Count() > 0)
                                    {
                                        analysisRequestS3Message.analysisRequest.hasreplicates = "true";
                                    }
                                }

                                analysisRequestS3Message.analysisResult = new AnalysisResult
                                {
                                    resultBucket = cloudStorageService.FetchAWSS3Bucket().ARN
                                };


                                LogUtil.Info(logger, "Updating Analysis Request in DB");
                                System.Console.WriteLine("\nUpdating Analysis Request in DB");

                                try
                                {
                                    //update analysis request and status in DB
                                    UpdateAnalysisRequestAndStatusInDB(analysisRequest);

                                    LogUtil.Info(logger, "Send Analysis Request Message to Queue");
                                    System.Console.WriteLine("\nSend Analysis Request Message to Queue");

                                    /**
                                     * tfrege 2016.03
                                     * Store a file in S3 with the full message: this will prevent errors when the message size is > 256KB
                                     */
                                    //build message for queue 

                                    var s3Bucket = db.AWSResources.Where(r => r.AWSResourceType.Name == "S3 bucket for Analysis Requests").FirstOrDefault();

                                    AnalysisRequestSqsMessage analysisRequestSqsMessage = new AnalysisRequestSqsMessage
                                    {
                                        id = analysisRequest.Id.ToString(),
                                        pathToData = new PathToData
                                        {
                                            bucket = s3Bucket.ARN, //tfrege change this
                                            key = "message_" + analysisRequest.Id + ".txt"
                                        }
                                    };

                                    AnalysisRequestMessage SqsMessage = new AnalysisRequestMessage();
                                    SqsMessage.AnalysisRequest = analysisRequestSqsMessage;

                                    analysisRequestPendingCloudMessageService.StoreMessage(analysisRequestS3Message, SqsMessage.AnalysisRequest.pathToData.bucket, SqsMessage.AnalysisRequest.pathToData.key);

                                    //send message to PENDING queue
                                    analysisRequestPendingCloudMessageService.SendMessage(SqsMessage);

                                    LogUtil.Info(logger, "Create billing transactions for each analysis sample");
                                    System.Console.WriteLine("\nCreate billing transactions for each analysis sample");

                                    //create billing transactions for each analysis sample
                                    await this.transactionService.CreateBillingTransactionsPerAnalysisSample(
                                        userContact,
                                        analysisRequest,
                                        analysisRequest.AnalysisType.Name,
                                        analysisRequest.Description);

                                    //bool auditResult = new GNCloudNoSQLService().LogEvent(userContact, analysisRequest.Id, this.ENTITY, "N/A", "START_ANALYSIS");
                                }
                                catch (Exception updateDBEx)
                                {
                                    
                                    throw updateDBEx;
                                }
                                
                            }
                            catch (Exception buildMessageEx)
                            {
                                LogUtil.Error(logger, "Unable to build and send analysis request message to PENDING Queue", buildMessageEx);
                                System.Console.WriteLine("\nUnable to build and send analysis request message to PENDING Queue: " + buildMessageEx.StackTrace);
                                throw buildMessageEx;
                            }
                        }
                    }
                    catch (Exception findAnalysisEx)
                    {
                        
                        throw findAnalysisEx;
                    }

                    
                }
                else
                {
                    System.Console.WriteLine("\nBilling Account is not authorized to start analysis due to insufficient funds and/or missing payment methods. Analysis Request ID = " + analysisRequestId);
                    throw new Exception("Billing Account is not authorized to start analysis due to insufficient funds and/or missing payment methods. Analysis Request ID = " + analysisRequestId);
                }
            }
            catch (Exception e1)
            {
                
                throw e1;
            }

        }

        private AnalysisConfig BuildAnalysisConfig(GNContact userContact)
        {
            AnalysisConfig analysisConfig = null;

            string amiId = null;

            try
            {
                LogUtil.Info(logger, "Get AMI ID");

                //get AMI ID
                var awsConfig = this.db.AWSConfigs
                    .Include(a => a.AWSResources)
                    .Where(a => a.Id == userContact.GNOrganization.AWSConfigId)
                    .FirstOrDefault();
                amiId = awsConfig.AWSResources
                    .Where(r => r.AWSResourceType.Name == "AMI")
                    .FirstOrDefault()
                    .ARN;

                //get settings template for org
                Dictionary<string, object> analysisConfigOptions = null;

                var analysisConfigSettingsTemplate =
                    this.db.GNSettingsTemplates
                        .Include(t => t.GNSettingsTemplateConfigs)
                        .Where(t => t.GNOrganizations.Any(o => o.Id == userContact.GNOrganizationId))
                        .FirstOrDefault();

                if (analysisConfigSettingsTemplate != null
                    && analysisConfigSettingsTemplate.GNSettingsTemplateConfigs != null
                    && analysisConfigSettingsTemplate.GNSettingsTemplateConfigs.Count != 0)
                {
                    analysisConfigOptions = new Dictionary<string, object>();

                    //retrieve ONLY analysis request settings
                    var analysisConfigSettingsInDB =
                        analysisConfigSettingsTemplate.GNSettingsTemplateConfigs
                        .Where(s => s.GNSettingsTemplateField.ConfigType == "ANALYSIS_REQUEST");

                    foreach (var configSetting in analysisConfigSettingsInDB)
                    {
                        if (configSetting.GNSettingsTemplateField != null
                            && !string.IsNullOrEmpty(configSetting.GNSettingsTemplateField.Type)
                            && !string.IsNullOrEmpty(configSetting.Value))
                        {
                            if (configSetting.GNSettingsTemplateField.Type.ToLower() == "bool")
                            {
                                bool boolVal = false;
                                bool.TryParse(configSetting.Value, out boolVal);
                                analysisConfigOptions.Add(configSetting.GNSettingsTemplateField.Id, boolVal);

                            }
                            else if (configSetting.GNSettingsTemplateField.Type.ToLower() == "int")
                            {
                                int intVal = 0;
                                int.TryParse(configSetting.Value, out intVal);
                                analysisConfigOptions.Add(configSetting.GNSettingsTemplateField.Id, intVal);
                            }
                            else if (configSetting.GNSettingsTemplateField.Type.ToLower() == "long")
                            {
                                long longVal = 0;
                                long.TryParse(configSetting.Value, out longVal);
                                analysisConfigOptions.Add(configSetting.GNSettingsTemplateField.Id, longVal);
                            }
                            else if (configSetting.GNSettingsTemplateField.Type.ToLower() == "double")
                            {
                                double doubleVal = 0;
                                double.TryParse(configSetting.Value, out doubleVal);
                                analysisConfigOptions.Add(configSetting.GNSettingsTemplateField.Id, doubleVal);
                            }
                            else
                            {
                                analysisConfigOptions.Add(configSetting.GNSettingsTemplateField.Id, configSetting.Value);
                            }
                        }
                    }
                }

                //build analysis config
                analysisConfig = new AnalysisConfig
                {
                    machineImageId = amiId,
                    options = (analysisConfigOptions != null && analysisConfigOptions.Count == 0 ? null : analysisConfigOptions)
                };
            }
            catch (Exception ex)
            {
                LogUtil.Error(logger, "Unable to build Analysis Configuration!", ex);
                throw ex;
            }

            return analysisConfig;
        }

        private AnalysisRequest BuildAnalysisRequest(GNAnalysisRequest analysisRequest)
        {
            AnalysisRequest analysisRequestForMsg = new AnalysisRequest
            {
                id = analysisRequest.Id.ToString(),
                name = StringTransformUtil.BuildAnalysisRequestName(
                            null, /* 20150408 change analysisRequest.Project.Name to null to shorten the filenames coming back fr*/
                            analysisRequest.Description,
                            analysisRequest.Id.ToString()),
                type = analysisRequest.AnalysisType.TypeValue,
                analysistype = analysisRequest.GNAnalysisRequestTypeCode,  //tfrege: TumorNormal project
                adaptor = analysisRequest.GNAnalysisAdaptorCode,  //tfrege: RNA project
                samples = new List<Sample>()
            };

            LogUtil.Info(logger, "Loading samples into Analysis Request Message");

            //load samples into analysis request
            foreach (var sampleObj in analysisRequest.GNAnalysisRequestGNSamples)
            {
                string varFather = "0";
                string varMother = "0";
                try
                {
                    if (sampleObj.GNSample.GNSampleLeftRelationships.Where(t => t.GNSampleRelationshipType.Name.Equals("FATHER")) != null)
                    {
                        varFather = sampleObj.GNSample.GNSampleLeftRelationships.Where(t => t.GNSampleRelationshipType.Name.Equals("FATHER")).First().GNRightSampleId.ToString();
                    }

                    if (sampleObj.GNSample.GNSampleLeftRelationships.Where(t => t.GNSampleRelationshipType.Name.Equals("MOTHER")) != null)
                    {
                        varMother = sampleObj.GNSample.GNSampleLeftRelationships.Where(t => t.GNSampleRelationshipType.Name.Equals("MOTHER")).First().GNRightSampleId.ToString();
                    }
                }
                catch (Exception parentNotFound)
                {
                    LogUtil.Warn(logger, parentNotFound.Message + sampleObj.GNSampleId);
                }


                Sample sample = new Sample
                {
                    id = sampleObj.GNSample.Id.ToString(),
                    name = StringTransformUtil.CleanCloudEntityNameStr(sampleObj.GNSample.Name),
                    type = sampleObj.GNSample.SampleType.TypeValue,
                    qualifier = sampleObj.GNSample.GNSampleQualifierCode,  //tfrege: addition for TumorNormal project

                    //place father and mother here for now, per JamesF's request. Fix later
                    father = varFather,
                    mother = varMother,
                    affected = sampleObj.AffectedIndicator,
                    target = sampleObj.TargetIndicator,

                    files = new List<File>()
                };

                foreach (var cloudFile in sampleObj.GNSample.CloudFiles)
                {
                    sample.files.Add(new File
                    {
                        bucket = cloudFile.Volume,
                        key = cloudFile.FileName
                    });
                }

                analysisRequestForMsg.samples.Add(sample);

            }

            return analysisRequestForMsg;
        }

        private AnalysisRequest BuildRnaAnalysisRequest(GNAnalysisRequest analysisRequest, AnalysisRequestS3Message analysisRequestS3Message)
        {
            LogUtil.Info(logger, "Building RNA Analysis Request");
            
            LogUtil.Info(logger, "Loading Groups and Comparison Sets into Analysis Request Message");
            
            analysisRequestS3Message.analysisRequest.groups = new List<Group>();

            foreach (var groupObj in analysisRequest.GNAnalysisRequestGroups)
            {
                Group group = new Group
                {
                    id = groupObj.Id.ToString(),
                    name = groupObj.Name,
                    samples = new List<string>()
                };


                foreach (GNSample sampleObj in groupObj.GNSamples)
                {
                    group.samples.Add(sampleObj.Id.ToString());                    
                }

                
                analysisRequestS3Message.analysisRequest.groups.Add(group);
            }

            //Create Comparison Sets

            analysisRequestS3Message.analysisRequest.comparisons = new List<ComparisonSet>();

            foreach (var comparisonObj in analysisRequest.GNAnalysisRequestGroupComparisons)
            {
                ComparisonSet comparisonSet = new ComparisonSet
                { 
                    id = comparisonObj.Id.ToString(),
                    ControlGroupId = comparisonObj.GNAnalysisRequestControlGroupId.ToString(),
                    ComparisonGroupId = comparisonObj.GNAnalysisRequestComparisonGroupId.ToString()
                };

                analysisRequestS3Message.analysisRequest.comparisons.Add(comparisonSet);
            }



            //Create Replicate Sets

            analysisRequestS3Message.analysisRequest.replicates = new List<ReplicateSet>();

            foreach (var analysisSample in analysisRequest.GNAnalysisRequestGNSamples)
            {
                if (analysisRequestS3Message.analysisRequest.replicates.Where(a => a.id.Equals(analysisSample.GNSample.GNReplicate.Name)).Count() > 0)
                {
                    analysisRequestS3Message.analysisRequest.replicates.Where(a => a.id.Equals(analysisSample.GNSample.GNReplicate.Name)).FirstOrDefault().samples.Add(analysisSample.GNSample.Id.ToString());
                }
                else 
                {
                    List<String> listOfSamples = new List<String>();
                    listOfSamples.Add(analysisSample.GNSample.Id.ToString());
                    string replicateId = analysisSample.GNSample.GNReplicate.Name;
                    ReplicateSet replicateSet = new ReplicateSet
                    {
                        id = replicateId,
                        name = "Replicates " + replicateId,
                        samples = listOfSamples
                    };

                    analysisRequestS3Message.analysisRequest.replicates.Add(replicateSet);
                }
            }


            return analysisRequestS3Message.analysisRequest;
        }

        private void UpdateAnalysisRequestAndStatusInDB(GNAnalysisRequest analysisRequest)
        {
            try
            {
                LogUtil.Info(logger, "Update Analysis Request Progress in DB");

                analysisRequest.RequestDateTime = DateTime.Now;
                analysisRequest.RequestProgress = 0;
                db.Entry(analysisRequest).State = EntityState.Modified;
                db.SaveChanges();
                
                try
                {
                    try
                    {
                        //remove pre-existing analysis status
                        db.GNAnalysisStatus.RemoveRange(db.GNAnalysisStatus.Where(s => s.GNAnalysisRequestId == analysisRequest.Id));
                        db.SaveChanges();

                        //remove pre-existing analysis results
                        db.GNAnalysisResults.RemoveRange(db.GNAnalysisResults.Where(r => r.AnalysisRequest.Id == analysisRequest.Id));
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(logger, "Unable to remove pre-existing analysis statuses and/or results from database", ex);
                    }

                    LogUtil.Info(logger, "Add Initial Analysis Status to DB");

                    //update db
                    db.GNAnalysisStatus.Add(new GNAnalysisStatus
                    {
                        Status = "SUBMITTED",
                        Message = "Analysis Request Submitted",
                        PercentComplete = 0,
                        IsError = false,
                        CreateDateTime = DateTime.Now,
                        GNAnalysisRequestId = analysisRequest.Id
                    });

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(logger, "Unable to save initial analysis status to database", ex);
                    System.Console.WriteLine("Unable to save initial analysis status to database : " + ex.StackTrace);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(logger, "Unable to update analysis request in database", ex);
                System.Console.WriteLine("Unable to update analysis request in database : "+ ex.StackTrace);
                throw ex;
            }
        }

        public void AddResultFile(File resultFile, GNAnalysisRequest analysisRequest, GNAnalysisResult analysisResult)
        {
            int resultFileCategoryId = 6; // GetResultFileCategoryId(resultFile, resultFileCategoryList);
            
            Guid resultFileId = Guid.NewGuid();
            string objectURL = GNCloudStorageService.GetObjectUrl(resultFile.bucket, resultFile.key);

            var tx = db.Database.BeginTransaction();

            string insertResultFileSQL =
                "INSERT INTO [gn].[GNCloudFiles]" +
                "([Id],[FileURL],[Volume],[FileName],[FolderPath],[FileSize],[Description]," +
                "[GNCloudFileCategoryId],[AWSRegionSystemName],[CreatedBy],[CreateDateTime]) " +
                "VALUES " +
                "(@Id," + //uniqueidentifier,>
                "@FileURL," + //nvarchar(max),>
                "@Volume," + //nvarchar(max),>
                "@FileName," + //nvarchar(max),>
                "@FolderPath," + //nvarchar(max),>
                "@FileSize," + //bigint,>
                "@Description," + //nvarchar(max),>
                "@GNCloudFileCategoryId," + //int,>
                "@AWSRegionSystemName," + //nvarchar(100),>
                "@CreatedBy," + //uniqueidentifier,>
                "@CreateDateTime)"; //datetime,>

            db.Database.ExecuteSqlCommand(
                insertResultFileSQL,
                new SqlParameter("@Id", resultFileId),
                new SqlParameter("@FileURL", objectURL),
                new SqlParameter("@Volume", resultFile.bucket),
                new SqlParameter("@FileName", resultFile.key),
                new SqlParameter("@FolderPath", resultFile.bucket),
                new SqlParameter("@FileSize", Int64.Parse("0")),
                new SqlParameter("@Description", resultFile.bucket + " : " + resultFile.key),
                new SqlParameter("@GNCloudFileCategoryId", resultFileCategoryId),
                new SqlParameter("@AWSRegionSystemName", analysisRequest.AWSRegionSystemName),
                new SqlParameter("@CreatedBy", analysisRequest.CreatedBy),
                new SqlParameter("@CreateDateTime", DateTime.Now));

            string associateFileToResultSQL =
                "INSERT INTO [gn].[GNAnalysisResultGNCloudFile]" +
                "([GNAnalysisResultGNCloudFile_GNCloudFile_Id],[ResultFiles_Id]) " +
                "VALUES " +
                "(@GNAnalysisResultGNCloudFile_GNCloudFile_Id," + //uniqueidentifier,>
                "@ResultFiles_Id)"; //uniqueidentifier,>

            db.Database.ExecuteSqlCommand(
                associateFileToResultSQL,
                new SqlParameter("@GNAnalysisResultGNCloudFile_GNCloudFile_Id", analysisResult.Id),
                new SqlParameter("@ResultFiles_Id", resultFileId));

            tx.Commit();

            bool auditResult = new GNCloudNoSQLService().LogEvent(analysisRequest.CreatedByContact, resultFileId, "RESULT_FILE", "N/A", "EVENT_INSERT");
        }
        
        public void UpdateDBMetadataForResultFiles(GNAnalysisResult analysisResult)
        {
            try
            {
                if (analysisResult.ResultFiles.Count() > 0)
                {
                    foreach (var item in analysisResult.ResultFiles.Where(a => a.GNCloudFileCategoryId == 6))
                    {
                        item.FileSize = this.cloudStorageService.GetObjectSize(item.Volume, item.FileName);
                        if (item.FileSize < 0)
                        {
                            analysisResult.Success = false;
                            break;
                        }
                    }
                    db.Entry(analysisResult).State = EntityState.Modified;
                    db.SaveChanges();
                    LogUtil.Info(logger, "Result files metadata updated");
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Unable to calculate size for file", e);
            }
        }


        public void UpdateFileExtensions(GNAnalysisResult analysisResult)
        {
            if (analysisResult.ResultFiles.Where(a => a.GNCloudFileCategoryId == 6).Count() > 0)
            {
                try
                {
                    IQueryable<GNCloudFileCategory> resultFileCategoryList = db.GNCloudFileCategories.Where(c => c.Type == "OUTPUT" && c.Id != 6);
                    if (resultFileCategoryList.Count() > 0)
                    {
                        Dictionary<string, int> extensionsDictionary = new Dictionary<string, int>();
                        foreach (GNCloudFileCategory item in resultFileCategoryList)
                        {
                            foreach (string ext in item.FileExtensions.Split(','))
                            {
                                if (!extensionsDictionary.ContainsKey(ext))
                                {
                                    extensionsDictionary.Add(ext, item.Id);
                                }                                
                            }                            
                        }

                        foreach (var item in analysisResult.ResultFiles.Where(a => a.GNCloudFileCategoryId == 6))
                        {
                            foreach (KeyValuePair<string, int> entry in extensionsDictionary)
                            {                    
                                if (item.FileName.Contains(entry.Key))
                                {
                                    item.GNCloudFileCategoryId = entry.Value;
                                }
                            }
                        }
                        db.SaveChanges();
                        LogUtil.Info(logger, "Result files extensions updated");
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(logger, "Error getting result file category id for File ", ex);
                }
            }
        }
        

        public override GNAnalysisRequest EvalEntitySecurity(GNContact userContact, GNAnalysisRequest analysisRequest)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (analysisRequest != null)
            {
                GNProject project = null;
                if (analysisRequest != null && analysisRequest.Project != null)
                {
                    project = analysisRequest.Project;
                }
                else if (analysisRequest != null && analysisRequest.GNProjectId != null)
                {
                    project = base.db.GNProjects.Find(analysisRequest.GNProjectId);
                }

                bool isAnalysisRequestOwnedByOrganization = IsAnalysisRequestOwnedByOrganization(userContact, analysisRequest, project);

                analysisRequest.CanCreate = true;

                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    analysisRequest.CanView = true;
                    analysisRequest.CanEdit = true;
                    analysisRequest.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    if (isAnalysisRequestOwnedByOrganization)
                    {
                        analysisRequest.CanView = true;
                        analysisRequest.CanEdit = true;
                        analysisRequest.CanDelete = true;
                    }
                }
                //TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
                else if (isAnalysisRequestOwnedByOrganization)
                {
                    analysisRequest.CanView = true;  //readonly mode for all members of an organization

                    bool isAssignedToTeamOfProjectForAnalysisRequest = project == null ? false :
                        teamService.IsUserAssignedToTeam(userContact, project.Teams.FirstOrDefault());

                    bool isAssignedToTeamAsLeadOfProjectForAnalysisRequest = project == null ? false :
                        teamService.IsUserAssignedToTeamAsLead(userContact, project.Teams.FirstOrDefault());

                    bool isCreatorOfProjectForAnalysisRequest = project == null ? false :
                        project.CreatedBy != null && project.CreatedBy != Guid.Empty && project.CreatedBy == userContact.Id;

                    bool isCreatorOfAnalysisRequest = project == null ? false :
                        analysisRequest.CreatedBy != null && project.CreatedBy != Guid.Empty && analysisRequest.CreatedBy == userContact.Id;

                    if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeamAsLeadOfProjectForAnalysisRequest)
                    {
                        analysisRequest.CanEdit = true;
                        analysisRequest.CanDelete = true;
                    }
                    else if (userContact.IsInRole("TEAM_MANAGER") && isAssignedToTeamOfProjectForAnalysisRequest)
                    {
                        analysisRequest.CanEdit = true;
                        analysisRequest.CanDelete = true;
                    }
                    else if (userContact.IsInRole("PROJECT_MANAGER") && isAssignedToTeamOfProjectForAnalysisRequest)
                    {
                        if (isCreatorOfProjectForAnalysisRequest || isCreatorOfAnalysisRequest)
                        {
                            analysisRequest.CanEdit = true;
                            analysisRequest.CanDelete = true;
                        }
                        else
                        {
                            analysisRequest.CanEdit = false;
                            analysisRequest.CanDelete = false;
                        }
                    }
                    else if (userContact.IsInRole("TEAM_MEMBER") && isAssignedToTeamOfProjectForAnalysisRequest)
                    {
                        if (isCreatorOfAnalysisRequest)
                        {
                            analysisRequest.CanEdit = true;
                            analysisRequest.CanDelete = true;
                        }
                        else
                        {
                            analysisRequest.CanEdit = false;
                            analysisRequest.CanDelete = false;
                        }
                    }
                }

                //Prevent Deletion of analysis requests with associated entities
                if (analysisRequest.CanDelete
                    && (analysisRequest.AnalysisResult == null)
                    && (analysisRequest.AnalysisStatus == null || analysisRequest.AnalysisStatus.Count == 0)
                    && (analysisRequest.GNAnalysisRequestGNSamples == null || analysisRequest.GNAnalysisRequestGNSamples.Count == 0))
                {
                    analysisRequest.CanDelete = true;
                }
                else
                {
                    analysisRequest.CanDelete = false;
                }
            }

            return analysisRequest;
        }

        private bool IsAnalysisRequestOwnedByOrganization(GNContact userContact, GNAnalysisRequest analysisRequest, GNProject project)
        {
            if (analysisRequest != null && project != null && project.Teams != null)
            {
                return userContact.GNOrganizationId == project.Teams.FirstOrDefault().OrganizationId;
            }
            else
            {
                return false;
            }
        }

        public async Task StartTertiaryAnalysis(GNContact userContact, Guid analysisRequestId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            //check if account is authorized to start analysis
            bool isAuthAllowedForAccount = await IsAccountAuthToStartAnalysis(analysisRequestId, userContact);

            if (isAuthAllowedForAccount)
            {
                if (this.analysisRequestPendingCloudMessageService == null
                    || this.cloudStorageService == null)
                {
                    this.InitCloudServices(userContact.GNOrganization.AWSConfigId);
                }

                LogUtil.Info(logger, "Finding analysis request [" + analysisRequestId + "]");


                //GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Where(a => a.Id == analysisRequestId).FirstOrDefault();

                
                //fetch analysis request from DB
                GNAnalysisRequest analysisRequest = db.GNAnalysisRequests
                    .Include(a => a.AnalysisType)
                    .Include(a => a.GNAnalysisRequestGNSamples.Select(s => s.GNSample).Select(s => s.CloudFiles))
                    .Include(a => a.Project)
                    .Include(a => a.Project.Teams)
                    .Include(a => a.Project.Teams.Select(t => t.Organization))
                    .Include(a => a.Project.Teams.Select(t => t.Organization.Account))
                    .Where(a => a.Id == analysisRequestId)
                    .FirstOrDefault();


                if (analysisRequest != null)
                {
                    AnalysisRequestS3Message analysisRequestS3Message = null;

                    try
                    {
                        LogUtil.Info(logger, "Building Analysis Request Message");
                        System.Console.WriteLine("\nBuilding Analysis Request Message");


                        //build analysis request
                        analysisRequestS3Message = new AnalysisRequestS3Message();
                        analysisRequestS3Message.analysisConfig = BuildAnalysisConfig(userContact);
                        analysisRequestS3Message.analysisRequest = BuildAnalysisRequest(analysisRequest);

                        analysisRequestS3Message.analysisConfig.options.Add("TERTIARY_ONLY", true);   // <-- to indicate ONLY Tertiary analysis

                        analysisRequestS3Message.analysisResult = new AnalysisResult
                        {
                            resultBucket = cloudStorageService.FetchAWSS3Bucket().ARN
                        };

                        LogUtil.Info(logger, "Updating Analysis Request in DB");
                        System.Console.WriteLine("\nUpdating Analysis Request in DB");

                        //update analysis request and status in DB
                        //<< update db
                        LogUtil.Info(logger, "Update Analysis Request Progress in DB");

                        db.GNAnalysisRequests.Find(analysisRequestId).RequestDateTime = DateTime.Now;
                        db.GNAnalysisRequests.Find(analysisRequestId).RequestProgress = 0;
                        db.GNAnalysisRequests.Find(analysisRequestId).LatestRequestPhase = "TERTIARY";
                        db.Entry(analysisRequest).State = EntityState.Modified;

                        db.GNAnalysisStatus.Add(new GNAnalysisStatus
                        {
                            Status = "TERTIARY ANALYSIS REQUESTED",
                            Message = "Tertiary Analysis Requested",
                            PercentComplete = 0,
                            IsError = false,
                            CreateDateTime = DateTime.Now,
                            CreatedBy = userContact.Id,
                            GNAnalysisRequestId = analysisRequest.Id,
                            AnalysisPhase = "TERTIARY"
                        });

                        db.SaveChanges();
                        // update db >>

                        LogUtil.Info(logger, "Send Analysis Request Message to Queue");
                        System.Console.WriteLine("\nSend Analysis Request Message to Queue");

                        /**
                         * tfrege 2016.03
                         * Store a file in S3 with the full message: this will prevent errors when the message size is > 256KB
                         */
                        //build message for queue   

                        var s3Bucket = db.AWSResources.Where(r => r.AWSResourceType.Name == "S3 bucket for Analysis Requests").FirstOrDefault();

                        AnalysisRequestSqsMessage analysisRequestSqsMessage = new AnalysisRequestSqsMessage
                        {
                            id = analysisRequest.Id.ToString(),
                            pathToData = new PathToData
                            {
                                bucket = s3Bucket.ARN, //tfrege change this
                                key = "message_" + analysisRequest.Id + "_tertiary.txt"
                            }
                        };

                        AnalysisRequestMessage SqsMessage = new AnalysisRequestMessage();
                        SqsMessage.AnalysisRequest = analysisRequestSqsMessage;


                        analysisRequestPendingCloudMessageService.StoreMessage(analysisRequestS3Message, SqsMessage.AnalysisRequest.pathToData.bucket, SqsMessage.AnalysisRequest.pathToData.key);

                        //send message to PENDING queue
                        analysisRequestPendingCloudMessageService.SendMessage(SqsMessage);


                        LogUtil.Info(logger, "Create billing transactions for each analysis sample");
                        System.Console.WriteLine("\nCreate billing transactions for each analysis sample");

                        string TransactionType = "ANALYSIS_TERTIARY_RERUN_" + analysisRequest.AnalysisType.TypeValue;
                        if (analysisRequest.AnalysisType.Id.ToString().ToUpper().Equals("PANEL"))
                        {
                            TransactionType = "ANALYSIS_TERTIARY_RERUN_PANEL";
                        }

                        int i = 1;
                        foreach (var item in analysisRequest.GNAnalysisRequestGNSamples)
                        {

                            //create billing transactions for each analysis sample
                            await this.transactionService.CreateBillingTransactionsPerAnalysisSample(
                                userContact,
                                analysisRequest,
                                analysisRequest.AnalysisType.Name,
                                analysisRequest.Description + "_" + i,
                                false,
                                true,
                                null,
                                null);

                            //create billing transactions for each analysis sample
                            //   await this.transactionService.CreateTransaction(
                            //      userContact, TransactionType, analysisRequest.Description+"_"+i, 1, "REQUEST_PER_SAMPLE");
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(logger, "Unable to build and send analysis request message to PENDING Queue", ex);
                        System.Console.WriteLine("\nUnable to build and send analysis request message to PENDING Queue: " + ex.StackTrace);
                        throw ex;
                    }
                }
            }
            else
            {
                System.Console.WriteLine("\nBilling Account is not authorized to start analysis due to insufficient funds and/or missing payment methods. Analysis Request ID = " + analysisRequestId);
                throw new Exception("Billing Account is not authorized to start analysis due to insufficient funds and/or missing payment methods. Analysis Request ID = " + analysisRequestId);
            }
        }


        public void CreateAnalysisRequestGroup(Guid analysisRequestId, GNAnalysisRequestGroup GNGroup)
        {
            if (analysisRequestId != Guid.Empty)
            {
                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNAnalysisRequestGroups]([Id], [Name], [GNAnalysisRequestId], [CreatedBy], [CreateDateTime]) " +
                    "VALUES(@Id, @name, @analysisRequestId, @createdBy, @createDateTime)",
                    new SqlParameter("@Id", GNGroup.Id),
                    new SqlParameter("@name", GNGroup.Name),
                    new SqlParameter("@analysisRequestId", analysisRequestId),
                    new SqlParameter("@createdBy", GNGroup.CreatedBy),
                    new SqlParameter("@createDateTime", GNGroup.CreateDateTime));

                tx.Commit();
            }
        }



        public void DeleteAnalysisRequestGroup(GNAnalysisRequestGroup GNGroup)
        {
            if (GNGroup != null)
            {
                var tx = db.Database.BeginTransaction();
                
                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNAnalysisRequestGNSamples] " +
                    "WHERE GNAnalysisRequestId = @GNAnalysisRequestId " +
                    "AND GNSampleId IN (SELECT GNSamples_Id FROM gn.GNAnalysisRequestGroupGNSample WHERE GNAnalysisRequestGroups_Id = @GroupId)",
                    new SqlParameter("@GroupId", GNGroup.Id),
                    new SqlParameter("@GNAnalysisRequestId", GNGroup.GNAnalysisRequestId));

                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNAnalysisRequestGroupComparisons] " +
                    "WHERE GNAnalysisRequestControlGroupId = @GNAnalysisRequestId "+
                    "OR GNAnalysisRequestComparisonGroupId = @GNAnalysisRequestControlGroupId ",
                    new SqlParameter("@GNAnalysisRequestId", GNGroup.Id),
                    new SqlParameter("@GNAnalysisRequestControlGroupId", GNGroup.Id));

                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNAnalysisRequestGroupGNSample] " +
                    "WHERE GNAnalysisRequestGroups_Id = @GNAnalysisRequestGroups_Id ",
                    new SqlParameter("@GNAnalysisRequestGroups_Id", GNGroup.Id));

                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNAnalysisRequestGroups] " +
                    "WHERE Id = @Id",
                    new SqlParameter("@Id", GNGroup.Id));
                
                tx.Commit();
            }
        }


        public void CreateAnalysisRequestGroupAssociation(Guid analysisRequestId, Guid controlGroupId, Guid comparisonGroupId)
        {
            if (controlGroupId != Guid.Empty && comparisonGroupId != Guid.Empty)
            {
                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNAnalysisRequestGroupComparisons]([Id], [GNAnalysisRequestId], [GNAnalysisRequestControlGroupId], [GNAnalysisRequestComparisonGroupId]) " +
                    "VALUES(@Id, @GNAnalysisRequestId, @controlGroupId, @comparisonGroupId)",
                    new SqlParameter("@Id", Guid.NewGuid()),
                    new SqlParameter("@GNAnalysisRequestId", analysisRequestId),
                    new SqlParameter("@controlGroupId", controlGroupId),
                    new SqlParameter("@comparisonGroupId", comparisonGroupId));

                tx.Commit();
            }
        }



        public void DeleteAnalysisRequestGroupAssociation(Guid analysisRequestId, Guid controlGroupId, Guid comparisonGroupId)
        {
            if (controlGroupId != Guid.Empty && comparisonGroupId != Guid.Empty)
            {
                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNAnalysisRequestGroupComparisons] " +
                    "WHERE GNAnalysisRequestId = @analysisRequestId AND GNAnalysisRequestControlGroupId = @controlGroupId AND GNAnalysisRequestComparisonGroupId = @comparisonGroupId",
                    new SqlParameter("@analysisRequestId", analysisRequestId),
                    new SqlParameter("@controlGroupId", controlGroupId),
                    new SqlParameter("@comparisonGroupId", comparisonGroupId));

                tx.Commit();
            }
        }

        /**
         * Text format - DEPRECATED
         */
        public async Task PrepareLaunchViewerTextMessage(GNAnalysisRequest analysisRequest, List<GNSample> samplesSelected, GNContact userContact)
        {
            GNCloudFile GNCloufVCF = (from a in analysisRequest.AnalysisResult.ResultFiles
                                      where a.GNCloudFileCategoryId == 2
                                      select a).FirstOrDefault<GNCloudFile>();
            string vcfFile = GNCloufVCF.Description.Substring(GNCloufVCF.Description.LastIndexOf('/') + 1);
            string fileContents = "";
            //List<string>.Enumerator enumerator = samplesSelected.GetEnumerator();
            try
            {
                int lineNum = 0;
                foreach (GNSample sampleInput in samplesSelected)
                {
                    lineNum++;
                    string sampleInputName = sampleInput.Name;
                    sampleInputName = sampleInputName.Replace(" ", "_").Replace("(", "_").Replace(")", "_").ToUpper();

                    string baiFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string bamFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bam") && !a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string item = bamFile + " " + baiFile + " " + analysisRequest.Id.ToString() + " " + vcfFile + " " + sampleInput.Name + " " + lineNum + " " + analysisRequest.Description.Replace(" ", "_").Trim().ToUpper();

                    fileContents = fileContents + item + "\n";

                    await this.transactionService.CreateBillingTransactionsPerAnalysisSample(userContact, analysisRequest, analysisRequest.AnalysisType.Name, analysisRequest.Description, false, false, "REPORT_BAM_VIEWER", sampleInput);
                }
                
            }
            catch (Exception e)
            {
                logger.Error("Error while launching BAM Viewer " + e.Message);
            }

            string fileName = "file.txt";
            GNCloudStorageService storage = new GNCloudStorageService();
            string bucket = "genomenext-bam-viewer";
            try
            {
                storage.PutObjectOnBucket(bucket, fileName, fileContents);
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw;
            }
        }

        /**
         * JSON Format
         */
        public async Task PrepareLaunchViewer(GNAnalysisRequest analysisRequest, List<GNSample> samplesSelected, GNContact userContact)
        {
            GNCloudFile GNCloudVCF = (from a in analysisRequest.AnalysisResult.ResultFiles
                                      where a.GNCloudFileCategoryId == 2
                                      select a).FirstOrDefault<GNCloudFile>();
            string vcfFile = GNCloudVCF.Description.Substring(GNCloudVCF.Description.LastIndexOf('/') + 1);
            string fileContents = "";
            //List<string>.Enumerator enumerator = samplesSelected.GetEnumerator();

            BamConfigFile bamRequest = new BamConfigFile();
            bamRequest.analysisId = analysisRequest.Id.ToString();
            bamRequest.analysisDescription = analysisRequest.Description;
            bamRequest.vcfFilename = vcfFile;
            bamRequest.listOfSamples = new List<AnalysisResultSample>();
            bamRequest.totalNumberOfSamples = samplesSelected.Count();

            try
            {
                int lineNum = 0;
                foreach (GNSample sampleInput in samplesSelected)
                {
                    lineNum++;
                    string sampleInputName = sampleInput.Name;
                    sampleInputName = sampleInputName.Replace(" ", "_").Replace("(", "_").Replace(")", "_").ToUpper();

                    string baiFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string bamFile = analysisRequest.AnalysisResult.ResultFiles.Where(a => a.FileName.Contains(sampleInputName) && a.FileName.Contains("bam") && !a.FileName.Contains("bai")).FirstOrDefault().FileName;
                    string item = bamFile + " " + baiFile + " " + analysisRequest.Id.ToString() + " " + vcfFile + " " + sampleInput.Name + " " + lineNum + " " + analysisRequest.Description.Replace(" ", "_").Trim().ToUpper();

                    fileContents = fileContents + item + "\n";

                 //   await this.transactionService.CreateBillingTransactionsPerAnalysisSample(userContact, analysisRequest, analysisRequest.AnalysisType.Name, analysisRequest.Description, false, false, "REPORT_BAM_VIEWER", sampleInput);
                    bamRequest.listOfSamples.Add(new AnalysisResultSample
                                    {
                                        index = lineNum,
                                        sampleId = sampleInput.Id.ToString(),
                                        name = sampleInputName,
                                        baiFilename = baiFile,
                                        bamFilename = bamFile
                                    }
                        );
                }

            }
            catch (Exception e)
            {
                logger.Error("Error while launching BAM Viewer " + e.Message);
            }

            string fileName = "jbrowse.json";
            GNCloudStorageService storage = new GNCloudStorageService();
            string bucket = "genomenext-bam-viewer";

            try
            {
                BamViewerService bamViewerService = new BamViewerService();
                bamViewerService.StoreMessage(bamRequest, bucket, fileName);
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                throw;
            }
        }


        public bool WaitForBamViewerResponse(Guid analysisId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod(), "Entering");
            
            BamViewerService bamViewerService = new BamViewerService();
            bamViewerService.matchingAnalysisId = analysisId;
          //  string indexFile = "https://secure.genomenext.net/GNPortal-1.1/jb/bamviewer/" + analysisId.ToString() + "/index.html";
            string indexFile = @"\\10.102.3.186\home\ubuntu\sync\" + analysisId.ToString() + @"\index.html";
            

            bool indexExists = false;
            try
            {
                do
                {
                    System.Threading.Thread.Sleep(1000); //wait 1 second

                    System.IO.FileInfo myFile = new System.IO.FileInfo(indexFile);
                    indexExists = myFile.Exists;
                    
                   // var lastDateModified = System.IO.File.GetLastWriteTime(indexFile);
                    //  } while (bamViewerService.CheckNewMessages() == false);
                } while (indexExists == false);
            }
            catch (Exception exception)
            {
                Exception exception2 = new Exception("Unable to read message from GN_BAM_VIEWER queue.", exception);
                LogUtil.Warn(logger, exception2.Message, exception2);
                return false;
            }
            return true;
        }

        public bool LaunchBamViewer(string fileName, string analysisId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod(), "Entering");
           // AnalysisRequestCloudMessageService analysisRequestCloudMessageService = new AnalysisRequestCloudMessageService();

            try
            {
                BamViewerMessage message = new BamViewerMessage();
                message.fileName = fileName;
                message.analysisId = analysisId;
                //nalysisRequestCloudMessageService.SendMessage(message);
            }
            catch (Exception exception)
            {
                Exception exception2 = new Exception("Unable to put message in queue.", exception);
                LogUtil.Warn(logger, exception2.Message, exception2);
                return false;
            }
            return true;
        }

 

    }

    /// <summary>
    /// Analysis Type Service
    /// </summary>
    public class AnalysisTypeService : GNEntityService<GNAnalysisType>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnalysisTypeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNAnalysisType>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNAnalysisType> entities =
                await db.GNAnalysisTypes
                .ToListAsync();

            return entities;
        }

        public override async Task<GNAnalysisType> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNAnalysisTypes.FindAsync(keys);
        }
    }


    /// <summary>
    /// Analysis Type Service
    /// </summary>
    public class AnalysisRequestTypeService : GNEntityService<GNAnalysisRequestType>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnalysisRequestTypeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNAnalysisRequestType>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNAnalysisRequestType> entities =
                await db.GNAnalysisRequestTypes
                .ToListAsync();

            return entities;
        }

        public override async Task<GNAnalysisRequestType> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNAnalysisRequestTypes.FindAsync(keys);
        }
    }

    /// <summary>
    /// Analysis Sample Affected Indicator Service
    /// </summary>
    public class AnalysisSampleAffectedIndicatorService : GNEntityService<GNAnalysisSampleAffectedIndicator>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnalysisSampleAffectedIndicatorService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNAnalysisSampleAffectedIndicator>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNAnalysisSampleAffectedIndicator> entities =
                await db.GNAnalysisSampleAffectedIndicators.ToListAsync();

            return entities;
        }

        public override async Task<GNAnalysisSampleAffectedIndicator> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNAnalysisSampleAffectedIndicators.FindAsync(keys);
        }
    }

    /// <summary>
    /// Message Consumer/Producer for 'GN_ANALYSIS_REQUEST_PENDING' queue
    /// </summary>
    public class AnalysisRequestPendingCloudMessageService : GNCloudMessageService<AnalysisRequestMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_ANALYSIS_REQUEST_PENDING";

        public AnalysisRequestCloudMessageService analysisRequestCloudMessageService { get; set; }
        public GNCloudComputeService cloudComputeService { get; set; }

        private IDictionary<string, int> justLaunchedAnalysesCount = new Dictionary<string, int>();

        public AnalysisRequestPendingCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
            analysisRequestCloudMessageService = new AnalysisRequestCloudMessageService(
                AWSConfigId, AnalysisRequestCloudMessageService.QUEUE_NAME);
            cloudComputeService = new GNCloudComputeService(AWSConfigId);
        }

        public override bool ProcessMessage(AnalysisRequestMessage analysisRequestMessage, object queueMessage)
        {
            bool success = false;

            //fetch analysis request
            GNAnalysisRequest analysisRequest = FetchAnalysisRequest(analysisRequestMessage);

            bool canInvokeAnalysis = false;

            //1. determine analysis resource availability
            IDictionary<string, int> resourcesAvailableByAWSEnv = CalcResourcesAvailableByAWSEnv(analysisRequest);

            //2. if resource requirements less than availability, send to GN_ANALYSIS_REQUEST queue
            //   else keep message in this queue
            foreach (var awsEnvKey in resourcesAvailableByAWSEnv.Keys)
            {
                AWSComputeEnvironment awsComputeEnv = db.AWSComputeEnvironments.Find(awsEnvKey);

                if (awsComputeEnv != null
                    && awsComputeEnv.MaxInstanceRequiredPerAnalysis <= resourcesAvailableByAWSEnv[awsEnvKey])
                {
                    if (analysisRequestMessage.AnalysisRequest.id != null)
                    {
                        /* TFS382 - TFrege 2015.03.26 Commented out the overwrite of awsEnvKey because it was causing all analyses with 1c to 
                         * be sent to the listener with 1d, which has few resources
                        //if resources available, set AWS_ENV and NUM_INSTANCES value
                        if (analysisRequestMessage.analysisConfig.options != null)
                        {
                            analysisRequestMessage.analysisConfig.options["AWS_ENV"] = awsEnvKey;
                        }
                        */

                        //set invoke flag to true
                        canInvokeAnalysis = true;

                        //increment just launched counter
                        justLaunchedAnalysesCount[awsEnvKey]++;

                        //break from loop
                        break;
                    }
                    else
                    {
                        throw new Exception("Unable to send analysis request message because 'analysisConfig' is missing.");
                    }
                }
            }

            //3. if resources available, invoke analysis
            if (canInvokeAnalysis)
            {
                analysisRequestCloudMessageService.SendMessage(analysisRequestMessage);
                success = true;
            }

            return success;
        }

        private IDictionary<string, int> CalcResourcesAvailableByAWSEnv(GNAnalysisRequest analysisRequest)
        {
            //set default values
            IDictionary<string, int> numInstancesAvailableByAWSEnv = new Dictionary<string, int>();

            //fetch organization
            System.Console.WriteLine("Searching Organization");

            var organization = analysisRequest.Project.Teams.FirstOrDefault().Organization;



            System.Console.WriteLine("Organization template ID: " + organization.GNSettingsTemplateId);


            if (organization != null
                && organization.GNSettingsTemplateId != null)
            {

                System.Console.WriteLine("Searching template");
                GNSettingsTemplate settingsTemplate = db.GNSettingsTemplates.Find(organization.GNSettingsTemplateId);


                System.Console.WriteLine("Template found");

                string configuredAWSEnv =
                    settingsTemplate.GNSettingsTemplateConfigs
                        .Where(ct =>
                            ct.GNSettingsTemplateField.ConfigType == "ANALYSIS_REQUEST"
                            && ct.GNSettingsTemplateField.Id == "AWS_ENV")
                        .Select(ct => ct.Value)
                        .FirstOrDefault();

                AWSComputeEnvironment awsComputeEnv = null;


                System.Console.WriteLine("configuredAWSEnv = " + configuredAWSEnv);


                if (!string.IsNullOrEmpty(configuredAWSEnv))
                {
                    //var awsComputeEnvironmentService = new AWSComputeEnvironmentService(this.db);
                    //awsComputeEnvironmentService.UpdateComputeCapacityInDB();

                    awsComputeEnv = new GNEntityModelContainer().AWSComputeEnvironments.Find(configuredAWSEnv);

                    var awsComputeEnvsInSameVPC = db.AWSComputeEnvironments
                        .Where(e => (e.VPC == awsComputeEnv.VPC))
                        .OrderBy(e => e.InstanceRunningCount);

                    foreach (var awsComputeEnvInVPC in awsComputeEnvsInSameVPC)
                    {
                        int justLaunchedAnalysesCountForEnv = 0;
                        if (justLaunchedAnalysesCount.Keys.Contains(awsComputeEnvInVPC.Id))
                        {
                            justLaunchedAnalysesCountForEnv = justLaunchedAnalysesCount[awsComputeEnvInVPC.Id];
                        }
                        else
                        {
                            justLaunchedAnalysesCount.Add(awsComputeEnvInVPC.Id, 0);
                        }

                        int numInstancesAvailable =
                            Math.Min(awsComputeEnvInVPC.MaxInstanceExpectedCount, awsComputeEnvInVPC.IPAvailCount)
                            - awsComputeEnvInVPC.InstanceRunningCount
                            - awsComputeEnvInVPC.InstancePendingCount
                            - justLaunchedAnalysesCountForEnv;

                        numInstancesAvailableByAWSEnv.Add(awsComputeEnvInVPC.Id, numInstancesAvailable);
                    }
                }

                if (awsComputeEnv == null)
                {
                    throw new Exception("Unable to calculate resources available due to a missing/invalid AWS_ENV config for analysis request " + analysisRequest.Id + "!");
                }
            }

            return numInstancesAvailableByAWSEnv;
        }

        private GNAnalysisRequest FetchAnalysisRequest(AnalysisRequestMessage analysisRequestMessage)
        {
            //get request by id
            GNAnalysisRequest analysisRequest = null;

            try
            {
                Guid analysisRequestId = Guid.Parse(analysisRequestMessage.AnalysisRequest.id.ToString().Trim());
                analysisRequest = db.GNAnalysisRequests.Find(analysisRequestId);
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Analysis Request Does Not Exist in DB!!", e);
                throw e;
            }

            return analysisRequest;
        }
    }

    /// <summary>
    /// Message Producer for 'GN_ANALYSIS_REQUEST' queue
    /// </summary>
    public class AnalysisRequestCloudMessageService : GNCloudMessageService<AnalysisRequestMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_ANALYSIS_REQUEST";

        public AnalysisRequestCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }
    }

    /// <summary>
    /// Message Consumer for 'GN_ANALYSIS_STATUS' queue
    /// </summary>
    public class AnalysisStatusCloudMessageService : GNCloudMessageService<AnalysisStatusMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_ANALYSIS_STATUS";

        public AnalysisStatusCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public override bool ProcessMessage(AnalysisStatusMessage analysisStatusMessage, object queueMessage)
        {
            bool success = false;

            if (analysisStatusMessage != null && analysisStatusMessage.analysisStatus != null)
            {
                //fetch analysis request
                GNAnalysisRequest analysisRequest = FetchAnalysisRequest(analysisStatusMessage);

                //check for an ERROR status and ensure isError flag is set
                if (!string.IsNullOrEmpty(analysisStatusMessage.analysisStatus.status)
                    && analysisStatusMessage.analysisStatus.status.Trim().ToUpper() == "ERROR")
                {
                    analysisStatusMessage.analysisStatus.isError = true;
                    analysisStatusMessage.analysisStatus.error = true;
                }

                if(analysisStatusMessage.analysisStatus.isError)
                {
                    analysisStatusMessage.analysisStatus.error = true;
                }

                if (analysisStatusMessage.analysisStatus.error)
                {
                    analysisStatusMessage.analysisStatus.isError = true;
                }

                //persist status
                db.GNAnalysisStatus.Add(new GNAnalysisStatus
                {
                    IsError = analysisStatusMessage.analysisStatus.isError,
                    Status = analysisStatusMessage.analysisStatus.status,
                    Message = analysisStatusMessage.analysisStatus.message,
                    PercentComplete = analysisStatusMessage.analysisStatus.percentComplete,
                    CreateDateTime = DateTime.Now,
                    GNAnalysisRequestId = analysisRequest.Id
                });

                db.SaveChanges();
                
                try
                {
                    var tx = db.Database.BeginTransaction();

                    db.Database.ExecuteSqlCommand(
                        "UPDATE [gn].[GNAnalysisRequests] " +
                        "SET [RequestProgress] = @requestProgress " +
                        "WHERE [Id] = @analysisRequestId",
                        new SqlParameter("@requestProgress", analysisStatusMessage.analysisStatus.percentComplete),
                        new SqlParameter("@analysisRequestId", analysisRequest.Id));

                    tx.Commit();
                }
                catch (Exception e)
                {
                    LogUtil.Warn(logger, "Unable to update RequestProgress on GNAnalysisRequests", e);
                }
                

                //if IsError=1, notify GNTech
                HandleStatusErrorMessage(analysisStatusMessage);

                success = true;
            }

            return success;
        }

        private GNAnalysisRequest FetchAnalysisRequest(AnalysisStatusMessage analysisStatusMessage)
        {
            //get request by id
            GNAnalysisRequest analysisRequest = null;

            try
            {
                Guid analysisRequestId = Guid.Parse(analysisStatusMessage.analysisRequestId.ToString().Trim());
                System.Console.WriteLine(">>>>>>>>>analysisRequestId " + analysisRequestId);  
                analysisRequest = db.GNAnalysisRequests.Find(analysisRequestId);
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Analysis Request Does Not Exist in DB!!", e);
                throw e;
            }

            return analysisRequest;
        }

        private void HandleStatusErrorMessage(AnalysisStatusMessage analysisStatusMessage)
        {
            try
            {
                if (analysisStatusMessage.analysisStatus.isError)
                {
                    Guid AnalysisRequestId = Guid.Parse(analysisStatusMessage.analysisRequestId.ToString().Trim());
                    GNContact Creator = db.GNAnalysisRequests.Where(a => a.Id.Equals(AnalysisRequestId)).Select(b => b.CreatedByContact).FirstOrDefault();

                    bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            "ANALYSIS_STATUS_UPDATE_RETURNED_ERROR",
                            Creator.Email,
                            "GNAnalysisRequest:" + analysisStatusMessage.analysisRequestId,
                            new Dictionary<string, string>
                                        {
                                            {"AnalysisRequestId",analysisStatusMessage.analysisRequestId},
                                            {"AnalysisStatus",analysisStatusMessage.analysisStatus.status},
                                            {"AnalysisStatusMessage",analysisStatusMessage.analysisStatus.message},
                                            {"PercentComplete",analysisStatusMessage.analysisStatus.percentComplete.ToString()},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });

                    notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            "ANALYSIS_STATUS_UPDATE_RETURNED_ERROR",
                            "support@genomenext.com",
                            "GNAnalysisRequest:" + analysisStatusMessage.analysisRequestId,
                            new Dictionary<string, string>
                                        {
                                            {"AnalysisRequestId",analysisStatusMessage.analysisRequestId},
                                            {"AnalysisStatus",analysisStatusMessage.analysisStatus.status},
                                            {"AnalysisStatusMessage",analysisStatusMessage.analysisStatus.message},
                                            {"PercentComplete",analysisStatusMessage.analysisStatus.percentComplete.ToString()},
                                            {"CreateDateTime",DateTime.Now.ToString()}
                                        });

                }
            }
            catch (Exception notificationEx)
            {
                LogUtil.Warn(logger, "Unable to send notification : " + notificationEx.Message, notificationEx);
            }
        }
    }

    /// <summary>
    /// Message Consumer for 'GN_ANALYSIS_RESULT' queue
    /// </summary>
    public class AnalysisResultCloudMessageService : GNCloudMessageService<AnalysisResultMessage>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int MIN_NUM_EXPECTED_RESULT_FILES = 3; //move later to config file or DB entry

        public const string QUEUE_NAME = "GN_ANALYSIS_RESULT";

        public AnalysisRequestService analysisRequestService { get; set; }
        public GNCloudStorageService cloudStorageService { get; set; }

        public AnalysisResultCloudMessageService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
            this.analysisRequestService = new AnalysisRequestService(new GNEntityModelContainer(), new IdentityModelContainer());
            this.cloudStorageService = new GNCloudStorageService();
            this.cloudStorageService.AWSConfigId = AWSConfigId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisResultMessage"></param>
        /// <param name="queueMessage"></param>
        /// <returns></returns>
        public override bool ProcessMessage(AnalysisResultMessage analysisResultMessage, object queueMessage)
        {
            bool success = false;
            
            //fetch analysis request
            GNAnalysisRequest analysisRequest = FetchAnalysisRequest(analysisResultMessage);
           
            //save OR update analysis result
            GNAnalysisResult analysisResult = SaveUpdateAnalysisResult(analysisResultMessage, analysisRequest, queueMessage);
           
            //if analysis result is a failure - process refund
            if (analysisResult.Success == false)
            {
                var t = Task.Run(async delegate
                {
                    TransactionService transactionService = new TransactionService(this.analysisRequestService.db);
                    await transactionService.ProcessRefundForAnalysisFailure(analysisRequest, analysisResult);
                });
            }

            //loop output files and update size
            this.analysisRequestService.UpdateDBMetadataForResultFiles(analysisResult);

            //update extensions
            this.analysisRequestService.UpdateFileExtensions(analysisResult);
            
            //send notification when analysis completes
            SendNotificationOnResult(analysisResult);
            
            //set success to true
            success = true;

            return success;
        }

        private bool ProcessPostAnalysisTasks(GNAnalysisRequest analysisRequest)
        {
            GNSettingsTemplateConfig config = analysisRequest.Project.Teams.FirstOrDefault().Organization.GNSettingsTemplate.GNSettingsTemplateConfigs.Where(a => a.Id.Equals("SEND_BAM_URL_POST_ANALYSIS")).FirstOrDefault();
            if(config != null)
            {
                string sqs_name = config.Value;
                BamPostAnalysisService bamService = new BamPostAnalysisService();
                bamService.Notify(analysisRequest);

                String NotificationTopic = "RESULTS_SENT_TO_CARTAGENIA";
                GNContact Creator = analysisRequest.CreatedByContact;
                
                //update db
                db.GNAnalysisStatus.Add(new GNAnalysisStatus
                {
                    Status = "CARTAGENIA",
                    Message = "VCF Sent to Cartagenia",
                    PercentComplete = 90,
                    IsError = false,
                    CreateDateTime = DateTime.Now,
                    GNAnalysisRequestId = analysisRequest.Id
                });

                db.SaveChanges();

                bool notifySuccess =
                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                        NotificationTopic,
                        Creator.Email,
                        "GNAnalysisRequest:" + analysisRequest.Id.ToString(),
                        new Dictionary<string, string>
                                        {
                                            {"AnalysisRequestId", analysisRequest.Id.ToString()},
                                            {"CreatorName", (Creator.FirstName != "" ? Creator.FirstName : "User")},
                                            {"AnalysisRequestDescription", analysisRequest.Description}
                                        });
            }

            BamConfigFileService bamRequestService = new BamConfigFileService();
            bamRequestService.PrepareBamViewer(analysisRequest);
            
            return true;
        }

        private GNAnalysisRequest FetchAnalysisRequest(AnalysisResultMessage analysisResultMessage)
        {
            //get request by id
            GNAnalysisRequest analysisRequest = null;

            try
            {
                Guid analysisRequestId = Guid.Parse(analysisResultMessage.analysisRequestId.ToString().Trim());
                analysisRequest = this.analysisRequestService.db.GNAnalysisRequests.Find(analysisRequestId);
            }
            catch (Exception e)
            {
                LogUtil.Warn(logger, "Analysis Request Does Not Exist in DB!!", e);
                throw e;
            }

            return analysisRequest;
        }

        private GNAnalysisResult SaveUpdateAnalysisResult(
            AnalysisResultMessage analysisResultMessage,
            GNAnalysisRequest analysisRequest,
            object queueMessage)
        {
            GNAnalysisResult analysisResult = analysisRequest.AnalysisResult;
            bool isNew = false;

            if (analysisResult == null || analysisResult.Id == null || analysisResult.Id == Guid.Empty)
            {
                isNew = true;

                analysisResult = new GNAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = analysisRequest.CreatedBy,
                    CreateDateTime = DateTime.Now
                };
            }

            analysisResult.Success = analysisResultMessage.analysisResult.success;
            analysisResult.AWSRegionSystemName = analysisRequest.AWSRegionSystemName;

            if (analysisResultMessage.analysisResult != null && analysisResultMessage.analysisResult.executionStartDateTime != 0)
            {
                analysisResult.AnalysisStartDateTime = new DateTime(1970, 1, 1).AddMilliseconds(analysisResultMessage.analysisResult.executionStartDateTime);
            }
            else
            {
                analysisResult.AnalysisStartDateTime = new DateTime();
            }

            if (analysisResultMessage.analysisResult != null && analysisResultMessage.analysisResult.executionEndDateTime != 0)
            {
                analysisResult.AnalysisEndDateTime = new DateTime(1970, 1, 1).AddMilliseconds(analysisResultMessage.analysisResult.executionEndDateTime);
            }
            else
            {
                analysisResult.AnalysisEndDateTime = new DateTime();
            }

            //persist result
            if (isNew)
            {
                this.analysisRequestService.db.GNAnalysisResults.Add(analysisResult);
            }
            this.analysisRequestService.db.SaveChanges();

            //link result to analysis request
            if (isNew)
            {
                analysisResult = this.analysisRequestService.db.GNAnalysisResults.Find(analysisResult.Id);
                analysisResult.AnalysisRequest = analysisRequest;

                this.analysisRequestService.db.Entry(analysisResult).State = EntityState.Modified;
                this.analysisRequestService.db.SaveChanges();
            }

            //load and link analysis result files
            if (analysisResultMessage.analysisResult.resultFiles != null
                && analysisResultMessage.analysisResult.resultFiles.Count != 0)
            {
                foreach (var resultFile in analysisResultMessage.analysisResult.resultFiles)
                {
                    this.analysisRequestService.AddResultFile(resultFile, analysisRequest, analysisResult);


                    //tfrege 2016.09 - if the result file is VCF, notify VCF-QC
                    if (resultFile.key.ToUpper().Contains("VCF") && resultFile.key.ToUpper().EndsWith(".VCF"))
                    {
                        System.Console.WriteLine("***\n ****** NOTIFY NotifyQCSystem " + resultFile.key);
                        StartQcReportService startReport = new StartQcReportService();
                        startReport.NotifyQCSystem(analysisRequest, resultFile.bucket, resultFile.key);

                        //Send VCF to Cartagenia 
                        this.ProcessPostAnalysisTasks(analysisRequest);
                    }
                }
            }

            if (analysisResult != null)
            {
                this.DeleteMessage(queueMessage);
            }

            return analysisResult;
        }

        private void SendNotificationOnResult(GNAnalysisResult analysisResult)
        {
            LogUtil.Info(logger, "SendNotificationOnResult " + analysisResult.AnalysisRequest.LatestRequestPhase);
            try
            {
                string NotificationTopic = "ANALYSIS_RESULT_RECEIVED";
                
                GNAnalysisRequest request = db.GNAnalysisRequests.Where(a => a.Id.Equals(analysisResult.AnalysisRequest.Id)).FirstOrDefault();
                Guid CreatedBy = Guid.Parse(request.CreatedBy.ToString().Trim());
                
                if (request.LatestRequestPhase.Equals("TERTIARY"))
                {
                    NotificationTopic = "ANALYSIS_TERTIARY_RUN_COMPLETED";
                    try
                    {
                        string stringStatusName = "TERTIARY ANALYSIS REQUESTED";
                        List<GNAnalysisStatus> ListStatus = db.GNAnalysisStatus.Where(a => a.GNAnalysisRequestId == request.Id && a.Status == stringStatusName).OrderByDescending(b => b.CreateDateTime).ToList();
                        if (ListStatus.Count() > 0)
                        {
                            Guid CreatedByGuid = Guid.Parse(ListStatus.FirstOrDefault().CreatedBy.ToString());
                        
                            if (CreatedByGuid != null)
                            {
                                CreatedBy = CreatedByGuid;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(">>>>>>>>>can't find it!!! "+ ex.Message + ex.StackTrace + ex.InnerException);    
                    }                                     
                } 
 
                GNContact Creator = db.GNContacts.Where(a => a.Id.Equals(CreatedBy)).FirstOrDefault();
                String CreatorName = Creator.FirstName.ToString();
                string EndDateTime = TimeZoneInfo.ConvertTime((DateTime)analysisResult.AnalysisEndDateTime, Creator.GNOrganization.OrgTimeZoneInfo).ToString();

                bool notifySuccess =
                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                        NotificationTopic,
                        Creator.Email,
                        "GNAnalysisRequest:" + analysisResult.AnalysisRequest.Id.ToString(),
                        new Dictionary<string, string>
                                        {
                                            {"AnalysisRequestId", analysisResult.AnalysisRequest.Id.ToString()},
                                            {"CreatorName", (CreatorName != "" ? CreatorName : "User")},
                                            {"AnalysisRequestDescription", analysisResult.AnalysisRequest.Description},
                                            {"NumFiles", analysisResult.ResultFiles.Count().ToString()},
                                            {"EndDateTime", EndDateTime}
                                        });
            }
            catch (Exception notificationEx)
            {
                LogUtil.Warn(logger, "Unable to send notification : " + notificationEx.Message, notificationEx);
            }
        }

        private void SendNotificationOnAnalysisComplete(GNAnalysisRequest analysisRequest, GNAnalysisResult analysisResult)
        {
            string NotificationTopic = "ANALYSIS_COMPLETED_SUCCESSFULLY";
            // NotificationTopic = "ANALYSIS_RETURNED_ERROR";
            if (analysisRequest.LatestRequestPhase == "TERTIARY")
            {
                NotificationTopic = "ANALYSIS_TERTIARY_RUN_COMPLETED";
            }            

            int NumResultFiles = db.GNAnalysisRequests.Where(a => a.Id.Equals(analysisResult.AnalysisRequest.Id)).FirstOrDefault().AnalysisResult.ResultFiles.Count();

            if (NumResultFiles >= MIN_NUM_EXPECTED_RESULT_FILES)
            {
                try
                {
                    Guid CreatedBy = Guid.Parse(analysisResult.AnalysisRequest.CreatedBy.ToString().Trim());
                    GNContact Creator = db.GNContacts.Where(a => a.Id.Equals(CreatedBy)).FirstOrDefault();
                    String CreatorName = Creator.FirstName.ToString();
                    string EndDateTime = TimeZoneInfo.ConvertTime((DateTime)analysisResult.AnalysisEndDateTime, Creator.GNOrganization.OrgTimeZoneInfo).ToString();

                    bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                            NotificationTopic,
                            Creator.Email,
                            "GNAnalysisRequest:" + analysisResult.AnalysisRequest.Id.ToString(),
                            new Dictionary<string, string>
                                        {
                                            {"AnalysisRequestId", analysisResult.AnalysisRequest.Id.ToString()},
                                            {"CreatorName", (CreatorName != "" ? CreatorName : "User")},
                                            {"AnalysisRequestDescription", analysisResult.AnalysisRequest.Description},
                                            {"EndDateTime", EndDateTime}
                                        });
                }
                catch (Exception notificationEx)
                {
                    LogUtil.Warn(logger, "Unable to send notification : " + notificationEx.Message, notificationEx);
                }
            }
        }



    }

    public class AnalysisAdaptorService : GNEntityService<GNAnalysisAdaptor>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnalysisAdaptorService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNAnalysisAdaptor>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNAnalysisAdaptor> entities =
                await db.GNAnalysisAdaptors.ToListAsync();

            return entities;
        }

        public override async Task<GNAnalysisAdaptor> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNAnalysisAdaptors.FindAsync(keys);
        }
    }


    public class AnalysisBamViewerService : GNCloudMessageService<BamViewer>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string QUEUE_NAME = "GN_BAM_VIEWER";

        public AnalysisBamViewerService(Guid AWSConfigId, string queueName)
            : base(AWSConfigId, queueName)
        {
        }

        public AnalysisBamViewerService()
        {
            base.AWSConfigId = new GNEntityModelContainer().AWSConfigs.FirstOrDefault().Id;
            base.Connect();
            base.LoadQueueURL(QUEUE_NAME);
        }

        public override bool ProcessMessage(BamViewer bamViewerMessage, object queueMessage)
        {
            System.Console.WriteLine("***\n ---->BAM VIEWER PROCESSING FOR : " + bamViewerMessage.analysisId);

            bool success = false;

            Guid analysisId = Guid.Parse(bamViewerMessage.analysisId);


            System.Console.WriteLine("***\n ---->analysisId : " + analysisId);

            GNAnalysisRequest analysisRequest = db.GNAnalysisRequests.Find(bamViewerMessage);


            string url = "localhost/jbrowse/" + analysisRequest.Id.ToString();
            System.Diagnostics.Process.Start(url);
            success = true;
            return success;
        }

    }

}