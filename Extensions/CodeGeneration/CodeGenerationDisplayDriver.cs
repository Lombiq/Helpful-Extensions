using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Lombiq.HelpfulExtensions.Extensions.CodeGeneration;

public class CodeGenerationDisplayDriver : ContentTypeDefinitionDisplayDriver
{
    private readonly IStringLocalizer T;

    public CodeGenerationDisplayDriver(IStringLocalizer<CodeGenerationDisplayDriver> stringLocalizer) =>
        T = stringLocalizer;

    public override IDisplayResult Edit(ContentTypeDefinition model) =>
        Initialize<ContentTypeMigrationsViewModel>(
            "ContentTypeMigrations_Edit",
            viewModel => viewModel.MigrationCodeLazy = new Lazy<string>(() =>
            {
                var codeBuilder = new StringBuilder();

                // Building the code for the type.
                var name = model.Name;

                // This would be great in a Helpful Libraries extension method but unless we construct and manage an
                // StringBuilder.AppendInterpolatedStringHandler instance by hand (to be able to use pass on a
                // FormattableString received from here to StringBuilder.AppendLine(IFormatProvider? provider, ref
                // AppendInterpolatedStringHandler handler)) it won't work.
                codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"_contentDefinitionManager.AlterTypeDefinition(\"{name}\", type => type");
                codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"    .DisplayedAs(\"{model.DisplayName}\")");

                GenerateCodeForSettings(codeBuilder, model.GetSettings<ContentTypeSettings>());
                AddSettingsWithout<ContentTypeSettings>(codeBuilder, model.Settings, 4);
                GenerateCodeForParts(codeBuilder, model.Parts);
                codeBuilder.AppendLine(");");

                GenerateCodeForPartsWithFields(codeBuilder, model.Parts);

                return codeBuilder.ToString();
            }))
        .PlaceInContent(7);

    private void GenerateCodeForParts(StringBuilder codeBuilder, IEnumerable<ContentTypePartDefinition> parts)
    {
        foreach (var part in parts)
        {
            var partSettings = part.GetSettings<ContentTypePartSettings>();

            codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"    .WithPart(\"{part.Name}\", part => part");

            var partStartingLength = codeBuilder.Length;

            AddWithLine(codeBuilder, nameof(partSettings.DisplayName), partSettings.DisplayName);
            AddWithLine(codeBuilder, nameof(partSettings.Description), partSettings.Description);
            AddWithLine(codeBuilder, nameof(partSettings.Position), partSettings.Position);
            AddWithLine(codeBuilder, nameof(partSettings.DisplayMode), partSettings.DisplayMode);
            AddWithLine(codeBuilder, nameof(partSettings.Editor), partSettings.Editor);

            AddSettingsWithout<ContentTypePartSettings>(codeBuilder, part.Settings, 8);

            // Checking if anything was added to the part's settings.
            if (codeBuilder.Length == partStartingLength)
            {
                // Remove ", part => part" and the line break.
                codeBuilder.Length -= 16;
                codeBuilder.Append(")" + Environment.NewLine);
            }
            else
            {
                codeBuilder.AppendLine("    )");
            }
        }
    }

    /// <summary>
    /// Building those parts that have fields separately (fields can't be configured inline in types).
    /// </summary>
    private void GenerateCodeForPartsWithFields(
        StringBuilder codeBuilder,
        IEnumerable<ContentTypePartDefinition> parts)
    {
        var partDefinitions = parts
            .Where(part => part.PartDefinition.Fields.Any())
            .Select(part => part.PartDefinition);
        foreach (var part in partDefinitions)
        {
            codeBuilder.AppendLine();
            codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"_contentDefinitionManager.AlterPartDefinition(\"{part.Name}\", part => part");

            var partSettings = part.GetSettings<ContentPartSettings>();
            if (partSettings.Attachable) codeBuilder.AppendLine("    .Attachable()");
            if (partSettings.Reusable) codeBuilder.AppendLine("    .Reusable()");

            AddWithLine(codeBuilder, nameof(partSettings.DisplayName), partSettings.DisplayName);
            AddWithLine(codeBuilder, nameof(partSettings.Description), partSettings.Description);
            AddWithLine(codeBuilder, nameof(partSettings.DefaultPosition), partSettings.DefaultPosition);

            AddSettingsWithout<ContentPartSettings>(codeBuilder, part.Settings, 4);

            foreach (var field in part.Fields)
            {
                codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"    .WithField(\"{field.Name}\", field => field");
                codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"        .OfType(\"{field.FieldDefinition.Name}\")");

                var fieldSettings = field.GetSettings<ContentPartFieldSettings>();
                AddWithLine(codeBuilder, nameof(fieldSettings.DisplayName), fieldSettings.DisplayName);
                AddWithLine(codeBuilder, nameof(fieldSettings.Description), fieldSettings.Description);
                AddWithLine(codeBuilder, nameof(fieldSettings.Editor), fieldSettings.Editor);
                AddWithLine(codeBuilder, nameof(fieldSettings.DisplayMode), fieldSettings.DisplayMode);
                AddWithLine(codeBuilder, nameof(fieldSettings.Position), fieldSettings.Position);

                AddSettingsWithout<ContentPartFieldSettings>(codeBuilder, field.Settings, 8);

                codeBuilder.AppendLine("    )");
            }

            codeBuilder.AppendLine(");");
        }
    }

    private string ConvertJToken(JToken jToken, int indentationDepth)
    {
        switch (jToken)
        {
            case JValue jValue:
                var value = jValue.Value;
                return value switch
                {
                    bool boolValue => boolValue ? "true" : "false",
                    string => $"\"{value}\"",
                    _ => value?.ToString()?.Replace(',', '.'), // Replace decimal commas.
                };
            case JArray jArray:
                return ConvertJArray(jArray, indentationDepth);
            case JObject jObject:
                return ConvertJObject(jObject, indentationDepth);
            default:
                throw new NotSupportedException($"Settings values of type {jToken.GetType()} are not supported.");
        }
    }

    private string ConvertJArray(JArray jArray, int indentationDepth)
    {
        var indentation = new string(' ', indentationDepth + 4);

        var items = jArray.Select(item => ConvertJToken(item, indentationDepth + 8)).ToList();

        // If the items are formatted (for ListValueOption) then don't inject line-by-line formatting.
        if (items.Any(item => item.ContainsOrdinalIgnoreCase(Environment.NewLine)))
        {
            var token = string.Join(string.Empty, items);
            return $"new[]\n{indentation}{{\n{token}{indentation}}}";
        }

        // Otherwise, make sure that we have proper formatting for string arrays.
        var stringArrayCodeBuilder = new StringBuilder("new[]");
        stringArrayCodeBuilder.AppendLine();
        stringArrayCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{indentation}{{");

        var itemIndentation = new string(' ', indentationDepth + 8);

        foreach (var item in items)
        {
            stringArrayCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{itemIndentation}{item},");
        }

        stringArrayCodeBuilder.Append(CultureInfo.InvariantCulture, $"{indentation}}}");

        return stringArrayCodeBuilder.ToString();
    }

    private string ConvertJObject(JObject jObject, int indentationDepth)
    {
        var braceIndentation = new string(' ', indentationDepth);
        var propertyIndentation = new string(' ', indentationDepth + 4);
        if (jObject["name"] != null && jObject["value"] != null)
        {
            var objectCodeBuilder = new StringBuilder();
            objectCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{braceIndentation}new ListValueOption");
            objectCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{braceIndentation}{{");
            objectCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{propertyIndentation}Name = \"{jObject["name"]}\",");
            objectCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{propertyIndentation}Value = \"{jObject["value"]}\",");
            objectCodeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{braceIndentation}}},");

            return objectCodeBuilder.ToString();
        }

        // Using a quoted string so it doesn't mess up the syntax highlighting of the rest of the code.
        return T["\"FIX ME! Couldn't determine the actual type to instantiate.\" {0}", jObject.ToString()];
    }

    private void AddSettingsWithout<T>(StringBuilder codeBuilder, JObject settings, int indentationDepth)
    {
        var indentation = new string(' ', indentationDepth);

        var filteredSettings = ((IEnumerable<KeyValuePair<string, JToken>>)settings)
            .Where(setting => setting.Key != typeof(T).Name);

        foreach (var setting in filteredSettings)
        {
            var properties = setting.Value.Where(property => property is JProperty).Cast<JProperty>().ToArray();

            if (properties.Length == 0) continue;

            codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{indentation}.WithSettings(new {setting.Key}");
            codeBuilder.AppendLine(indentation + "{");

            // This doesn't support multi-level object hierarchies for settings but come on, who uses complex settings
            // objects?
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];

                var propertyValue = ConvertJToken(property.Value, indentationDepth);

                propertyValue ??= "\"\"";

                codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"{indentation}    {property.Name} = {propertyValue},");
            }

            codeBuilder.AppendLine(indentation + "})");
        }
    }

    private static void GenerateCodeForSettings(StringBuilder codeBuilder, ContentTypeSettings contentTypeSettings)
    {
        if (contentTypeSettings.Creatable) codeBuilder.AppendLine("    .Creatable()");
        if (contentTypeSettings.Listable) codeBuilder.AppendLine("    .Listable()");
        if (contentTypeSettings.Draftable) codeBuilder.AppendLine("    .Draftable()");
        if (contentTypeSettings.Versionable) codeBuilder.AppendLine("    .Versionable()");
        if (contentTypeSettings.Securable) codeBuilder.AppendLine("    .Securable()");
        if (!string.IsNullOrEmpty(contentTypeSettings.Stereotype))
        {
            codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"    .Stereotype(\"{contentTypeSettings.Stereotype}\")");
        }
    }

    private static void AddWithLine(StringBuilder codeBuilder, string name, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            codeBuilder.AppendLine(CultureInfo.InvariantCulture, $"        .With{name}(\"{value}\")");
        }
    }
}
