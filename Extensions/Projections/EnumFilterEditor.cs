using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Projections.FilterEditors;
using System;
using System.Web.Mvc;

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
            string op = formState.Operator.ToString();
            string value = formState.Value.ToString();
            bool.TryParse(formState.DoNotFilterIfEmpty?.ToString(), out bool doNotfilterIfEmpty);

            if (doNotfilterIfEmpty && string.IsNullOrEmpty(value)) return e => e.Not(inner => inner.Eq(property, value));

            if (op == "NotEquals") return e => e.Not(inner => inner.Eq(property, value));
            return e => e.Eq(property, value);
        }

        public LocalizedString Display(string property, dynamic formState)
        {
            string op = formState.Operator.ToString();
            if (op == "NotEquals") return T("The enum {0} is not equal to '{1}'", property, formState.Value);
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
                {
                    var f = _shapeFactory.Form(
                            _Operator: _shapeFactory.SelectList(
                                    Id: "Operator", Name: "Operator",
                                    Title: T("Operator"),
                                    Description: T("The operator to use with the value."),
                                    Size: 1,
                                    Multiple: false),
                            _Value: _shapeFactory.TextBox(
                                    Id: "value", Name: "Value",
                                    Title: T("Value"),
                                    Classes: new[] { "textMedium", "tokenized" },
                                    Description: T("The value will be parsed to the enum.")),
                            _DoNotFilterIfEmpty: _shapeFactory.Checkbox(
                                    Id: "doNotfilterIfEmpty", Name: "DoNotFilterIfEmpty",
                                    Title: T("Do not filter if empty"),
                                    Checked: false, Value: "true",
                                    Description: T("When checked, do not filter if no value provided.")));

                    f._Operator.Add(new SelectListItem { Value = "Equals", Text = T("Equals").Text });
                    f._Operator.Add(new SelectListItem { Value = "NotEquals", Text = T("Not Equals").Text });

                    return f;
                };

            context.Form(FormName, form);

        }
    }
}