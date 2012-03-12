using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using StarterTemplate.Helpers;

namespace StarterTemplate.Controllers
{
    [EnablePostRequestGet]
    public abstract class ApplicationControllerBase : Controller
    {
        protected bool Try(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                if (!HandleException(ex))
                    throw;

                return false;
            }
        }

        protected bool HandleException(Exception ex)
        {
            if (ex is ApplicationException)
            {
                ModelState.AddModelError("", ex.Message);
                return true;
            }

            if (ex is DbEntityValidationException)
            {
                foreach (var error in ((DbEntityValidationException)ex).EntityValidationErrors)
                    foreach (var validationError in error.ValidationErrors)
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);

                return true;
            }

            return false;
        }

        protected JsonResult JsonHtml(string viewName = null, object model = null)
        {
            return Json(new
            {
                html = RenderPartialViewToString(viewName, model)
            }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonRedirect(string redirectUrl)
        {
            return Json(new
            {
                redirectUrl
            });
        }

        protected JsonResult JsonSuccess(object model = null)
        {
            return Json(new
            {
                success = true,
                model
            });
        }

        protected JsonResult JsonFailure()
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray()
            });
        }

        protected string RenderPartialViewToString(string viewName = null, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = "_" + ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer);
                viewResult.View.Render(viewContext, writer);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
