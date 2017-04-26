using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace bvlf_v2.Helpers
{
    public static class CheckBoxListExtension
    {
        public static string CheckBoxListFor<TModel, TCheckboxListValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, CheckBoxListViewModel<TCheckboxListValue>>> expression,
            bool newLine, string group,
            IDictionary<string, object> htmlAttributes) where TModel : class
        {
            var inputName = GetInputName(expression);

            var checkboxList = GetValue(htmlHelper, expression);

            if (checkboxList == null)
            {
                return String.Empty;
            }

            if (checkboxList.ListItems == null)
            {
                return String.Empty;
            }

            var divTag = new TagBuilder("div");
            divTag.MergeAttribute("id", inputName);
            divTag.MergeAttribute("class", "radio");
            foreach (var item in checkboxList.ListItems)
            {
                var checboxTag = SdSessionCheckBox(
                    htmlHelper,
                    inputName,
                    new SelectListItem {Text = item.Text, Selected = item.Selected, Value = item.Value.ToString()},
                    newLine,
                    htmlAttributes, group, item.IsVolzet);

                divTag.InnerHtml += checboxTag;
            }

            return divTag.ToString(); // + htmlHelper.ValidationMessage(inputName, "*");
        }

        private static string SdSessionCheckBox(this HtmlHelper htmlHelper, string name, SelectListItem item,
            bool newline, IDictionary<string, object> htmlattributes, string group, bool isvolzet = false)
        {
            if (item == null)
                return string.Empty;
            var inputIdSb = string.Format("{0}_{1}", name, item.Value);
            var contentString = "";
            if (isvolzet)
            {
                contentString = string.Format(
                    "<img src='/Media/Volzet.png' title='Dit atelier is helaas volzet' alt='volzet' />");
            }
            else
            {
                contentString = GetCheckBox(inputIdSb, item.Value, item.Selected, group);
            }

            var outputscript = new CustomTagBuilder()
                .StartTag("div")
                .AddAttribute("class", "SessieSelectionItem left-align")
                .AddContent(string.Format("<label for=\"{2}\" /> at {0} </label> {1}", item.Text, contentString,
                    inputIdSb))
                .EndTag()
                .ToString();
            return outputscript;
        }

        private static string GetCheckBox(string name, string value, bool isselected, string group)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("id", name);
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", value);
            builder.MergeAttribute("class", group);
            if (isselected)
                builder.MergeAttribute("checked", "checked");
            return builder.ToString(TagRenderMode.SelfClosing);
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
    }
}