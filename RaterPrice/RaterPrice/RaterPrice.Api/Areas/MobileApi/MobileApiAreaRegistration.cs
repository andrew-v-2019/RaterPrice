using System.Web.Http;
using System.Web.Mvc;

namespace RaterPrice.Api.Areas.MobileApi
{
    public class MobileApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "api"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "searchGood",
                "api/goods/search",
                new {action = "search", AreaName = "api", controller = "GoodsMobileApi"});

            context.Routes.MapHttpRoute(
                "BestGoods",
                "api/goods/best",
                new {action = "search", AreaName = "api", controller = "GoodsMobileApi"});

            context.Routes.MapHttpRoute(
                "GetGoodById",
                "api/goods/{id}",
                new
                {
                    action = "GetGoodById",
                    AreaName = "api",
                    controller = "GoodsMobileApi",
                    id = UrlParameter.Optional
                });


            context.Routes.MapHttpRoute(
                "GetPrices",
                "api/goods/{id}/prices",
                new
                {
                    action = "GetPricesInTheCity",
                    AreaName = "api",
                    controller = "GoodsMobileApi",
                    id = UrlParameter.Optional
                });

            context.Routes.MapHttpRoute(
                "GetCities",
                "api/cities",
                new
                {
                    action = "GetCities",
                    AreaName = "api",
                    controller = "CitiesApi"
                });

            context.Routes.MapHttpRoute(
              "AddPrice",
              "api/prices/{goodid}",
              new
              {
                  action = "AddGoodPrice",
                  AreaName = "api",
                  controller = "GoodsMobileApi",
                  goodid = UrlParameter.Optional
              });

            context.Routes.MapHttpRoute(
            "GetShops",
            "api/shops/search",
            new
            {
                action = "GetShops",
                AreaName = "api",
                controller = "ShopsApi"
            });

            context.Routes.MapHttpRoute(
           "Registration",
           "api/registration",
           new
           {
               action = "Registration",
               AreaName = "api",
               controller = "RegistrationApi"
           });

         context.Routes.MapHttpRoute(
        "ConfirmRegistration",
        "api/ConfirmRegistration",
        new
        {
            action = "ConfirmRegistration",
            AreaName = "api",
            controller = "RegistrationApi"
        });


        }
    }
}