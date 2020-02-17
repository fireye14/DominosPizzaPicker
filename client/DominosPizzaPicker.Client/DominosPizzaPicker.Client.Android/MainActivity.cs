using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;

namespace DominosPizzaPicker.Client.Droid
{
    [Activity(Label = "Domino's Pizza Picker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();

            // Initialize Xamarin Forms
            Forms.Init(this, savedInstanceState);

            // Set local db if in emulator
            var f = Build.Fingerprint;
            //Constants.ApplicationURL = f.Contains("vbox") || f.Contains("generic") ? @"http://10.0.2.2/DominosPizzaPicker/" : @"https://dominospizzapicker.azurewebsites.net";
            Constants.ApplicationURL = f.Contains("vbox") || f.Contains("generic") ? @"https://dominospizzapicker.azurewebsites.net" : @"https://dominospizzapicker.azurewebsites.net";

            // Load the main application
            LoadApplication(new App());
        }
    }
}