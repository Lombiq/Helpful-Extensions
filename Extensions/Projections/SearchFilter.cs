using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Forms.Services;
using Orchard.Indexing;
using Orchard.Localization;
using Orchard.Projections.Descriptors.Filter;
using Orchard.Search.Helpers;
using Orchard.Search.Models;
using Orchard.Search.Services;
using Orchard.Settings;
using Piedone.HelpfulLibraries.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Piedone.HelpfulExtensions.Extensions.Projections
{
    [OrchardFeature("Piedone.HelpfulExtensions.Projections.Search")]
    public class SearchFilter : Orchard.Projections.Services.IFilterProvider
    {
        private readonly ISearchService _searchService;
        private readonly ISiteService _siteService;

        public Localizer T { get; set; }


        public SearchFilter(ISearchService searchService, ISiteService siteService)
        {
            _searchService = searchService;
            _siteService = siteService;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeFilterContext describe)
        {
            describe.For("Search", T("Search"), T("Search"))
                .Element("SearchFilter", T("Search filter"), T("Filters for items matching a search query in the site search index."),
                    ApplyFilter,
                    DisplayFilter,
                    nameof(SearchFilter)
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            string query = context.State.SearchQuery;

            if (string.IsNullOrEmpty(query)) return;

            var settings = _siteService.GetSiteSettings().As<SearchSettingsPart>();
            string index = context.State.Index;

            var hitCountLimit = 0;
            int.TryParse(context.State.HitCountLimit.ToString(), out hitCountLimit);

            var hits = _searchService.Query(query, 0, hitCountLimit != 0 ? new int?(hitCountLimit) : null, 
                                            settings.FilterCulture, index, SearchSettingsHelper.GetSearchFields(settings, index), 
                                            hit => hit);

            if (hits.Any()) context.Query.WhereIdIn(hits.Select(hit => hit.ContentItemId));
            else context.Query.WhereIdIn(new[] { 0 });
        }

        public LocalizedString DisplayFilter(FilterContext context) =>
            T("Content items matched by the search query {0} in the search index \"{1}\".", context.State.SearchQuery, context.State.Index);
    }


    [OrchardFeature("Piedone.HelpfulExtensions.Projections.Search")]
    public class SearchFilterForm : IFormProvider
    {
        private readonly IIndexProvider _indexProvider;
        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }


        public SearchFilterForm(IShapeFactory shapeFactory, IIndexProvider indexProvider)
        {
            _shapeFactory = shapeFactory;
            _indexProvider = indexProvider;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form = shape =>
            {
                var f = _shapeFactory.Form(
                    Id: nameof(SearchFilterForm),
                    _Index: _shapeFactory.SelectList(
                        Id: "Index", Name: "Index",
                        Title: T("Index"),
                        Description: T("The selected index will be queried."),
                        Size: 5,
                        Multiple: false),
                    _SearchQuery: _shapeFactory.Textbox(
                        Id: "SearchQuery", Name: "SearchQuery",
                        Title: T("Search query"),
                        Description: T("The search query to match against."),
                        Classes: new[] { "tokenized" }),
                    _HitCountLimit: _shapeFactory.Textbox(
                        Id: "HitCountLimit", Name: "HitCountLimit",
                        Title: T("Hit count limit"),
                        Description: T("For performance reasons you can limit the maximal number of search hits used in the query. Having thousands of search hits will result in poor performance and increased load on the database server."))
                    );

                foreach (var index in _indexProvider.List())
                    f._Index.Add(new SelectListItem { Value = index, Text = index });

                return f;
            };

            context.Form(nameof(SearchFilter), form);
        }
    }
}