using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Helpers.Wrappable
{
    public class GNPanel: IDisposable
    {
        protected ViewContext _viewContext;

        public GNPanel(ViewContext context, bool isFlushLeft)
        {
            if (context == null)
                throw new ArgumentNullException("context");
 
            _viewContext = context;

            Begin(isFlushLeft);
 
            return;
        }

        private void Begin(bool isFlushLeft)
        {
            _viewContext.Writer.Write("<div class=\"col-xs-12 col-sm-12 col-md-12 col-lg-12 " + (isFlushLeft ? "flushLeft" : "") + "\">");
            _viewContext.Writer.Write("    <div class=\"panel border-gn-dark-gray\">");
        }

        private void End()
        {
            _viewContext.Writer.Write("    </div>");
            _viewContext.Writer.Write("</div>");
        }

        public void Dispose()
        {
            End();
        }
    }

    public class GNPanelHeading : IDisposable
    {
        protected ViewContext _viewContext;

        public GNPanelHeading(ViewContext context, string panelTitle, string glyphicon, string id = null)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;

            Begin(panelTitle, glyphicon, id);

            return;
        }

        private void Begin(string panelTitle, string glyphicon, string id = null)
        {
            _viewContext.Writer.Write("        <div class=\"panel-heading bg-gn-dark-gray\" >");
            _viewContext.Writer.Write("            <h3 class=\"panel-title flushLeft white\">");
            if(glyphicon != null)
            {
                _viewContext.Writer.Write("                <span class=\"glyphicon glyphicon-" + glyphicon + "\"></span>");
            }
            _viewContext.Writer.Write("                <span>" + panelTitle  + "</span>");
        }

        private void End()
        {
            _viewContext.Writer.Write("            </h3>");
            _viewContext.Writer.Write("        </div>");
        }

        public void Dispose()
        {
            End();
        }
    }

    public class GNPanelBody : IDisposable
    {
        protected ViewContext _viewContext;

        public GNPanelBody(ViewContext context, bool isListGroup, string id = null)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;

            Begin(isListGroup, id);

            return;
        }

        private void Begin(bool isListGroup, string id = null)
        {
            if(isListGroup)
            {
                _viewContext.Writer.Write("        <div class=\"list-group\">");
            }
            else
            {
                _viewContext.Writer.Write("        <div class=\"panel-body\" id=\""+id+"\">");
            }
        }

        private void End()
        {
            _viewContext.Writer.Write("            </div>");
        }

        public void Dispose()
        {
            End();
        }
    }

    public class GNPanelFooter : IDisposable
    {
        protected ViewContext _viewContext;

        public GNPanelFooter(ViewContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _viewContext = context;

            Begin();

            return;
        }

        private void Begin()
        {
            _viewContext.Writer.Write("            <div class=\"panel-footer clearfix\">");
        }

        private void End()
        {
            _viewContext.Writer.Write("            </div>");
        }

        public void Dispose()
        {
            End();
        }
    }
}