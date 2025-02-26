public static class LocalService
{
    #region Preference Keys
    private const string WalkKey = "SelectedWalk";
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
        Preferences.Set(ThemeKey, (int)theme);

    // Retrieve the locally stored theme
    public static AppTheme GetTheme() => 
        (AppTheme)Preferences.Get(ThemeKey, 0);
    #endregion Theme Methods

    #region Selected Walk Preference
    // Set the selected walk id preference
    public static void SetWalk(int id) =>
        Preferences.Set(WalkKey, id);

    // Retrieve the selected walk id preference and remove it
    public static int? GetWalk()
    {
        int id = Preferences.Get(WalkKey, -1);
        Preferences.Remove(WalkKey);

        return id != -1 ? id : null;
    }

    // Remove a selected walk id prefernce
    public static void RemoveWalk() =>
        Preferences.Remove(WalkKey);
    #endregion Selected Walk Preference
}
