namespace BiyaHero
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Wrap the LoginPage in a NavigationPage to enable navigation
            // Set properties to remove the navigation bar
            MainPage = new NavigationPage(new LoginPage())
            {
                // These properties work with NavigationPage in MAUI
                BarBackgroundColor = Color.FromHex("#13a073"),  // Transparent background for navigation bar
                BarTextColor = Colors.White,       // Transparent text color for navigation items
                            };

            // Alternatively, if you want to completely hide the navigation bar:
            // MainPage = new LoginPage();
        }
    }
}
