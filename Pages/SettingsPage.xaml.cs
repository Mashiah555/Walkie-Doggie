using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly FirebaseService _db;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingViewModel();
            _db = new FirebaseService();
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("התנתקות", "האם את/ה בטוח/ה שברצונך להתנתק מהמערכת?",
                "אישור", "ביטול", FlowDirection.RightToLeft);

            if (result)
            {
                LocalService.RemoveUsername();
                await Toast.Make("התנתקת מהמערכת", ToastDuration.Short).Show();
                Application.Current!.MainPage = new LoginPage(null);
            }
        }
    }
}