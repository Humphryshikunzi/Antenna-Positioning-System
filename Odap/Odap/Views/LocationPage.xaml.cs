using Odap.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Odap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {

        SensorsViewModel vm = new SensorsViewModel();
        public LocationPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing(); 
        }
    }
}