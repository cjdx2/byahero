namespace BiyaHero
{
    public partial class AdminHomePage : ContentPage
    {
        public AdminHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = true;
        }

        private async void OnAddDriverClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverRegisterPage());
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = false;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Home Screen displayed", "You are currently viewing Home.", "Close");
        }

        private async void OnDriverAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverAccountsPage());
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAccountsPage());
        }

        private async void OnAdminLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportsPage());
        }

    }
}
