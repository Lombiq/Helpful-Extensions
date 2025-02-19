using Lombiq.HelpfulLibraries.Attributes;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using static Lombiq.HelpfulExtensions.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions.Extensions.Trumbowyg;

[ConstantFromJson("PrismVersion", "package.json", "prismjs")]
[ConstantFromJson("TrumbowygVersion", "package.json", "trumbowyg")]
public partial class TrumbowygResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private const string WwwRoot = "~/" + FeatureIds.Base + "/";
    private const string Css = WwwRoot + "css/";
    private const string Vendors = WwwRoot + "vendors/";
    private static readonly ResourceManifest _manifest = new();

    static TrumbowygResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineScript(Prism)
            .SetUrl(Vendors + "prismjs/prism.js")
            .SetVersion(PrismVersion);

        _manifest
            .DefineStyle(Prism)
            .SetUrl(Vendors + "prismjs/themes/prism.min.css", Vendors + "prismjs/themes/prism.css")
            .SetVersion(PrismVersion);

        _manifest
            .DefineScript(TrumbowygHighlight)
            .SetUrl(
                Vendors + "trumbowyg/plugins/highlight/trumbowyg.highlight.min.js",
                Vendors + "trumbowyg/plugins/highlight/trumbowyg.highlight.js")
            .SetDependencies("jQuery", "trumbowyg", Prism)
            .SetVersion(TrumbowygVersion);

        _manifest
            .DefineStyle(TrumbowygHighlight)
            .SetUrl(Css + "trumbowyg.highlight.css");
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);
}
