using Microsoft.AspNetCore.Html;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Services;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

public class SiteTextService(IContentManager contentManager, IMarkdownService markdownService) : SiteTextServiceBase(contentManager, markdownService)
{
    public override async Task<HtmlString> RenderHtmlByIdAsync(string contentItemId)
    {
        var part = await GetSiteTextMarkdownBodyPartByIdAsync(contentItemId);
        return await RenderMarkdownAsync(part.Markdown);
    }
}
