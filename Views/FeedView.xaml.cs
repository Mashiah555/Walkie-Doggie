using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Views;

public partial class FeedView : ContentPage
{
	public FeedView()
	{
		InitializeComponent();

        BindingContext = new FeedViewModel();
    }

    public static TaskCompletionSource<bool>? CompletionSource;
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CompletionSource?.TrySetResult(true);
    }
}