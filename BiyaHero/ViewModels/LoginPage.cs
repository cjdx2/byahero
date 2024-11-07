using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class LoginPage : ContentPage
    {

        // Event handler for the Register button click
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the RegisterPage
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}

