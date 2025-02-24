﻿using Walkie_Doggie.Pages;
using Walkie_Doggie.Views;

namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        public string WalksTabIcon { get; set; } = "walks_static.png"; // Default static icon

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AppShell), typeof(AppShell));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(DogView), typeof(DogView));
        }
    }
}
