using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Orchard.Search.Services;
using Piedone.HelpfulLibraries.Utilities;
using Orchard.ContentManagement;
using Orchard.Search.Models;

namespace Piedone.HelpfulExtensions.Extensions.Projections
{
    [OrchardFeature("Piedone.HelpfulExtensions.Projections.Search")]
    public class SearchFilter : Orchard.Projections.Services.IFilterProvider
    {
        private readonly ISearchService _searchService;
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }


        public SearchFilter(ISearchService searchService, IWorkContextAccessor wca)
        {
            _searchService = searchService;
            _wca = wca;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeFilterContext describe)
        {
            describe.For("Search", T("Search"), T("Search"))
                .Element("SearchFilter", T("Search filter"), T("Filters for items matching a search query in the site search index."),
                    ApplyFilter,
                    DisplayFilter,
                    "SearchFilter"
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            string query = context.State.SearchQuery;

            if (string.IsNullOrEmpty(query)) return;

            var settings = _wca.GetContext().CurrentSite.As<SearchSettingsPart>();

            var hits = _searchService.Query(query, 0, null, settings.FilterCulture, settings.SearchIndex, settings.SearchedFields, hit => hit);
            if (hits.Any())
            {
                context.Query.WhereIdIn(hits.Select(hit => hit.ContentItemId));
            }
            else
            {
                context.Query.WhereIdIn(new[] { 0 });
            }
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            return T("Content items matched by the search query {0} in the site search index.", context.State.SearchQuery);
        }
    }

    [OrchardFeature("Piedone.HelpfulExtensions.Projections.Search")]
    public class ContentTypesFilterForms : IFormProvider
    {
        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }


        public ContentTypesFilterForms(IShapeFactory shapeFactory)
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
                        Id: "SearchFilterForm",
                        _Parts: _shapeFactory.Textbox(
                            Id: "SearchQuery", Name: "SearchQuery",
                            Title: T("Search query"),
                            Description: T("The search query to match against."),
                            Classes: new[] { "tokenized" })
                        );


                    return f;
                };

            context.Form("SearchFilter", form);
        }
    }
}