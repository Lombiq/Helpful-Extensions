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

        public static int? CalculateAgeInYears(
            this DateTime? dateTimeUtc,
            IClock clock)
        {
            if (!dateTimeUtc.HasValue) return null;

            var utcNow = clock.UtcNow;
            var age = utcNow.Year - dateTimeUtc.Value.Year;

            // Go back to the year in which the person was born in case of a leap year.
            if (dateTimeUtc.Value.Date > utcNow.AddYears(-age)) age--;

            return age;
        }
    }
}