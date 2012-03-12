using System.Web.Mvc;
using System.Web.Mvc.Html;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DisplayIf(this HtmlHelper helper, bool condition)
        {
            return condition ? null : MvcHtmlString.Create("style=\"display: none\"");
        }
    }
}