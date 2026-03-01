using Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.PrismLanguageNames;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.TrumbowygResourceNames;

namespace Lombiq.HelpfulExtensions.Extensions.TrumbowygBlogPosts;

public class ResourceFilters : IResourceFilterProvider
{
    public void AddResourceFilter(ResourceFilterBuilder builder)
    {
        const string BlogPost = "BlogPost";

        var resourceFilters = new[]
            {
                builder.WhenContentType(BlogPost),
                builder.WhenContentTypeEditor(BlogPost),
                builder.WhenContentTypeCreate(BlogPost),
                builder.WhenContentTypePreview(BlogPost),
            };

        foreach (var resourceFilter in resourceFilters)
        {
            resourceFilter.RegisterStylesheet(Prism, PrismCoyTheme, PrismLineHighlight, TrumbowygHighlight);
            resourceFilter.RegisterFootScript(Prism, PrismLineHighlight, TrumbowygHighlight, TrumbowygHighlightExtension);
            foreach (var language in AllLanguage)
            {
                resourceFilter.RegisterFootScript(language);
            }
        }
    }
}
