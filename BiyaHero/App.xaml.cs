namespace BiyaHero
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromArgb("#13a073"),
                BarTextColor = Colors.White,
                            };
        }
    }
}
