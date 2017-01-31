using System.Web;
using System.Web.Mvc;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GetContactUserAttribute());
            filters.Add(new AuthorizeRedirect());
            filters.Add(new GNHandleErrorAttribute());
            filters.Add(new GetNavBarInfoAttribute());
        }
    }
}
