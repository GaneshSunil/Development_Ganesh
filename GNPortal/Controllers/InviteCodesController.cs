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
    public class InviteCodesController : GNEntityController<GNInviteCode>
    {
        public InviteCodesController()
            : base()
        {
            entityService = new InviteCodeService(base.db);
        }

        public override GNInviteCode CreateOnLoad()
        {
            GNInviteCode inviteCode = new GNInviteCode 
            {
                UseCount = 0
            };

            return inviteCode;
        }

        public override ActionResult CreateOnSuccess(GNInviteCode entity)
        {
            return RedirectToAction("Details", new { id = entity.InviteCode});
        }

        public override ActionResult EditOnSuccess(GNInviteCode entity)
        {
            return RedirectToAction("Details", new { id = entity.InviteCode});
        }

        protected override object ConvertEntityId(string id)
        {
            return id;
        }
    }
}
