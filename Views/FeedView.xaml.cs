using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Views;

public partial class FeedView : ContentPage
{
	public FeedView()
	{
		InitializeComponent();

        BindingContext = new FeedViewModel();
    }
}