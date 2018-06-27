using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Services;
using Orchard.Services;
using Orchard.Tokens;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Tokens.Filters
{
    /// <summary>
    /// Fixes that HtmlElements can't provide Content token values using the original TokensFilter since the
    /// IHtmlFilter.ProcessContent method doesn't have a context property.
    /// </summary>
    [OrchardSuppressDependency("Orchard.Tokens.Filters.TokensFilter")]
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    public class TokensFilter : ContentHandler, IHtmlFilter, IElementFilter
    {
        private readonly ITokenizer _tokenizer;
        private ContentItem _displayed;


        public TokensFilter(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }


        public string ProcessContent(string text, string flavor) =>
            TokensReplace(text);

        public string ProcessContent(string text, string flavor, IDictionary<string, object> context) =>
            TokensReplace(text, context);


        protected override void BuildDisplayShape(BuildDisplayContext context) =>
            _displayed = context.ContentItem;


        private string TokensReplace(string text, IDictionary<string, object> data = null)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if (!text.Contains("#{")) return text;

            if (data == null) data = new Dictionary<string, object>();

            if (_displayed != null) data["Content"] = _displayed;

            text = _tokenizer.Replace(text, data);

            _displayed = null;

            return text;
        }
    }
}