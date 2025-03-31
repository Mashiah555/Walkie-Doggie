using Google.Cloud.Firestore.V1;
using System.Diagnostics;
using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        private readonly FirebaseService _db;
        public AppShell()
        {
            InitializeComponent();
            _db = new FirebaseService();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(DogView), typeof(DogView));
            Routing.RegisterRoute(nameof(WalkView), typeof(WalkView));
            Routing.RegisterRoute(nameof(FeedView), typeof(FeedView));
        }
    }
}
