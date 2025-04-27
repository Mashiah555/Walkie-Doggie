using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.Json;
using Walkie_Doggie.Popups;

namespace Walkie_Doggie.Helpers;

public static class NavigationFlags
{
    public static bool IsMessagePopedUp = false;
}

public class NetworkService
{
    public static bool IsConnected() => 
        Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    public async static Task NetworkCheck(TimeSpan? timeout = null)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            return;

        if (!NavigationFlags.IsMessagePopedUp)
        {
            NavigationFlags.IsMessagePopedUp = true;
            await MauiPopup.PopupAction.DisplayPopup(new MessagePopup(
                "אין חיבור",
                "!נדרש חיבור לאינטרנט על מנת להשתמש באפליקציה" +
                "\nהאפליקציה תחזור לפעול כשמכשיר זה יחובר לרשת.",
                ContextImage.NoInternet,
                ButtonSet.NoneAndForce));
        }

    Recheck:
        DateTime startTime = DateTime.UtcNow;
        timeout ??= TimeSpan.FromSeconds(60);

        while (DateTime.UtcNow - startTime < timeout)
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                await MauiPopup.PopupAction.ClosePopup();
                NavigationFlags.IsMessagePopedUp = false;
                await Toast.Make("מחובר לאינטרנט", ToastDuration.Short).Show();
                return;
            }
            await Task.Delay(1500);
        }

        bool recheck = false;
        var snack = Snackbar.Make(
            "החיבור לרשת נכשל, האפליקציה תיסגר.",
            () => { recheck = true; },
            "נסה שוב",
            TimeSpan.FromSeconds(3));
        await snack.Show();
        await Task.Delay(3000);

        if (recheck) goto Recheck;

        await Toast.Make("החיבור לאינטרנט נכשל", ToastDuration.Short).Show();
        AppService.QuitApp();
        return;
    }
}

public static class AppService
{
    #region Version Diagnostics
    public static string CurrentVersion
    {
        get
        {
            // Convert Major.Minor.Build.Revision to Major.Minor.Build
            string[] parts = AppInfo.VersionString.Split('.');
            return string.Join('.', parts.Take(3));
        }
    }

    public static async Task CheckForUpdatesAsync()
    {
        try
        {
            await NetworkService.NetworkCheck();

            if (string.IsNullOrEmpty(AppInfo.BuildString))
                throw new Exception();

            using HttpClient client = new();
            string json = await client.GetStringAsync(
                "https://raw.githubusercontent.com/Mashiah555/Walkie-Doggie/master/Services/version.json");
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            VersionInfo? remote = JsonSerializer.Deserialize<VersionInfo>(json, options);
            if (remote == null)
                return;

            int current = int.Parse(AppInfo.BuildString);
            int latest = remote.LatestVersion;

            if (current == 0)
                throw new Exception("AppInfo.BuildString failed to parse into int");
            if (latest == 0)
                throw new Exception("Failed to deserialize the json's url");
            if (latest <= current)
                return;

            //LinkPopup popup = new LinkPopup();
            bool update = await Shell.Current.DisplayAlert("עדכון זמין", remote.Message, "עדכון", "מה חדש");
            await Launcher.OpenAsync(update ? remote.DownloadUrl : remote.AboutUrl);

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("שגיאה", ex.Message, "סגור");
        }
    }

    private class VersionInfo
    {
        /* version.json Structural Properties.
           Must match the JSON properties to deserialize correctly. */
        public int LatestVersion { get; set; }
        public string Message { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public string AboutUrl { get; set; } = string.Empty;
    }
    #endregion Version Diagnostics

    #region App Configuration
    public static void QuitApp()
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
       System.Diagnostics.Process.GetCurrentProcess().Kill();  
#else
       // For unsupported platforms:
       Application.Current!.Quit();
       Environment.Exit(0);
#endif
    }
    public static void ReloadApp()
    {
        //WIP (Work In Progress)
    }
    #endregion App Configuration
}

interface I1
{

}
