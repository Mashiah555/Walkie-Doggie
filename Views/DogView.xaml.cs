using System.Threading.Tasks;
using Walkie_Doggie.Services;
using Walkie_Doggie.ViewModels;
namespace Walkie_Doggie.Views;

public partial class DogView : ContentPage
{
    
    private bool _hasDog = false;

    public DogView()
	{
		InitializeComponent();
        BindingContext = new DogViewModel();
        

        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        _hasDog = await DbService.Dogs.HasDogAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        return !_hasDog; // Prevents back navigation
    }
}