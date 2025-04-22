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
            using var client = new HttpClient();
            var json = await client.GetStringAsync("https://yourdomain.com/version.json");

            var remote = JsonSerializer.Deserialize<VersionInfo>(json);
            Version latest = new Version(remote!.LatestVersion);
            Version current = AppInfo.Version;

            if (latest > current)
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
        public string LatestVersion { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
