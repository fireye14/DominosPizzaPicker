using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.Controls
{
    public class CustomEntry : Entry
    {
        private void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Entry entry = (Entry)bindable;
            //EventHandler<TextChangedEventArgs> textChanged = entry.TextChanged;
            EventHandler<TextChangedEventArgs> textChanged = TextChanged;
            if (textChanged == null)
            {
                return;
            }
            textChanged(entry, new TextChangedEventArgs((string)oldValue, (string)newValue));

            //entry.TextChanged?.Invoke(entry, new TextChangedEventArgs((string)oldValue, (string)newValue));

        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public CustomEntry()
        {
             //TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Entry), null, BindingMode.TwoWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnTextChanged), null, null, null);
        }
    }
}
