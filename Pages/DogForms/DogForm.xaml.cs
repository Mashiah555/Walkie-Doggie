using System.Threading.Tasks;
using Walkie_Doggie.ViewModels;
namespace Walkie_Doggie.Pages.DogForms;

public partial class DogForm : ContentPage
{
    private readonly FirebaseService _db;

    public DogForm()
	{
		InitializeComponent();
        BindingContext = new DogViewModel();
        _db = new FirebaseService();
    }
}