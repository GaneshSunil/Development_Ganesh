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
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    public class SequencerJobsController : GNEntityController<GNSequencerJob>
    {
        public SequencerJobsController()
            : base()
        {
            entityService = new SequencerJobService(base.db);
        }

    }
}
