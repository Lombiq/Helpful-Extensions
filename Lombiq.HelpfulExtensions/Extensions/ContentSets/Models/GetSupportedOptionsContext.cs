using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;

public record GetSupportedOptionsContext(
    ContentTypePartDefinition Definition,
    ContentSetPart ContentSetPart)
{
    public ContentItem ContentItem => ContentSetPart?.ContentItem;

    public IDictionary<string, object> ToDictionary() =>
        new Dictionary<string, object>
        {
            [nameof(Definition)] = Definition,
            [nameof(ContentSetPart)] = ContentSetPart,
            [nameof(ContentItem)] = ContentItem,
        };
}
