using AfriLearn.Services;
using Newtonsoft.Json;
using Odap.Models;
using Odap.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Odap.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RControl : ContentPage
    {
        public RControl()
        {
            InitializeComponent();
        }

        private async void rControlBtn_Clicked(object sender, EventArgs e)
        {
            alertLbl.IsVisible = false;
            int angle;
            var angleValid = int.TryParse(rControlValueEntry.Text,out angle);
            if (angleValid==false)
            {
                NavigationService.DisplayAlert("Error", "Enter valid integers only","Okay");
                return;
            }
            var rControlAngle = new RAngle()
            {
                Id = 0,
                Angle =  angle
            };

            if (!InternetService.Internet())
            {
                await InternetService.NoInternet();
            }
            else
            {
                HttpClient _httpClient = new HttpClient();
                var jsonDataUser = JsonConvert.SerializeObject(rControlAngle);
                var httpcontent = new StringContent(jsonDataUser);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _httpClient.PostAsync(Constants.BaseUrl + Constants.MessageUrl, httpcontent);
                alertLbl.IsVisible = true;
            }
        }
    }
}