using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class EditDriverPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly Driver _driver;

        public EditDriverPage(Driver driver)
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _driver = driver;
            UsernameEntry.Text = driver.Username;
            FirstNameEntry.Text = driver.FirstName;
            LastNameEntry.Text = driver.LastName;
            VehicleTypeEntry.Text = driver.VehicleType;
            VehiclePlateNumberEntry.Text = driver.VehiclePlateNumber;
            EmailEntry.Text = driver.Email;
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            _driver.Username = UsernameEntry.Text;
            _driver.FirstName = FirstNameEntry.Text;
            _driver.LastName = LastNameEntry.Text;
            _driver.VehicleType = VehicleTypeEntry.Text;
            _driver.VehiclePlateNumber = VehiclePlateNumberEntry.Text;
            _driver.Email = EmailEntry.Text;
            await _databaseService.UpdateDriverAsync(_driver);
            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
