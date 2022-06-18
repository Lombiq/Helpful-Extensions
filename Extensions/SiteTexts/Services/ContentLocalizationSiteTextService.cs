using Microsoft.AspNetCore.Html;
using OrchardCore.ContentLocalization;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Services;
using System.Globalization;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

public class ContentLocalizationSiteTextService : SiteTextServiceBase
{
    private readonly IContentLocalizationManager _contentLocalizationManager;

    public ContentLocalizationSiteTextService(
        IContentManager contentManager,
        IContentLocalizationManager contentLocalizationManager,
        IMarkdownService markdownService)
        : base(contentManager, markdownService) =>
        _contentLocalizationManager = contentLocalizationManager;

    public override async Task<HtmlString> RenderHtmlByIdAsync(string contentItemId)
    {
        var part = await GetSiteTextMarkdownBodyPartByIdAsync(contentItemId);

        if (part.As<LocalizationPart>() is { } localizationPart &&
            localizationPart.Culture != CultureInfo.CurrentCulture.Name)
        {
            var contentItem = await _contentLocalizationManager.GetContentItemAsync(
                localizationPart.LocalizationSet,
                CultureInfo.CurrentCulture.Name);

            if (contentItem?.As<MarkdownBodyPart>() is { } localizedPart) part = localizedPart;
        }

        return RenderMarkdown(part.Markdown);
    }
}
