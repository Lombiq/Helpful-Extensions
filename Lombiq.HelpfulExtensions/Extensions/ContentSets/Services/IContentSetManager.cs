using Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Services;

/// <summary>
/// A service for getting and setting content sets.
/// </summary>
public interface IContentSetManager
{
    /// <summary>
    /// Returns all content item IDs in a content set with the given <paramref name="setId"/>.
    /// </summary>
    Task<IEnumerable<string>> GetContentItemIdsAsync(string setId);

    /// <summary>
    /// Returns all content items in a content set with the given <paramref name="setId"/>.
    /// </summary>
    Task<IEnumerable<ContentItem>> GetContentItemsAsync(string setId);

    /// <summary>
    /// Creates and publishes a new content item in the same content set by cloning the one with the given ID.
    /// </summary>
    /// <param name="fromContentItemId">The content item ID of the content to be cloned.</param>
    /// <param name="fromPartName">
    /// The name of the <see cref="ContentSetPart"/> whose set has to be amended. If it's not a named content part, then
    /// it can be <see langword="null"/>.
    /// </param>
    /// <param name="newKey">The new key of the content set item, must be unique within the set..</param>
    /// <returns>The newly created content item, or <see langword="null"/> if the source was not found.</returns>
    Task<ContentItem> CloneContentItemAsync(string fromContentItemId, string fromPartName, string newKey);
}
