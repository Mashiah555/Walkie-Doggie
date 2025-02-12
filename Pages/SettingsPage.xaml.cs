using Walkie_Doggie.Helpers;

namespace Walkie_Doggie.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly FirebaseService _db;

        public SettingsPage()
        {
            InitializeComponent();
            _db = new FirebaseService();
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex == -1)
                return;

            switch (picker.SelectedItem)
            {
                case "Light":
                    Application.Current.UserAppTheme = AppTheme.Light;
                    Preferences.Set("AppTheme", "Light");
                    break;
                case "Dark":
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    Preferences.Set("AppTheme", "Dark");
                    break;
                case "System Default":
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    Preferences.Set("AppTheme", "System");
                    break;
            }

            // Apply the new theme colors immediately
            App.ApplyTheme();
        }
    }
}