using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Core.Contents.Settings;
using Orchard.Environment.Extensions;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Piedone.HelpfulExtensions.CodeGeneration
{
    [OrchardFeature(Constants.FeatureNames.CodeGeneration)]
    public class ContentTypeEditorEvents : ContentDefinitionEditorEventsBase, IDisposable
    {
        private CodeDomProvider _codeDomProvider;
        public CodeDomProvider CodeDomProvider
        {
            get
            {
                if (_codeDomProvider == null)
                {
                    _codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
                }

                return _codeDomProvider;
            }
        }


        public override IEnumerable<TemplateViewModel> TypeEditor(ContentTypeDefinition definition)
        {
            var typeSettings = definition.Settings.GetModel<ContentTypeSettings>();

            // The funny indentations below are so the displayed code looks good.

            // First generating the type definition.
            var generatedCode =
@"ContentDefinitionManager.AlterTypeDefinition(""" + definition.Name + @""",
    type => type";

            if (typeSettings.Creatable)
            {
                generatedCode += @"
        .Creatable()";
            }
            if (typeSettings.Draftable)
            {
                generatedCode += @"
        .Draftable()";
            }
            if (typeSettings.Listable)
            {
                generatedCode += @"
        .Listable()";
            }
            if (typeSettings.Securable)
            {
                generatedCode += @"
        .Securable()";
            }

            generatedCode += @"
        .DisplayedAs(""" + definition.DisplayName + "\")";

            var otherTypeSettings = definition.Settings.Where(s => !s.Key.StartsWith(typeof(ContentTypeSettings).Name + "."));
            foreach (var typeSetting in otherTypeSettings)
            {
                generatedCode += @"
        .WithSetting(""" + typeSetting.Key + "\", " + ToLiteral(typeSetting.Value) + ")";
            }

            foreach (var part in definition.Parts)
            {
                if (part.Settings.Any())
                {
                    generatedCode += @"
        .WithPart(""" + part.PartDefinition.Name + @""",
            part => part";

                    foreach (var typePartSetting in part.Settings)
                    {
                        generatedCode += @"
                .WithSetting(""" + typePartSetting.Key + "\", " + ToLiteral(typePartSetting.Value) + ")";
                    }

                    foreach (var field in part.PartDefinition.Fields)
                    {

                    }

                    generatedCode += @"
            )";
                }
                else
                {
                    generatedCode += @"
        .WithPart(""" + part.PartDefinition.Name + "\")";
                }
            }

            generatedCode += @"
    );";

            // Then generating the necessary part definitions.
            foreach (var part in definition.Parts)
            {
                if (!part.PartDefinition.Settings.Any() && !part.PartDefinition.Fields.Any())
                {
                    continue;
                }

                generatedCode +=
@"

ContentDefinitionManager.AlterPartDefinition(""" + part.PartDefinition.Name + @""",
    part => part";

                foreach (var partSetting in part.PartDefinition.Settings)
                {
                    generatedCode += @"
        .WithSetting(""" + partSetting.Key + "\", " + ToLiteral(partSetting.Value) + ")";
                }

                foreach (var field in part.PartDefinition.Fields)
                {
                    generatedCode += @"
        .WithField(""" + field.Name + @""",
            field => field
                .OfType(""" + field.FieldDefinition.Name + @""")
                .WithDisplayName(""" + field.DisplayName + @""")";

                    foreach (var fieldSetting in field.Settings.Where(setting => setting.Key != "DisplayName"))
                    {
                        generatedCode += @"
                .WithSetting(""" + fieldSetting.Key + "\", " + ToLiteral(fieldSetting.Value) + ")";
                    }

                    generatedCode += @"
            )";
                }

                generatedCode += @"
    );";
            }

            yield return DefinitionTemplate(
                new ContentTypeMigrationsViewModel { MigrationCode = generatedCode },
                "ContentTypeMigrations",
                typeof(ContentTypeMigrationsViewModel).Name);
        }

        public void Dispose()
        {
            if (_codeDomProvider == null) return;

            _codeDomProvider.Dispose();
            _codeDomProvider = null;
        }


        private string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                var options = new CodeGeneratorOptions { IndentString = string.Empty };
                CodeDomProvider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, options);
                return writer.ToString().Replace("\" +" + Environment.NewLine + "\"", "");
            }
        }
    }
}