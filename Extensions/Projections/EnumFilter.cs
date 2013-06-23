using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Projections.FilterEditors;
using Orchard.Projections.ModelBinding;

namespace Piedone.HelpfulExtensions.Projections
{
    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class EnumFilterEditor : IFilterEditor
    {
        public string FormName { get { return EnumFilterForm.FormName; } }
        public Localizer T { get; set; }


        public EnumFilterEditor()
        {
            T = NullLocalizer.Instance;
        }


        public bool CanHandle(Type type)
        {
            return type.IsEnum;
        }

        public Action<IHqlExpressionFactory> Filter(string property, dynamic formState)
        {
            return e => e.Eq(property, formState.Value.ToString());
        }

        public LocalizedString Display(string property, dynamic formState)
        {
            return T("The enum {0} is equal to '{1}'", property, formState.Value);
        }
    }


    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class EnumFilterForm : IFormProvider
    {
        public const string FormName = "EnumFilter";

        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }


        public EnumFilterForm(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form =
                shape =>
                _shapeFactory.Form(
                    _Value: _shapeFactory.TextBox(
                            Id: "value", Name: "Value",
                            Title: T("Value"),
                            Classes: new[] { "textMedium", "tokenized" },
                            Description: T("The value will be parsed to the enum."))
                );

            context.Form(FormName, form);

        }
    }
}