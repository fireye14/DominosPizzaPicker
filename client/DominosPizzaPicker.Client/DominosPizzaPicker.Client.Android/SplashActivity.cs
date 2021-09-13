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
    [Activity(Label = "Pizza Picker", Icon = "@mipmap/pizza", RoundIcon = "@mipmap/pizza_round", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //// Initialize Azure Mobile Apps
            //CurrentPlatform.Init();

            //// Initialize Xamarin Forms
            //Forms.Init(this, savedInstanceState);

            //// Set local db if in emulator
            //var IsEmulator = Build.Fingerprint.Contains("vbox") || Build.Fingerprint.Contains("generic");
            ////IsEmulator = false;

            //// mwj test to use local iis instead of azure
            ////Constants.ApplicationURL = IsEmulator ? @"http://10.0.2.2/DominosPizzaPicker/" : @"https://dominospizzapicker.azurewebsites.net";
            //Constants.ApplicationURL = IsEmulator ? @"http://10.0.2.2/DominosPizzaPicker/" : @"http://192.168.1.13/DominosPizzaPicker/";
            ////Constants.ApplicationURL = IsEmulator ? @"http://10.0.2.2/DominosPizzaPicker/" : @"https://dominospizzapicker-backend.conveyor.cloud/DominosPizzaPicker/";
            //// eom


            //await SimulateStartup();
            StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));

        }

        // Do nothing when back pressed
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        public async Task SimulateStartup()
        {
            var pizzaViewMan = PizzaViewManager.DefaultManager;

            //// this will ensure that all tables are hit - not sure if that makes a difference
            //var p = await pizzaMan.GetRandomUneatenPizza();
            //await p.ToStringAsync();
            CachedData.RecentEatenPizzaCache.Clear();
            CachedData.RecentEatenPizzaCache = await pizzaViewMan.GetRecentAsync();
        }
    }
}