using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class EditUserPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private User _user;

        public EditUserPage(User user)
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _user = user;
            BindData();
        }

        private void BindData()
        {
            // Bind user data to UI elements (Entry fields, etc.)
            UsernameEntry.Text = _user.Username;
            FirstNameEntry.Text = _user.FirstName;
            LastNameEntry.Text = _user.LastName;
            EmailEntry.Text = _user.Email;
        }

        // Save the updated user data
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _user.Username = UsernameEntry.Text;
            _user.FirstName = FirstNameEntry.Text;
            _user.LastName = LastNameEntry.Text;
            _user.Email = EmailEntry.Text;

            // Update the user in the database
            await _databaseService.UpdateUserAsync(_user);

            // Show success message
            await DisplayAlert("Success", "User updated successfully.", "OK");

            // Go back to the previous page
            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
