using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Services;
using Orchard.Tokens;
using System;
using System.Linq;

namespace Piedone.HelpfulExtensions.Contents.Tokens
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    // Extends the original RequestTokens found in the Orchard.Tokens module.
    public class RequestTokens : ITokenProvider
    {
        private readonly IWorkContextAccessor _wca;
        private readonly IJsonConverter _jsonConverter;

        public Localizer T { get; set; }


        public RequestTokens(IWorkContextAccessor wca, IJsonConverter jsonConverter)
        {
            _wca = wca;
            _jsonConverter = jsonConverter;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            context
                .For("Request", T("Http Request"), T("Current Http Request tokens."))
                .Token("FullQueryString", T("FullQueryString"), T("The full Query String in the current HTTP request."))
                .Token("QueryStringValuesJson:*", T("QueryStringValuesJson:<element>"), T("The value(s) of the specified Query String parameter key serialized to JSON."));
        }

        public void Evaluate(EvaluateContext context)
        {
            var httpContext = _wca.GetContext().HttpContext;
            if (httpContext == null) return;

            context
                .For("Request", httpContext.Request)
                .Token("FullQueryString", request => request.Url.Query)
                .Token(
                    token => token.StartsWith("QueryStringValuesJson:", StringComparison.OrdinalIgnoreCase) ? token.Substring("QueryStringValuesJson:".Length) : null,
                    (token, request) =>
                    {
                        var values = request.QueryString.GetValues(token);

                        return values?.Any() ?? false ? _jsonConverter.Serialize(values) : null;
                    }
                );
        }
    }

}
