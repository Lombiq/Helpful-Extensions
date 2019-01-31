using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Common.Settings;
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

        public static ContentPartFieldDefinitionBuilder WithTextFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            TextFieldSettings settings)
        {
            var prefix = $"{nameof(TextFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(TextFieldSettings.Flavor), settings.Flavor)
                .WithSetting(prefix + nameof(TextFieldSettings.Required), settings.Required.ToString())
                .WithSetting(prefix + nameof(TextFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(TextFieldSettings.Placeholder), settings.Placeholder)
                .WithSetting(prefix + nameof(TextFieldSettings.DefaultValue), settings.DefaultValue);
        }

        public static ContentPartFieldDefinitionBuilder WithBooleanFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            BooleanFieldSettings settings)
        {
            var prefix = $"{nameof(BooleanFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(BooleanFieldSettings.DefaultValue), settings.DefaultValue.ToString())
                .WithSetting(prefix + nameof(BooleanFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(BooleanFieldSettings.NotSetLabel), settings.NotSetLabel)
                .WithSetting(prefix + nameof(BooleanFieldSettings.OffLabel), settings.OffLabel)
                .WithSetting(prefix + nameof(BooleanFieldSettings.OnLabel), settings.OnLabel)
                .WithSetting(prefix + nameof(BooleanFieldSettings.Optional), settings.Optional.ToString())
                .WithSetting(prefix + nameof(BooleanFieldSettings.SelectionMode), settings.SelectionMode.ToString());
        }

        public static ContentPartFieldDefinitionBuilder WithDateTimeFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            DateTimeFieldSettings settings)
        {
            var prefix = $"{nameof(DateTimeFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(DateTimeFieldSettings.DefaultValue), settings.DefaultValue.ToString())
                .WithSetting(prefix + nameof(DateTimeFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(DateTimeFieldSettings.Display), settings.Display.ToString())
                .WithSetting(prefix + nameof(DateTimeFieldSettings.Required), settings.Required.ToString())
                .WithSetting(prefix + nameof(DateTimeFieldSettings.DatePlaceholder), settings.DatePlaceholder)
                .WithSetting(prefix + nameof(DateTimeFieldSettings.TimePlaceholder), settings.TimePlaceholder);
        }

        public static ContentPartFieldDefinitionBuilder WithEnumerationFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            EnumerationFieldSettings settings)
        {
            var prefix = $"{nameof(EnumerationFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(EnumerationFieldSettings.DefaultValue), settings.DefaultValue.ToString())
                .WithSetting(prefix + nameof(EnumerationFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(EnumerationFieldSettings.Required), settings.Required.ToString())
                .WithSetting(prefix + nameof(EnumerationFieldSettings.ListMode), settings.ListMode.ToString())
                .WithSetting(prefix + nameof(EnumerationFieldSettings.Options), settings.Options);
        }
    }
}