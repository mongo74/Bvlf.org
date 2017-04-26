using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bvlf_v2.Helpers
{
    public class CustomTagBuilder
    {
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();
        private readonly StringBuilder body = new StringBuilder();
        private readonly CustomTagBuilder parent;
        private readonly string tagName;

        public CustomTagBuilder()
        {
        }

        public CustomTagBuilder(string TagName, CustomTagBuilder Parent)
        {
            tagName = TagName;
            parent = Parent;
        }

        public CustomTagBuilder AddContent(string Content)
        {
            body.Append(Content);
            return this;
        }

        public CustomTagBuilder AddContentFormat(string Format, params object[] args)
        {
            body.AppendFormat(Format, args);
            return this;
        }

        public CustomTagBuilder StartTag(string TagName)
        {
            var tag = new CustomTagBuilder(TagName, this);

            return tag;
        }

        public CustomTagBuilder EndTag()
        {
            parent.AddContent(ToString());
            return parent;
        }

        public CustomTagBuilder AddAttribute(string Name, string Value)
        {
            _attributes.Add(Name, Value);
            return this;
        }

        public override string ToString()
        {
            var tag = new StringBuilder();

            // preamble
            if (!string.IsNullOrEmpty(tagName))
                tag.AppendFormat("<{0}", tagName);

            if (_attributes.Count > 0)
            {
                tag.Append(" ");
                tag.Append(
                    string.Join(" ",
                        _attributes
                            .Select(
                                kvp => string.Format("{0}='{1}'", kvp.Key, kvp.Value))
                            .ToArray()));
            }

            // body/ending
            if (body.Length > 0)
            {
                if (!string.IsNullOrEmpty(tagName) || _attributes.Count > 0)
                    tag.Append(">");
                tag.Append(body);
                if (!string.IsNullOrEmpty(tagName))
                    tag.AppendFormat("</{0}>", tagName);
            }
            else if (!string.IsNullOrEmpty(tagName))
                tag.Append("/>");

            return tag.ToString();
        }
    }
}