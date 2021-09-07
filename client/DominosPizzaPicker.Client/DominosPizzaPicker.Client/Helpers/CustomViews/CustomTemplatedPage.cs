using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Reflection;

namespace DominosPizzaPicker.Client.Helpers.CustomViews
{
    public class CustomTemplatedPage<V> : TemplatedPage, ICustomPage<V> where V : CustomViewModel
    {
        public V ViewModel { get; set; }

        public CustomTemplatedPage()
        {
            this.Initialize<CustomTemplatedPage<V>, V>();
        }
    }
}
