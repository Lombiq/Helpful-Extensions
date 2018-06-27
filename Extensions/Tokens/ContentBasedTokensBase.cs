using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;

namespace Piedone.HelpfulExtensions.Tokens
{
    [OrchardFeature(Constants.FeatureNames.Tokens)]
    public abstract class ContentBasedTokensBase : Component, ITokenProvider
    {
        protected readonly IContentManager _contentManager;
        protected abstract string Target { get; }
        protected abstract LocalizedString Name { get; }
        protected abstract LocalizedString Description { get; }


        public ContentBasedTokensBase(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }


        public virtual void Describe(DescribeContext context)
        {
            Describe(context, context.For(Target, Name, Description)
                .Token("Content", T("Content Item"), T("Content Item")));
        }

        public virtual void Evaluate(EvaluateContext context)
        {
            Evaluate(context, context.ForCustomTokenData(Target, () => _contentManager.NewDummyItem())
                .Chain("Content", "Content", content => content.ContentItem));
        }


        protected virtual void Describe(DescribeContext context, DescribeFor describeFor) { }

        protected virtual void Evaluate(EvaluateContext context, EvaluateFor<IContent> evaluateFor) { }
    }
}