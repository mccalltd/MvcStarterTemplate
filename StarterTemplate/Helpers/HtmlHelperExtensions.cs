using System.Web.Mvc;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ClientValidationSummaryErrorsTemplate(this HtmlHelper helper, string message, string viewModelServerErrorsPropertyPath)
        {
            var validationSummaryErrors = new TagBuilder("div");
            validationSummaryErrors.AddCssClass("validation-summary-errors alert alert-error");
            validationSummaryErrors.Attributes.Add("data-bind", "visible: {0}().length > 0".FormatWith(viewModelServerErrorsPropertyPath));
            validationSummaryErrors.Attributes.Add("style", "display: none");

            var span = new TagBuilder("span");
            span.SetInnerText(message);

            validationSummaryErrors.InnerHtml += span;

            var ul = new TagBuilder("ul");
            ul.Attributes.Add("data-bind", "foreach: {0}".FormatWith(viewModelServerErrorsPropertyPath));

            var li = new TagBuilder("li");
            li.Attributes.Add("data-bind", "text: $data");

            ul.InnerHtml += li;
            validationSummaryErrors.InnerHtml += ul;

            return MvcHtmlString.Create(validationSummaryErrors.ToString());
        }

        public static MvcHtmlString DisplayIf(this HtmlHelper helper, bool condition)
        {
            return condition ? null : MvcHtmlString.Create("style=\"display: none\"");
        }
    }
}