using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Common.Fields;
using Orchard.Fields.Fields;
using System;

namespace Piedone.HelpfulExtensions
{
    public static class ContentPartDefinitionBuilderExtensions
    {
        public static ContentPartDefinitionBuilder WithField(
            this ContentPartDefinitionBuilder builder,
            string fieldType,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(fieldName, fieldDefinitionBuilder =>
            {
                fieldDefinitionBuilder.OfType(fieldType);

                configuration?.Invoke(fieldDefinitionBuilder);
            });

        public static ContentPartDefinitionBuilder WithInputField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(nameof(InputField), fieldName, configuration);

        public static ContentPartDefinitionBuilder WithTextField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(nameof(TextField), fieldName, configuration);

        public static ContentPartDefinitionBuilder WithBooleanField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(nameof(BooleanField), fieldName, configuration);

        public static ContentPartDefinitionBuilder WithDateTimeField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(nameof(DateTimeField), fieldName, configuration);
    }
}