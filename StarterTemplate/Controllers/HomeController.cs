using System.Web.Mvc;
using AttributeRouting;

namespace StarterTemplate.Controllers
{
    public class HomeController : ApplicationControllerBase
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("secure")]
        [Authorize]
        public ActionResult Secure()
        {
            return RedirectToAction("Index");
        }
    }
}
