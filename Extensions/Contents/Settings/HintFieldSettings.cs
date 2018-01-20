using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Environment.Extensions;
using Piedone.HelpfulExtensions.Contents.Models;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Contents.Settings
{
    [OrchardFeature(Constants.FeatureNames.Contents)]
    public class HintFieldSettings
    {
        public string Hint { get; set; }
    }

    [OrchardFeature(Constants.FeatureNames.Contents)]
    public class HintFieldEditorEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> PartFieldEditor(ContentPartFieldDefinition definition)
        {
            if (definition.FieldDefinition.Name == typeof(HintField).Name)
            {
                var model = definition.Settings.GetModel<HintFieldSettings>();
                yield return DefinitionTemplate(model);
            }
        }

        public override IEnumerable<TemplateViewModel> PartFieldEditorUpdate(ContentPartFieldDefinitionBuilder builder, IUpdateModel updateModel)
        {
            if (builder.FieldType != "HintField")
            {
                yield break;
            }

            var model = new HintFieldSettings();
            if (updateModel.TryUpdateModel(model, "HintFieldSettings", null, null))
            {
                builder.WithSetting("HintFieldSettings.Hint", model.Hint);

                yield return DefinitionTemplate(model);
            }
        }
    }
}