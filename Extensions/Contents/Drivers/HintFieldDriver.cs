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
            return ContentShape("Fields_Hint",
                GetDifferentiator(field, part),
                () => shapeHelper.Fields_Hint(Hint: field.PartFieldDefinition.Settings.GetModel<HintFieldSettings>().Hint));
        }

        protected override DriverResult Editor(ContentPart part, HintField field, dynamic shapeHelper)
        {
            return Display(part, field, string.Empty, shapeHelper);
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