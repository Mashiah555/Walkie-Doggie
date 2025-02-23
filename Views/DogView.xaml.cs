using System.Threading.Tasks;
using Walkie_Doggie.ViewModels;
namespace Walkie_Doggie.Views;

public partial class DogView : ContentPage
{
    private readonly FirebaseService _db;

    public DogView()
	{
		InitializeComponent();
        BindingContext = new DogViewModel();
        _db = new FirebaseService();
    }
}