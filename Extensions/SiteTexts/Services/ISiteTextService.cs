using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.SiteTexts.Services;

/// <summary>
/// A service for getting the value of the site text as HTML.
/// </summary>
public interface ISiteTextService
{
    /// <summary>
    /// Depending on the implementation it may look at the referenced content item, or any localized versions.
    /// </summary>
    Task<HtmlString> RenderHtmlByIdAsync(string contentItemId);
}
