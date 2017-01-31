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
    public class SampleQualifierGroupsController : GNEntityController<GNSampleQualifierGroup>
    {
        public SampleQualifierGroupsController()
            : base()
        {
            entityService = new SampleQualifierGroupService(base.db);
        }
    }
}
