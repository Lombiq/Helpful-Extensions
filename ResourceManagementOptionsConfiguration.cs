using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Lombiq.HelpfulExtensions;
public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration() =>
        _manifest
            .DefineScript("Lombiq.HelpfulExtensions.TargetBlank")
            .SetUrl("~/Lombiq.HelpfulExtensions/TargetBlank/TargetBlank.js")
            .SetPosition(ResourcePosition.Last);

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
