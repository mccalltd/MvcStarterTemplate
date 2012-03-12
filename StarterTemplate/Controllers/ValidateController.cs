using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AttributeRouting;
using StarterTemplate.Core.Validation;

namespace StarterTemplate.Controllers
{
    [RoutePrefix("validate")]
    public class ValidateController : ApplicationControllerBase
    {
        [POST("email-is-available")]
        public ActionResult EmailIsAvailable(FormCollection form)
        {
            return JsonValidate<EmailIsAvailableAttribute>(form);
        }

        [POST("email-is-registered")]
        public ActionResult EmailIsRegistered(FormCollection form)
        {
            return JsonValidate<EmailIsRegisteredAttribute>(form);
        }

        private JsonResult JsonValidate<TValidationAttribute>(FormCollection form)
            where TValidationAttribute : ValidationAttribute, new()
        {
            if (form.Count == 0)
                return Json(true);

            var validator = new TValidationAttribute();

            return Json(validator.IsValid(form[0]));
        }
    }
}
