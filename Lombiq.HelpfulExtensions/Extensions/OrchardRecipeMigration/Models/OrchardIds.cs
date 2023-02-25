using OrchardCore.ContentManagement;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Models;

/// <summary>
/// Stores Orchard 1 ID and container ID.
/// </summary>
public class OrchardIds : ContentPart
{
    public string ExportId { get; set; }
    public string Parent { get; set; }
}
