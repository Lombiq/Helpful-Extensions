using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.ContentManagement
{
    public static class HqlQueryExtensions
    {
        /// <summary>
        /// Causes the query not to return any results.
        /// </summary>
        public static IHqlQuery NullQuery(this IHqlQuery query) =>
            query.Where(alias => alias.ContentItem(), filter => filter.Eq("Id", 0));

        /// <summary>
        /// Applies a partitioned IN() clause on the query, taken the collection of IDs. For each partition a new IN() 
        /// is created, thus the limitation on the maximal amount of arguments for IN() can be circumvented.
        /// </summary>
        public static void WhereIdIn(this IHqlQuery query, IEnumerable<int> ids, int partitionSize = 1000)
        {
            query.Where(a => a.ContentItem(), p =>
                p.PartitionedExpression((e, i) => e.In("Id", i), ids, partitionSize));
        }

        /// <summary>
        /// Applies a partitioned NOT IN() clause on the query, taken the collection of IDs. For each partition a new NOT IN() 
        /// is created, thus the limitation on the maximal amount of arguments for IN() can be circumvented.
        /// </summary>
        public static void WhereIdNotIn(this IHqlQuery query, IEnumerable<int> ids, int partitionSize = 1000)
        {
            query.Where(a => a.ContentItem(), p =>
                p.PartitionedExpression((e, i) => e.Not(inner => inner.In("Id", i)), ids, partitionSize, true));
        }
    }
}