namespace BiyaHero
{
    public partial class AdminHomePage : ContentPage
    {
        public AdminHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // Toggle sidebar visibility when Menu button is clicked
        private void OnMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = true;
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = false;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Home", "Navigating to Home.", "OK");
            // Logic for navigating to the Home content can go here
        }

        private async void OnDriverAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverAccountsPage()); // Navigate to DriverAccountsPage
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAccountsPage()); // Navigate to UserAccountsPage
        }
        private async void OnAdminLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage()); // Navigate to LoginPage
        }

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportsPage()); // Navigate to ReportsPage
        }

    }
}
