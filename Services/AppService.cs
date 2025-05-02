using CommunityToolkit.Maui.Alerts;
using System.Text.Json;

namespace Walkie_Doggie.Services;

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

            bool update = false;
            if (!remote.IsMandatory)
                await Snackbar.Make(
                    "עדכון חדש זמין",
                    () => { update = true; },
                    "עדכן",
                    TimeSpan.FromSeconds(8)).Show();
            if (!update) return;

        UpdatePopup:
            update = await Shell.Current.DisplayAlert("עדכון זמין", remote.Message, "עדכון", "מה חדש");
            await Launcher.OpenAsync(update ? remote.DownloadUrl : remote.AboutUrl);

            if (remote.IsMandatory)
                goto UpdatePopup;
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
        public bool IsMandatory { get; set; } = false;
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
