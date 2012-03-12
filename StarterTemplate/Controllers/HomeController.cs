using System;
using System.Web.Mvc;
using AttributeRouting;
using StarterTemplate.Models;

namespace StarterTemplate.Controllers
{
    public class HomeController : ApplicationControllerBase
    {
        private readonly MailController _mailer;

        public HomeController(MailController mailer)
        {
            _mailer = mailer;
        }

        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("about")]
        public ActionResult About()
        {
            return View();
        }

        [GET("contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [POST("contact")]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(HomeContactModel model)
        {
            if (ModelState.IsValid
                && Try(() => _mailer.SendContactMessage(model).DeliverAsync()))
            {
                return RedirectToAction("ContactSuccess");
            }

            return RedirectToAction("Contact");
        }

        [GET("contact-success")]
        public ActionResult ContactSuccess()
        {
            return View();
        }

        /// <summary>
        /// Just a test for when trying to access secured pages.
        /// </summary>
        [GET("secure")]
        [Authorize]
        public ActionResult Secure()
        {
            return RedirectToAction("Index");
        }
    }
}
