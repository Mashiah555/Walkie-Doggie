using System.Globalization;

public enum ContentStates
{
    Normal,
    Warning,
    Error
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

class ConvertToColor : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ContentStates state)
        {
            switch (state)
            {
                case ContentStates.Normal:
                    return Application.Current!.UserAppTheme == AppTheme.Light ?
                        (Color)Application.Current!.Resources["SecondaryTextColorLight"] :
                        (Color)Application.Current!.Resources["SecondaryTextColorDark"];

                case ContentStates.Warning:
                    return Application.Current!.UserAppTheme == AppTheme.Light ?
                        (Color)Application.Current!.Resources["WarningColorLight"] :
                        (Color)Application.Current!.Resources["WarningColorDark"];

                case ContentStates.Error:
                    return Application.Current!.UserAppTheme == AppTheme.Light ?
                        (Color)Application.Current!.Resources["ErrorColorLight"] :
                        (Color)Application.Current!.Resources["ErrorColorDark"];
            }
        }

        return Application.Current!.UserAppTheme == AppTheme.Light ?
            (Color)Application.Current!.Resources["SecondaryTextColorLight"] :
            (Color)Application.Current!.Resources["SecondaryTextColorDark"];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
