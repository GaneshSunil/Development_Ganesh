using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Helpers.Wrappable
{
    public class GNModuleIndexHeading : IDisposable
    {
        protected ViewContext _viewContext;

        public GNModuleIndexHeading(ViewContext context, string title, string iconPath, int titleColsWidth)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;

            Begin(title, iconPath, titleColsWidth);

            return;
        }

        private void Begin(string title, string iconPath, int titleColsWidth)
        {
            _viewContext.Writer.Write("<div id=\"moduleIndexHeading\" class=\"col-md-12 panel panel-body bg-gn-dark-gray\">" +
                "<img class=\"col-xs-2 col-sm-2 col-md-2 col-lg-2 img-responsive\"" +
                "     style=\"max-height:70px;max-width:70px;margin:0px;padding:0px;\"" +
                "     src=\"" + iconPath + "\" />" +
                "<h2 class=\"col-xs-10 col-sm-" + titleColsWidth + " col-md-" + titleColsWidth + " col-lg-" + titleColsWidth + " white text-inline\">" + title + "</h2>");
        }

        private void End()
        {
            _viewContext.Writer.Write("        </div>");
        }

        public void Dispose()
        {
            End();
        }
    }
}