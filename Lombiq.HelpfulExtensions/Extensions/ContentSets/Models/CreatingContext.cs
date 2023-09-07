using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;

public record CreatingContext(
    ContentItem ContentItem,
    ContentTypePartDefinition Definition,
    string ContentSet,
    string NewKey)
{
    public IDictionary<string, object> ToDictionary() => new RouteValueDictionary(this);
}
