using Google.Cloud.Firestore.V1;
using System.Diagnostics;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;
using Walkie_Doggie.Popups;

namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(DogView), typeof(DogView));
            Routing.RegisterRoute(nameof(WalkView), typeof(WalkView));
            Routing.RegisterRoute(nameof(FeedView), typeof(FeedView));
            Routing.RegisterRoute(nameof(MessagePopup), typeof(MessagePopup));

        }
    }
}
