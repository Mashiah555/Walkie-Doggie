using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

    public async static Task<bool> NetworkCheck()
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            return true;

        if (!NavigationFlags.IsMessagePopedUp)
        {
            await MauiPopup.PopupAction.DisplayPopup(new MessagePopup(
                "אין חיבור",
                "נדרש חיבור לאינטרנט על מנת להשתמש באפליקציה",
                ContextImage.NoInternet,
                ButtonSet.NoneAndForce));
        }

        while (true)
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                await MauiPopup.PopupAction.ClosePopup();
                await Toast.Make("מחובר לאינטרנט", ToastDuration.Short).Show();
                return true;
            }
            await Task.Delay(1500);
        }

        //return false;
    }
}

public static class VersionService
{
    public static Version CurrentVersion { get; } = AppInfo.Version;

    public static async Task CheckForUpdatesAsync()
    {
        try
        {
            if (!await NetworkService.NetworkCheck())
                return;
            if (string.IsNullOrEmpty(AppInfo.BuildString))
                throw new Exception();

            using var client = new HttpClient();
            var json = await client.GetStringAsync(
                "https://raw.githubusercontent.com/Mashiah555/Walkie-Doggie/master/Services/version.json");
            var remote = JsonSerializer.Deserialize<VersionInfo>(json);
            if (remote == null)
                return;

            int current = int.Parse(AppInfo.BuildString);
            int latest = remote.LatestVersion;

            if (latest == 0 || current == 0)
                throw new Exception("AppInfo.BuildString failed to parse into int");
            else if (latest > current)
            {
                //LinkPopup popup = new LinkPopup();
                bool update = await Shell.Current.DisplayAlert("עדכון זמין", remote.Message, "עדכן", "סגור");
                if (update)
                {
                    await Launcher.OpenAsync(remote.DownloadUrl);
                }
            }
        }
        catch
        {
            // Optionally log or ignore errors silently
        }
    }

    private class VersionInfo
    {
        public int LatestVersion { get; set; } = int.Parse(AppInfo.BuildString);
        public string Message { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public string AboutUrl { get; set; } = string.Empty;
    }
}
