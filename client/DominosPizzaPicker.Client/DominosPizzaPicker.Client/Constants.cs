using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client
{
    public static class Constants
    {

        private static string _applicationURL;

        //public static string ApplicationURL = @"https://bitchtitsactivitieswebapp.azurewebsites.net";
        /// <summary>
        /// http://10.0.2.2/DominosPizzaPicker/
        /// </summary>
        public static string ApplicationURL
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationURL))
                {
                    _applicationURL = @"http://10.0.2.2/DominosPizzaPicker/";
                }
                return _applicationURL;
            }
            set
            {
                _applicationURL = value;
            }
        }
    }
}
