using Orchard.Localization.Services;
using Orchard.Services;
using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {
        public static string ToIsoDateTimeString(this DateTime dateTime) => dateTime.ToString("O");

        public static string ToIsoDateString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        public static DateTime FirstDayOfMonth(this DateTime dateTime) =>
            new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, dateTime.Kind);

        public static DateTime LastDayOfMonth(this DateTime dateTime) =>
            FirstDayOfMonth(dateTime).AddMonths(1).AddDays(-1);

        /// <summary>
        /// Converts the given date to USA format.
        /// </summary>
        /// <param name="dateTime">Date to be converted to USA format.</param>
        /// <returns>USA-formatted date string.</returns>
        public static string ConvertToUsaDateFormat(this DateTime dateTime) =>
            dateTime.ToString("d", CultureInfo.CreateSpecificCulture("en-US"));

        /// <summary>
        /// Converts the given date to USA format.
        /// </summary>
        /// <param name="dateTime">Date to be converted to USA format.</param>
        /// <returns>USA-formatted date string, empty string if null.</returns>
        public static string ConvertToUsaDateFormat(this DateTime? dateTime) =>
            dateTime?.ConvertToUsaDateFormat() ?? "";

        /// <summary>
        /// Converts the given date to long USA format.
        /// </summary>
        /// <param name="dateTime">Date to be converted to USA format, months with letters.</param>
        /// <returns>USA-formatted long date string.</returns>
        public static string ConvertToLongUsaDateFormat(this DateTime dateTime) =>
            dateTime.ToString("D", CultureInfo.CreateSpecificCulture("en-US"));

        /// <summary>
        /// Converts the given date to long USA format.
        /// </summary>
        /// <param name="dateTime">Date to be converted to USA format, months with letters.</param>
        /// <returns>USA-formatted long date string, empty string if null.</returns>
        public static string ConvertToLongUsaDateFormat(this DateTime? dateTime) =>
            dateTime?.ConvertToLongUsaDateFormat() ?? "";

        /// <summary>
        /// Converts the given date-time to USA format.
        /// </summary>
        /// <param name="dateTime">Date-time to be converted to USA format.</param>
        /// <returns>USA-formatted date-time string.</returns>
        public static string ConvertToUsaDateTimeFormat(this DateTime dateTime) =>
            dateTime.ToString("g", CultureInfo.CreateSpecificCulture("en-US"));

        /// <summary>
        /// Converts the given date-time to USA format.
        /// </summary>
        /// <param name="dateTime">Date-time to be converted to USA format.</param>
        /// <returns>USA-formatted date-time string, empty string if null.</returns>
        public static string ConvertToUsaDateTimeFormat(this DateTime? dateTime) =>
            dateTime?.ConvertToUsaDateTimeFormat() ?? "";

        // This name is oddly specific...
        public static string DateTimeOrUtcNowToSiteTimeZoneInUsaDateTimeFormat(
            this DateTime? dateTime,
            IClock clock,
            IDateLocalizationServices dateLocalizationServices) =>
            dateLocalizationServices.ConvertToSiteTimeZone(dateTime ?? clock.UtcNow).ConvertToUsaDateTimeFormat();

        public static string DateTimeOrUtcNowToSiteTimeZoneInUsaDateFormat(
            this DateTime? dateTime,
            IClock clock,
            IDateLocalizationServices dateLocalizationServices) =>
            dateLocalizationServices.ConvertToSiteTimeZone(dateTime ?? clock.UtcNow).ConvertToUsaDateFormat();

        public static string ConvertToUsaTimeFormat(this DateTime? time) =>
            time?.ToString("hh:mm tt") ?? "";

        public static string DateTimeOrUtcNowToSiteTimeZoneInUsaTimeFormat(
            this DateTime? dateTime,
            IClock clock,
            IDateLocalizationServices dateLocalizationServices) =>
            (dateLocalizationServices.ConvertToSiteTimeZone(dateTime ?? clock.UtcNow) as DateTime?).ConvertToUsaTimeFormat();

        /// <summary>
        /// Calculates the full calendar years passed from the left operand until the right operand.
        /// It's essentially TimeSpan.TotalYears, but it needs to be implemented this way,
        /// because TimeSpan wouldn't know if a full year has passed or not (also taking leap years into account).
        /// </summary>
        /// <param name="left">Left DateTime? operand.</param>
        /// <param name="right">Right DateTime? operand.</param>
        /// <returns>The number of full calendar years passed between the operands.</returns>
        public static int? TotalYearsSpan(this DateTime? left, DateTime? right)
        {
            if (!(left.HasValue && right.HasValue)) return null;

            var years = left.Value.Year - right.Value.Year;

            // Correcting with a year that has not fully passed.
            if (right.Value.Date > left.Value.AddYears(-years)) years--;

            return years;
        }

        public static DateTime? ParseToNullableDateTime(this string text) =>
            DateTime.TryParse(text, out var date) ? date : (DateTime?)null;
    }
}