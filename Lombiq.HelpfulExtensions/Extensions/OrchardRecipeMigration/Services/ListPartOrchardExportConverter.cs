using Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

/// <summary>
/// A post-processing converter that looks for added <see cref="OrchardIds"/> content parts in the prepared list of
/// content items where the <see cref="OrchardIds.Parent"/> is set by custom converters. If the content item of the
/// parent ID has <see cref="ListPart"/> then the child is assigned to its list.
/// </summary>
public class ListPartOrchardExportConverter : IOrchardExportConverter
{
    public Task UpdateContentItemsAsync(XDocument document, IList<ContentItem> contentItems)
    {
        var itemsById = contentItems
            .SelectWhere(
                item => (Item: item, Ids: item.GetOrCreate<OrchardIds>()),
                pair => !string.IsNullOrEmpty(pair.Ids.ExportId) && !string.IsNullOrEmpty(pair.Ids.Parent))
            .ToDictionary(item => item.Ids.ExportId);

        foreach (var (item, ids) in itemsById.Values)
        {
            if (!itemsById.TryGetValue(ids.Parent, out var parentPair) || !parentPair.Item.Has<ListPart>()) continue;

            item.Alter<ContainedPart>(part =>
            {
                part.ListContentItemId = parentPair.Item.ContentItemId;
                part.ListContentType = parentPair.Item.ContentType;
            });
        }

        return Task.CompletedTask;
    }
}
