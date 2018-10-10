using Orchard.ContentManagement;
using Orchard.Taxonomies.Fields;
using Orchard.Taxonomies.Models;
using System.Collections.Generic;
using System.Linq;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    public static class TaxonomyFieldExtensions
    {
        public static TermPart GetFirstTerm(this TaxonomyField field) =>
            field?.Terms?.Any() ?? false ? TermPart.Sort(field.Terms).FirstOrDefault() : null;

        public static string GetFirstTermName(this TaxonomyField field) =>
            GetFirstTerm(field)?.Name;

        public static string GetTermNames(this TaxonomyField field) =>
            field?.Terms?.Any() ?? false ? string.Join(", ", TermPart.Sort(field.Terms).Select(term => term.Name)) : "";

        // Use this method to correctly fetch the first Term's name immediately after updating the content item.
        public static TermPart GetFirstTermByRecord(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager) =>
            contentManager.Get<TermPart>(
                content.As<TermsPart>()?.Terms.FirstOrDefault(term => term.Field == field.Name)?.TermRecord.Id ?? 0);

        // Use this method to correctly fetch the first Term immediately after updating the content item.
        public static string GetFirstTermNameByRecord(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager) =>
            GetFirstTermByRecord(field, content, contentManager)?.Name;

        // Use this method to correctly fetch each Term immediately after updating the content item.
        public static IEnumerable<TermPart> GetTermsByRecord(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager) =>
            contentManager.GetMany<TermPart>(
                content.As<TermsPart>()?.Terms.Where(term => term.Field == field.Name).Select(term => term.Id) ?? Enumerable.Empty<int>(),
                VersionOptions.Published,
                QueryHints.Empty);
    }
}