using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Fields.Settings;
using Orchard.Indexing.Settings;

namespace Piedone.HelpfulExtensions
{
    public static class ContentPartFieldDefinitionBuilderExtensions
    {
        public static ContentPartFieldDefinitionBuilder WithFieldIndexingIncluded(this ContentPartFieldDefinitionBuilder builder) =>
            builder.WithSetting($"{nameof(FieldIndexing)}.{nameof(FieldIndexing.Included)}", "True");


        public static ContentPartFieldDefinitionBuilder WithInputFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            InputFieldSettings settings)
        {
            var prefix = $"{nameof(InputFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(InputFieldSettings.AutoComplete), settings.AutoComplete.ToString())
                .WithSetting(prefix + nameof(InputFieldSettings.AutoFocus), settings.AutoFocus.ToString())
                .WithSetting(prefix + nameof(InputFieldSettings.DefaultValue), settings.DefaultValue)
                .WithSetting(prefix + nameof(InputFieldSettings.EditorCssClass), settings.EditorCssClass)
                .WithSetting(prefix + nameof(InputFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(InputFieldSettings.MaxLength), settings.MaxLength.ToString())
                .WithSetting(prefix + nameof(InputFieldSettings.Pattern), settings.Pattern)
                .WithSetting(prefix + nameof(InputFieldSettings.Placeholder), settings.Placeholder)
                .WithSetting(prefix + nameof(InputFieldSettings.Required), settings.Required.ToString())
                .WithSetting(prefix + nameof(InputFieldSettings.Title), settings.Title)
                .WithSetting(prefix + nameof(InputFieldSettings.Type), settings.Type.ToString());
        }
    }
}