using Microsoft.AspNetCore.Html;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Services;
using System;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

public abstract class SiteTextServiceBase : ISiteTextService
{
    protected readonly IContentManager _contentManager;
    protected readonly IMarkdownService _markdownService;

    protected SiteTextServiceBase(IContentManager contentManager, IMarkdownService markdownService)
    {
        _contentManager = contentManager;
        _markdownService = markdownService;
    }

    public abstract Task<HtmlString> RenderHtmlByIdAsync(string contentItemId);

    protected async Task<MarkdownBodyPart> GetSiteTextMarkdownBodyPartByIdAsync(string contentItemId)
    {
        ArgumentNullException.ThrowIfNull(contentItemId);

        // A bit of syntactic sugar for IDs using our conventions. This "~help popular topics" will be translated to
        // the ID "helppopulartopics000000000".
        if (contentItemId[0] == '~')
        {
#pragma warning disable CA1308
            contentItemId = contentItemId[1..].Replace(" ", string.Empty).ToLowerInvariant();
#pragma warning restore CA1308

            if (contentItemId.Length < 26) contentItemId = contentItemId.PadRight(26, '0');
        }

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

    protected HtmlString RenderMarkdown(string markdown) => new(_markdownService.ToHtml(markdown));
}
