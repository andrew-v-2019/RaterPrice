using System.Linq;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace RaterPrice.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы Web API
            // Настройка Web API для использования только проверки подлинности посредством маркера-носителя.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Маршруты Web API
            config.MapHttpAttributeRoutes();


            //    config.Routes.MapHttpRoute(
            //    name: "GetByName",
            //    routeTemplate: "api/{controller}/{action}/{p}",
            //    defaults: new { name = RouteParameter.Optional }
            //);

            //    config.Routes.MapHttpRoute(
            //       name: "GetByBarcode",
            //       routeTemplate: "api/{controller}/{action}/{p}",
            //       defaults: new { barcode = RouteParameter.Optional }
            //   );


            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{p}",
            //    defaults: new { p = RouteParameter.Optional }
            //);


            var appXmlType =
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}