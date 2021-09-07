using DominosPizzaPicker.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers
{
    public static class Messages
    {
        #region Strings
        public static string SuccessfulUpdate { get; set; }

        #endregion


        #region ArgsList

        // name of message and arguments for a DisplayAlert call
        public static Dictionary<string, object[]> ArgsList = new Dictionary<string, object[]>
        {
            // title = "Success", message = "Update Successful!", cancel button = "OK"

            { nameof(SuccessfulUpdate), new []{"Success", "Update Successful!", "OK"} }
        };

        #endregion



    }
}
