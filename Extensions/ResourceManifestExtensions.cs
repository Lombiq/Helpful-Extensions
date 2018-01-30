using Orchard.UI.Resources;
using Orchard.Validation;

namespace Piedone.HelpfulExtensions
{
    public static class ResourceManifestExtensions
    {
        /// <summary>
        /// Defines a resource with both minified and debug file.
        /// </summary>
        /// <param name="manifest">The ResourceManifest object.</param>
        /// <param name="type">The type of the resource.</param>
        /// <param name="name">The name of the resource</param>
        /// <param name="extensionlessFileName">The matching file name without extension.</param>
        /// <param name="subfolder">The subfolder where the files can be found (optional).</param>
        /// <returns>The resource definition.</returns>
        public static ResourceDefinition DefineMinifiedResourcePair(
            this ResourceManifest manifest, ResourceType type, string name, string extensionlessFileName, string subfolder = "")
        {
            Argument.ThrowIfNullOrEmpty(name, nameof(name));
            Argument.ThrowIfNullOrEmpty(extensionlessFileName, nameof(extensionlessFileName));

            var extension = type == ResourceType.Script ? "js" : "css";
            var subfolderPrefix = string.IsNullOrEmpty(subfolder) ? "" : subfolder.EndsWith("/") ? subfolder : subfolder + "/";

            return manifest
                .DefineResource(type.ToString().ToLower(), name)
                .SetUrl($"{subfolderPrefix}{extensionlessFileName}.min.{extension}", $"{subfolderPrefix}{extensionlessFileName}.{extension}");
        }

        public static ResourceDefinition DefineMinifiedScriptPair(
            this ResourceManifest manifest, string name, string extensionlessFileName, string subfolder = "") =>
            DefineMinifiedResourcePair(manifest, ResourceType.Script, name, extensionlessFileName, subfolder);

        public static ResourceDefinition DefineMinifiedStylePair(
            this ResourceManifest manifest, string name, string extensionlessFileName, string subfolder = "") =>
            DefineMinifiedResourcePair(manifest, ResourceType.StyleSheet, name, extensionlessFileName, subfolder);
    }

    public enum ResourceType
    {
        Script,
        StyleSheet
    }
}