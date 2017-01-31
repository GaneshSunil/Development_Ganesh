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
using GenomeNext.Billing;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class ProductsController : GNEntityController<GNProduct>
    {
        public ProductsController()
            : base()
        {
            entityService = new ProductService(base.db);
        }

        public override GNProduct PopulateSelectLists(GNProduct product = null)
        {
            product = base.PopulateSelectLists(product);

            var subFreqIntervals = from GNAccountProductSubscription.SubscriptionFrequencyInterval s
                               in Enum.GetValues(typeof(GNAccountProductSubscription.SubscriptionFrequencyInterval))
                           select new { Id = (int)s, Name = s.ToString() };

            if (product != null)
            {
                ViewBag.GNProductTypeId = new SelectList(db.GNProductTypes, "Id", "Name", product.GNProductTypeId);
                ViewBag.GNAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description", product.GNAccountTypeId);
                ViewBag.SubscribeFrequency = new SelectList(subFreqIntervals, "Id", "Name", product.SubscribeFrequency);
            }
            else
            {
                ViewBag.GNProductTypeId = new SelectList(db.GNProductTypes, "Id", "Name");
                ViewBag.GNAccountTypeId = new SelectList(db.GNAccountTypes, "Id", "Description");
                ViewBag.SubscribeFrequency = new SelectList(subFreqIntervals, "Id", "Name");
            }

            return product;
        }
    }
}
