using System.Web.Mvc.Html;
using System;
using System.Web.Mvc;
using System.Linq.Expressions;

using System.Text;
namespace HaloUIHelpers.Helpers
{
    public static class EditableInput
    {




        public static void SetEditableDefaultSettings(this HtmlHelper helper, bool DisableOnload,
            string datapk = "0", string dataurl = "/default", string formPlacement = "right")
        {

            string optPlacement = string.Empty;
            string optDataPk = string.Empty;
            string optDataUrl = string.Empty;
            string optEnabledOnload = "\n$.fn.editable.defaults.disabled = '" + DisableOnload.ToString().ToLower() + "';";
            if (datapk.Equals("0") == false)
            {
                optDataPk = "\n$.fn.editable.defaults.pk = '" + datapk + "';";
            }
            if (dataurl.Equals("/default") == false)
            {
                optDataUrl = "\n$.fn.editable.defaults.url = '" + dataurl + "';";

            }
            if (formPlacement.Equals("top") == false)
            {
                optPlacement = "\n$.fn.editable.defaults.placement = '" + formPlacement + "';";
            }
            helper.ViewData["HaloUi.jsInit"] = optEnabledOnload + optDataPk + optDataUrl + optPlacement + helper.ViewData["HaloUi.jsInit"];
        }

        public static MvcHtmlString EditableInputTextBox(this HtmlHelper helper, string name, string value,
            string datatype, string datatitle, string sourcelist = "0", string datapk = "0", string dataurl = "/default")
        {
            var tagA = new TagBuilder("a");
            tagA.SetInnerText(value);
            tagA.Attributes["id"] = name;
            tagA.Attributes["data-type"] = datatype;
            tagA.Attributes["data-title"] = datatitle;

            if (!datapk.Equals("0"))
            {
                tagA.Attributes["data-pk"] = datapk;
            }

            if (!dataurl.Equals("/default"))
            {
                tagA.Attributes["data-url"] = dataurl;
            }
            if (!sourcelist.Equals("0"))
            {
                tagA.Attributes["data-source"] = sourcelist.ToString();
            }
            helper.ViewData["HaloUi.editableInput"] = true;
            helper.ViewData["HaloUi.jsInit"] += "\n\t$('#" + name + "').editable({success:_savepartialresponse});";
            return MvcHtmlString.Create(tagA.ToString(TagRenderMode.Normal));
        }



    }
}