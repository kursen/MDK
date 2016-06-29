using System.Web.Mvc.Html;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Razor;
using System.Linq.Expressions;

using System.Text;
namespace HaloUIHelpers.Helpers
{
    public static class Helpers
    {
        #region page helper
        /// <summary>
        /// Write page title 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Title"></param>
        /// <param name="SubTitle"></param>
        /// <returns></returns>
        public static MvcHtmlString WritePageTitle(this HtmlHelper helper, string Title, string SubTitle)
        {
            
            
            var divOuter = new TagBuilder("div");
            divOuter.AddCssClass("panel panel-primary");
            var divPanelBody = new TagBuilder("div");
            divPanelBody.AddCssClass("panel-body");
            var h5 = new TagBuilder("h4");
            h5.AddCssClass("text-uppercase margin-none");
            var itag = new TagBuilder("i");
            itag.AddCssClass("fa fa-list-alt");
            itag.SetInnerText(" " + Title);
            var pTag = new TagBuilder("p");
            pTag.AddCssClass("text-muted text-xs margin-none");
            pTag.SetInnerText(SubTitle);
            h5.InnerHtml = itag.ToString() + pTag.ToString();
            divPanelBody.InnerHtml = h5.ToString();
            divOuter.InnerHtml = divPanelBody.ToString();

            return MvcHtmlString.Create(divOuter.ToString(TagRenderMode.Normal));

        }
        /// <summary>
        /// Render page header
        /// </summary>
        /// <param name="Title">Page title</param>
        /// <param name="SubTitle">sub title</param>
        /// <param name="ActionLink">link to anoter page (eg: back, create, edit, etc)</param>
        /// <example>
        /// when rendered, this html tags will be produced:
        /// <code>
        /// <div class="panel panel-primary">
        ///     <div class="panel-body">
        ///         <div class="pull-right"><a class="btn btn-default" href="/PO">Back</a></div>
        ///         <h5 class="text-uppercase margin-none"><i class="fa fa-list-alt"> Purchase Order</i>
        ///         <p class="text-muted text-xs margin-none">Register New Purchase Order</p></h5>
        ///     </div>
        /// </div>
        /// </code>
        /// </example>
        /// <returns></returns>
        public static MvcHtmlString WritePageTitleWithButtons(this HtmlHelper helper, string Title, string SubTitle, MvcHtmlString ActionLink)
        {

            var divOuter = new TagBuilder("div");
            divOuter.AddCssClass("panel panel-primary");
            var divPanelBody = new TagBuilder("div");
            divPanelBody.AddCssClass("panel-body");
            var h5 = new TagBuilder("h5");
            h5.AddCssClass("text-uppercase margin-none");
            var itag = new TagBuilder("i");
            itag.AddCssClass("fa fa-list-alt");
            itag.SetInnerText(" " + Title);
            var pTag = new TagBuilder("p");
            pTag.AddCssClass("text-muted text-xs margin-none");
            pTag.SetInnerText(SubTitle);
            h5.InnerHtml = itag.ToString() + pTag.ToString();
            var divPullRight = new TagBuilder("div");
            divPullRight.AddCssClass("pull-right");
            divPullRight.InnerHtml = ActionLink.ToString();
            divPanelBody.InnerHtml = divPullRight.ToString() + h5.ToString();
            divOuter.InnerHtml = divPanelBody.ToString();

            return MvcHtmlString.Create(divOuter.ToString(TagRenderMode.Normal));

        }
        #endregion
  




