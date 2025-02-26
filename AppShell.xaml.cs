using System.Diagnostics;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        private readonly FirebaseService _db;
        public string WalksTabIcon { get; set; } = "walks_static.png"; // Default static icon

        public AppShell()
        {
            InitializeComponent();
            _db = new FirebaseService();

            Routing.RegisterRoute(nameof(AppShell), typeof(AppShell));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(DogView), typeof(DogView));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(WalksPage), typeof(WalksPage));
            Routing.RegisterRoute(nameof(StatsPage), typeof(StatsPage));

            //InitializeApp();
            
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
    }
}
