﻿using Akavache;
using Xamarin.Forms;

namespace Odap
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Registrations.Start("Odap");
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}