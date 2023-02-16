using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string WwwRoot = "~/Lombiq.HelpfulExtensions/";

    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration() =>
        _manifest
            .DefineScript(TargetBlank)
            .SetUrl(WwwRoot + "scripts/target-blank.min.js", WwwRoot + "scripts/target-blank.js")
            .SetVersion("1.0.0");

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
