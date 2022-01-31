using Odap.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Odap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
    }
}