using AfriLearn.Services;
using Akavache;
using Newtonsoft.Json;
using Odap.Models;
using Odap.Services;
using Plugin.Geolocator;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
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
            //Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

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

        private ObservableCollection<Item> LoadItems()
        {
            Items = new ObservableCollection<Item>()
            {
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Lat", Description="UE Latitude", Value=0, Unit="" },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Long", Description="UE Longitude", Value=0, Unit= "" },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Alt", Description="UE Altitude" , Value = 0, Unit="m"},
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Acc X", Description="Acceleration on X Axis", Value=0 },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Acc Y", Description="Acceleration on Y Axis", Value=0 },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Acc Z", Description="Acceleration on Z Axis", Value=0 },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "RSSI", Description="The UE received Signal Strength from the BTS", Value=0f, Unit="dB" }
            };
            return Items;
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
            locator.PositionChanged += Locator_PositionChanged; 
        }

        private  void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            //read the location of device
            var location = e.Position;
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

            Items = new ObservableCollection<Item>()
            {
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Lat", Description="UE Latitude", Value=(float)Lat, Unit=directionNorthSouth },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Long", Description="UE Longitude", Value=(float)Long, Unit=directionEastWest },
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "Alt", Description="UE Altitude" , Value = (float)Alt, Unit="m"},               
                new Item { UniqueItemKey = Guid.NewGuid().ToString(), Text = "RSSI", Description="The UE received Signal Strength from the BTS", Value=-89f, Unit="dB" }
            };

            //unique id for the device for identification
            string deviceId = DeviceInfo.Name + "_" + Guid.NewGuid().ToString();
            /*
            try
            {
                var deviceIdStore = BlobCache.UserAccount.GetObject<string>("deviceId");
                deviceId = deviceIdStore.ToString();
            }
            catch (Exception)
            {
                deviceId = DeviceInfo.Name + "_" + Guid.NewGuid().ToString();
                await BlobCache.UserAccount.InsertObject("uniqueDeviceId", deviceId);

            }*/
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

            var httpClientService = new HttpClientService();
            if (!InternetService.Internet())
            {
                InternetService.NoInternet();
            }
            else
            {
                HttpClient _httpClient = new HttpClient();
                var jsonDataUser = JsonConvert.SerializeObject(deviceLocation);
                var httpcontent = new StringContent(jsonDataUser);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response =  _httpClient.PostAsync(Constants.BaseUrl+Constants.MessageUrl, httpcontent); 
            }            
            //wait for 5 seconds before reponding to the next event
            //fired for location change
             Task.Delay(5000);
        }

        private void GetCellInfor()
        {
             

        }
    }
}