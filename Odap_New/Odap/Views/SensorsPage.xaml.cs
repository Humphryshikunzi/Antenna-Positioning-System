using Plugin.Geolocator;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Odap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsPage : ContentPage
    {
        public SensorsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DetectLocationChangeAsync();
        }

        private async void DetectLocationChangeAsync()
        {

            var locator = CrossGeolocator.Current;
            if (locator.IsListening)
            {
                // check if the geolocator is listening, continue with execution if not.
                return;
            } 
            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(TimeSpan.FromMinutes(0), 0.1);
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var lat = e.Position.Latitude;
            var longi = e.Position.Longitude;
            // send to server here, fala hii
        }
    }
}