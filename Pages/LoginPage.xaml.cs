using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Walkie_Doggie.ViewModels;
using Walkie_Doggie.Views;
namespace Walkie_Doggie.Pages;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseService _db;
    public UserViewModel ViewModel { get; set; }

    public LoginPage()
    {
        InitializeComponent();

        _db = new FirebaseService();
        ViewModel = new UserViewModel();
        BindingContext = ViewModel;

        QueryCollection();
    }

    private async void QueryCollection()
    {
        UsersCollection.ItemsSource = await _db.GetAllUsersAsync();
    }

    private async void ButtonSignUp_Clicked(object sender, EventArgs e)
    {
        string? msg = null;
        string result = await MauiPopup.PopupAction.DisplayPopup(new Popups.EntryPopup(
            "הקלד/י את שמך הפרטי בתיבה.\nהשם הזה ישמש אותך במהלך השימוש במערכת.",
            "שם פרטי",
            Keyboard.Default,
            "הרשמה", "ביטול"));

        if (result is null)
            msg = "ההרשמה נכשלה. נסה להירשם שוב";
        else if (string.IsNullOrEmpty(result))
            msg = "הרשמה בוטלה";
        else if (!string.IsNullOrEmpty(LocalService.GetUsername()))
            msg = "אתה כבר רשום במערכת";
        else if (await _db.HasUserAsync(result))
            msg = "השם הזה כבר קיים במערכת";

        if (msg is not null)
            await Toast.Make(msg, ToastDuration.Long).Show();
        else if (result is not null)
        {
            await _db.AddUserAsync((string)result);
            Login((string)result);
        }
    }

    private void UsersCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is UserModel selectedUser)
        {
            ViewModel.SelectedUser = selectedUser;
            LoginButton.IsEnabled = true;
        }
    }

    private void ButtonLogin_Clicked(object sender, EventArgs e)
    {
        if (ViewModel.SelectedUser is not null)
            Login(ViewModel.SelectedUser.Name);
    }

    private async void Login(string username)
    {
        LocalService.SetUsername(username);
        await Toast.Make("ההרשמה הצליחה", ToastDuration.Short).Show();

        if (!await _db.HasDog())
        {
            await Shell.Current.GoToAsync(nameof(DogView), true);
            Shell.Current.Navigation.RemovePage(this);
        }
        else
            await Shell.Current.GoToAsync("..", true);
    }

    protected override bool OnBackButtonPressed()
    {
        return true; // Prevents back navigation
    }
}
