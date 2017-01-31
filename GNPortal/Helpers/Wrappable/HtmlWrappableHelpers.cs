using GenomeNext.Portal.Helpers.Wrappable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GenomeNext.Portal.Helpers
{
    public static class HtmlWrappableHelpers
    {
        public static GNClickableListGroupItem BeginGNClickableListGroupItem(this HtmlHelper html, string url, bool isAlternateRow = false)
        {
            return new GNClickableListGroupItem(html.ViewContext, url, isAlternateRow);
        }

        public static GNModuleIndexHeading BeginModuleIndexHeading(this HtmlHelper html, string title, string iconPath, int titleColsWidth = 4)
        {
            return new GNModuleIndexHeading(html.ViewContext, title, iconPath, titleColsWidth);
        }

        public static GNPanel BeginGNPanel(this HtmlHelper html, bool isFlushLeft = true)
        {
            return new GNPanel(html.ViewContext, isFlushLeft);
        }

        public static GNPanelHeading BeginGNPanelHeading(this HtmlHelper html, string panelTitle, string glyphicon = null, string id = null)
        {
            return new GNPanelHeading(html.ViewContext, panelTitle, glyphicon, id);
        }

        public static GNPanelBody BeginGNPanelBody(this HtmlHelper html, bool isListGroup = false, string id = null)
        {
            return new GNPanelBody(html.ViewContext, isListGroup, id);
        }

        public static GNPanelFooter BeginGNPanelFooter(this HtmlHelper html)
        {
            return new GNPanelFooter(html.ViewContext);
        }
    }
}