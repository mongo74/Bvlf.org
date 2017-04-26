using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace bvlf_v2.BOL.Helpers
{
    public interface ITemplateRenderer
    {
        string RenderTemplate<TModel>(string templateName, TModel model);
    }

    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly string _templatePath;

        public TemplateRenderer(string templatePath)
        {
            _templatePath = templatePath;
        }

        public string RenderTemplate<TModel>(string templateName, TModel model)
        {
            var templateBody = GetTemplateBody(templateName);
            templateBody = ReplacePlaceholdersWithValues(templateBody, model);

            return templateBody;
        }

        public string RenderTemplate(string templateName, IDictionary model)
        {
            var templateBody = GetTemplateBody(templateName);
            templateBody = ReplacePlaceholdersWithValues(templateBody, model);

            return templateBody;
        }

        private static string ReplacePlaceholdersWithValues(string templateBody, IDictionary model)
        {
            foreach (var parameterName in model.Keys)
            {
                var key = "{{" + parameterName + "}}";
                if (templateBody.Contains(key))
                    templateBody = templateBody.Replace(key, model[parameterName].ToString());
            }

            return templateBody;
        }

        private static string ReplacePlaceholdersWithValues<TModel>(string templateBody, TModel model)
        {
            var properties = typeof (TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var key = "{{" + property.Name + "}}";
                if (templateBody.Contains(key))
                    templateBody = templateBody.Replace(key, property.GetValue(model, null).ToString());
            }

            return templateBody;
        }

        private string GetTemplateBody(string templateName)
        {
            string templateBody;

            var templatePath = Path.Combine(_templatePath, templateName);
            using (var reader = new StreamReader(templatePath, Encoding.UTF7))
                templateBody = reader.ReadToEnd();

            return templateBody;
        }
    }
}