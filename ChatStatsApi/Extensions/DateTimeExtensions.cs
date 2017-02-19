using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatStatsApi.Extensions
{
    public static class DateTimeExtensions
    {

        public static DateTime ToFirstOfMonth(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-MM-01"));
        }

    }
}