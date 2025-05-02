using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Popups;

namespace Walkie_Doggie.Services;

public static class NetworkService
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
