using System.Web.Mvc;

namespace SKA.Areas.PettyCashes
{
    public class PettyCashesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PettyCashes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PettyCashes_default",
                "PettyCashes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
