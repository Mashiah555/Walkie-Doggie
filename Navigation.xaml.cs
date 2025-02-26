using System.Diagnostics;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie;

public partial class Navigation : ContentPage
{
    private readonly FirebaseService _db;
    public Navigation()
	{
		InitializeComponent();
        _db = new FirebaseService();

        InitializeApp();
    }

    private async void InitializeApp()
    {
        if (string.IsNullOrEmpty(LocalService.GetUsername()))
            await Shell.Current.GoToAsync(nameof(LoginPage), true);
        else if (!(await _db.HasDog()))
            await Shell.Current.GoToAsync(nameof(DogView), true);
        //else
        //{
        //    if (Shell.Current != null)
        //        await Shell.Current.GoToAsync(nameof(AppShell), true);
        //    else
        //        Debug.WriteLine("Shell.Current is null, cannot navigate.");
        //}
    }
}