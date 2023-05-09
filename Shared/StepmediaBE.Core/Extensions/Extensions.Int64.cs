using System;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        public static DateTime ToDateTime(this long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
        }

        public static DateTime? ToDateTime(this long? seconds)
        {
            if (seconds == null)
                return null;
            
            return DateTimeOffset.FromUnixTimeSeconds(seconds.Value).LocalDateTime;
        }

        public static long ToTimestamp(this DateTime dateTime)
        {
            var epoch = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            return (long) epoch.TotalSeconds;
        }
        
        
        public static decimal TimeLeftPercent(this DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date;
            if (toDate < fromDate)
                return 0;

            var today = DateTime.Today;
            if (fromDate > today)
                return 100;

            if (toDate > today)
            {
                var f = today - fromDate;
                var t = toDate - fromDate;

                return Convert.ToDecimal(((t.TotalDays - f.TotalDays) / t.TotalDays) * 100);
            }

            return 0;
        }
    }
}