using System.Threading.Tasks;
using Walkie_Doggie.ViewModels;
namespace Walkie_Doggie.Views;

public partial class DogView : ContentPage
{
    private readonly FirebaseService _db;
    private bool _hasDog = false;

    public DogView()
	{
		InitializeComponent();
        BindingContext = new DogViewModel();
        _db = new FirebaseService();

        InitializeAsync();
    }
    private async void InitializeAsync()
    {
        _hasDog = await _db.HasDog();
    }

    protected override bool OnBackButtonPressed()
    {
        return !_hasDog; // Prevents back navigation
    }
}