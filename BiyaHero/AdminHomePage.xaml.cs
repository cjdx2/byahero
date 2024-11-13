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
            await DisplayAlert("Home", "Navigating to Home.", "OK");
            // Logic for navigating to the Home content can go here
        }

        private async void OnDriverAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverAccountsPage()); // Navigate to DriverAccountsPage
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("User Accounts", "Viewing user accounts.", "OK");
            // Logic for navigating to User Accounts Management content can go here
        }

        private async void OnDriverHistoryClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Driver Ride History", "Viewing driver ride history.", "OK");
            // Logic for navigating to Driver Ride History content can go here
        }
    }
}
