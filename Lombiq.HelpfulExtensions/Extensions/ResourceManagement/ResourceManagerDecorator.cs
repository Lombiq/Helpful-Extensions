using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Manifest;
using OrchardCore.DisplayManagement.Theming;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;

/// <summary>
/// This decorator is replacing the old ReplaceBootstrapMiddleware with the same purpose.
/// Removes the built-in Bootstrap resource if the currently selected theme uses Lombiq.BaseTheme as its base theme.
/// Themes derived from Lombiq.BaseTheme use Bootstrap from NPM as it's compiled into their site stylesheet. So this
/// duplicate resource is not needed and can cause problems if not removed. This situation can arise when a module
/// (such as Lombiq.DataTables) depends on Bootstrap and doesn't explicitly depend on Lombiq.BaseTheme so the built-in
/// resource would be injected if this middleware didn't remove it.
/// </summary>
public class ResourceManagerDecorator(
    IResourceManager resourceManager,
    IThemeManager themeManager,
    IOptions<ResourceManagementOptions> resourceManagementOptions) : IResourceManager
{
    private readonly ResourceManagementOptions _options = resourceManagementOptions.Value;

    public ResourceManifest InlineManifest => resourceManager.InlineManifest;

    public void AppendMeta(MetaEntry meta, string contentSeparator) => resourceManager.AppendMeta(meta, contentSeparator);

    public ResourceDefinition FindResource(RequireSettings settings) => resourceManager.FindResource(settings);

    public IEnumerable<IHtmlContent> GetRegisteredFootScripts() => resourceManager.GetRegisteredFootScripts();

    public IEnumerable<IHtmlContent> GetRegisteredHeadScripts() => resourceManager.GetRegisteredHeadScripts();

    public IEnumerable<LinkEntry> GetRegisteredLinks() => resourceManager.GetRegisteredLinks();

    public IEnumerable<MetaEntry> GetRegisteredMetas() => resourceManager.GetRegisteredMetas();

    public IEnumerable<IHtmlContent> GetRegisteredStyles() => resourceManager.GetRegisteredStyles();

    public IEnumerable<ResourceRequiredContext> GetRequiredResources(string resourceType) =>
        resourceManager.GetRequiredResources(resourceType);

    public void NotRequired(string resourceType, string resourceName) =>
        resourceManager.NotRequired(resourceType, resourceName);

    public void RegisterFootScript(IHtmlContent script) => resourceManager.RegisterFootScript(script);

    public void RegisterHeadScript(IHtmlContent script) => resourceManager.RegisterHeadScript(script);

    public void RegisterLink(LinkEntry link) => resourceManager.RegisterLink(link);

    public void RegisterMeta(MetaEntry meta) => resourceManager.RegisterMeta(meta);

    public RequireSettings RegisterResource(string resourceType, string resourceName) =>
        resourceManager.RegisterResource(resourceType, resourceName);

    public void RegisterStyle(IHtmlContent style) => resourceManager.RegisterStyle(style);

    public RequireSettings RegisterUrl(string resourceType, string resourcePath, string resourceDebugPath) =>
        resourceManager.RegisterUrl(resourceType, resourcePath, resourceDebugPath);

    public void RenderFootScript(TextWriter writer) => resourceManager.RenderFootScript(writer);

    public void RenderHeadLink(TextWriter writer) => resourceManager.RenderHeadLink(writer);

    public void RenderHeadScript(TextWriter writer) => resourceManager.RenderHeadScript(writer);

    public void RenderLocalScript(RequireSettings settings, TextWriter writer) => resourceManager.RenderLocalScript(settings, writer);

    public void RenderLocalStyle(RequireSettings settings, TextWriter writer) => resourceManager.RenderLocalStyle(settings, writer);

    public void RenderMeta(TextWriter writer) => resourceManager.RenderMeta(writer);

    // Apart from the marked sections, this does the same as the method in OC's ResourceManager. This needs to be kept
    // up-to-date with Orchard upgrades.
    [SuppressMessage(
        "StyleCop.CSharp.ReadabilityRules",
        "SA1123:Do not place regions within elements",
        Justification = "Needed for easier Orchard upgrades.")]
    public void RenderStylesheet(TextWriter writer)
    {
        #region CustomCode
        var displayedTheme = themeManager.GetThemeAsync().GetAwaiter().GetResult();
        var currentThemeUsesLombiqBaseTheme = displayedTheme?.Id == "Lombiq.BaseTheme" ||
            displayedTheme?.Manifest.ModuleInfo is ThemeAttribute { BaseTheme: "Lombiq.BaseTheme" };
        #endregion

        var first = true;

        var styleSheets = GetRequiredResources("stylesheet").ToList();

        foreach (var context in styleSheets)
        {
            if (context.Settings.Location == ResourceLocation.Inline)
            {
                continue;
            }

            #region CustomCode
            if (context.Resource.Name.EqualsOrdinalIgnoreCase("bootstrap") && currentThemeUsesLombiqBaseTheme)
            {
                continue;
            }
            #endregion

            if (!first)
            {
                writer.Write(Environment.NewLine);
            }

            first = false;

            context.WriteTo(writer, _options.ContentBasePath);
        }

        var registeredStyles = GetRegisteredStyles().ToArray();
        for (var i = 0; i < registeredStyles.Length; i++)
        {
            var context = registeredStyles[i];
            if (!first)
            {
                writer.Write(Environment.NewLine);
            }

            first = false;

            context.WriteTo(writer, NullHtmlEncoder.Default);
        }
    }
}
