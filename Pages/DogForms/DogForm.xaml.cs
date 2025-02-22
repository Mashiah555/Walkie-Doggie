namespace Walkie_Doggie.Pages.DogForms;

public partial class DogForm : ContentPage
{
	public DogForm()
	{
		InitializeComponent();
        BindingContext = new ViewModels.DogViewModel();
    }
}