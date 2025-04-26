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
            if (!await NetworkService.NetworkCheck())
                return;
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
}
