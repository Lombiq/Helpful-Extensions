using LombiqDotCom.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LombiqDotCom.Services;

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
            .Where(item => !string.IsNullOrEmpty(item.As<OrchardIds>()?.ExportId))
            .ToDictionary(item => item.As<OrchardIds>().ExportId);

        foreach (var item in itemsById.Values.Where(item => !string.IsNullOrEmpty(item.As<OrchardIds>().Parent)))
        {
            var parentId = item.As<OrchardIds>().Parent;
            if (!itemsById.TryGetValue(parentId, out var parent) || !parent.Has<ListPart>()) continue;

            item.Alter<ContainedPart>(part =>
            {
                part.ListContentItemId = parent.ContentItemId;
                part.ListContentType = parent.ContentType;
            });
        }

        return Task.CompletedTask;
    }
}
