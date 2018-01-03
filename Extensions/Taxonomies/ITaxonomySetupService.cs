using Orchard;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    /// <summary>
    /// Service for creating Taxonomies during setup.
    /// </summary>
    public interface ITaxonomySetupService : IDependency
    {
        /// <summary>
        /// Create Taxonomies with the given names.
        /// </summary>
        /// <param name="names">Names of the Taxonomies to create.</param>
        void CreateTaxonomies(params string[] names);

        /// <summary>
        /// Creates a Taxonomy with the given name and adding the terms to it.
        /// </summary>
        /// <param name="taxonomyName">Names of the Taxonomy to create.</param>
        /// <param name="termNames">Names of the Terms to create and add to the Taxonomy.</param>
        void CreateTaxonomiesWithTerms(string taxonomyName, List<string> termNames);
    }
}
