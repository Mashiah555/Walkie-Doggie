using Google.Cloud.Firestore;
using System.Globalization;
using static Walkie_Doggie.Helpers.Enums;
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
    /// Converts values to string form.
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

    /// <summary>
    /// Converts values to visibility form.
    /// </summary>
    public static Visibility ConvertToVisibility(object value)
    {
        if (value is null)
            return Visibility.Collapsed;

        if (value is bool boolValue)
            return boolValue ? Visibility.Visible : Visibility.Collapsed;

        return Visibility.Visible;
    }

    /// <summary>
    /// Converts values to boolean form.
    /// </summary>
    public static bool ConvertToBool(object value)
    {
        if (value is null)
            return false;
        return true;
    }
}

class ConvertToBool : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return true;
    }
}

class ConvertToRoundedString : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return string.Empty;

        if (value is int)
            return value.ToString() ?? string.Empty;

        if (value is double)
            return (Math.Floor((double)value * 2 + 0.5) / 2)
                .ToString() ?? string.Empty;

        if (value is float)
            return (Math.Floor((float)value * 2 + 0.5) / 2)
                .ToString() ?? string.Empty;

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return 0;

        if (double.TryParse(value as string, out double doubleResult))
            return doubleResult;

        if (float.TryParse(value as string, out float floatResult))
            return floatResult;

        if (int.TryParse(value as string, out int intResult))
            return intResult;

        return 0;
    }
}
