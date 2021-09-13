using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.Controls
{
    public class CustomViewCell : ViewCell
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.View.BackgroundColor = Color.FromHex("#B53F3F");
        }
    }

    public class CustomButton : Button, IBorderElement
    {
        bool IBorderElement.IsBorderWidthSet()
        {
            return false;
        }
    }
}
