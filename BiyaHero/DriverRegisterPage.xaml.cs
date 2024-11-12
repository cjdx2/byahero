namespace BiyaHero
{
    public partial class DriverRegisterPage : ContentPage
    {
        public DriverRegisterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            // Placeholder action for front-end testing
            await DisplayAlert("Success", "Registration successful! You can now log in.", "OK");
        }
    }
}
