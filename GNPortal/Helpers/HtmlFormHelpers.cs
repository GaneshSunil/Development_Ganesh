using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GenomeNext.Portal.Helpers
{
    public static class HtmlFormHelpers
    {
        public static MvcHtmlString FormFieldFor<TModel, TValue>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> fieldExpression, bool editable = true,
            Type modelPropType = null, int labelCols = 4, int fieldCols = 8, bool createHiddenField = true, 
            string customLabel = null)
        {
            
            string htmlStr = "";

            htmlStr += "<div class=\"form-group\">";

            if (customLabel == null)
            {
                htmlStr += System.Web.Mvc.Html.LabelExtensions.LabelFor(
                    helper, fieldExpression, new { @class = "control-label col-md-" + labelCols }).ToHtmlString();
            }
            else
            {
                htmlStr += "<label class=\"control-label col-md-" + labelCols + "\">" + customLabel + "</label>";
            }

            htmlStr += "<div class=\"col-md-" + fieldCols + "\">";

            if(editable)
            {
                if (modelPropType == null)
                {
                    htmlStr += System.Web.Mvc.Html.EditorExtensions.EditorFor(helper, fieldExpression, new { htmlAttributes = new { @class = "form-control" } }).ToHtmlString();
                }
                else if (modelPropType == typeof(DateTime))
                {
                    htmlStr += System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, fieldExpression, new { @class = "form-control datepicker" }).ToHtmlString();
                }
            }
            else
            {
                htmlStr += System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, fieldExpression).ToHtmlString();
                if(createHiddenField)
                {
                    htmlStr += System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, fieldExpression).ToHtmlString();
                }
            }

            htmlStr += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, fieldExpression, "", new { @class = "text-danger" }).ToHtmlString();

            htmlStr += "</div>";

            htmlStr += "</div>";
            
            return MvcHtmlString.Create(htmlStr);
        }

        public static MvcHtmlString DisplayFieldFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string customLabel = null)
        {
            string htmlStr = "";

            htmlStr += "<dt>";

            if(customLabel != null)
            {
                htmlStr += customLabel;
            }
            else
            {
                htmlStr += System.Web.Mvc.Html.DisplayNameExtensions.DisplayNameFor(helper, expression).ToHtmlString();
            }

            htmlStr += "</dt>";

            htmlStr += "<dd>";

            htmlStr += System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToHtmlString();

            htmlStr += "</dd>";

            return MvcHtmlString.Create(htmlStr);
        }
    }
}