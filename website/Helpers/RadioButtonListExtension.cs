using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using bvlf_v2.Models;

namespace bvlf_v2.Helpers
{
    public static class RadioButtonListExtension
    {
        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, CustomListViewModel<TRadioButtonListValue>>> expression,
            bool newLine)
            where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, newLine, null);
        }

        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, CustomListViewModel<TRadioButtonListValue>>> expression,
            bool newLine,
            object htmlAttributes)
            where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, newLine, new RouteValueDictionary(htmlAttributes));
        }

        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, CustomListViewModel<TRadioButtonListValue>>> expression,
            bool newLine,
            IDictionary<string, object> htmlAttributes)
            where TModel : class
        {
            var inputName = GetInputName(expression);

            var radioButtonList = GetValue(htmlHelper, expression);

            if (radioButtonList == null)
            {
                return String.Empty;
            }

            if (radioButtonList.ListItems == null)
            {
                return String.Empty;
            }

            var divTag = new TagBuilder("div");
            divTag.MergeAttribute("id", inputName);
            divTag.MergeAttribute("class", "radio");
            foreach (var item in radioButtonList.ListItems)
            {
                var radioButtonTag = RadioButton(
                    htmlHelper,
                    inputName,
                    new SelectListItem {Text = item.Text, Selected = item.Selected, Value = item.Value.ToString()},
                    newLine,
                    htmlAttributes);

                divTag.InnerHtml += radioButtonTag;
            }

            return divTag.ToString(); // + htmlHelper.ValidationMessage(inputName, "*");
        }

        public static TProperty GetValue<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
            where TModel : class
        {
            if (htmlHelper == null)
            {
                return default(TProperty);
            }

            var model = htmlHelper.ViewData.Model;
            if (model == null)
            {
                return default(TProperty);
            }

            if (expression == null)
            {
                return default(TProperty);
            }

            var func = expression.Compile();
            return func(model);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, SelectListItem listItem, bool newLine,
            IDictionary<string, object> htmlAttributes)
        {
            if (listItem == null)
            {
                return string.Empty;
            }

            var inputIdSb = new StringBuilder();
            inputIdSb.Append(name)
                .Append("_")
                .Append(listItem.Value);

            var sb = new StringBuilder();

            var builder = new TagBuilder("input");
            if (listItem.Selected)
            {
                builder.MergeAttribute("checked", "checked");
            }

            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", listItem.Value);
            builder.MergeAttribute("id", inputIdSb.ToString());
            builder.MergeAttribute("name", name + ".SelectedValue");

            // builder.MergeAttribute("name", name);
            builder.MergeAttributes(htmlAttributes);
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append(RadioButtonLabel(inputIdSb.ToString(), listItem.Text, htmlAttributes));
            if (newLine.Equals(true))
            {
                sb.Append("<br />");
            }

            return sb.ToString();
        }

        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }

            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression) expression.Body;
                var name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);
            }

            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            // p => p.Foo.Bar().Baz.ToString() => p.Foo OR throw... 
            var methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }

            return expression.Object.ToString();
        }

        public static string RadioButtonLabel(string inputId, string displayText,
            IDictionary<string, object> htmlAttributes)
        {
            var labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", inputId);
            // labelBuilder.MergeAttributes(htmlAttributes);
            labelBuilder.InnerHtml = displayText;

            return labelBuilder.ToString(TagRenderMode.Normal);
        }
    }
}