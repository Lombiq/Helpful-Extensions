using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Piedone.HelpfulExtensions.Contents.Models;
using Piedone.HelpfulExtensions.Contents.Settings;

namespace Piedone.HelpfulExtensions.Contents.Drivers
{
    [OrchardFeature("Piedone.HelpfulExtensions.Contents")]
    public class HintFieldDriver : ContentFieldDriver<HintField>
    {
        public Localizer T { get; set; }


        public HintFieldDriver()
        {
            T = NullLocalizer.Instance;
        }


        protected override DriverResult Display(ContentPart part, HintField field, string displayType, dynamic shapeHelper)
        {
            return CreateResult("Fields_Hint", part, field, shapeHelper);
        }

        protected override DriverResult Editor(ContentPart part, HintField field, dynamic shapeHelper)
        {
            return CreateResult("Fields_Hint_Edit", part, field, shapeHelper);
        }


        private DriverResult CreateResult(string shapeType, ContentPart part, HintField field, dynamic shapeHelper)
        {
            return ContentShape(shapeType,
                GetDifferentiator(field, part),
                () => shapeHelper.Fields_Hint(Hint: field.PartFieldDefinition.Settings.GetModel<HintFieldSettings>().Hint));
        }

        private static string GetPrefix(ContentField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(ContentField field, ContentPart part)
        {
            return field.Name;
        }
    }
}