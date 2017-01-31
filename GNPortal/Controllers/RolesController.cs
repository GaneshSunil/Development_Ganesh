using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Data.IdentityModel;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class RolesController : IdentityController<AspNetRole>
    {
        public RolesController()
            : base()
        {
            entityService = new AspNetRoleService(base.identityDB);
        }
    }
}
