using Newtonsoft.Json;
using Odap.Models;
using Odap.Services;
using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace Odap.ViewModels
{
    public class SensorsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        SensorSpeed speed = SensorSpeed.UI;
        public ObservableCollection<Item> Items { get; set; }
        CancellationTokenSource cts;
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

        private  double?  alt;

        public  double?  Alt
        {
            get { return  alt; }
            set 
            {
                alt = value;
                OnPropertyChanged(nameof(Alt));
            }
        }

        private  double?  accuracyGPS;

        public  double?  AccuracyGPS
        {
            get { return  accuracyGPS; }
            set 
            {
                accuracyGPS = value;
                OnPropertyChanged(nameof(AccuracyGPS));
            }
        }


        private float accX;

        public float AccX
        {
            get { return accX; }
            set
            {
                accX = value;
                OnPropertyChanged(nameof(AccX));
            }
        }

        private float accY;

        public float AccY
        {
            get { return accY; }
            set
            {
                accY = value;
                OnPropertyChanged(nameof(AccY));
            }
        }

        private float accZ;

        public float AccZ
        {
            get { return accZ; }
            set
            {
                accZ = value;
                OnPropertyChanged(nameof(AccZ));
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

        private  string directionEastWest;

        public  string DirectionEastWest
        {
            get { return directionEastWest; }
            set 
            {
                directionEastWest = value;
                OnPropertyChanged(nameof(DirectionEastWest));
            }
        }

        private  string directionNorthSouth;

        public  string DirectionNorthSouth
        {
            get { return directionNorthSouth; }
            set 
            {
                directionNorthSouth = value;
                OnPropertyChanged(nameof(DirectionNorthSouth));
            }
        }


        public SensorsViewModel()
        {
            Title = "Sensors";
            //ToggleAccelerometer();
            //Items = new ObservableCollection<Item>();
            sensorData = new SensorsData();
             
            InitializeGeolocation();
            // Register for reading changes, be sure to unsubscribe when finished
            // Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

       
        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            //get acceleration reading
            var accData = e.Reading;
            sensorData.AccX = accData.Acceleration.X;
            sensorData.AccY = accData.Acceleration.Y;
            sensorData.AccZ = accData.Acceleration.Z;

            //get gps location
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            //var location = await Geolocation.GetLocationAsync(request, cts.Token);
            var location = await Geolocation.GetLastKnownLocationAsync();
           
            // send the data to raspbery pi here
            // await Task.Delay(5000);
        }
        private async void InitializeGeolocation()
        {
            var locator = CrossGeolocator.Current;
            if (locator.IsListening)
            {
                // check if the geolocator is listening, continue with execution if not.
                return;
            }
            await locator.StartListeningAsync(TimeSpan.FromSeconds(0), 0.1);
           // locator.PositionChanged += Locator_PositionChanged;
        }        

        public async void Locator_PositionChanged()
        {
            //read the location of device
            var location = await Geolocation.GetLastKnownLocationAsync();
            Lat = location.Latitude;
            Long = location.Longitude;
            Alt = location.Altitude;
            AccuracyGPS = location.Accuracy;              

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
           
            //object to hold the data for sending
            var deviceLocation = new SensorsData()
            {
                Lat = Lat,
                Long = Long,
                Alt = Alt, // in meters
                DirectionNorthSouth = DirectionNorthSouth,
                DirectionEastWest = directionEastWest,
                UniqueDeviceId = deviceId,
                AccuracyGPS = AccuracyGPS
            };
             
            if (!InternetService.Internet())
            {
              await  InternetService.NoInternet();
            }
            else
            {
                HttpClient _httpClient = new HttpClient();
                var jsonDataUser = JsonConvert.SerializeObject(deviceLocation);
                var httpcontent = new StringContent(jsonDataUser);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await  _httpClient.PostAsync(Constants.BaseUrl+Constants.MessageUrl, httpcontent); 
            }            
            //wait for 5 seconds before responding to the next event
            //fired for location change
            await  Task.Delay(5000);
        }

        private void GetCellInfor()
        {
             

        }
    }
}