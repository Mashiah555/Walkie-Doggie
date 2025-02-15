using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
namespace Walkie_Doggie.Pages;

public partial class DogPage : ContentPage
{
    private readonly FirebaseService _db;

    public DogPage()
    {
        InitializeComponent();
        _db = new FirebaseService();

        InitializeLastWalkData();
        InitializeLastFeedData();
    }

    #region Data Initialization
    private async void InitializeLastWalkData()
    {
        var lastWalk = await _db.GetLastWalkAsync();

        LastWalkTime.Text = "טייל בפעם האחרונה בשעה " + 
            Converters.ConvertToString(lastWalk.WalkTime);

        LastWalkPassedTime.Text = "עברו מאז " + Converters.ConvertToString(
            DateTime.UtcNow - Converters.ConvertToDateTime(lastWalk.WalkTime)) + " שעות";

        LastWalkPooped.Text = lastWalk.IsPooped ? "לואי עשה קקי בטיול" : "לואי לא עשה קקי";

        LastWalker.Text = lastWalk.WalkerName;

        if (lastWalk.Notes is not null)
        {
            WalkNotesFrame.IsVisible = true;
            LastWalkNotes.Text = lastWalk.Notes;
        }
        else
            WalkNotesFrame.IsVisible = false;
    }

    private async void InitializeLastFeedData()
    {
        var lastFeed = await _db.GetLastFeedAsync();

        LastFeedTime.Text = "קיבל אוכל בפעם האחרונה בשעה " +
            Converters.ConvertToString(lastFeed.FeedTime);

        LastFeedPassedTime.Text = "עברו מאז " + Converters.ConvertToString(
            DateTime.UtcNow - Converters.ConvertToDateTime(lastFeed.FeedTime)) + " שעות";

        LastFeedAmount.Text = lastFeed.FeedAmount.ToString() + " גרם";

        LastFeeder.Text = lastFeed.FeederName;

        if (lastFeed.Notes is not null)
        {
            FeedNotesFrame.IsVisible = true;
            LastFeedNotes.Text = lastFeed.Notes;
        }
        else
            FeedNotesFrame.IsVisible = false;
    }
    #endregion Data Initialization

    #region Button Click Events
    private void ButtonWalk_Clicked(object sender, EventArgs e)
    {

    }

    private void ButtonFeed_Clicked(object sender, EventArgs e)
    {

    }

    #endregion Button Click Events
}
