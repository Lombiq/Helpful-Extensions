using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Workflows.Models;

public record CreatingContext(
    ContentItem ContentItem,
    ContentTypePartDefinition Definition,
    string ContentSet,
    string NewKey);
