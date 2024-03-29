using AngleSharp;
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

    protected Task<MarkdownBodyPart> GetSiteTextMarkdownBodyPartByIdAsync(string contentItemId)
    {
        ArgumentNullException.ThrowIfNull(contentItemId);
        return GetSiteTextMarkdownBodyPartByIdInnerAsync(contentItemId);
    }

    protected async Task<HtmlString> RenderMarkdownAsync(string markdown)
    {
        var html = _markdownService.ToHtml(markdown.Trim());

        using var context = BrowsingContext.New(Configuration.Default);
        using var doc = await context.OpenAsync(response => response.Content($"<html><body>{html}</body></html>"));

        // If it's a single-line expression, then it's presumably inline so don't wrap it in a <p> element.
        if (doc.Body is { ChildElementCount: 1, FirstElementChild: { } first } &&
            first.TagName.EqualsOrdinalIgnoreCase("P"))
        {
            html = first.InnerHtml.Trim();
        }

        return new(html);
    }

    private async Task<MarkdownBodyPart> GetSiteTextMarkdownBodyPartByIdInnerAsync(string contentItemId)
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
