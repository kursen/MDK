using System.Web.Mvc.Html;
using System;
using System.Web.Mvc;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Text;
namespace HaloUIHelpers.Helpers
{
    public static class DateInputHelper
    {
       
        /// <summary>
        /// Render bootstrap-datepicker for model
        /// </summary>
        /// <typeparam name="TModel">TModel</typeparam>
        /// <typeparam name="TProperty">TProperty</typeparam>
        /// <param name="helper"></param>
        /// <param name="expression">Expression</param>
        /// <param name="option">see: <see cref="http://github.com/eternicode/bootstrap-datepicker"/> </param>
        /// <returns></returns>
        /// <example>
        /// This is how to use the extension:
        /// <code>
        ///  var dtPickerReceivedDate = new { format = "dd-mm-yyyy", autoclose = true, clearBtn = true };
        ///   @Html.WriteFormDateInputFor(model => model.ReceivedDate, "PO Date", dtPickerReceivedDate)
        /// </code>
        /// </example>
        public static MvcHtmlString DateInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProperty>> expression, Object option)
        {


            var name = ExpressionHelper.GetExpressionText(expression);
            var value = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            System.DateTime? datevalue = null;// default(System.DateTime?);
            if (value.Model != null && value.Model is System.DateTime)
            {
                datevalue = Convert.ToDateTime(value.Model);
            }

            if (datevalue.Equals(System.DateTime.MinValue))
            {
                datevalue = null;
            }

            return helper.DateInput(name, datevalue, option);
        }

        /// <summary>
        /// Render Bootstrap-datepicker for input date data
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name">input control name</param>
        /// <param name="datevalue">Date value</param>
        /// <param name="option">see: <see cref="http://github.com/eternicode/bootstrap-datepicker"/> </param>
        /// <returns></returns>
        public static MvcHtmlString DateInput(this HtmlHelper helper, string name, DateTime? datevalue, Object option)
        {
            var divgroup = new TagBuilder("div");
            divgroup.AddCssClass("input-group date");
            divgroup.Attributes["id"] = "dtpk_" + name;
            divgroup.Attributes["data-date-container"] = "#content";
            var spangroup = new TagBuilder("span");
            spangroup.AddCssClass("input-group-addon");
            spangroup.InnerHtml = "<i class='fa fa-calendar'></i>";

            string jsonOptions = string.Empty;
            string dateformat = "dd-MM-yyyy";//default format; when this changed, please change also default format in bootstrap-datepicker.js file;
            if (option != null)
            {
                jsonOptions = helper.Raw(JsonConvert.SerializeObject(option)).ToString();

                var prop = option.GetType().GetProperty("format");
                if (prop != null)
                {
                    dateformat = prop.GetValue(option, null).ToString();
                    dateformat = dateformat.Replace("m", "M");
                }
                prop = option.GetType().GetProperty("container");
                if (prop != null)
                {
                    var container = prop.GetValue(option, null).ToString();
                    divgroup.Attributes["data-date-container"] =  container;
                }
                    
            }
            string renderedvalue = datevalue.HasValue ? datevalue.Value.ToString(dateformat) : "";
            helper.ViewData["HaloUi.jsInit"] += "\n\t$('#dtpk_" + name + "').datepicker(" + jsonOptions + ");";
             helper.ViewData["HaloUi.dateInput"] = true;
            divgroup.InnerHtml = helper.TextBox(name, renderedvalue, new { @class = "form-control" }).ToString() + spangroup.ToString();
            return new MvcHtmlString("\n" + divgroup.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Render date input as part of form item
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="caption">input Label</param>
        /// <param name="option"></param>
        /// <param name="labelWidth">width of label</param>
        /// <param name="controlwidth">width of input control</param>
        /// <returns></returns>
        public static MvcHtmlString WriteFormDateInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProperty>> expression, string caption, Object option,
            int smLabelWidth = 4, int smControlWidth = 4, int lgLabelWidth = 3, int lgControlWidth = 4)
        {
            var dateinput = helper.DateInputFor(expression, option);
            return helper.WriteFormInput(dateinput, caption, smLabelWidth: smLabelWidth,
                smControlWidth: smControlWidth, lgLabelWidth: lgLabelWidth, lgControlWidth: lgControlWidth);
        }
       
    }
}