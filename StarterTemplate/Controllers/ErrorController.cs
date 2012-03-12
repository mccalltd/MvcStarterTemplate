using System.Web.Mvc;
using AttributeRouting;

namespace StarterTemplate.Controllers
{
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        [GET("File-Not-Found")]
        public ActionResult FileNotFound()
        {
            return View();
        }
    }
}
