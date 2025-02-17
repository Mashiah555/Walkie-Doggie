public static class LocalService
{
    private const string UsernameKey = "LocalUsername";

    // Save the username locally
    public static void SaveUsername(string username)
    {
        Preferences.Set(UsernameKey, username);
    }

    // Retrieve the locally stored username
    public static string? GetUsername()
    {
        return Preferences.Get(UsernameKey, null); // Default to null if not set
    }

    // Remove the locally stored username
    public static void RemoveUsername()
    {
        if (GetUsername() is not null)
            Preferences.Remove(UsernameKey);
    }
}
