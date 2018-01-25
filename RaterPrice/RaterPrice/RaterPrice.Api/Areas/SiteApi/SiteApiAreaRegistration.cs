using System.Web.Http;
using System.Web.Mvc;

namespace RaterPrice.Api.Areas.SiteApi
{
    public class SiteApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "SiteApi"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "SiteApi_default",
                "SiteApi/{controller}/{action}"
                //,
                //new { action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}