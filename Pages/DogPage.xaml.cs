using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
namespace Walkie_Doggie.Pages;

public partial class DogPage : ContentPage
{
    private readonly FirebaseService _db;

    public string LastWalk { get; set; } = "אין עדיין טיולים במערכת";

    public DogPage()
    {
        InitializeComponent();
        _db = new FirebaseService();

        InitializeData();
    }

    private async void InitializeData()
    {
        var lastWalk = await _db.GetLastWalkAsync();

        LastWalkTime.Text = "טייל בפעם האחרונה בשעה " + 
            Converters.ConvertToString(lastWalk.WalkTime);

        LastWalkPassedTime.Text = "עברו מאז " + Converters.ConvertToString(
            DateTime.UtcNow - Converters.ConvertToDateTime(lastWalk.WalkTime)) + " שעות";

        LastWalkPooped.Text = lastWalk.IsPooped ? "לואי עשה קקי בטיול" : "לואי לא עשה קקי";

        LastWalkWalker.Text = lastWalk.WalkerName;

        if (lastWalk.Notes is not null)
        {
            NotesFrame.IsVisible = true;
            LastWalkNotes.Text = lastWalk.Notes;
        }
        else
            NotesFrame.IsVisible = false;
    }

    private void ButtonWalk_Clicked(object sender, EventArgs e)
    {

    }
}
