using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;

public record GetSupportedOptionsContext(
    ContentTypePartDefinition Definition,
    ContentSetPart ContentSetPart)
{
    public ContentItem ContentItem => ContentSetPart?.ContentItem;

    public IDictionary<string, object> ToDictionary() =>
        new Dictionary<string, object>
        {
            // We create a new type here to avoid circular references which break JSON serialization.
            [nameof(Definition)] = Definition == null ? null : new
            {
                Definition.Name,
                Definition.Settings,
                PartDefinition = new
                {
                    Definition.PartDefinition.Name,
                    Definition.PartDefinition.Settings,
                    Fields = Definition.PartDefinition.Fields
                        .Select(field => new { field.Name, field.Settings, field.FieldDefinition })
                        .ToList(),
                },
            },
            [nameof(ContentSetPart)] = ContentSetPart,
            [nameof(ContentItem)] = ContentItem,
        };
}
