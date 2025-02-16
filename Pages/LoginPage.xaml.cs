using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Walkie_Doggie.Pages;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseService _db;

    public LoginPage()
	{
		InitializeComponent();
        _db = new FirebaseService();

        QueryCollection();
    }

	private async void QueryCollection()
	{
        UsersCollection.ItemsSource = await _db.GetAllUsersAsync();
    }

    private async void ButtonSignUp_Clicked(object sender, EventArgs e)
    {
		var result = await this.ShowPopupAsync(new Popups.EntryPopup(
			"הקלד/י את שמך הפרטי בתיבה.\nהשם הזה ישמש אותך במהלך השימוש במערכת.",
			"שם פרטי",
			Keyboard.Default,
			"הרשמה", "ביטול"));

		string msg;
		if (result is null)
			msg = "ההרשמה בוטלה";
		else if (string.IsNullOrEmpty((string)result))
			msg = "ההרשמה נכשלה. נסה להירשם שוב";
		else if (LocalService.GetUsername() is not null)
            msg = "אתה כבר רשום במערכת";
        else
        {
            LocalService.SaveUsername((string)result);
			await _db.AddUserAsync((string)result);
            msg = "ההרשמה הצליחה";
        }

		await Toast.Make(msg, ToastDuration.Long).Show();
    }
}