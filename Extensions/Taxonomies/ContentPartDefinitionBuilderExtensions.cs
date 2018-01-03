using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Environment.Extensions;
using Orchard.Taxonomies.Fields;
using System;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    [OrchardFeature("Piedone.HelpfulExtensions.Taxonomies")]
    public static class ContentPartDefinitionBuilderExtensions
    {
        public static ContentPartDefinitionBuilder WithTaxonomyField(
            this ContentPartDefinitionBuilder builder,
            string fieldName,
            Action<ContentPartFieldDefinitionBuilder> configuration = null) =>
            builder.WithField(nameof(TaxonomyField), fieldName, configuration);
    }
}