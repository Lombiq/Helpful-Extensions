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

namespace Piedone.HelpfulExtensions.Extensions.Projections
{
    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class ForeignKeyFilterEditor : IFilterEditor
    {
        public string FormName { get { return ForeignKeyFilterForm.FormName; } }
        public Localizer T { get; set; }


        public ForeignKeyFilterEditor()
        {
            T = NullLocalizer.Instance;
        }


        public bool CanHandle(Type type)
        {
            return type.IsClass;
        }

        public Action<IHqlExpressionFactory> Filter(string property, dynamic formState)
        {
            return e => e.Eq(property + ".Id", int.Parse(formState.Value.ToString()));
        }

        public LocalizedString Display(string property, dynamic formState)
        {
            return T("The foreign key {0} is equal to '{1}'", property, formState.Value);
        }
    }


    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class ForeignKeyFilterForm : IFormProvider
    {
        public const string FormName = "ForeignKeyFilter";

        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }


        public ForeignKeyFilterForm(IShapeFactory shapeFactory)
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
                            Description: T("The value that will be used for the foreign key reference. Only works with integer IDs."))
                );

            context.Form(FormName, form);

        }
    }
}