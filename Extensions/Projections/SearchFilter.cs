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
                .Element(nameof(SearchFilter), T("Search filter"), T("Filters for items matching a search query in the site search index."),
                    ApplyFilter,
                    DisplayFilter,
                    nameof(SearchFilter)
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            var values = new SearchFilterFormElements(context.State);

            if (string.IsNullOrEmpty(values.SearchQuery)) return;

            if (values.ExactMatch) values.SearchQuery = values.SearchQuery.Replace(" ", "+");

            var settings = _siteService.GetSiteSettings().As<SearchSettingsPart>();

            var hits = _searchService.Query(values.SearchQuery, 0, values.HitCountLimit == 0 ? null : new int?(values.HitCountLimit), 
                                            settings.FilterCulture, values.Index, SearchSettingsHelper.GetSearchFields(settings, values.Index), 
                                            hit => hit);

            if (hits.Any()) context.Query.WhereIdIn(hits.Select(hit => hit.ContentItemId));
            else context.Query.WhereIdIn(new[] { 0 });
        }

        public LocalizedString DisplayFilter(FilterContext context) =>
            T("Content items matched by the search query {0} in the search index \"{1}\".", context.State.SearchQuery, context.State.Index);
    }


    internal class SearchFilterFormElements
    {
        public string Index { get; set; }
        public string SearchQuery { get; set; }
        public bool ExactMatch { get; set; }
        public int HitCountLimit { get; set; }


        public SearchFilterFormElements(dynamic formState)
        {
            Index = formState[nameof(Index)];
            SearchQuery = formState[nameof(SearchQuery)];

            bool.TryParse(formState[nameof(ExactMatch)]?.ToString(), out bool exactMatch);
            ExactMatch = exactMatch;

            int.TryParse(formState[nameof(HitCountLimit)].ToString(), out int hitCountLimit);
            HitCountLimit = hitCountLimit;
        }
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
            object form(IShapeFactory shape)
            {
                var f = _shapeFactory.Form(
                    Id: nameof(SearchFilterForm),
                    _Index: _shapeFactory.SelectList(
                        Id: nameof(SearchFilterFormElements.Index), Name: nameof(SearchFilterFormElements.Index),
                        Title: T("Index"),
                        Description: T("The selected index will be queried."),
                        Size: 5,
                        Multiple: false),
                    _SearchQuery: _shapeFactory.Textbox(
                        Id: nameof(SearchFilterFormElements.SearchQuery), Name: nameof(SearchFilterFormElements.SearchQuery),
                        Title: T("Search query"),
                        Description: T("The search query to match against."),
                        Classes: new[] { "tokenized" }),
                    _ExactMatch: _shapeFactory.Checkbox(
                        Id: nameof(SearchFilterFormElements.ExactMatch), Name: nameof(SearchFilterFormElements.ExactMatch),
                        Title: T("Exact match"),
                        Checked: false, Value: "true",
                        Description: T("When checked, words in the search query will be processed exactly in order instead of word-by-word.")),
                    _HitCountLimit: _shapeFactory.Textbox(
                        Id: nameof(SearchFilterFormElements.HitCountLimit), Name: nameof(SearchFilterFormElements.HitCountLimit),
                        Title: T("Hit count limit"),
                        Description: T("For performance reasons you can limit the maximal number of search hits used in the query. Having thousands of search hits will result in poor performance and increased load on the database server."))
                    );

                foreach (var index in _indexProvider.List())
                    f._Index.Add(new SelectListItem { Value = index, Text = index });

                return f;
            }

            context.Form(nameof(SearchFilter), form);
        }
    }
}