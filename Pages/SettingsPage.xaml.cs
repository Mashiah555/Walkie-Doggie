using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Walkie_Doggie.ViewModels;

namespace Walkie_Doggie.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private readonly FirebaseService _db;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingViewModel();
            _db = new FirebaseService();
        }
    }
}