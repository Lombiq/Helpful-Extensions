using Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

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
            };

        foreach (var resourceFilter in resourceFilters)
        {
            resourceFilter.RegisterStylesheet(Prism, PrismLineHighlight, TrumbowygHighlight);
            resourceFilter.RegisterFootScript(Prism, PrismLineHighlight, TrumbowygHighlight);
        }
    }
}
