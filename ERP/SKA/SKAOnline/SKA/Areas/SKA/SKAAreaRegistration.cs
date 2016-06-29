using System.Web.Mvc;

namespace SKA.Areas.SKA
{
    public class SKAAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SKA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SKA_default",
                "SKA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
