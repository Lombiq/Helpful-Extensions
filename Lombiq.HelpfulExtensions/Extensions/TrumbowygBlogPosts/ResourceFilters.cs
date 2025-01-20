using Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions.Extensions.TrumbowygBlogPosts;

public class ResourceFilters : IResourceFilterProvider
{
    public void AddResourceFilter(ResourceFilterBuilder builder)
    {
        const string BlogPost = "BlogPost";

        builder.WhenContentType(BlogPost).RegisterStylesheet(Prism);
        builder.WhenContentType(BlogPost).RegisterFootScript(Prism);

        builder.WhenContentTypeEditor(BlogPost).RegisterFootScript(TrumbowygHighlight);
        builder.WhenContentTypeEditor(BlogPost).RegisterStylesheet(TrumbowygHighlight);
        builder.WhenContentTypeCreate(BlogPost).RegisterFootScript(TrumbowygHighlight);
        builder.WhenContentTypeCreate(BlogPost).RegisterStylesheet(TrumbowygHighlight);
    }
}
