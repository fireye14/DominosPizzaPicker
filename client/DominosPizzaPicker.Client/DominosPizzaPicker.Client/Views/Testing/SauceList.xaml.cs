using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SauceList : ContentPage
    {
        SauceManager manager;

        public SauceList()
        {
            InitializeComponent();

            manager = SauceManager.DefaultManager;
            //if (Device.RuntimePlatform == Device.UWP)
            //{
            //    var refreshButton = new Button
            //    {
            //        Text = "Refresh",
            //        HeightRequest = 30
            //    };
            //    refreshButton.Clicked += OnRefreshItems;
            //    buttonsPanel.Children.Add(refreshButton);
            //    if (manager.IsOfflineEnabled)
            //    {
            //        var syncButton = new Button
            //        {
            //            Text = "Sync items",
            //            HeightRequest = 30
            //        };
            //        syncButton.Clicked += OnSyncItems;
            //        buttonsPanel.Children.Add(syncButton);
            //    }
            //}
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                sauceList.ItemsSource = await manager.GetSaucesAsync(syncItems);
            }
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        private void OnComplete(object sender, EventArgs e)
        {
            // not used
        }
    }

}
