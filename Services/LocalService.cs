public static class LocalService
{
    private const string UsernameKey = "LocalUsername";
    private const string ThemeKey = "LocalTheme";

    #region Username Methods
    // Save the username locally
    public static void SetUsername(string username) =>
        Preferences.Set(UsernameKey, username);

    // Retrieve the locally stored username
    public static string? GetUsername() =>
        Preferences.Get(UsernameKey, null); // Default to null if not set

    // Remove the locally stored username
    public static void RemoveUsername()
    {
        if (GetUsername() is not null)
            Preferences.Remove(UsernameKey);
    }
    #endregion Username Methods

    #region Theme Methods
    // Save the theme locally
    public static void SetTheme(AppTheme theme) =>
        Preferences.Set(ThemeKey, theme.ToString());

    // Retrieve the locally stored theme
    public static AppTheme GetTheme()
    {
        var themeString = Preferences.Get(ThemeKey, null);

        if (themeString is not null && Enum.TryParse(themeString, out AppTheme theme))
            return theme;
        return AppTheme.Unspecified;
    }
    #endregion Theme Methods
}
