using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data;

namespace GenomeNext.App
{
    public class SettingsTemplateService : GNEntityService<GNSettingsTemplate>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SettingsTemplateService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSettingsTemplate>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSettingsTemplate> Templates = db.GNSettingsTemplates;


            return Templates.ToList();
        }

        public override async Task<GNSettingsTemplate> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSettingsTemplates.FindAsync(keys);
        }
    }

    public class SettingsTemplateConfigService : GNEntityService<GNSettingsTemplateConfig>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SettingsTemplateConfigService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNSettingsTemplateConfig>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSettingsTemplateConfig> TemplateConfigs = db.GNSettingsTemplateConfigs;


            return TemplateConfigs.ToList();
        }

        public override async Task<GNSettingsTemplateConfig> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSettingsTemplateConfigs.FindAsync(keys);
        }


        public override async Task<GNSettingsTemplateConfig> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            GNSettingsTemplateConfig config = (GNSettingsTemplateConfig)entity;

            try
            {
                db.Entry(config).State = EntityState.Modified;

                await db.SaveChangesAsync();

                var tx = db.Database.BeginTransaction();

                db.Database.ExecuteSqlCommand(
                    "UPDATE [gn].[GNSettingsTemplateConfigs] SET [Value] = @value, " +
                    "[GNSettingsTemplate_Id] = @GNSettingsTemplate_Id, " +
                    "[GNSettingsTemplateField_Id] = @GNSettingsTemplateField_Id " +
                    "WHERE [Id] = @configId",
                    new SqlParameter("@value", config.Value),
                    new SqlParameter("@GNSettingsTemplate_Id", config.GNSettingsTemplate.Id),
                    new SqlParameter("@GNSettingsTemplateField_Id", config.GNSettingsTemplateField.Id),
                    new SqlParameter("@configId", config.Id));

                tx.Commit();
            }
            catch (Exception e1)
            {
                string errorMsg = "Unable to update GNSettingsTemplateConfigs.";
                errorMsg += GetSqlExceptionErrorMessage(e1);

                Exception e2 = new Exception(errorMsg, e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            return config;
        }

    }


    public class SettingsTemplateFieldService : GNEntityService<GNSettingsTemplateField>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SettingsTemplateFieldService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }
        
        public override async Task<List<GNSettingsTemplateField>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNSettingsTemplateField> TemplateFields = db.GNSettingsTemplateFields;


            return TemplateFields.ToList();
        }

        public override async Task<GNSettingsTemplateField> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNSettingsTemplateFields.FindAsync(keys);
        }
    }


}
