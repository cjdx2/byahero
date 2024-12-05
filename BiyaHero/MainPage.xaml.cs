using Map = Microsoft.Maui.Controls.Maps.Map;

namespace BiyaHero
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Map map = new Map();
            Content = map;
        }


        protected override void OnAppearing() {
            base.OnAppearing();
            DisplayAlert("Welcome", "Page has appeared!", "OK");
        }
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the Target Page
            await Navigation.PushAsync(new UserHomePage());
        }
    }

}
