using Walkie_Doggie.Helpers;

namespace Walkie_Doggie
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly FirebaseService _db;

        public MainPage()
        {
            InitializeComponent();
            _db = new FirebaseService();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            //SemanticScreenReader.Announce(CounterBtn.Text);

            string walkerName = "Yuval"; // Get from UI input
            bool isPooped = true; // Get from UI checkbox
            string notes = "Dog had fun!"; // Get from UI input
            await _db.AddWalkAsync(walkerName, DateTime.Now, isPooped, notes);

            var walks = await _db.GetAllWalksAsync();
            foreach (var walk in walks)
            {
                await DisplayAlert("Walks List", $"{walk.WalkerName} walked on {Converters.ConvertToDateTime(walk.WalkTime)} - Pooped: {walk.IsPooped}", "OK");
            }
        }
    }
}
