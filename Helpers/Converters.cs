using Google.Cloud.Firestore;
using System;
namespace Walkie_Doggie.Helpers;

public static class Converters
{
    private static readonly TimeZoneInfo IsraelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

    /// <summary>
    /// Converts local DateTime to Firestore Timestamp (stored in UTC).
    /// </summary>
    public static Timestamp ConvertToTimestamp(DateTime dateTime)
    {
        DateTime israelTime = TimeZoneInfo.ConvertTime(dateTime, IsraelTimeZone);
        return Timestamp.FromDateTime(israelTime.ToUniversalTime());
    }

    /// <summary>
    /// Converts Firestore Timestamp to local Israel time.
    /// </summary>
    public static DateTime ConvertToDateTime(Timestamp timestamp)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(timestamp.ToDateTime(), IsraelTimeZone);
    }
}
