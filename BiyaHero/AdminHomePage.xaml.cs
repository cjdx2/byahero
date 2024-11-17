namespace BiyaHero
{
    public partial class AdminHomePage : ContentPage
    {
        public AdminHomePage()
        {
            InitializeComponent();
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
            await DisplayAlert("Home displayed", "You are currenlty viewing Home screen", "Close");

        }

        private async void OnDriverAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverAccountsPage());
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAccountsPage());
        }

        private async void OnDriverHistoryClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Driver Ride History", "Viewing driver ride history.", "OK");
        }

        private async void OnAddDriverClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverRegisterPage());
        }
    }
}
