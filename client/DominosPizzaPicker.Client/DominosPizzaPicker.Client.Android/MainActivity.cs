using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Essentials;
using Android.Net;
using Android.Net.Wifi;

namespace DominosPizzaPicker.Client.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Initialize Xamarin Forms
            Forms.Init(this, savedInstanceState);


            // Load the main application
            LoadApplication(new App());
        }
    }
}