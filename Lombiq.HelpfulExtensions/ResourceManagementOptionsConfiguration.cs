using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string ScriptRoot = "~/Lombiq.HelpfulExtensions/js/";
    private static readonly ResourceManifest _manifest = new();

    static ResourceManagementOptionsConfiguration() =>
        _manifest
            .DefineScript(TargetBlank)
            .SetUrl(ScriptRoot + "target-blank.min.js", ScriptRoot + "target-blank.js")
            .SetVersion("1.0.0");

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
