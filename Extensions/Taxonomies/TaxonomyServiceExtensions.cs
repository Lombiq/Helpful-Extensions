using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Taxonomies.Models;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.Taxonomies.Services
{
    public static class TaxonomyServiceExtensions
    {
        public static IEnumerable<TermPart> GetTermsUnderParent(
            this ITaxonomyService taxonomyService,
            string taxonomyName,
            TermPart parent) =>
            taxonomyService.GetTermsUnderParent(taxonomyService.GetTaxonomyByName(taxonomyName), parent);

        public static IEnumerable<TermPart> GetTermsUnderParent(
            this ITaxonomyService taxonomyService,
            TaxonomyPart taxonomy,
            TermPart parent)
        {
            if (taxonomy == null || parent == null) return Enumerable.Empty<TermPart>();

            return taxonomyService.GetTermsQuery(taxonomy.Id)
                .Where<CommonPartRecord>(common => common.Container != null && common.Container.Id == taxonomy.Id)
                .List();
        }

        public static IEnumerable<TermPart> GetOrCreateTerms(
            this ITaxonomyService taxonomyService,
            IContentManager contentManager,
            string taxonomyName,
            TermPart parent = null,
            params string[] termNames) =>
            taxonomyService.GetOrCreateTerms(
                contentManager,
                taxonomyService.GetTaxonomyByName(taxonomyName),
                parent,
                termNames);

        public static IEnumerable<TermPart> GetOrCreateTerms(
            this ITaxonomyService taxonomyService,
            IContentManager contentManager,
            TaxonomyPart taxonomy,
            TermPart parent = null,
            params string[] termNames)
        {
            if (taxonomy == null || !termNames.Any(termName => !string.IsNullOrEmpty(termName)))
                return Enumerable.Empty<TermPart>();

            var existingTerms = parent == null ?
                taxonomyService.GetTerms(taxonomy.Id) :
                taxonomyService.GetTermsUnderParent(taxonomy, parent);

            return termNames.Select(termName =>
            {
                var existingTerm = existingTerms.FirstOrDefault(term => term.Name == termName);

                if (existingTerm == null)
                {
                    existingTerm = taxonomyService.NewTerm(taxonomy, parent);
                    existingTerm.Name = termName;
                    contentManager.Create(existingTerm);
                }

                return existingTerm;
            });
        }
    }
}