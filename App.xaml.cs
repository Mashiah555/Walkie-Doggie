namespace Walkie_Doggie
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(LocalService.GetUsername()))
                MainPage = new Pages.LoginPage();
            else
                MainPage = new AppShell();

            ApplyTheme();
            // Detect system theme changes
            Current!.RequestedThemeChanged += (s, e) => ApplyTheme();
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
