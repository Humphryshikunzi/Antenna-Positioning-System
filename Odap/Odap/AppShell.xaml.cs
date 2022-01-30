using System;
using Xamarin.Forms;

namespace Odap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent(); 
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

