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
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class ReplicatesController : GNEntityController<GNReplicate>
    {
        public ReplicatesController()
            : base()
        {
            entityService = new ReplicateService(base.db);
        }

        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Create(GNReplicate entity)
        {

            return await base.Create(entity);
        }

    }
}
