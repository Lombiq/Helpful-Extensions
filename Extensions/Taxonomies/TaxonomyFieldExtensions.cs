using Orchard.Taxonomies.Fields;
using Orchard.Taxonomies.Models;
using System.Linq;

namespace Piedone.HelpfulExtensions.Taxonomies
{
    public static class TaxonomyFieldExtensions
    {
        public static string GetFirstTermName(this TaxonomyField field) =>
            field?.Terms?.Any() ?? false ? TermPart.Sort(field.Terms).First().Name : "";

        public static string GetTermNames(this TaxonomyField field) =>
            field?.Terms?.Any() ?? false ? string.Join(", ", TermPart.Sort(field.Terms).Select(term => term.Name)) : "";
    }
}