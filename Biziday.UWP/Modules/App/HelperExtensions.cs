using System;
using System.Collections.Generic;
using System.Linq;
namespace Biziday.UWP.Modules.App
{
    public static class HelperExtensions
    {
        public static bool IsEmpty(this string data)
        {
            return string.IsNullOrWhiteSpace(data);
        }

        public static bool IsNotEmpty(this string data)
        {
            return !string.IsNullOrWhiteSpace(data);
        }

        public static long GetTimestamp(this DateTime date)
        {
            var totalSeconds = (date - new DateTime(1970, 1, 1)).TotalSeconds;
            return (long)totalSeconds;
        }

        public static DateTime GetDate(this long timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }
    }
}
