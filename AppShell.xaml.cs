namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        private bool _isAnimating = false;
        public string WalksTabIcon { get; set; } = "walks_static.png"; // Default static icon

        public AppShell()
        {
            InitializeComponent();
        }
    }
}
