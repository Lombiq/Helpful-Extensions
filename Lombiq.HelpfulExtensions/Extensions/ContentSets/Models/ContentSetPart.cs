using OrchardCore.ContentManagement;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;

public class ContentSetPart : ContentPart
{
    public const string Default = nameof(Default);

    public string ContentSet { get; set; }
    public string Key { get; set; } = Default;
}
