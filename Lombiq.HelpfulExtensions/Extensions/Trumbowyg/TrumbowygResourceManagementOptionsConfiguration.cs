using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions.Extensions.Trumbowyg;

[Feature(FeatureIds.Trumbowyg)]
public class TrumbowygResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string WwwRoot = "~/" + FeatureIds.Base + "/";
    private const string Css = WwwRoot + "css/";
    private const string Vendors = WwwRoot + "vendors/";
    private static readonly ResourceManifest _manifest = new();

    static TrumbowygResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Prism)
            .SetUrl(Vendors + "prism/Scripts/prism.js")
            .SetVersion("1.29.0");

        _manifest
            .DefineStyle(Prism)
            .SetUrl(Vendors + "prism/Styles/prism.css")
            .SetVersion("1.29.0");

        _manifest
            .DefineScript(TrumbowygHighlight)
            .SetUrl(Vendors + "trumbowyg/plugins/highlight/trumbowyg.highlight.js")
            .SetDependencies("jQuery", "trumbowyg", Prism);

        _manifest
            .DefineStyle(TrumbowygHighlight)
            .SetUrl(Css + "trumbowyg.highlight.css");
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
