using System;
using Orchard;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Tokens;
using Piedone.HelpfulLibraries.Libraries.Contents;

namespace Piedone.HelpfulExtensions.Contents.Tokens
{
    [OrchardFeature("Piedone.HelpfulExtensions.Tokens")]
    public class ContentTokens : ITokenProvider
    {
        private readonly IContentManager _contentManager;
        private readonly ICurrentContentIdAccessor _currentContentIdAccessor;

        private IContent _currentContent = null;
        private IContent CurrentContent
        {
            get
            {
                if (_currentContent == null)
                {
                    var currentContentId = _currentContentIdAccessor.GetCurrentContentId();

                    if (currentContentId != 0) _currentContent = _contentManager.Get(currentContentId);
                    else _currentContent = _contentManager.New("Dummy"); // _currentContent isn't null so chained tokens don't throw a NE
                }

                return _currentContent;
            }
        }

        public Localizer T { get; set; }


        public ContentTokens(
            IContentManager contentManager,
            ICurrentContentIdAccessor currentContentIdAccessor)
        {
            _contentManager = contentManager;
            _currentContentIdAccessor = currentContentIdAccessor;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            context.For("Content", T("Content Items"), T("Content Items"))
                .Token("Current", T("Current content"), T("If the current request is for a content item, returns the item."));
        }

        public void Evaluate(EvaluateContext context)
        {
            // Dummy item as workaround for this: https://github.com/OrchardCMS/Orchard/issues/3522
            context.For("Content", () => (IContent)new ContentItem())
                .Token("Current", content => string.Empty)
                .Chain("Current", "Content", content => CurrentContent);
        }
    }
}
