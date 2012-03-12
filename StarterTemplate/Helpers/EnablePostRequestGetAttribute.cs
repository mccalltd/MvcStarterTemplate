using System.Web.Mvc;

namespace StarterTemplate.Helpers
{
    public class EnablePostRequestGetAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(EnablePostRequestGetAttribute).FullName;

        public bool ForceExport { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Export when ModelState is not valid or when we want to force it
            if (!filterContext.Controller.ViewData.ModelState.IsValid || ForceExport)
            {
                ExportModelState(filterContext);
            }
            else if (filterContext.HttpContext.Request.HttpMethod.ToUpperInvariant() == "GET")
            {
                ImportModelState(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }

        private void ExportModelState(ActionExecutedContext filterContext)
        {
            //Export if we are redirecting
            if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
        }


        private void ImportModelState(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;
            if (modelState != null)
            {
                // Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                else //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}