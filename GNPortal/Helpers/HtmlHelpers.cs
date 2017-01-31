using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GenomeNext.Portal.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString BootstrapRowSpacer(this HtmlHelper helper, int numCols = 12)
        {
            return MvcHtmlString.Create("<div class=\"row\"><div class=\"col-md-" + numCols + "\">&nbsp;</div></div>");
        }

        public static MvcHtmlString BootstrapFormSaveButton(this HtmlHelper helper, string text = "Save", string glyphicon = "floppy-disk", string styleScheme = "success", bool glyphiconRight = false)
        {
            /*
            string functionName = "submitForm_"+new Random().Next();

            string formSubmitLogic = "$('form[action=\\'" + HttpContext.Current.Request.Url.AbsolutePath + "\\']').submit();";//return false;";

            string scriptBlock = "<script>function " + functionName + "(){" + formSubmitLogic + "}</script>";

            string url = functionName + "();";
            */
            string scriptBlock = "";
            string url = "#";

            string htmlButton = BootstrapActionButton(
                helper, text, null, null, null,
                styleScheme, glyphicon, null, glyphiconRight, "center", true, url,false).ToHtmlString();

            return MvcHtmlString.Create(scriptBlock + htmlButton);
        }


        public static MvcHtmlString BootstrapFormConfirmDeleteButton(this HtmlHelper helper, string text = "Save", string glyphicon = "floppy-disk", string styleScheme = "success", bool glyphiconRight = false)
        {
            string url = "$('form[action=\\'" + HttpContext.Current.Request.Url.PathAndQuery + "\\']').submit();";

            return BootstrapActionButton(
                helper, text, null, null, null,
                styleScheme, glyphicon, null, glyphiconRight, "center", true, url, false);
        }

        public static MvcHtmlString BootstrapActionIcon(
            this HtmlHelper helper, string text, string action, string controller = null, RouteValueDictionary routeValues = null,
            string styleScheme = "default", string glyphicon = null, string size = "sm", bool glyphiconRight = false, string textAlign = "center", bool displayText = false, string url = null)
        {
            return BootstrapActionButton(
                helper, text, action, controller, routeValues,
                styleScheme, glyphicon, size, glyphiconRight, textAlign, displayText, url);
        }

        public static MvcHtmlString BootstrapActionIconLink(
            this HtmlHelper helper, string text, string action, string controller = null, RouteValueDictionary routeValues = null,
            string styleScheme = "default", string glyphicon = null, string size = null, bool glyphiconRight = false, bool displayText = false, bool newWindow = false)
        {
            return BootstrapActionButton(
                helper, text, action, controller, routeValues,
                styleScheme, glyphicon, size, glyphiconRight, "left", displayText,null,false,true,newWindow);
        }

        public static MvcHtmlString BootstrapActionButton(
            this HtmlHelper helper, string text, string action, string controller = null, RouteValueDictionary routeValues = null,
            string styleScheme = "default", string glyphicon = null, string size = null, bool glyphiconRight = false, string textAlign = "center", bool displayText = true, string url = null, bool asLinkButton = true, bool asLink = false, bool newWindow = false)
        {
            if (glyphicon == null)
            {
                glyphicon = getIcon(text);
            }

            if (size != null)
            {
                size = "btn-" + size;
            }
            else
            {
                size = "";
            }

            if (glyphicon != null)
            {
                glyphicon = "<span class=\"glyphicon glyphicon-" + glyphicon + "\"></span>";
            }
            else
            {
                glyphicon = "";
            }

            string glyphiconRightHTML = "";
            if (glyphiconRight)
            {
                glyphiconRightHTML = "&nbsp;" + glyphicon;
                glyphicon = "";
            }
            else if(!string.IsNullOrEmpty(text) && displayText)
            {
                glyphicon = glyphicon + "&nbsp;";
            }


            //generate url
            if(action != null)
            {
                url = UrlHelper.GenerateUrl(null, action, controller, routeValues, helper.RouteCollection, helper.ViewContext.RequestContext, false);

                if (newWindow)
                {
                    url = "javascript:window.open('" + url + "')";
                }                
            }

            if(asLinkButton)
            {
                if (!string.IsNullOrEmpty(styleScheme) 
                    && (styleScheme == "default" || styleScheme == "primary" 
                    || styleScheme == "info" || styleScheme == "warning"
                    || styleScheme == "danger" || styleScheme == "success"))
                {
                    styleScheme = "btn-" + styleScheme;
                }
                    
                return MvcHtmlString.Create(String.Format("<a href=\"{0}\" class=\"btn {1} {2}\" style=\"text-align:{3}\" title=\"{4}\" alt=\"{5}\">{6}{7}{8}</a>",
                    url, styleScheme, size, textAlign, text, text, glyphicon, (displayText ? text : ""), (displayText ? glyphiconRightHTML : "")));
            }
            else
            {
                if (asLink)
                {
                    if (!string.IsNullOrEmpty(styleScheme)
                        && (styleScheme == "default" || styleScheme == "primary"
                        || styleScheme == "info" || styleScheme == "warning"
                        || styleScheme == "danger" || styleScheme == "success"))
                    {
                        styleScheme = "text-" + styleScheme;
                    }

                    return MvcHtmlString.Create(String.Format("<a href=\"{0}\" class=\"{1}\" style=\"text-align:{2}\" title=\"{3}\" alt=\"{4}\">{5}{6}{7}</a>",
                        url, styleScheme, textAlign, text, text, glyphicon, (displayText ? text : ""), (displayText ? glyphiconRightHTML : "")));
                }
                else
                {
                    if (!string.IsNullOrEmpty(styleScheme)
                        && (styleScheme == "default" || styleScheme == "primary"
                        || styleScheme == "info" || styleScheme == "warning"
                        || styleScheme == "danger" || styleScheme == "success"))
                    {
                        styleScheme = "btn-" + styleScheme;
                    }

                    return MvcHtmlString.Create(String.Format("<button type=\"submit\" onclick=\"{0}\" class=\"btn {1} {2}\" style=\"text-align:{3}\" title=\"{4}\" alt=\"{5}\">{6}{7}{8}</button>",
                        url, styleScheme, size, textAlign, text, text, glyphicon, (displayText ? text : ""), (displayText ? glyphiconRightHTML : "")));
                }
            }
        }

        public static string getIcon(string action)
        {
            string icon = "";
            switch (action)
            {
                case "Manage Contacts":
                case "Subscribers":
                    icon = "user";
                    break;
                case "Create":
                case "Create New":
                    icon = "plus";
                    break;
                case "View":
                case "Details":
                    icon = "th-large";
                    break;
                case "Edit":
                    icon = "pencil";
                    break;
                case "Delete":
                    icon = "trash";
                    break;
                case "Start":
                    icon = "play";
                    break;
                case "Notify":
                case "Re-send":
                case "Send":
                case "Invite":
                    icon = "envelope";
                    break;
                case "Download":
                    icon = "cloud-download";
                    break;
                case "Sample":
                    icon = "tint";
                    break;
                case "Logs":
                    icon = "list-alt";
                    break;
                default:
                    icon = "chevron-left";
                    break;
            }
            return icon;
        }
    }
}