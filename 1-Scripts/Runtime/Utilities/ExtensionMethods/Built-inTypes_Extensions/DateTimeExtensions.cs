using System;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class DateTimeExtensions
    {
        public static int ToUnixTimestamp(this DateTime value) => (int) Math.Truncate(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

        public static int UnixTimestamp(this DateTime ignored) => (int) Math.Truncate(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

        public static DateTime FromUnixTimestamp(int unixTimeStamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) unixTimeStamp).ToLocalTime();
        
        public static String GetUnixTimestamp(this DateTime value)
        {
            return new DateTimeOffset(value).ToUnixTimeSeconds().ToString();
        }
    
        public static bool TryParseUnixTimestampStrToDateTime(string unixTimeStampStr, out DateTime parsedDateTime,
            bool inLocalTime = true)
        {
            parsedDateTime = default;
        
            if (long.TryParse(unixTimeStampStr, out long timestamp))
            {
                DateTimeOffset unixTimeStamp = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                parsedDateTime = inLocalTime ? unixTimeStamp.UtcDateTime : unixTimeStamp.LocalDateTime;
                return true;
            }

            return false;
        }
    }
}