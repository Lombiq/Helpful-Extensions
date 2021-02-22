using Orchard.Events;
using System.Collections.Generic;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    /// <summary>
    /// Handles Taxonomy setup in a deferred way, through <see cref="Orchard.Environment.State.IProcessingEngine"/>.
    /// </summary>
    /// <remarks>Works together with <see cref="ITaxonomySetupService"/>.</remarks>
    public interface ITaxonomySetupHandler : IEventHandler
    {
        /// <summary>
        /// Applies the previously invoked creation of Taxonomies.
        /// </summary>
        /// <param name="names">Names of the Taxonomies to create.</param>
        void ApplyTaxonomiesCreation(string[] names);

        /// <summary>
        /// Applies the previously invoked creation of Taxonomy with Terms.
        /// </summary>
        /// <param name="taxonomyName">Name of the Taxonomy to create.</param>
        /// <param name="termNames">Names of the Terms to create and add to the Taxonomy.</param>
        void ApplyTaxonomyCreationWithTerms(string taxonomyName, List<string> termNames);

        /// <summary>
        /// Applies the previously invoked creation of Taxonomy with Terms and Taxonomy settings.
        /// </summary>
        /// <param name="taxonomyName">Name of the Taxonomy to create.</param>
        /// <param name="termNames">Names of the Terms to create and add to the Taxonomy.</param>
        /// <param name="settings">The settings to add to the Terms of the Taxonomy.</param>
        void ApplyTaxonomyCreationWithTermsAndSettings(string taxonomyName, List<string> termNames, Dictionary<string, string> settings);
    }
}
