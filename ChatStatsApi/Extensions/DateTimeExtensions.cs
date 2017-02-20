using System;

namespace ChatStatsApi.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToFirstOfMonth(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-MM-01"));
        }

        public static string ToJsDate(this DateTime date)
        {
            return string.Format("Date.UTC({0}, {1}, {2})", date.Date.Year, date.Date.Month, date.Date.Day);
        }

        /// <summary>
        /// Outputs the equivalend of Date.UTC(...) in Javascript (unix time I believe)
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Total milliseconds since 1970</returns>
        public static double ToJsUtcDateMilliseconds(this DateTime date)
        {
            return date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}