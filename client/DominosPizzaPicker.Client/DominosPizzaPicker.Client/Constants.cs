using Android.OS;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client
{
    public static class Constants
    {

        /// <summary>
        /// 
        /// </summary>
        private static string _applicationURL;
        public static string ApplicationURL
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationURL))
                {
                    //_applicationURL = @"http://10.0.2.2/DominosPizzaPicker/";
                    _applicationURL = LocalIISURL;
                }

                return _applicationURL;
            }
            set
            {
                _applicationURL = value;
            }
        }

        public static bool IsEmulator = Build.Fingerprint.Contains("vbox") || Build.Fingerprint.Contains("generic");
        public static string EmulatorURL = @"http://10.0.2.2/DominosPizzaPicker/";


        // If Local IIS URL randomly stops working, it probably means that the ISP changed the IP.
        // go into internet settings, find the active internet connection, look for the IPv4 address and change this property based on the value
        // or open a command prompt and type "ipconfig" and find the IPv4 Address there
        // once this is changed, rebuild and redeploy to device
        public static string LocalIISURL = @"http://192.168.1.69/DominosPizzaPicker/";


        public static string ConveyorURL = @"https://dominospizzapicker-backend.conveyor.cloud/DominosPizzaPicker/";

        public static int RecentEatenListCount { get { return 50; } }

        //var c = Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi);
        //Application.Context.GetSystemService(ConnectivityService) as ConnectivityManager
        //if (GetSystemService(ConnectivityService) is ConnectivityManager s)
        //{
        //    var ni = s.ActiveNetworkInfo;
        //    if (ni != null && ni.IsConnectedOrConnecting && ni.Type == ConnectivityType.Wifi)
        //    {
        //        if (GetSystemService(WifiService) is WifiManager wifi)
        //        {
        //            if (wifi.StartScan())
        //            {
        //                var r = wifi.ScanResults;
        //            }

        //            Constants.ApplicationURL = wifi.ConnectionInfo.SSID.Contains("jattmonsoon") ? @"http://192.168.1.13/DominosPizzaPicker/" : @"https://dominospizzapicker-backend.conveyor.cloud/DominosPizzaPicker/";
        //        }
        //    }
        //}
    }
}
