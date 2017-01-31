using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Data.EntityModel;
using GenomeNext.App;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class LogsController : GNEntityController<GNLog>
    {
        public LogsController()
            : base()
        {
            entityService = new LogEntityService(base.db);
        }

        public ActionResult SetLogLevel(int level, string levelName)
        {
            foreach (var repo in LogManager.GetAllRepositories())
            {
                foreach (var logger in repo.GetCurrentLoggers().OfType<Logger>())
                {
                    logger.Level = new Level(level, levelName);
                }
            }

            return RedirectToAction("Index", new { filters = Request["filters"]});
        }
    }
}
