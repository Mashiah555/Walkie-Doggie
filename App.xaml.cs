using System.Diagnostics;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie
{
    public partial class App : Application
    {
        private readonly FirebaseService _db;
        public App()
        {
            InitializeComponent();
            _db = new FirebaseService();

            // Assign a temporary loading page to avoid NotImplementedException
            //MainPage = new ContentPage { Content = new ActivityIndicator 
            //{ 
            //    IsRunning = true,
            //    HeightRequest = 100,
            //    WidthRequest = 100,
            //    Margin = new Thickness(250)
            //} };
            MainPage = new AppShell();

            //InitializeApp();

            ApplyTheme();
            // Detect system theme changes
            Current!.RequestedThemeChanged += (s, e) => ApplyTheme();
        }

        private async void InitializeApp()
        {
            if (string.IsNullOrEmpty(LocalService.GetUsername()))
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            else if (await _db.HasDog())
            {
                if (Shell.Current != null)
                    await Shell.Current.GoToAsync($"{nameof(AppShell)}");
                else
                    Debug.WriteLine("Shell.Current is null, cannot navigate.");
            }
            else
                await Shell.Current.GoToAsync($"{nameof(DogView)}");
        }

        public static void ApplyTheme()
        {
            bool isDarkMode = Current!.UserAppTheme == AppTheme.Dark ||
                (Current.UserAppTheme == AppTheme.Unspecified && 
                Current.RequestedTheme == AppTheme.Dark);

            // Set global colors dynamically
            Current.Resources["AccentColor"] = isDarkMode
                ? Current.Resources["AccentColorDark"]
                : Current.Resources["AccentColorLight"];

            Current.Resources["PrimaryColor"] = isDarkMode
                ? Current.Resources["PrimaryColorDark"]
                : Current.Resources["PrimaryColorLight"];

            Current.Resources["SecondaryColor"] = isDarkMode
                ? Current.Resources["SecondaryColorDark"]
                : Current.Resources["SecondaryColorLight"];

            Current.Resources["ActionTextColor"] = isDarkMode
                ? Current.Resources["ActionTextColorDark"]
                : Current.Resources["ActionTextColorLight"];

            Current.Resources["PrimaryTextColor"] = isDarkMode
                ? Current.Resources["PrimaryTextColorDark"]
                : Current.Resources["PrimaryTextColorLight"];

            Current.Resources["SecondaryTextColor"] = isDarkMode
                ? Current.Resources["SecondaryTextColorDark"]
                : Current.Resources["SecondaryTextColorLight"];

            Current.Resources["ErrorColor"] = isDarkMode
                ? Current.Resources["ErrorColorDark"]
                : Current.Resources["ErrorColorLight"];

            Current.Resources["WarningColor"] = isDarkMode
                ? Current.Resources["WarningColorDark"]
                : Current.Resources["WarningColorLight"];
        }

        protected override void OnStart()
        {
            string savedTheme = Preferences.Get("AppTheme", "System");

            if (savedTheme == "Light")
                Current!.UserAppTheme = AppTheme.Light;
            else if (savedTheme == "Dark")
                Current!.UserAppTheme = AppTheme.Dark;
            else
                Current!.UserAppTheme = AppTheme.Unspecified;

            ApplyTheme(); // Ensure colors are updated
        }
    }
}
