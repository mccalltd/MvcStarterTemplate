using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using StarterTemplate.Core;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Helpers
{
    public static class BootstrapHtmlHelperExtensions
    {
        public static MvcHtmlString BootstrapClientValidationSummaryTemplate(this HtmlHelper helper, string message, string viewModelServerErrorsPropertyPath)
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

        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper, string message)
        {
            return helper.ValidationSummary(message, new { @class = "alert alert-error" });
        }

        public static BootstrapContent BeginBootstrapNavDropdown(this HtmlHelper helper, string linkText, string linkUrl = null, object htmlAttributes = null)
        {
            var writer = helper.ViewContext.Writer;

            var dropdown = new TagBuilder("li");
            dropdown.AddCssClass("dropdown");
            dropdown.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            writer.Write(dropdown.ToString(TagRenderMode.StartTag));

            var dropdownToggle = new TagBuilder("a");
            dropdownToggle.AddCssClass("dropdown-toggle");
            dropdownToggle.Attributes.Add("data-toggle", "dropdown");
            dropdownToggle.Attributes.Add("href", linkUrl ?? "#");

            writer.Write(dropdownToggle.ToString(TagRenderMode.StartTag));
            writer.Write(linkText + " ");

            var caret = new TagBuilder("b");
            caret.AddCssClass("caret");

            writer.Write(caret.ToString());
            writer.Write(dropdownToggle.ToString(TagRenderMode.EndTag));

            var dropdownMenu = new TagBuilder("ul");
            dropdownMenu.AddCssClass("dropdown-menu");

            writer.Write(dropdownMenu.ToString(TagRenderMode.StartTag));

            return new BootstrapContent(() =>
            {
                writer.Write(dropdownMenu.ToString(TagRenderMode.EndTag));
                writer.Write(dropdown.ToString(TagRenderMode.EndTag));
            });
        }

        public static MvcForm BeginAjaxLoadedBootstrapForm(this HtmlHelper helper, string action = null, string controller = null, object routeValues = null, object htmlAttributes = null)
        {
            helper.EnableClientValidation();
            helper.EnableUnobtrusiveJavaScript();

            // Ensure there is a unique id for the form when delivered to the calling page.
            // By default, MVC does a formN iterator, which causes problems for ajax loaded forms
            // because the numbering restarts with each request. -tim
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (!htmlAttributesDict.ContainsKey("id"))
                htmlAttributesDict.Add("id", DateTime.Now.Ticks);
            
            return BeginBootstrapForm(helper, action, controller, routeValues, htmlAttributesDict);
        }

        public static MvcForm BeginBootstrapForm(this HtmlHelper helper, string action = null, string controller = null, object routeValues = null, object htmlAttributes = null)
        {
            var htmlAttributesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return BeginBootstrapForm(helper, action, controller, routeValues, htmlAttributesDict);
        }

        private static MvcForm BeginBootstrapForm(this HtmlHelper helper, string action = null, string controller = null, object routeValues = null, IDictionary<string, object> htmlAttributes = null)
        {
            var routeData = helper.ViewContext.RouteData;
            var actionName = action ?? routeData.GetRequiredString("action");
            var controllerName = controller ?? routeData.GetRequiredString("controller");

            var routeValuesDict = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var htmlAttributesDict = htmlAttributes ?? new Dictionary<string, object>();

            return helper.BeginForm(actionName, controllerName, routeValuesDict, FormMethod.Post, htmlAttributesDict);
        }

        public static MvcHtmlString BootstrapControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, MvcHtmlString editor = null, bool usePlaceholder = true)
        {
            var controlGroup = new TagBuilder("div");
            controlGroup.AddCssClass("control-group");
            
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var labelText = metadata.GetDisplayName();

            if (!usePlaceholder)
            {
                var fullHtmlFieldName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

                var controlLabel = new TagBuilder("label");
                controlLabel.AddCssClass("control-label");
                controlLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(fullHtmlFieldName));
                controlLabel.SetInnerText(labelText);

                controlGroup.InnerHtml += controlLabel;
            }

            var controls = new TagBuilder("div");
            controls.AddCssClass("controls");
            controls.InnerHtml += editor ?? helper.EditorFor(expression);
            controls.InnerHtml += helper.ValidationMessageFor(expression, "", new { @class = "help-inline" });

            controlGroup.InnerHtml += controls;

            return MvcHtmlString.Create(controlGroup.ToString());
        }
        
        public static BootstrapContent BeginBootstrapControlGroup<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var writer = helper.ViewContext.Writer;

            var controlGroup = new TagBuilder("div");
            controlGroup.AddCssClass("control-group");

            writer.Write(controlGroup.ToString(TagRenderMode.StartTag));

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var controlLabel = new TagBuilder("label");
            controlLabel.AddCssClass("control-label");
            controlLabel.MergeAttribute("for", TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression))));
            controlLabel.SetInnerText(metadata.GetDisplayName());

            writer.Write(controlLabel.ToString());

            var controls = new TagBuilder("div");
            controls.AddCssClass("controls");

            writer.Write(controls.ToString(TagRenderMode.StartTag));

            return new BootstrapContent(() =>
            {
                writer.Write(controls.ToString(TagRenderMode.EndTag));
                writer.Write(controlGroup.ToString(TagRenderMode.EndTag));
            });
        }

        public static MvcHtmlString BootstrapFormActions(this HtmlHelper helper, string primaryButtonValue, string icon = null)
        {
            var formLine = new TagBuilder("div");
            formLine.AddCssClass("form-actions");

            var btnPrimary = new TagBuilder("button");
            btnPrimary.AddCssClass("btn");
            btnPrimary.AddCssClass("btn-primary");
            btnPrimary.AddCssClass("btn-large");
            btnPrimary.MergeAttribute("type", "submit");

            var i = new TagBuilder("i");
            i.AddCssClass(icon.HasValue() ? "icon-" + icon : "collapsed");
            
            btnPrimary.InnerHtml += i;
            btnPrimary.InnerHtml += " " + primaryButtonValue;
            formLine.InnerHtml += btnPrimary;

            return MvcHtmlString.Create(formLine.ToString());
        }

        public static BootstrapContent BeginBootstrapFormActions(this HtmlHelper helper, string primaryButtonValue, string icon = null)
        {
            var writer = helper.ViewContext.Writer;

            var formActions = new TagBuilder("div");
            formActions.AddCssClass("form-actions");

            writer.Write(formActions.ToString(TagRenderMode.StartTag));

            var btnPrimary = new TagBuilder("button");
            btnPrimary.AddCssClass("btn");
            btnPrimary.AddCssClass("btn-primary");
            btnPrimary.AddCssClass("btn-large");
            btnPrimary.MergeAttribute("type", "submit");

            writer.Write(btnPrimary.ToString(TagRenderMode.StartTag));

            var i = new TagBuilder("i");
            i.AddCssClass(icon.HasValue() ? "icon-" + icon : "collapsed");

            writer.Write(i.ToString());
            writer.Write(primaryButtonValue);
            writer.Write(btnPrimary.ToString(TagRenderMode.EndTag));

            return new BootstrapContent(() => writer.Write(formActions.ToString(TagRenderMode.EndTag)));
        }

        public class BootstrapContent : DisposableObject
        {
            private readonly Action _closeContent;

            public BootstrapContent(Action closeContent)
            {
                if (closeContent == null) throw new ArgumentNullException("closeContent");

                _closeContent = closeContent;
            }

            protected override void OnDisposing(bool disposing)
            {
                _closeContent();
            }
        }
    }
}