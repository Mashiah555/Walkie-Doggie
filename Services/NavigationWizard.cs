using Walkie_Doggie.Database;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie.Services;

public static class NavigationWizard
{
    private static readonly FirebaseService _db = new();
    public static async void InitializeAppAsync()
    {
        if (Shell.Current != null)
        {
            if (string.IsNullOrEmpty(LocalService.GetUsername()))
                await Shell.Current.GoToAsync(nameof(LoginPage), true);
            else if (!(await _db.HasDog()))
                await Shell.Current.GoToAsync(nameof(DogView), true);
        }

        // Check for updates
        await AppService.CheckForUpdatesAsync(); //todo: option to force or not force updating

        // Detect and clean full storage space of the cloud database
        await DbService.Operations.FreeStorageSpaceAsync();
    }
}
