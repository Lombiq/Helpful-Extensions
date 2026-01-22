using Lombiq.HelpfulLibraries.Attributes;
using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;
using System;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.PrismLanguageNames;
using static Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants.ResourceNames;

namespace Lombiq.HelpfulExtensions.Extensions.Trumbowyg;

[LibManVersions]
public partial class TrumbowygResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    [Obsolete($"Use the values in {nameof(LibManVersions)}.")]
    public const string PrismVersion = LibManVersions.Prismjs;

    [Obsolete($"Use the values in {nameof(LibManVersions)}.")]
    public const string TrumbowygVersion = LibManVersions.Trumbowyg;

    private const string WwwRoot = "~/" + FeatureIds.Base + "/";
    private const string Css = WwwRoot + "css/";
    private const string Vendors = WwwRoot + "vendors/";
    private static readonly ResourceManifest _manifest = new();

    static TrumbowygResourceManagementOptionsConfiguration()
    {
        _manifest
            .DefineStyle(Prism)
            .SetUrl(Vendors + "prismjs/themes/prism.min.css", Vendors + "prismjs/themes/prism.css")
            .SetVersion(LibManVersions.Prismjs);

        _manifest
            .DefineStyle(PrismCoyTheme)
            .SetUrl(Vendors + "prismjs/themes/prism-coy.min.css", Vendors + "prismjs/themes/prism-coy.css")
            .SetVersion(LibManVersions.Prismjs);

        _manifest
            .DefineStyle(PrismLineHighlight)
            .SetUrl(
                Vendors + "prismjs/plugins/line-highlight/prism-line-highlight.min.css",
                Vendors + "prismjs/plugins/line-highlight/prism-line-highlight.css")
            .SetVersion(LibManVersions.Prismjs);

        _manifest
            .DefineStyle(TrumbowygHighlight)
            .SetUrl(Css + "trumbowyg.highlight.css");

        _manifest
            .DefineScript(Prism)
            .SetUrl(Vendors + "prismjs/prism.js")
            .SetVersion(LibManVersions.Prismjs);

        LoadPrismLanguages();

        _manifest
            .DefineScript(PrismLineHighlight)
            .SetUrl(Vendors + "prismjs/plugins/line-highlight/prism-line-highlight.js")
            .SetVersion(LibManVersions.Prismjs);

        _manifest
            .DefineScript(TrumbowygHighlight)
            .SetUrl(
                Vendors + "trumbowyg/plugins/highlight/trumbowyg.highlight.min.js",
                Vendors + "trumbowyg/plugins/highlight/trumbowyg.highlight.js")
            .SetDependencies("jQuery", "trumbowyg", Prism)
            .SetVersion(LibManVersions.Trumbowyg);

        _manifest
            .DefineScript(TrumbowygHighlightExtension)
            .SetUrl(WwwRoot + "js/trumbowyg.highlight.extension.js")
            .SetDependencies("jQuery", "trumbowyg", Prism)
            .SetVersion("1.0.0");
    }

    public void Configure(ResourceManagementOptions options) => options.ResourceManifests.Add(_manifest);

    private static void LoadPrismLanguages()
    {
        // Markup templating languages are required by other languages e.g. liquid.
        _manifest
            .DefineScript(nameof(MarkupTemplating))
            .SetUrl(
                Vendors + $"prismjs/components/prism-{MarkupTemplating}.min.js",
                Vendors + $"prismjs/components/prism-{MarkupTemplating}.js")
            .SetVersion(LibManVersions.Prismjs);

        foreach (var language in AllLanguage)
        {
            // Prism language files are all lowercase named.
#pragma warning disable CA1308 // CA1308: In method 'LoadPrismLanguages', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'
            var lowercaseLanguage = language.ToLowerInvariant();
#pragma warning restore CA1308
            _manifest
                .DefineScript(language)
                .SetUrl(
                    Vendors + $"prismjs/components/prism-{lowercaseLanguage}.min.js",
                    Vendors + $"prismjs/components/prism-{lowercaseLanguage}.js")
                .SetDependencies(nameof(MarkupTemplating))
                .SetVersion(LibManVersions.Prismjs);
        }
    }
}
