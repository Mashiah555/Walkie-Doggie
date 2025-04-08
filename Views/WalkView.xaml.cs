using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Views;

public partial class WalkView : ContentPage
{
	public WalkView()
	{
		InitializeComponent();

        BindingContext = new WalkViewModel();
    }

    public static TaskCompletionSource<bool>? CompletionSource;
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CompletionSource?.TrySetResult(true);
    }
}