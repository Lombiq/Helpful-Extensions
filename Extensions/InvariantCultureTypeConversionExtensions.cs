using System.Globalization;

namespace System
{
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
        public static string ToInvariantString(this DateTime value, string format) => 
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns the string representation of the given <paramref name="value"/> using the <see
        /// cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <returns>A predictable string representation of the given <paramref name="value"/>.</returns>
        public static string ToInvariantString(this DateTime? value) =>
            !value.HasValue ? string.Empty : value.Value.ToString(CultureInfo.InvariantCulture);
    }
}
