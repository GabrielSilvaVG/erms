namespace Eventra.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime NowBrasilia()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
            );
        }
    }
}
