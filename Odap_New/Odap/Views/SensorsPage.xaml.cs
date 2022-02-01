using Newtonsoft.Json;
using Odap.Models;
using Odap.Services;
using Plugin.Geolocator;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Odap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorsPage : ContentPage
    {
        SensorsData sensorData;

        private double lat;

        public double Lat
        {
            get { return lat; }
            set
            {
                lat = value;
                OnPropertyChanged(nameof(Lat));
            }
        }

        private double longitude;

        public double Long
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged(nameof(Long));
            }
        }

        private double? alt;

        public double? Alt
        {
            get { return alt; }
            set
            {
                alt = value;
                OnPropertyChanged(nameof(Alt));
            }
        }

        private double? accuracyGPS;

        public double? AccuracyGPS
        {
            get { return accuracyGPS; }
            set
            {
                accuracyGPS = value;
                OnPropertyChanged(nameof(AccuracyGPS));
            }
        }

        private float rssi;

        public float RSSI
        {
            get { return rssi; }
            set
            {
                rssi = value;
                OnPropertyChanged(nameof(RSSI));
            }
        }

        private string directionEastWest;

        public string DirectionEastWest
        {
            get { return directionEastWest; }
            set
            {
                directionEastWest = value;
                OnPropertyChanged(nameof(DirectionEastWest));
            }
        }

        private string directionNorthSouth;

        public string DirectionNorthSouth
        {
            get { return directionNorthSouth; }
            set
            {
                directionNorthSouth = value;
                OnPropertyChanged(nameof(DirectionNorthSouth));
            }
        }



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

        private async void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        { 
             
            Lat =  e.Position.Latitude;
            Long = e.Position.Longitude;
            Alt =  e.Position.Altitude;
            AccuracyGPS = e.Position.Accuracy;
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;

            //check for the direction
            if (Lat.ToString().Contains("-"))
            {
                DirectionNorthSouth = "S";
            }
            else
            {
                DirectionNorthSouth = "N";
            }
            if (Long.ToString().Contains("-"))
            {
                DirectionEastWest = "W";
            }
            else
            {
                DirectionEastWest = "E";
            }


            //unique id for the device for identification
            string deviceId = DeviceInfo.Name + "_" + Guid.NewGuid().ToString();

             
            latValueSpan.Text = Lat.ToString();
            latUnitSpan.Text = DirectionNorthSouth;
            lonValueSpan.Text = Long.ToString();
            lonUnitSpan.Text = DirectionEastWest;
            altValueSpan.Text =  Alt.ToString();
            dateValueSpan.Text = date.ToString();
            timeValueSpan.Text = time.ToString();
            

            //object to hold the data for sending
            var deviceLocation = new SensorsData()
            {
                Lat = Lat,
                Long = Long,
                Alt = Alt, // in meters
                DirectionNorthSouth = DirectionNorthSouth,
                DirectionEastWest = directionEastWest,
                UniqueDeviceId = deviceId,
                AccuracyGPS = AccuracyGPS,
                Date = date,
                Time = time
            };

            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();
            }
            else
            {
                HttpClient _httpClient = new HttpClient();
                var jsonDataUser = JsonConvert.SerializeObject(deviceLocation);
                var httpcontent = new StringContent(jsonDataUser);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //await _httpClient.PostAsync(Constants.BaseUrl + Constants.MessageUrl, httpcontent);
            }
            //wait for 5 seconds before responding to the next event fired for location change
            await Task.Delay(5000);
        }
    }
}