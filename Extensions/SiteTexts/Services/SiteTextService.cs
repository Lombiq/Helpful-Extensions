using Microsoft.AspNetCore.Html;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Services;
using System;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

public class SiteTextService
    : ISiteTextService
{
    private readonly IContentManager _contentManager;
    private readonly IMarkdownService _markdownService;

    public SiteTextService(IContentManager contentManager, IMarkdownService markdownService)
    {
        _contentManager = contentManager;
        _markdownService = markdownService;
    }

    public async Task<HtmlString> RenderHtmlByIdAsync(string contentItemId)
    {
        var part = await GetSiteTextMarkdownBodyPartByIdAsync(contentItemId);
        return new HtmlString(_markdownService.ToHtml(part.Markdown));
    }

    internal async Task<MarkdownBodyPart> GetSiteTextMarkdownBodyPartByIdAsync(string contentItemId)
    {
        if (await _contentManager.GetAsync(contentItemId) is not { } contentItem)
        {
            throw new InvalidOperationException($"A content with the ID \"{contentItemId}\" does not exist.");
        }

        if (contentItem.As<MarkdownBodyPart>() is not { } part)
        {
            throw new InvalidOperationException(
                $"A content with the ID \"{contentItemId}\" does not have a {nameof(MarkdownBodyPart)}.");
        }

        return part;
    }
}
