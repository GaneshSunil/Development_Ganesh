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

namespace GenomeNext.App
{
    public class LogEntityService : GNEntityService<GNLog>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LogEntityService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNLog>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNLog> logs = db.GNLogs;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Server"))
                {
                    filterVal = (string)filters["Server"];
                    logs = logs.Where(l => l.Server.Contains(filterVal));
                }

                if (filters.ContainsKey("Thread"))
                {
                    filterVal = (string)filters["Thread"];
                    logs = logs.Where(l => l.Thread.Contains(filterVal));
                }

                if (filters.ContainsKey("Level"))
                {
                    filterVal = (string)filters["Level"];
                    logs = logs.Where(l => l.Level.Contains(filterVal));
                }

                if (filters.ContainsKey("Logger"))
                {
                    filterVal = (string)filters["Logger"];
                    logs = logs.Where(l => l.Logger.Contains(filterVal));
                }

                if (filters.ContainsKey("Message"))
                {
                    filterVal = (string)filters["Message"];
                    logs = logs.Where(l => l.Message.Contains(filterVal));
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    logs = logs
                        .Where(l =>
                            l.Server.Contains(filterVal)
                            || l.Thread.Contains(filterVal)
                            || l.Level.Contains(filterVal)
                            || l.Logger.Contains(filterVal)
                            || l.Message.Contains(filterVal));
                }
            }

            //Order By Results
            logs = logs
                .OrderBy(l => l.Server)
                .OrderBy(l => l.Thread)
                .OrderBy(l => l.Level)
                .OrderBy(l => l.Logger)
                .OrderByDescending(l => l.Date);

            //Limit Result Size
            logs = logs.Skip(start).Take(end - start);

            return await logs.ToListAsync();
        }

        public override async Task<GNLog> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNLogs.FindAsync(keys);
        }
    }
}
