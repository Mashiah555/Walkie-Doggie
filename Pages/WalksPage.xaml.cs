using CommunityToolkit.Maui.Views;
using Google.Cloud.Firestore;
using System.Diagnostics;
using Walkie_Doggie.Services;
using Walkie_Doggie.Popups;
using Walkie_Doggie.Views;
using static Walkie_Doggie.Helpers.Converters;
namespace Walkie_Doggie.Pages;

public partial class WalksPage : ContentPage
{
    private readonly FirebaseService _db;

    public WalksPage()
	{
		InitializeComponent();
        _db = new FirebaseService();

        // Initialize the app and check if the user is logged in,
        // and if they have a dog registered.
        // If not, navigate to the appropriate page.
        NavigationWizard.InitializeAppAsync();

        InitializeLastWalkData();
    } 

    private async void InitializeLastWalkData()
    {
        try
        {
            WalkModel lastWalk = await _db.GetLastWalkAsync(LocalService.GetUsername());

            LastWalkTime.Text = "טייל בפעם האחרונה בשעה " +
                ConvertToString(lastWalk.WalkTime);

            LastWalkPassedTime.Text = "עברו מאז " + ConvertToString(
                DateTime.UtcNow - ConvertToDateTime(lastWalk.WalkTime)) + " שעות";

            LastWalkPooped.Text = lastWalk.IsPooped ? "לואי עשה קקי בטיול" : "לואי לא עשה קקי";

            if (lastWalk.Notes is not null)
            {
                WalkNotesFrame.IsVisible = true;
                LastWalkNotes.Text = lastWalk.Notes;
            }
            else
                WalkNotesFrame.IsVisible = false;
        }
        catch (Exception ex)
        {
            //await DisplayAlert("InitializeLastWalkData",
            //    ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }
}