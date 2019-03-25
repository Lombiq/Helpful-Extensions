using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Piedone.HelpfulExtensions
{
    public static class QueryStringExtensions
    {
        /// <summary>
        /// Given the collection of query parameters and the key and value of a query parameter,
        /// this helper will update (or add if not added yet) that query parameter (when the value is empty,
        /// the query parameter will be removed from the collection) and then build the complete query string.
        /// </summary>
        /// <param name="queryString">The collection of query parameters.</param>
        /// <param name="key">The key of the query parameter to update.</param>
        /// <param name="value">The value of the query parameter to update. It's empty by default that means the key needs to be removed.</param>
        /// <returns>The query string built from the collection of query parameters.</returns>
        public static string UpdateAndBuildQueryString(this NameValueCollection queryString, string key, params object[] values)
        {
            // Creating a mutable collection.
            var queryParameters = GetQueryParameterDictionary(queryString);

            // Empty value -> Remove.
            if (!values?.Any(value => value != null) ?? true) queryParameters.Remove(key);
            // Non-empty value -> Add or Update.
            else queryParameters[key] = values;

            var builder = new StringBuilder();
            builder.Append("?");

            foreach (var currentKey in queryParameters.Keys)
                foreach (var value in queryParameters[currentKey])
                    builder.Append($"{currentKey}={value}&");

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        /// <summary>
        /// Given the collection of query parameters and the key of a query parameter,
        /// this helper will determine whether that key with any value exists among the query parameters.
        /// </summary>
        /// <param name="queryString">The collection of query parameters.</param>
        /// <param name="key">The key of the query parameter to check.</param>
        /// <returns>Whether the given key with any value exists among the query parameters.</returns>
        public static bool IsQueryStringParameterPresent(this NameValueCollection queryString, string key) =>
            queryString?.AllKeys.Contains(key) ?? false;

        /// <summary>
        /// Given the collection of query parameters and the key of a query parameter,
        /// this helper will determine whether that key with a non-empty value exists among the query parameters.
        /// </summary>
        /// <param name="queryString">The collection of query parameters.</param>
        /// <param name="key">The key of the query parameter to check.</param>
        /// <returns>Whether the given key with any value exists among the query parameters.</returns>
        public static bool IsQueryStringParameterActive(this NameValueCollection queryString, string key) =>
            IsQueryStringParameterPresent(queryString, key) && queryString.GetValues(key).Any(value => !string.IsNullOrEmpty(value));

        /// <summary>
        /// Given the collection of query parameters and the key and value of a query parameter,
        /// this helper will determine whether that key-value pair exists among the query parameters.
        /// </summary>
        /// <param name="queryString">The collection of query parameters.</param>
        /// <param name="key">The key of the query parameter to check.</param>
        /// <param name="value">The value of the query parameter to check.</param>
        /// <returns>Whether the given key-value pair exists among the query parameters.</returns>
        public static bool IsQueryStringParameterValueActive(this NameValueCollection queryString, string key, string value)
        {
            if (!queryString?.AllKeys.Any(k => k != null) ?? true) return string.IsNullOrEmpty(value);

            var queryParameters = GetQueryParameterDictionary(queryString);

            if (string.IsNullOrEmpty(value) && !queryParameters.ContainsKey(key)) return true;

            if (queryParameters.ContainsKey(key) && queryParameters[key].Any(item => item.ToString() == value))
                return true;

            return false;
        }

        public static IEnumerable<string> GetQueryStringParameterValues(this NameValueCollection queryString, string technicalName) =>
            (IsQueryStringParameterPresent(queryString, technicalName)) ?
                queryString[technicalName].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;


        private static IDictionary<string, IEnumerable<object>> GetQueryParameterDictionary(NameValueCollection queryString)
        {
            var queryParameters = new Dictionary<string, IEnumerable<object>>();

            if (!queryString?.AllKeys.Any(k => k != null) ?? true) return queryParameters;

            foreach (var key in queryString.AllKeys) if (key != null) queryParameters.Add(key, queryString.GetValues(key));

            return queryParameters;
        }
    }
}