using Microsoft.AspNetCore.Html;
using OrchardCore.ContentLocalization;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Services;
using System.Globalization;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

public class ContentLocalizationSiteTextService(
    IContentManager contentManager,
    IContentLocalizationManager contentLocalizationManager,
    IMarkdownService markdownService) : SiteTextServiceBase(contentManager, markdownService)
{
    private readonly IContentLocalizationManager _contentLocalizationManager = contentLocalizationManager;

    public override async Task<HtmlString> RenderHtmlByIdAsync(string contentItemId)
    {
        var part = await GetSiteTextMarkdownBodyPartByIdAsync(contentItemId);
        var culture = CultureInfo.CurrentCulture.Name;

        if (part.As<LocalizationPart>() is { Culture: { } partCulture, LocalizationSet: { } localizationSet } &&
            partCulture != culture &&
            await _contentLocalizationManager.GetContentItemAsync(localizationSet, culture) is { } contentItem &&
            contentItem.As<MarkdownBodyPart>() is { } localizedPart)
        {
            part = localizedPart;
        }

        return await RenderMarkdownAsync(part.Markdown);
    }
}
