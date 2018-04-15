using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;
using System;

namespace Piedone.HelpfulExtensions.Contents.Tokens
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    public class IdentityResolverTokens : ITokenProvider
    {
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }


        public IdentityResolverTokens(IContentManager contentManager)
        {
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            context.For("IdentityResolver", T("Identity resolution"), T("Identity tokens to get the content item."))
                .Token("Identity:*", T("Identity:*<content identity>"), T("The content identity value to acccess."))
                .Token("Content", T("Content Items"), T("Content Items"));
        }

        public void Evaluate(EvaluateContext context)
        {
            context.For("IdentityResolver", () => _contentManager)
                .Token(token => token.StartsWith("Identity:", StringComparison.OrdinalIgnoreCase) ? token.Substring("Identity:".Length) : null, GetContentItem)
                .Chain(FilterChainParam, "Content", GetContentItem);
        }


        private static Tuple<string, string> FilterChainParam(string token)
        {
            var tokenLength = "Identity:".Length;
            var equalsIndex = token.IndexOf('=');
            var dotIndex = token.IndexOf('.', equalsIndex);
            var chainIndex = token.IndexOf('.');

            return token.StartsWith("Identity:", StringComparison.OrdinalIgnoreCase) && chainIndex > tokenLength ?
                new Tuple<string, string>(token.Substring(tokenLength, dotIndex - tokenLength), token.Substring(dotIndex + 1, token.Length - dotIndex - 1)) :
                null;
        }

        private static ContentItem GetContentItem(string contentIdentity, IContentManager contentManager)
        {
            var startIndex = contentIdentity.IndexOf('=');
            var index = contentIdentity.IndexOf(('.'), startIndex);

            if (index > -1) contentIdentity = contentIdentity.Substring(0, index);

            return contentManager.ResolveIdentity(new ContentIdentity(contentIdentity));
        }
    }
}