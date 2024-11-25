using BiyaHero.Models;
using BiyaHero.Services;
using System;

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
            BindUserData();
        }

        private void BindUserData()
        {
            UsernameEntry.Text = _user.Username;
            FirstNameEntry.Text = _user.FirstName;
            LastNameEntry.Text = _user.LastName;
            EmailEntry.Text = _user.Email;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _user.Username = UsernameEntry.Text;
            _user.FirstName = FirstNameEntry.Text;
            _user.LastName = LastNameEntry.Text;
            _user.Email = EmailEntry.Text;
            await _databaseService.UpdateUserAsync(_user);
            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
