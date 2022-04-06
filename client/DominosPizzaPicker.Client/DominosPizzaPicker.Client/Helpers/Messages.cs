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

        public static string DuplicateToppingNotAllowed { get; set; }
        public static string NoPizzasExist { get; set; }
        

        #endregion


        #region ArgsList

        // name of message and arguments for a DisplayAlert call
        public static Dictionary<string, string[]> ArgsList = new Dictionary<string, string[]>
        {
            // title = "Success", message = "Update Successful!", cancel button = "OK"

            { nameof(SuccessfulUpdate), new [] { "Success", "Update Successful!", "OK" } },

            { nameof(DuplicateToppingNotAllowed), new [] { "Invalid", "Duplicate Topping Selection Not Allowed.", "OK" } },

            { nameof(NoPizzasExist), new [] { "", "No pizzas exist with the given criteria. Adjust and try again.", "OK" } }

        };

        #endregion



    }
}
