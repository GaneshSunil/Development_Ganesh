using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Models;
using GenomeNext.Portal.ControllerExtensions;
using System.Data.SqlClient;
using GenomeNext.App;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    public class TemplateGenesController : GNEntityController<GNTemplateGene>
    {
        public TemplateGenesController()
            : base()
        {
            entityService = new TemplateGeneService(base.db);
        }
        
        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Create(GNTemplateGene entity)
        {
            return await base.Create(entity);
        }
    }
}
