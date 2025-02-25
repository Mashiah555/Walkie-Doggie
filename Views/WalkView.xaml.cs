using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Views;

public partial class WalkView : ContentPage
{
    private readonly FirebaseService _db;

    public WalkView()
	{
		InitializeComponent();

        BindingContext = new WalkViewModel();
        _db = new FirebaseService();
    }
}