using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Popups;

namespace Walkie_Doggie.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly FirebaseService _db;

        public SettingsPage()
        {
            InitializeComponent();
            _db = new FirebaseService();

            UserProfile.Text = "את/ה מחובר/ת כעת בתור " + LocalService.GetUsername();
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex == -1)
                return;

            switch (picker.SelectedItem)
            {
                case "Light":
                    Application.Current!.UserAppTheme = AppTheme.Light;
                    Preferences.Set("AppTheme", "Light");
                    break;
                case "Dark":
                    Application.Current!.UserAppTheme = AppTheme.Dark;
                    Preferences.Set("AppTheme", "Dark");
                    break;
                case "System Default":
                    Application.Current!.UserAppTheme = AppTheme.Unspecified;
                    Preferences.Set("AppTheme", "System");
                    break;
            }

            // Apply the new theme colors immediately
            App.ApplyTheme();
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            var result = await this.ShowPopupAsync(new ActionPopup(
                "האם את/ה בטוח/ה שברצונך להתנתק מהמערכת?", "התנתקות", "ביטול"));

            if (result is not null && result is string && (string)result == "התנתקות")
            {
                LocalService.RemoveUsername();
                await Toast.Make("התנתקת מהמערכת", ToastDuration.Short).Show();
                Application.Current!.MainPage = new LoginPage();
            }
        }
    }
}