using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Helpers.Wrappable
{
    public class GNClickableListGroupItem : IDisposable
    {
        protected ViewContext _viewContext;

        public GNClickableListGroupItem(ViewContext context, string url, bool isAlternateRow)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;

            Begin(url, isAlternateRow);

            return;
        }

        private void Begin(string url, bool isAlternateRow)
        {
            _viewContext.Writer.Write(
                "<div class=\"list-group-item clickable clearfix" + (isAlternateRow ? " bg-gn-light-gray" : "") + "\" " +
                        "onclick=\"window.location.href='"+url+"'\">");
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