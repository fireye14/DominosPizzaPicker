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
    public partial class PizzaList : ContentPage
    {
        PizzaManager manager;

        public PizzaList()
        {
            
            InitializeComponent();

            manager = PizzaManager.DefaultManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            await RefreshItems(true, syncItems: true);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator, 1000))
            {
                pizzaList.ItemsSource = await manager.GetAllPizzasAsync(syncItems);
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
