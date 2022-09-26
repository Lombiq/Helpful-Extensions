using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Models;
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

        public static string GetTermNames(this TaxonomyField field, string separator = ", ") =>
            field?.Terms?.Any() ?? false ? string.Join(separator, TermPart.Sort(field.Terms).Select(term => term.Name)) : "";

        public static IEnumerable<string> GetTermNamesEnumerable(this TaxonomyField field) =>
            field?.Terms?.Select(term => term.Name) ?? Enumerable.Empty<string>();

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
                content.As<TermsPart>()?.Terms.Where(term => term.Field == field.Name).Select(term => term.TermRecord.Id) ?? Enumerable.Empty<int>(),
                VersionOptions.Published,
                new QueryHints().ExpandRecords(nameof(ContentTypeRecord), nameof(CommonPartRecord), nameof(TermsPartRecord)));

        public static string GetTermNamesByRecord(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager,
            string separator = ", ") =>
            string.Join(separator, TermPart.Sort(field.GetTermsByRecord(content, contentManager)).Select(term => term.Name));

        public static IEnumerable<TermPart> GetTermsUnderParent(this TaxonomyField field, TermPart parent) =>
            parent == null ?
                Enumerable.Empty<TermPart>() :
                field.Terms.Where(term => term.Container?.Id == parent?.Id);

        public static TermPart GetFirstTermUnderParent(this TaxonomyField field, TermPart parent) =>
            GetTermsUnderParent(field, parent).FirstOrDefault();

        public static IEnumerable<TermPart> GetTermsByRecordUnderParent(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager,
            TermPart parent) =>
            parent == null ?
                Enumerable.Empty<TermPart>() :
                GetTermsByRecord(field, content, contentManager).Where(term => term.Container?.Id == parent.Id);

        public static TermPart GetFirstTermByRecordUnderParent(
            this TaxonomyField field,
            IContent content,
            IContentManager contentManager,
            TermPart parent) => GetTermsByRecordUnderParent(field, content, contentManager, parent).FirstOrDefault();
    }
}