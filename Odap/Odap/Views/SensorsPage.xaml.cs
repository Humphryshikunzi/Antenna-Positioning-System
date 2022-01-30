using Odap.ViewModels;
using Xamarin.Forms;

namespace Odap.Views
{
    public partial class ItemsPage : ContentPage
    {
        SensorsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SensorsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}