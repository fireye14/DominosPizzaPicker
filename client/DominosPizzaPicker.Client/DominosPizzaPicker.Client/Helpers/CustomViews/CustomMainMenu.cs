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
        public Command NavigateCommand { get; private set; }

        public new bool IsBusy { get; set; }

        public CustomMainMenu()
        {
            // Try to find InitializeComponent method of this object's type and invoke it.
            try
            {
                GetType().GetMethod("InitializeComponent", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
            }
            catch { }

            NavigateCommand = new Command<Type>(
                execute: async (Type pageType) =>
                {
                    await Navigation.PushAsync((Page)Activator.CreateInstance(pageType));
                },
                canExecute: (Type a) =>
                {
                    return !IsBusy;
                });

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // remove extended splash screen from navigation stack if it exists
            while (Navigation.NavigationStack[0] != null && Navigation.NavigationStack[0].GetType() != GetType())
                Navigation.RemovePage(Navigation.NavigationStack[0]);
        }
    }
}
