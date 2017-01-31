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
    public class SampleRelationshipTypeMappingsController : GNEntityController<GNSampleRelationshipTypeMapping>
    {
        public SampleRelationshipTypeMappingsController()
            : base()
        {
            entityService = new SampleRelationshipTypeMappingService(base.db);
        }
        
        public override GNSampleRelationshipTypeMapping PopulateSelectLists(GNSampleRelationshipTypeMapping mapping = null)
        {
            List<GNSampleRelationshipType> RelationshipTypes = db.GNSampleRelationshipTypes.OrderBy(a => a.MaxRelationships).ThenBy(a => a.Name).ToList();
            ViewBag.GNSampleRelationshipTypes = new SelectList(RelationshipTypes, "Name", "Name");

            return base.PopulateSelectLists(mapping);
        }
        
        public override async Task<ActionResult> Index()
        {
            PopulateSelectLists();

            return await base.Index();
        }
    }

}
