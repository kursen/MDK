using System.Web.Mvc;

namespace SKA.Areas.VoucherKas
{
    public class VoucherKasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "VoucherKas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "VoucherKas_default",
                "VoucherKas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
