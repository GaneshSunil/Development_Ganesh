using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data;
using GenomeNext.Cloud.CloudNoSQL;
using System.Reflection;
using System.Threading.Tasks;

namespace GenomeNext.App
{
    public class TemplateService : GNEntityService<GNTemplate>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "TEMPLATE";

        public TemplateService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNTemplate>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());


            IQueryable<GNTemplate> Templates = db.GNTemplates;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;
                
                if (filters.ContainsKey("Name"))
                {
                    filterVal = filters["Name"].ToString().Trim().ToUpper();
                    Templates = Templates.Where(t => t.Name.ToUpper().Contains(filterVal));
                }
            }

            //Order By Results
            Templates = Templates
                .OrderBy(t => t.Name);

            //Limit Result Size
            Templates = Templates.Skip(start).Take(end - start);

            return Templates.ToList();
        }

        public override async Task<GNTemplate> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await db.GNTemplates.FindAsync(keys);
        }


        public async Task<int> AddRelationship(int TemplateId, GNGene Gene, GNContact userContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            int result = 0;

            return await Task.Factory.StartNew<int>(() =>
            {
                var tx = db.Database.BeginTransaction();

                result = db.Database.ExecuteSqlCommand(
                    "INSERT INTO [gn].[GNTemplateGenes]([GNTemplateId],[GeneCode],[GNGeneId],[CreatedBy],[CreatedDateTime]) " +
                    "VALUES(@templateId, @geneCode, @geneId, @CreatedBy, @CreateDateTime)",
                    new SqlParameter("@templateId", TemplateId),
                    new SqlParameter("@geneCode", Gene.GeneCode),
                    new SqlParameter("@geneId", Gene.Id),
                    new SqlParameter("@CreatedBy", userContact.AspNetUserId),
                    new SqlParameter("@CreateDateTime", DateTime.Now));

                tx.Commit();

                return result;
            });

            //bool auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(leftSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");
            //auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(rigthSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");

        }

        public async Task<int> RemoveGeneFromTemplate(int AssociationId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            int result = 0;

            return await Task.Factory.StartNew<int>(() =>
            {
                var tx = db.Database.BeginTransaction();

                result = db.Database.ExecuteSqlCommand(
                    "DELETE FROM [gn].[GNTemplateGenes] " +
                    "WHERE Id = @AssociationId",
                    new SqlParameter("@AssociationId", AssociationId));

                tx.Commit();

                return result;
            });

            //bool auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(leftSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");
            //auditResult = new GNCloudNoSQLService().LogEvent(userContact, Guid.Parse(rigthSampleId), this.ENTITY, "N/A", "INSERT_RELATIONSHIP");

        }


        public async Task<int> AddTemplateToAnalysisRequest(int TemplateId, string analysisRequestId, GNContact UserContact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (TemplateId != null && !string.IsNullOrEmpty(analysisRequestId))
                {
                    var tx = db.Database.BeginTransaction();

                    result = db.Database.ExecuteSqlCommand(
                        "INSERT INTO [gn].[GNAnalysisRequestGNTemplates]([Id],[GNAnalysisRequestId],[GNTemplateId],[CreatedBy],[CreatedDateTime]) " +
                        "VALUES(@Id, @analysisRequestId, @TemplateId, @CreatedBy, @CreatedDateTime)",
                        new SqlParameter("@Id", Guid.NewGuid()),
                        new SqlParameter("@analysisRequestId", Guid.Parse(analysisRequestId)),
                        new SqlParameter("@TemplateId", TemplateId),
                        new SqlParameter("@CreatedBy", UserContact.Id),
                        new SqlParameter("@CreatedDateTime", DateTime.Now)
                        );

                    tx.Commit();
                }

                return result;
            });
        }
        
        public async Task<int> RemoveTemplateFromAnalysisRequest(int TemplateId, string analysisRequestId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                if (!string.IsNullOrEmpty(analysisRequestId))
                {
                    var tx = db.Database.BeginTransaction();

                    db.Database.ExecuteSqlCommand(
                        "DELETE FROM [gn].[GNAnalysisRequestGNTemplates] " +
                        "WHERE [GNAnalysisRequestId] = @analysisRequestId " +
                        "AND [GNTemplateId] = @TemplateId",
                        new SqlParameter("@analysisRequestId", Guid.Parse(analysisRequestId)),
                        new SqlParameter("@TemplateId", TemplateId));

                    tx.Commit();
                }

                return result;
            });
        }
    }

    public class TemplateGeneService : GNEntityService<GNTemplateGene>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "TEMPLATE_GENE";

        public TemplateGeneService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNTemplateGene>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());


            IQueryable<GNTemplateGene> TemplateGenes = db.GNTemplateGenes;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("GeneCode"))
                {
                    filterVal = filters["GeneCode"].ToString().Trim().ToUpper();
                    TemplateGenes = TemplateGenes.Where(t => t.GeneCode.ToUpper().Contains(filterVal));
                }
            }

            //Order By Results
            TemplateGenes = TemplateGenes
                .OrderBy(t => t.GeneCode);

            //Limit Result Size
            TemplateGenes = TemplateGenes.Skip(start).Take(end - start);

            return TemplateGenes.ToList();
        }

        public override async Task<GNTemplateGene> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            return await db.GNTemplateGenes.FindAsync(keys);
        }

    }
}
