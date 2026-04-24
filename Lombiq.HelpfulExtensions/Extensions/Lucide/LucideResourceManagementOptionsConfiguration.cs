using Lombiq.HelpfulLibraries.Attributes;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Lombiq.HelpfulExtensions.Extensions.Lucide;

[LibManVersions]
public partial class LucideResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string ModuleRoot = "~/" + FeatureIds.Base + "/";
    private const string Css = ModuleRoot + "css/";
    private const string Scripts = ModuleRoot + "js/";
    private const string Vendors = ModuleRoot + "vendors/";
    private static readonly ResourceManifest _manifest = new();

    static LucideResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Constants.ResourceNames.Lucide)
            .SetUrl(
                Vendors + "lucide/dist/umd/lucide.min.js",
                Vendors + "lucide/dist/umd/lucide.js")
            .SetVersion(LibManVersions.Lucide);

        _manifest
            .DefineScript(Constants.ResourceNames.LucideIconPicker)
            .SetUrl(Scripts + "lucide-icon-picker.js")
            .SetDependencies(Constants.ResourceNames.Lucide);

        _manifest
            .DefineStyle(Constants.ResourceNames.LucideIconPicker)
            .SetUrl(Css + "lucide-icon-picker.css");
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
