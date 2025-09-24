using System.Globalization;

namespace System
{
    /// <summary>
    /// The Orchard Core counterparts of these extensions can be found in the NumberExtensions.cs of Helpful Libraries.
    /// </summary>
    public static class InvariantCultureTypeConversionExtensions
    {
        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this int value) => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this int? value) =>
            value?.ToInvariantString() ?? string.Empty;

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this DateTime value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this DateTime? value) =>
            value?.ToInvariantString() ?? string.Empty;

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this DateTime value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this DateTime? value, string format) =>
            value?.ToInvariantString(format) ?? string.Empty;

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this double value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this double? value) =>
            value?.ToInvariantString() ?? string.Empty;

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/> up to the specified <paramref name="decimalPlaces"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="decimalPlaces">The format to convert the <paramref name="value"/> to.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this float value, int decimalPlaces)
        {
            var format = $"F{decimalPlaces.ToInvariantString()}";

            return value.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/> up to the specified <paramref name="decimalPlaces"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="decimalPlaces">The format to convert the <paramref name="value"/> to.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this decimal value, int decimalPlaces)
        {
            var format = $"F{decimalPlaces.ToInvariantString()}";

            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
