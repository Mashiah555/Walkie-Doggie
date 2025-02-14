using Google.Cloud.Firestore;
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

    /// <summary>
    /// Converts DateTime to string form.
    /// </summary>
    public static string ConvertToString(object value)
    {
        if (value is null)
            return string.Empty;

        if (value is DateTime dateTime)
            return dateTime.ToString("dd/MM/yyyy HH:mm");

        if (value is Timestamp timestamp)
            return ConvertToDateTime(timestamp).ToString("dd/MM/yyyy HH:mm");

        if (value is TimeSpan timeSpan)
            return timeSpan.ToString(@"hh\:mm");

        return string.Empty;
    }
}

