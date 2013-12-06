using System;
using System.Linq;
using Orchard.Core.Common.Models;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Piedone.HelpfulLibraries.Utilities;

namespace Piedone.HelpfulExtensions.Projections
{
    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class IdsInFilter : Orchard.Projections.Services.IFilterProvider
    {
        public Localizer T { get; set; }

        public IdsInFilter()
        {
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeFilterContext describe)
        {
            describe.For("Content", T("Content"), T("Content"))
                .Element("IdsInFilter", T("Ids In filter"), T("Filters for items having the specified ids."),
                    ApplyFilter,
                    DisplayFilter,
                    "IdsInFilter"
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            if (context.State.ContentIds == null) return;

            var ids = ((string)context.State.ContentIds).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsArray = (from p in ids select int.Parse(p.Trim())).ToArray();

            if (idsArray.Length == 0) return;

            context.Query.WhereIdIn(idsArray);
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            return T("Content items having either ids: " + context.State.ContentIds);
        }
    }


    [OrchardFeature("Piedone.HelpfulExtensions.Projections")]
    public class IdsInFilterForms : IFormProvider
    {
        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }

        public IdsInFilterForms(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;

            T = NullLocalizer.Instance;
        }

        public void Describe(Orchard.Forms.Services.DescribeContext context)
        {
            Func<IShapeFactory, object> form =
                shape =>
                {
                    var f = _shapeFactory.Form(
                        Id: "IdsInFilterForm",
                        _Parts: _shapeFactory.Textbox(
                            Id: "ContentIds", Name: "ContentIds",
                            Title: T("Contents Ids"),
                            Description: T("A comma-separated list of the ids of contents to match. Items should have CommonPart attached."),
                            Classes: new[] { "tokenized" })
                        );


                    return f;
                };

            context.Form("IdsInFilter", form);

        }
    }
}
