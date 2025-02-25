public static class LocalService
{
    #region Preference Keys
    private const string SyncedKey = "LocalSynced";
    private const string UsernameKey = "LocalUsername";
    private const string ThemeKey = "LocalTheme";
    #endregion Preference Keys

    #region Sync State Methods
    // Set the synced state of the settings
    public static void SetSyncState(bool synced) =>
        Preferences.Set(SyncedKey, synced);

    // Retrieve the sync state of the settings
    public static bool GetSyncState() =>
        Preferences.Get(SyncedKey, false);
    #endregion Sync State Methods

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
