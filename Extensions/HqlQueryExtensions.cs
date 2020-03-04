namespace Orchard.ContentManagement
{
    public static class HqlQueryExtensions
    {
        /// <summary>
        /// Causes the query not to return any results.
        /// </summary>
        public static IHqlQuery NullQuery(this IHqlQuery query) =>
            query.Where(alias => alias.ContentItem(), filter => filter.Eq("Id", 0));
    }
}