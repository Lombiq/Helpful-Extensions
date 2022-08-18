using Microsoft.AspNetCore.Html;
using OrchardCore.ContentManagement;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

/// <summary>
/// A service for getting the value of the site text as HTML.
/// </summary>
public interface ISiteTextService
{
    /// <summary>
    /// Looks up the referenced site text and returns it as HTML. Depending on the implementation, it may also perform
    /// other steps such looking for a localized version.
    /// </summary>
    /// <param name="contentItemId">The <see cref="ContentItem.ContentItemId"/> of the Site Text.</param>
    Task<HtmlString> RenderHtmlByIdAsync(string contentItemId);
}
