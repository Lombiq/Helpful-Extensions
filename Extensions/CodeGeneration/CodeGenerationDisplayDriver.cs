using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lombiq.HelpfulExtensions.Extensions.CodeGeneration
{
    public class CodeGenerationDisplayDriver : ContentTypeDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypeDefinition contentTypeDefinition) =>
            Initialize<ContentTypeMigrationsViewModel>("ContentTypeMigrations_Edit", model =>
                model.MigrationCodeLazy = new Lazy<string>(() =>
                {
                    var codeBuilder = new StringBuilder();

                    void AddSettingsWithout<T>(JObject settings, int indentationDepth)
                    {
                        var indentation = string.Join("", Enumerable.Repeat(" ", indentationDepth));

                        var filteredSettings = ((IEnumerable<KeyValuePair<string, JToken>>)settings)
                            .Where(setting => setting.Key != typeof(T).Name);
                        foreach (var setting in filteredSettings)
                        {
                            codeBuilder.AppendLine($"{indentation}.WithSettings(new {setting.Key}");
                            codeBuilder.AppendLine(indentation + "{");

                            // This doesn't support multi-level object hierarchies for settings but come on, who uses
                            // complex settings objects?
                            var properties = setting.Value.Where(property => property is JProperty).Cast<JProperty>().ToArray();
                            for (int i = 0; i < properties.Length; i++)
                            {
                                var property = properties[i];
                                var value = ((JValue)property.Value).Value;
                                var valueString = value switch
                                {
                                    bool _ => value.ToString().ToLowerInvariant(),
                                    string _ => $"\"{value}\"",
                                    _ => value.ToString().Replace(',', '.') // Replace decimal commas.
                                };

                                codeBuilder.AppendLine($"{indentation}    {property.Name} = {valueString}{(i != properties.Length - 1 ? "," : string.Empty)}");
                                if (i == properties.Length) codeBuilder.Length--; // Removing the trailing comma.
                            }

                            codeBuilder.AppendLine(indentation + "})");
                        }
                    }

                    // Building the code for the type.
                    codeBuilder.AppendLine($"_contentDefinitionManager.AlterTypeDefinition(\"{contentTypeDefinition.Name}\", type => type");

                    codeBuilder.AppendLine($"    .DisplayedAs(\"{contentTypeDefinition.DisplayName}\")");

                    var contentTypeSettings = contentTypeDefinition.GetSettings<ContentTypeSettings>();
                    if (contentTypeSettings.Creatable) codeBuilder.AppendLine("    .Creatable()");
                    if (contentTypeSettings.Listable) codeBuilder.AppendLine("    .Listable()");
                    if (contentTypeSettings.Draftable) codeBuilder.AppendLine("    .Draftable()");
                    if (contentTypeSettings.Versionable) codeBuilder.AppendLine("    .Versionable()");
                    if (contentTypeSettings.Securable) codeBuilder.AppendLine("    .Securable()");
                    if (!string.IsNullOrEmpty(contentTypeSettings.Stereotype))
                    {
                        codeBuilder.AppendLine($"    .Stereotype(\"{contentTypeSettings.Stereotype}\")");
                    }

                    AddSettingsWithout<ContentTypeSettings>(contentTypeDefinition.Settings, 4);

                    foreach (var part in contentTypeDefinition.Parts)
                    {
                        var partSettings = part.GetSettings<ContentTypePartSettings>();

                        codeBuilder.AppendLine($"    .WithPart(\"{part.Name}\", part => part");

                        var partStartingLength = codeBuilder.Length;

                        if (!string.IsNullOrEmpty(partSettings.DisplayName))
                        {
                            codeBuilder.AppendLine($"        .WithDisplayName(\"{partSettings.DisplayName}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.Description))
                        {
                            codeBuilder.AppendLine($"        .WithDescription(\"{partSettings.Description}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.Position))
                        {
                            codeBuilder.AppendLine($"        .WithPosition(\"{partSettings.Position}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.DisplayMode))
                        {
                            codeBuilder.AppendLine($"        .WithDisplayMode(\"{partSettings.DisplayMode}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.Editor))
                        {
                            codeBuilder.AppendLine($"        .WithEditor(\"{partSettings.Editor}\")");
                        }

                        AddSettingsWithout<ContentTypePartSettings>(part.Settings, 8);

                        // Checking if anything was added to the part's settings.
                        if (codeBuilder.Length == partStartingLength)
                        {
                            // Remove ", part => part" and the line break.
                            codeBuilder.Length -= 16;
                            codeBuilder.Append(")" + Environment.NewLine);
                        }
                        else codeBuilder.AppendLine("    )");
                    }

                    codeBuilder.AppendLine(");");

                    // Building those parts that have fields separately (fields can't be configured inline in types).
                    var partDefinitions = contentTypeDefinition.Parts
                        .Where(part => part.PartDefinition.Fields.Any())
                        .Select(part => part.PartDefinition);
                    foreach (var part in partDefinitions)
                    {
                        codeBuilder.AppendLine($"_contentDefinitionManager.AlterPartDefinition(\"{part.Name}\", part => part");

                        var partSettings = part.GetSettings<ContentPartSettings>();
                        if (partSettings.Attachable) codeBuilder.AppendLine("    .Attachable()");
                        if (partSettings.Reusable) codeBuilder.AppendLine("    .Reusable()");
                        if (!string.IsNullOrEmpty(partSettings.DisplayName))
                        {
                            codeBuilder.AppendLine($"    .WithDisplayName(\"{partSettings.DisplayName}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.Description))
                        {
                            codeBuilder.AppendLine($"    .WithDescription(\"{partSettings.Description}\")");
                        }
                        if (!string.IsNullOrEmpty(partSettings.DefaultPosition))
                        {
                            codeBuilder.AppendLine($"    .WithDefaultPosition(\"{partSettings.DefaultPosition}\")");
                        }

                        AddSettingsWithout<ContentPartSettings>(part.Settings, 4);

                        foreach (var field in part.Fields)
                        {
                            codeBuilder.AppendLine($"    .WithField(\"{field.Name}\", field => field");

                            codeBuilder.AppendLine($"        .OfType(\"{field.FieldDefinition.Name}\")");

                            var fieldSettings = field.GetSettings<ContentPartFieldSettings>();
                            if (!string.IsNullOrEmpty(fieldSettings.DisplayName))
                            {
                                codeBuilder.AppendLine($"        .WithDisplayName(\"{fieldSettings.DisplayName}\")");
                            }
                            if (!string.IsNullOrEmpty(fieldSettings.Description))
                            {
                                codeBuilder.AppendLine($"        .WithDescription(\"{fieldSettings.Description}\")");
                            }
                            if (!string.IsNullOrEmpty(fieldSettings.Editor))
                            {
                                codeBuilder.AppendLine($"        .WithEditor(\"{fieldSettings.Editor}\")");
                            }
                            if (!string.IsNullOrEmpty(fieldSettings.DisplayMode))
                            {
                                codeBuilder.AppendLine($"        .WithDisplayMode(\"{fieldSettings.DisplayMode}\")");
                            }
                            if (!string.IsNullOrEmpty(fieldSettings.Position))
                            {
                                codeBuilder.AppendLine($"        .WithPosition(\"{fieldSettings.Position}\")");
                            }

                            AddSettingsWithout<ContentPartFieldSettings>(field.Settings, 8);

                            codeBuilder.AppendLine("    )");
                        }

                        codeBuilder.AppendLine(");");
                    }

                    return codeBuilder.ToString();
                }))
            .Location("Content:7");
    }
}
