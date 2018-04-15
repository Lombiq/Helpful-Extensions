using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;

namespace Piedone.HelpfulExtensions.Contents.Tokens
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    // Extends the original RequestTokens found in the Orchard.Tokens module.
    public class RequestTokens : ITokenProvider
    {
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }


        public RequestTokens(IWorkContextAccessor wca)
        {
            _wca = wca;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            context
                .For("Request", T("Http Request"), T("Current Http Request tokens."))
                .Token("FullQueryString", T("FullQueryString"), T("The full Query String in the current HTTP request."));
        }

        public void Evaluate(EvaluateContext context)
        {
            var httpContext = _wca.GetContext().HttpContext;
            if (httpContext == null) return;

            context
                .For("Request", httpContext.Request)
                .Token("FullQueryString", request => request.Url.Query);
        }
    }

}
