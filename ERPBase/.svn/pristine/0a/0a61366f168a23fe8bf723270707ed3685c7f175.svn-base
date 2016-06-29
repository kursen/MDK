using System;
using System.Web.Mvc;

using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;

namespace HaloUIHelpers.Helpers
{
    public static class WriteJsSource
    {
        public static MvcHtmlString WriteDataTableSources(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();


            string path = UrlHelper.GenerateContentUrl("~/plugins/datatables/jquery.dataTables.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            path = UrlHelper.GenerateContentUrl("~/plugins/datatables/dataTables.bootstrap.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='"+path+"'></script>");
            path = UrlHelper.GenerateContentUrl("~/plugins/datatables/datatablehelper.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            
            
            return new MvcHtmlString(sb.ToString());
        }
        public static MvcHtmlString WriteDataTableTableToolsSources(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string path = UrlHelper.GenerateContentUrl("~/plugins/datatables/TableTools.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            path = UrlHelper.GenerateContentUrl("~/plugins/datatables/ZeroClipboard.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            return new MvcHtmlString(sb.ToString());
        }
        public static MvcHtmlString WriteTinyMCESources(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();


            string path = UrlHelper.GenerateContentUrl("~/plugins/tinymce/tinymce.min.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            path = UrlHelper.GenerateContentUrl("~/plugins/tinymce/jquery.tinymce.min.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString WriteJustifiedGallerySources(this HtmlHelper helper)
        {
           
            StringBuilder sb = new StringBuilder();
            string path = UrlHelper.GenerateContentUrl("~/plugins/justified-gallery/jquery.justifiedgallery.min.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// Load Full calendar javascript source
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString WriteFullCalendarSources(this HtmlHelper helper)
        {

            StringBuilder sb = new StringBuilder();
            string path = UrlHelper.GenerateContentUrl("~/plugins/fullcalendar/fullcalendar.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            
            path = UrlHelper.GenerateContentUrl("~/plugins/fullcalendar/fullcalendar.css", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<link href='"+path+"' rel=stylesheet'>");

            return new MvcHtmlString(sb.ToString());
        }
        public static MvcHtmlString WriteMomentJsSources(this HtmlHelper helper)
        {

            StringBuilder sb = new StringBuilder();
            string path = UrlHelper.GenerateContentUrl("~/plugins/moment/moment.min.js", helper.ViewContext.RequestContext.HttpContext);
            sb.AppendLine("<script type='text-javascript' src='" + path + "'></script>");
            return new MvcHtmlString(sb.ToString());
        }
    }
}
