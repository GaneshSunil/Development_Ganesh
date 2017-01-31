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

namespace GenomeNext.App
{
    public class ReplicateService : GNEntityService<GNReplicate>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReplicateService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }


        public override async Task<List<GNReplicate>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNReplicate> replicates = db.GNReplicates;


            //Order By Results
            replicates = replicates
                .OrderByDescending(t => t.CreateDateTime);

            return await replicates.ToListAsync();
        }


        public override async Task<GNReplicate> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNReplicates.FindAsync(keys);
        }




    }

}
