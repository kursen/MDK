using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Text;

namespace HaloUIHelpers.Helpers
{
    public static class MvcPanelExtension
    {
        public static MvcPanel BeginPanel(this HtmlHelper helper, string Title, string Subtitle, string Icon = "fa-files-o",
            MvcPanel.PanelType panelType = MvcPanel.PanelType.panelInfo,bool collapse=false)
        {

            return PanelHelper(helper, Title, Subtitle, null, Icon, panelType,collapse);
        }

        public static MvcPanel BeginPanel(this HtmlHelper helper, string Title, string Subtitle, MvcHtmlString[] buttons, string Icon = "fa-files-o",
            MvcPanel.PanelType panelType = MvcPanel.PanelType.panelInfo,bool collapse=false)
        {

            return PanelHelper(helper, Title, Subtitle, buttons, Icon, panelType,collapse);
        }

        public static MvcHtmlString PanelCollapseButton(this HtmlHelper helper, bool collapse = false)
        {
            string collapseIcon = "fa-caret-up";
            if (collapse)
            {
                collapseIcon = "fa-caret-down";
            }
            return MvcHtmlString.Create(@"<button type=""button"" class=""btn btn-default panel-collapse""><i class=""fa " + collapseIcon + @"""></i></button>");
        }

        internal static MvcPanel PanelHelper(this HtmlHelper helper, string Title, string Subtitle, MvcHtmlString[] buttons = null,
            string Icon = "fa-files-type-o",
            MvcPanel.PanelType panelType = MvcPanel.PanelType.panelInfo, bool collapse=false)
        {
            string sPanelType = string.Empty;
            switch (panelType)
            {
                case MvcPanel.PanelType.panelDefault:
                    sPanelType = "panel-default";
                    break;
                case MvcPanel.PanelType.panelDanger:
                    sPanelType = "panel-danger";
                    break;
                case MvcPanel.PanelType.panelInfo:
                    sPanelType = "panel-info";
                    break;
                case MvcPanel.PanelType.panelPrimary:
                    sPanelType = "panel-primary";
                    break;
                case MvcPanel.PanelType.panelSucces:
                    sPanelType = "panel-success";
                    break;
                case MvcPanel.PanelType.panelWarning:
                    sPanelType = "panel-warning";
                    break;
            }


            TagBuilder divPanel = new TagBuilder("div");
            divPanel.AddCssClass(sPanelType);
            divPanel.AddCssClass("panel");

            TagBuilder divPanelHeading = new TagBuilder("div");
            divPanelHeading.AddCssClass("panel-heading");
            StringBuilder sb = new StringBuilder();
            if (buttons != null)
            {

                sb.Append("\n<div class='pull-right'>");
                foreach (MvcHtmlString b in buttons)
                {
                    if (b != null)
                    {
                        sb.Append("\n<div class='btn-group'>");
                        sb.Append(b.ToString());
                        sb.Append("\n</div>");
                    }
                }
                sb.Append("\n</div>");

            }


            sb.Append("\n<h4 class='margin-none'>");
            sb.Append("\n<i class='fa " + Icon + " fa-fw'></i> ");
            sb.Append(Title);
            sb.Append("\n</h4>");
            sb.Append("\n<p class='margin-none text-xs text-muted'>" + Subtitle + "</p>");


            divPanelHeading.InnerHtml = sb.ToString();
            helper.ViewContext.Writer.Write(divPanel.ToString(TagRenderMode.StartTag));
            helper.ViewContext.Writer.Write(divPanelHeading.ToString(TagRenderMode.Normal));
            string collapseClass = string.Empty;
            if (collapse)
                collapseClass = "hidden";


            helper.ViewContext.Writer.Write("<div class='panel-body "+ collapseClass +"' >");
            MvcPanel div = new MvcPanel(helper.ViewContext);
            return div;
        }

        internal static void EndPanel(ViewContext viewContext)
        {
            viewContext.Writer.Write("\n  </div>");
            viewContext.Writer.Write("\n</div>");
            viewContext.OutputClientValidation();
            viewContext.FormContext = null;
        }
    }
}