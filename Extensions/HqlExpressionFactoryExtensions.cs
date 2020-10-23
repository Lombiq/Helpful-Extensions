using System;
using System.Collections.Generic;
using System.Linq;

namespace Orchard.ContentManagement
{
    public static class HqlExpressionFactoryExtensions
    {
        /// <summary>
        /// Applies the expression in partitions of given size, ORed or ANDed together.
        /// This helps to overcome limitations with certain clauses (like IN()) that only accept a specific amount of 
        /// arguments.
        /// </summary>
        /// <typeparam name="T">Type of the values used in the expression</typeparam>
        /// <param name="expressionFactory">The HQL expression factory</param>
        /// <param name="partitionExpression">Expression to apply on a partition of values</param>
        /// <param name="values">All the values to use</param>
        /// <param name="partitionSize">Determines how many values will be used for one partition</param>
        /// <param name="and">Determines the operator between the partitions (false = OR, true = AND)</param>
        public static void PartitionedExpression<T>(
            this IHqlExpressionFactory expressionFactory,
            Action<IHqlExpressionFactory, T[]> partitionExpression,
            IEnumerable<T> values,
            int partitionSize = 1000,
            bool and = false)
        {
            if (!values.Skip(partitionSize).Any())
            {
                partitionExpression(expressionFactory, values.ToArray());

                return;
            }

            if (and) expressionFactory.And(
                lhs => partitionExpression(lhs, values.Take(partitionSize).ToArray()),
                rhs => PartitionedExpression(rhs, partitionExpression, values.Skip(partitionSize), partitionSize, and));
            else expressionFactory.Or(
                lhs => partitionExpression(lhs, values.Take(partitionSize).ToArray()),
                rhs => PartitionedExpression(rhs, partitionExpression, values.Skip(partitionSize), partitionSize, and));
        }
    }
}