        #region Form Item
        /// <summary>
        /// Render layer input for a form.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="InputControl"></param>
        /// <param name="caption"></param>
        /// <param name="smLabelWidth"></param>
        /// <param name="smControlWidth"></param>
        /// <returns></returns>
        public static MvcHtmlString WriteFormInput(this HtmlHelper helper, MvcHtmlString InputControl,
            string caption, int smLabelWidth = 4, int smControlWidth = 4, int lgLabelWidth = 3, int lgControlWidth = 4)
        {
            var divFormGroup = new TagBuilder("div");
            divFormGroup.AddCssClass("form-group");
            var label = new TagBuilder("label");
            label.AddCssClass("control-label");
            label.AddCssClass("col-sm-" + smLabelWidth.ToString());
            label.AddCssClass("col-lg-" + lgLabelWidth.ToString());
            
            label.SetInnerText(caption);
            var divInput = new TagBuilder("div");
            divInput.AddCssClass("col-sm-" + smControlWidth.ToString());
            divInput.AddCssClass("col-lg-" + lgControlWidth.ToString());
            divInput.InnerHtml = InputControl.ToString() + "\n";
            divFormGroup.InnerHtml = label.ToString() + "\n" + divInput.ToString();
            return new MvcHtmlString("\n" + divFormGroup.ToString(TagRenderMode.Normal));
        }

       

        #endregion

        #region initscripts
        /// <summary>
        /// Write jQuery on-load function . The input to initilize will written automatically;
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString WriteInitScripts(this HtmlHelper helper)
        {
            MvcHtmlString tagIncludeScript = MvcHtmlString.Create("\n");
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            if (helper.ViewData["HaloUi.dateInput"] != null)
            {
                var tagBootstrapScript = new TagBuilder("script");
                tagBootstrapScript.Attributes["src"] = urlHelper.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker.js");
                tagBootstrapScript.Attributes["type"] = "text/javascript";
                var tagLocalescript = new TagBuilder("script");
                tagLocalescript.Attributes["src"] = urlHelper.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker.id.min.js");
                tagLocalescript.Attributes["type"] = "text/javascript";

                string dtpickercss = "<link href='" + urlHelper.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker3.css") + "' rel='stylesheet'>";
                tagIncludeScript = MvcHtmlString.Create("\n" + dtpickercss + "\n" +
                    tagBootstrapScript.ToString(TagRenderMode.Normal) + "\n" + tagLocalescript.ToString(TagRenderMode.Normal) +"\n");
            }


            if (helper.ViewData["HaloUi.editableInput"] != null)
            {
                var tagEditableScript = new TagBuilder("script");
                tagEditableScript.Attributes["src"] = urlHelper.Content("~/plugins/bootstrap-editable/bootstrap-editable.js");
                tagEditableScript.Attributes["type"] = "text/javascript";
                string editablecss = "<link href='" + urlHelper.Content("~/plugins/bootstrap-editable/bootstrap-editable.css") + "' rel='stylesheet'>";
                tagIncludeScript = Concat(tagIncludeScript, MvcHtmlString.Create("\n" + editablecss + "\n" +
                    tagEditableScript.ToString(TagRenderMode.Normal) + "\n"));

            }


            var tagscript = new TagBuilder("script");
            tagscript.Attributes["type"] = "text/javascript";
            string text = "$(function(){\n" + helper.ViewData["HaloUi.jsInit"] + "\n});\n <!--init script end-->\n";
            tagscript.InnerHtml = (text);
            return Concat(tagIncludeScript, MvcHtmlString.Create(tagscript.ToString(TagRenderMode.Normal)));
        }
        #endregion

        public static MvcHtmlString Concat(params MvcHtmlString[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MvcHtmlString v in value) if (v != null) sb.Append(v.ToString());
            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString SetActiveClass(this HtmlHelper helper, string action, string controller)
        {
            string rvalue = string.Empty;
            string currentController = helper.ViewContext.RouteData.Values["controller"].ToString();
            string currentAction = helper.ViewContext.RouteData.Values["action"].ToString();
            if(currentAction.Equals(action,StringComparison.CurrentCultureIgnoreCase) && currentController.Equals(controller,StringComparison.CurrentCultureIgnoreCase))
            {
                rvalue = "class='active'";
            }
            return new MvcHtmlString( rvalue);
        }
    }
}