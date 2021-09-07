using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Reflection;

namespace DominosPizzaPicker.Client.Helpers.CustomViews
{
    public class CustomContentPage<V> : ContentPage, ICustomPage<V> 
        where V : CustomViewModel
    {
        public V ViewModel { get; set; }
        
        public CustomContentPage()
        {
            // Call InitializeComponent of derived View class and Set ViewModel and BindingContext
            this.Initialize<CustomContentPage<V>, V>();
        }


        // behaves incorrectly if navigating back to page with a Pop

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //ViewModel.Dispose();
        //    ViewModel = null;
        //    GC.Collect();
        //}

        ~CustomContentPage()
        {
            if (ViewModel != null)
            {
                ViewModel.Dispose();
                ViewModel = null;
            }
            GC.Collect();
        }
    }
}
