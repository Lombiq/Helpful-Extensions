using Orchard.Services;
using Orchard.Utility.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Splits string by any new line characters found the given text.
        /// </summary>
        /// <param name="text">Text that needs to be split.</param>
        /// <param name="stringSplitOptions">String split options to be used.</param>
        /// <returns>Text split by new line characters.</returns>
        public static string[] SplitByNewLine(this string text, StringSplitOptions stringSplitOptions = StringSplitOptions.None) =>
            text.ReplaceNewLinesWith(Environment.NewLine).Split(new[] { Environment.NewLine }, stringSplitOptions);

        /// <summary>
        /// Replaces invalid file name characters to a replacement character.
        /// </summary>
        /// <param name="unsafeFileName">File name that needs to be converted to a valid file name.</param>
        /// <param name="replacementCharacter">Character to be used when replacing invalid characters.</param>
        /// <returns>Valid file name.</returns>
        public static string ToSafeFileName(this string unsafeFileName, char replacementCharacter = '_')
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            if (invalidCharacters.Contains(replacementCharacter))
                throw new ArgumentException("Replacement character should be a valid file name character.", nameof(replacementCharacter));

            var fileName = unsafeFileName;
            foreach (var invalidCharacter in invalidCharacters)
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }

            return fileName;
        }

        public static string RemoveTags(this string htmlText, params string[] tags)
        {
            if (string.IsNullOrEmpty(htmlText) || !tags.Any()) return htmlText;

            foreach (var tag in tags)
                htmlText = Regex.Replace(htmlText, $@"<{tag}[\d\D]*?>[\d\D]*?</{tag}>", "", RegexOptions.Singleline);

            return htmlText;
        }

        /// <summary>
        /// Removes HTML tags from the given text.
        /// </summary>
        /// <param name="htmlText">Text containing HTML tags.</param>
        /// <returns>Text without HTML tags.</returns>
        public static string StripHtml(this string htmlText) =>
            string.IsNullOrEmpty(htmlText) ? htmlText : Regex.Replace(htmlText, "<.*?>", "", RegexOptions.Singleline);

        /// <summary>
        /// Similar to HtmlClassify, this function converts a string to a CSS class name,
        /// but the result is compliant with BEM standards.
        /// </summary>
        /// <param name="text">The string to convert to a BEM class name.</param>
        /// <returns>The BEM-ified CSS class name.</returns>
        public static string HtmlBemClassify(this string text)
        {
            var segments = text.HtmlClassify().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (!segments.Any()) return null;

            if (!segments.Skip(1).Any()) return segments[0];

            for (int i = 1; i < segments.Count(); i++)
                segments[i] = segments[i].Skip(1).Any() ?
                    char.ToUpper(segments[i][0]) + segments[i].Substring(1) : segments[i].ToUpper();

            return string.Join("", segments);
        }

        public static string ConvertFromJsonStringArrayToCommaSeparatedString(this string serialized, IJsonConverter jsonConverter) =>
            jsonConverter.TryDeserialize<IEnumerable<string>>(serialized, out var values) ?
                string.Join(", ", values.OrderBy(x => x)) : "";

        public static DateTime? ToNullableDateTime(this string text) =>
            DateTime.TryParse(text, out var date) ? date : (DateTime?)null;

        /// <summary>
        /// Tries to convert a string to decimal, but returns 0 if the conversion fails.
        /// </summary>
        /// <param name="number">The string to convert to decimal.</param>
        /// <returns>Returns 0 if the given number is not parseable to decimal.</returns>
        public static decimal ToDecimalOrZero(this string number) => number.ToNullableDecimal() ?? 0;

        /// <summary>
        /// Tries to convert a string to decimal, but returns null if the conversion fails.
        /// </summary>
        /// <param name="number">The string to convert to decimal.</param>
        /// <returns>Returns null if the given number is not parseable to decimal.</returns>
        public static decimal? ToNullableDecimal(this string number) =>
            decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue) ?
                decimalValue : (decimal?)null;

        public static bool? ToNullableBoolean(this string text)
        {
            switch (text?.Trim().ToUpperInvariant() ?? "")
            {
                case "YES":
                case "TRUE":
                case "ACTIVE":
                    return true;
                case "NO":
                case "FALSE":
                case "INACTIVE":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Checks if the string contains a substring in a case-insensitive manner.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The substring to search for.</param>
        /// <returns>Returns <see langword="true"/> if the source string contains the substring (case-insensitive), otherwise <see langword="false"/>.</returns>
        public static bool ContainsIgnoreCase(this string source, string value) => 
            source?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Splits a string into a set of integers using the provided separator. Optional whitespace in the input is
        /// handled gracefully.
        /// </summary>
        /// <param name="value">The string to split into integers; may be null or empty.</param>
        /// <param name="separator">The separator character to split the string by, defaults to <c>','</c>.</param>
        /// <returns>The set of distinct integers found in the input string.</returns>
        public static ISet<int> SplitIntoIntegers(this string value, char separator = ',')
        {
            value = value ?? "";

            var collection = value
                .Split(separator)
                .Select(segment => int.TryParse(segment, out var result) ? (int?)result : null)
                .Where(num => num.HasValue)
                .Select(num => num.Value);

            return new HashSet<int>(collection);
        }

        /// <summary>
        /// Splits a string into a set of strings using the provided separator. Optional whitespace in the input is
        /// handled gracefully.
        /// </summary>
        /// <param name="value">The string to split into strings; may be null or empty.</param>
        /// <param name="separator">The separator character to split the string by, defaults to <c>','</c>.</param>
        /// <returns>The set of distinct strings found in the input string.</returns>
        public static ISet<string> SplitIntoStrings(this string value, char separator = ',') =>
            new HashSet<string>((value ?? string.Empty).Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries));

        /// <summary>
        /// Retrieves a substring from this instance safely. 
        /// The substring starts at a specified character position and has a specified length.
        /// Will never throw an exception unless start or length is negative.
        /// </summary>
        /// <param name="text">The string to substring.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string that is equivalent to the substring of length that begins at startIndex in this instance.</returns>
        public static string SafeSubstring(this string text, int startIndex, int length) =>
            string.IsNullOrEmpty(text)
                ? string.Empty
                : text.Length <= startIndex
                    ? string.Empty
                    : text.Substring(startIndex, Math.Min(text.Length - startIndex, length));
    }
}
