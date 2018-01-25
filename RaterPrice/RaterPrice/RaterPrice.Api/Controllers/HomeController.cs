using System.Web.Mvc;

namespace RaterPrice.Api.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}