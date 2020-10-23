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


        /// <summary>
        /// Given an expression and a list of values, this method will generate an aggregated expression factory
        /// that ORs together the applications of the expression on each value.
        /// </summary>
        /// <param name="expression">The expression to apply on each value</param>
        /// <param name="values">The values to use</param>
        /// <param name="property">The path of the property that the expression will compare the value to</param>
        public static Action<IHqlExpressionFactory> AggregateOrFactory(
            Func<string, object, Action<IHqlExpressionFactory>> expression,
            string property,
            object[] values)
        {
            if (!values?.Any() ?? true) return null;

            if (!values.Skip(1).Any()) return expression(property, values.First());

            return x => x.Or(
                expression(property, values.First()),
                AggregateOrFactory(expression, property, values.Skip(1).ToArray()));
        }
        
        /// <summary>
        /// Given an expression and a list of values, this method will generate an aggregated expression factory
        /// that ANDs together the applications of the expression on each value.
        /// </summary>
        /// <param name="expression">The expression to apply on each value</param>
        /// <param name="values">The values to use</param>
        /// <param name="property">The path of the property that the expression will compare the value to</param>
        public static Action<IHqlExpressionFactory> AggregateAndFactory(
            Func<string, object, Action<IHqlExpressionFactory>> expression,
            string property,
            object[] values)
        {
            if (!values?.Any() ?? true) return null;

            if (!values.Skip(1).Any()) return expression(property, values.First());

            return x => x.Or(
                expression(property, values.First()),
                AggregateAndFactory(expression, property, values.Skip(1).ToArray()));
        }
    }
}