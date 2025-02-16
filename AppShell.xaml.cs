namespace Walkie_Doggie
{
    public partial class AppShell : Shell
    {
        public string WalksTabIcon { get; set; } = "walks_static.png"; // Default static icon

        public AppShell()
        {
            InitializeComponent();
        }
    }
}
