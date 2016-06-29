using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Text;


namespace HaloUIHelpers.Helpers
{
    public static class JUIBoxExtension
    {
        public static JUIBox BeginJUIBox(this HtmlHelper helper, string Title, bool hasPadding=true, bool isMoveable = false, bool isExpandable = true, bool isCollapsible = true, bool isRemovable = false,
                string iconClass = "fa fa-table")
        {
            return JUIBoxHelper(helper, Title,hasPadding, isMoveable,isExpandable, isCollapsible, isRemovable, iconClass);
        }

        internal static JUIBox JUIBoxHelper(this HtmlHelper helper, string Title, bool hasPadding, bool isMoveable,bool isExpandable, bool isCollapsible, bool isRemovable, string iconClass)
        {
            TagBuilder divBox = new TagBuilder("div");
            divBox.AddCssClass("box");
            if (isMoveable!=true)
            {
                divBox.AddCssClass("no-drop");
            }
            TagBuilder divBoxHeader = new TagBuilder("div");
            divBoxHeader.AddCssClass("box-header");
            divBoxHeader.InnerHtml = 
                "<div class='box-name'>\n" +
                "  <i class='"+iconClass+"'></i>\n" +
                "  <span>"+Title+"</span>\n" +
                "</div>";

            TagBuilder divBoxIcons = new TagBuilder("div");
            divBoxIcons.AddCssClass("box-icons");
            StringBuilder sb = new StringBuilder();
            if (isCollapsible)
            {
                sb.AppendLine("<a class='collapse-link'><i class='fa fa-chevron-up'></i></a>");
            }
            if (isExpandable)
            {
                sb.AppendLine("<a class='expand-link'><i class='fa fa-expand'></i></a>");
            }
            if (isRemovable)
            {
                sb.AppendLine("<a class='close-link'><i class='fa fa-times'></i></a>");
            }
            
            divBoxIcons.InnerHtml = sb.ToString();
            sb.Clear();
            divBoxHeader.InnerHtml +=divBoxIcons.ToString(TagRenderMode.Normal) + "\n";
            divBoxHeader.InnerHtml += "<div class='no-move'></div>\n";

            
            helper.ViewContext.Writer.WriteLine(divBox.ToString(TagRenderMode.StartTag));
            helper.ViewContext.Writer.WriteLine(@"  "+divBoxHeader.ToString(TagRenderMode.Normal));
            helper.ViewContext.Writer.WriteLine("     <div class='box-content {0}'>", (hasPadding?"":"no-padding"));
            JUIBox obj = new JUIBox(helper.ViewContext);
            return obj;
        }

        internal static void EndJUIBox(ViewContext viewContext)
        {
            viewContext.Writer.Write("\n  </div><!--end box-content-->");
            viewContext.Writer.Write("\n</div><!--end box-->");
            viewContext.OutputClientValidation();
            viewContext.FormContext = null;
        }
    }
}
