using System;

namespace ERP_API.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo FusoBrasil =
           TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static DateTime ToBrasiliaDate(this DateTime utcDate)
        {
            var utc = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, FusoBrasil).Date;
        }
    }
}
