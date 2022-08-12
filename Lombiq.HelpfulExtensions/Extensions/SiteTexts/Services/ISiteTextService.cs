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
    /// Depending on the implementation it may look at the referenced content item, or any localized versions.
    /// </summary>
    /// <param name="contentItemId">
    /// The <see cref="ContentItem.ContentItemId"/> of the Site Text. If the first character is <c>~</c> then the rest
    /// can be separated with space characters and the trailing <c>0</c> characters omitted. This way
    /// <c>~help popular topics</c> can be used to look up the ID <c>helppopulartopics000000000</c>.
    /// </param>
    Task<HtmlString> RenderHtmlByIdAsync(string contentItemId);
}
