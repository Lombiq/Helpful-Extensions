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
        /// Creates Taxonomies with the given names.
        /// </summary>
        /// <param name="names">Names of the Taxonomies to create.</param>
        void CreateTaxonomies(params string[] names);

        /// <summary>
        /// Creates a Taxonomy with the given name and adds the Terms to it.
        /// </summary>
        /// <param name="taxonomyName">Name of the Taxonomy to create.</param>
        /// <param name="termNames">Names of the Terms to create and add to the Taxonomy.</param>
        void CreateTaxonomiesWithTerms(string taxonomyName, List<string> termNames);

        /// <summary>
        /// Creates a Taxonomy with the given name and settings and adds the Terms to it.
        /// </summary>
        /// <param name="taxonomyName">Name of the Taxonomy to create.</param>
        /// <param name="termNames">Names of the Terms to create and add to the Taxonomy.</param>
        /// <param name="settings">The settings to add to the Terms of the Taxonomy.</param>
        void CreateTaxonomiesWithTermsAndSettings(string taxonomyName, List<string> termNames, Dictionary<string, string> settings);
    }
}
