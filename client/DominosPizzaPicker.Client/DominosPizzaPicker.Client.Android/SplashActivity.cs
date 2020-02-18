using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using DominosPizzaPicker.Client.Models.Managers;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Droid
{
    [Activity(Label = "Pizza Picker", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();

            // Initialize Xamarin Forms
            Forms.Init(this, savedInstanceState);

            // Set local db if in emulator
            var IsEmulator = Build.Fingerprint.Contains("vbox") || Build.Fingerprint.Contains("generic");
            //IsEmulator = false;
            Constants.ApplicationURL = IsEmulator ? @"http://10.0.2.2/DominosPizzaPicker/" : @"https://dominospizzapicker.azurewebsites.net";

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();

            StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));
        }

        // Do nothing when back pressed
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        public async void SimulateStartup()
        {            
            var pizzaMan = PizzaManager.DefaultManager;
            await pizzaMan.GetUneatenPizzaCount();            
        }
    }
}