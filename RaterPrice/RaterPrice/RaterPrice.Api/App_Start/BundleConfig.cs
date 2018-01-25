using System.Web.Optimization;

namespace RaterPrice.Api
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/transition.js",
                "~/Scripts/collapse.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
                 ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            var goodsBundle = new Bundle("~/bundles/goods");
            goodsBundle
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-route.js")
                .Include("~/Scripts/angular-ui-router.js")
                .Include("~/Areas/SiteApi/app/rpApp.js")
                .Include("~/Areas/SiteApi/app/controllers/headerController.js")
                .Include("~/Areas/SiteApi/app/controllers/goodsController.js")            
                .Include("~/Areas/SiteApi/app/models/filterModel.js")
                .Include("~/Areas/SiteApi/app/services/userService.js")
                 .Include("~/Areas/SiteApi/app/controllers/registerController.js")
                .Include("~/Areas/SiteApi/app/controllers/loginController.js")

            ;




            bundles.Add(goodsBundle);
        }
    }
}