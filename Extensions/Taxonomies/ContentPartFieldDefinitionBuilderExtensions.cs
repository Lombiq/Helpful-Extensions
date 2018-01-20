using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Environment.Extensions;
using Orchard.Taxonomies.Settings;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    [OrchardFeature(Constants.FeatureNames.Taxonomies)]
    public static class ContentPartFieldDefinitionBuilderExtensions
    {
        public static ContentPartFieldDefinitionBuilder WithTaxonomyFieldSettings(
            this ContentPartFieldDefinitionBuilder builder,
            TaxonomyFieldSettings settings)
        {
            var prefix = $"{nameof(TaxonomyFieldSettings)}.";

            return builder
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.AllowCustomTerms), settings.AllowCustomTerms.ToString())
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.Autocomplete), settings.Autocomplete.ToString())
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.Hint), settings.Hint)
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.LeavesOnly), settings.LeavesOnly.ToString())
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.Required), settings.Required.ToString())
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.SingleChoice), settings.SingleChoice.ToString())
                .WithSetting(prefix + nameof(TaxonomyFieldSettings.Taxonomy), settings.Taxonomy);
        }
    }
}