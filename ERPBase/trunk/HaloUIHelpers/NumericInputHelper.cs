using System;
using System.Web.Mvc;

using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;
namespace HaloUIHelpers.Helpers
{
    public static class NumericInputHelper
    {
        public static MvcHtmlString IntegerInput(this HtmlHelper helper, string name, int? value, string thousandSeparator = ".")
        {
            string decimalseparator = ",";
            if (thousandSeparator == ",")
                decimalseparator = ".";
            var inputControl = helper.TextBox(name, value, new { @class = "form-control text-right" });
            string jsonOptions = "true, 0, \"" + decimalseparator + "\", " + "\"" + thousandSeparator + "\"";
            helper.ViewData["HaloUi.jsInit"] += "\n\t$('#" + name + "').number(" + jsonOptions + ");";
            helper.ViewData["HaloUi.numericInput"] = true;
            
            
            return inputControl;
        }

        public static MvcHtmlString IntegerInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            string thousandSeparator = ".")
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var value = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            int? intvalue = default(int?);
            if (value.Model != null && value.Model is int)
            {
                intvalue = Convert.ToInt32(value.Model);
            }
            return helper.IntegerInput(name, intvalue, thousandSeparator);
        }

        public static MvcHtmlString WriteFormIntegerInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            string caption, string thousandSeparator = ".", int smLabelWidth = 4, int smControlWidth = 8, int lgLabelWidth = 3, int lgControlWidth = 4)
        {
            var integerInput = helper.IntegerInputFor(expression, thousandSeparator);
            return helper.WriteFormInput(integerInput, caption, smLabelWidth: smLabelWidth,
                smControlWidth: smControlWidth, lgLabelWidth: lgLabelWidth, lgControlWidth: lgControlWidth);
        }


        public static MvcHtmlString DecimalInput(this HtmlHelper helper, string name, decimal? value, string thousandSeparator = ".", string decimalseparator=",", int digit=2)
        {
            
            var inputControl = helper.TextBox(name, value, new { @class = "form-control text-right" });
            //string jsonOptions = "true, 2, \"" + decimalseparator + "\", " + "\"" + thousandSeparator + "\"";
            string jsonOptions = string.Format("true,{0},\"{1}\",\"{2}\"", digit, decimalseparator, thousandSeparator);
            helper.ViewData["HaloUi.jsInit"] += "\n\t$('#" + name + "').number(" + jsonOptions + ");";
            helper.ViewData["HaloUi.numericInput"] = true;


            return inputControl;
        }

        public static MvcHtmlString DecimalInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            string thousandSeparator = ".", string decimalseparator=",")
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var value = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            decimal? decValue = default(decimal?);
            if (value.Model != null && value.Model is decimal)
            {
                decValue = Convert.ToDecimal(value.Model);
            }
            return helper.DecimalInput(name, decValue, thousandSeparator,decimalseparator);
        }

        public static MvcHtmlString WriteFormDecimalInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
          string caption, string thousandSeparator = ".",string decimalseparator=",", int smLabelWidth = 4, int smControlWidth = 8, int lgLabelWidth = 3, int lgControlWidth = 4)
        {
            var decimalinput = helper.DecimalInputFor(expression, thousandSeparator,decimalseparator);
            return helper.WriteFormInput(decimalinput, caption, smLabelWidth: smLabelWidth,
                smControlWidth: smControlWidth, lgLabelWidth: lgLabelWidth, lgControlWidth: lgControlWidth);
        }

        public static MvcHtmlString YearInput(this HtmlHelper helper, string name, int? value)
        {
            string decimalseparator = ".";
            string thousandSeparator = "";
            var inputControl = helper.TextBox(name, value, new { @class = "form-control text-right", maxlength = 4 });
            string jsonOptions = "true, 0, \"" + decimalseparator + "\", " + "\"" + thousandSeparator + "\"";
            helper.ViewData["HaloUi.jsInit"] += "\n\t$('#" + name + "').number(" + jsonOptions + ");";
            helper.ViewData["HaloUi.numericInput"] = true;
            return inputControl;
        }

        public static MvcHtmlString YearInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
           string thousandSeparator = ",")
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var value = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            int? intvalue = default(int?);
            if (value.Model != null && value.Model is int)
            {
                intvalue = Convert.ToInt32(value.Model);
            }
            return helper.YearInput(name, intvalue);
        }

        public static MvcHtmlString WriteFormYearInputFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
           string caption, int smLabelWidth = 4, int smControlWidth = 8, int lgLabelWidth = 4, int lgControlWidth = 8)
        {
            var YearInput = helper.YearInputFor(expression);
            return helper.WriteFormInput(YearInput, caption, smLabelWidth: smLabelWidth,
                smControlWidth: smControlWidth, lgLabelWidth: lgLabelWidth, lgControlWidth: lgControlWidth);
        }



    }
}
