using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Manifest;
using OrchardCore.DisplayManagement.Theming;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lombiq.HelpfulLibraries.OrchardCore.ResourceManagement;
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

    public void RenderStylesheet(TextWriter writer)
    {
        var first = true;

        var styleSheets = GetRequiredResources("stylesheet").ToList();
        var displayedTheme = themeManager.GetThemeAsync().GetAwaiter().GetResult();

        foreach (var context in styleSheets)
        {
            if (context.Settings.Location == ResourceLocation.Inline)
            {
                continue;
            }

            var resourceName = context.Resource.Name;
            var contextIsBootstrap = resourceName.EqualsOrdinalIgnoreCase("bootstrap");

            if (contextIsBootstrap && IsCurrentTheme(displayedTheme))
            {
                continue;
            }

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

    private static bool IsCurrentTheme(IExtensionInfo currentSiteTheme) =>
        currentSiteTheme?.Id == "Lombiq.BaseTheme" ||
        currentSiteTheme?.Manifest.ModuleInfo is ThemeAttribute { BaseTheme: "Lombiq.BaseTheme" };
}
