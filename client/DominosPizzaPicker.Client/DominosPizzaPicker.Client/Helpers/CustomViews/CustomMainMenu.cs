using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.CustomViews
{
    /// <summary>
    /// include the following in button xaml: Command="{Binding NavigateCommand}" CommandParameter="{x:Type local:<type of view to navigate to>}"
    /// </summary>
    public class CustomMainMenu : ContentPage
    {
        // use single command to navigate to any available page from main menu
        // include the following in button xaml: Command="{Binding NavigateCommand}" CommandParameter="{x:Type local:<type of view to navigate to>}"
        public ICommand NavigateCommand { get; private set; }

        public CustomMainMenu()
        {
            // Try to find InitializeComponent method of this object's type and invoke it.
            try
            {
                GetType().GetMethod("InitializeComponent", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
            }
            catch { }

            NavigateCommand = new Command<Type>(
                async (Type pageType) =>
                {
                    await Navigation.PushAsync((Page)Activator.CreateInstance(pageType));
                });

            BindingContext = this;
        }
    }
}
