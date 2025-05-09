using System.Threading.Tasks;
using Walkie_Doggie.Services;
using Walkie_Doggie.Views;
using static Walkie_Doggie.Helpers.Converters;
namespace Walkie_Doggie.Pages;

public partial class DogPage : ContentPage
{
    public DogPage()
    {
        InitializeComponent();
        

        InitializeDogData();
        InitializeLastWalkData();
        InitializeLastFeedData();
    }

    #region Data Initialization
    private async void InitializeDogData()
    {
        try
        {
            DogModel dog = await DbService.Dogs.GetAsync();
            //DogNameLabel.Text = dog.DogName;
        }
        catch (Exception ex)
        {
            //await DisplayAlert("InitializeDogData", 
            //    ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }

    private async void InitializeLastWalkData()
    {
        try
        {
            WalkModel lastWalk = await DbService.Walks.GetLastWalkAsync();

            LastWalkTime.Text = "טייל בפעם האחרונה בשעה " +
                ConvertToString(lastWalk.WalkTime);

            LastWalkPassedTime.Text = "עברו מאז " + ConvertToString(
                DateTime.UtcNow - lastWalk.WalkTime.ToDateTime()) + " שעות";

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
        catch (Exception ex)
        {
            //await DisplayAlert("InitializeLastWalkData", 
            //    ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }

    private async void InitializeLastFeedData()
    {
        try
        {
            FeedModel? lastFeed = await DbService.Feeds.GetLastFeedAsync();

            LastFeedTime.Text = "קיבל אוכל בפעם האחרונה בשעה " +
                ConvertToString(lastFeed.FeedTime);

            LastFeedPassedTime.Text = "עברו מאז " + ConvertToString(
                DateTime.UtcNow - ConvertToDateTime(lastFeed.FeedTime)) + " שעות";

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
        catch (Exception ex)
        {
            //await DisplayAlert("InitializeLastFeedData",
            //    ex.Message, "סגירה", FlowDirection.RightToLeft);
        }
    }
    #endregion Data Initialization

    #region Button Click Events
    private async void ButtonWalk_Clicked(object sender, EventArgs e)
    {
        WalkView.CompletionSource = new TaskCompletionSource<bool>();
        await Shell.Current.GoToAsync(nameof(WalkView), true);
        await WalkView.CompletionSource.Task; // Holds position until WalkView is closed

        InitializeLastWalkData();
    }

    private async void ButtonFeed_Clicked(object sender, EventArgs e)
    {
        FeedView.CompletionSource = new TaskCompletionSource<bool>();
        await Shell.Current.GoToAsync(nameof(FeedView), true);
        await FeedView.CompletionSource.Task; // Holds position until FeedView is closed

        InitializeLastFeedData();
    }

    private async void EditDogButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(DogView), true);
    }

    #endregion Button Click Events
}
