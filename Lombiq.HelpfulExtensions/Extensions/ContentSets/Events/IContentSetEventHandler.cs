using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using Lombiq.HelpfulExtensions.Extensions.ContentSets.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Events;

/// <summary>
/// Events relating to <see cref="ContentSetPart"/> containing content items.
/// </summary>
public interface IContentSetEventHandler
{
    /// <summary>
    /// Returns the available items relating to the content item that contains the <paramref name="part"/>. This can be
    /// used for a dropdown to access the other contents in the set.
    /// </summary>
    /// <returns>
    /// A collection of option links, or <see langword="null"/> if this even handler is not applicable for the <paramref
    /// name="part"/>.
    /// </returns>
    Task<IEnumerable<ContentSetLinkViewModel>> GetSupportedOptionsAsync(
        ContentSetPart part,
        ContentTypePartDefinition definition) =>
        Task.FromResult<IEnumerable<ContentSetLinkViewModel>>(null);

    /// <summary>
    /// The event triggered when a donor content item is cloned but before it's published.
    /// </summary>
    /// <param name="content">The new content item.</param>
    /// <param name="definition">
    /// The part definition indicating which <see cref="ContentSetPart"/> is responsible for this event.
    /// </param>
    /// <param name="contentSet">The unique ID of the content set.</param>
    /// <param name="newKey">The new item's key, which is unique within the content set.</param>
    Task CreatingAsync(
        ContentItem content,
        ContentTypePartDefinition definition,
        string contentSet,
        string newKey) =>
        Task.CompletedTask;
}
