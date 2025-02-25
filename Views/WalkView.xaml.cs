using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Views;

public partial class WalkView : ContentPage
{
    private readonly FirebaseService _db;

    public WalkView(int? walkId = null)
	{
		InitializeComponent();

        BindingContext = new WalkViewModel(walkId);
        _db = new FirebaseService();
    }
}