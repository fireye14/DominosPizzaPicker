using DominosPizzaPicker.Client.Models.Managers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashLoading : ContentPage
    {
        public SplashLoading()
        {
            InitializeComponent();

            LoadResources();
        }

        // void method called from View constructor (can't be awaited)
        public async void LoadResources()
        {
            await LoadResourcesAsync();
            Application.Current.MainPage = new NavigationPage(new MainMenu()) { BarTextColor = Color.Black};
        }

        public async Task LoadResourcesAsync()
        {
            Constants.ApplicationURL = Constants.LocalIISURL;

            var pizzaViewMan = PizzaViewManager.DefaultManager;

            lblLoading.Text = "Connecting using Local IIS URL...";
            if (!await pizzaViewMan.PingConnection())
            {
                lblLoading.Text = "Failed. Attempting connection with Conveyor URL...";
                Constants.ApplicationURL = Constants.ConveyorURL;
                pizzaViewMan.SetConnection();
                if (!await pizzaViewMan.PingConnection())
                {
                    Constants.ApplicationURL = "NOT CONNECTED";
                    return;
                }
            }

            CachedData.RecentEatenPizzaCache.Clear();
            CachedData.RecentEatenPizzaCache = await pizzaViewMan.GetRecentAsync();



            // test longer load times
            //if (CachedData.RecentEatenPizzaCache == null)
            //    return;
            //for (var i = 0; i < 19; i++)
            //{
            //    CachedData.RecentEatenPizzaCache.Clear();
            //    CachedData.RecentEatenPizzaCache = await pizzaViewMan.GetRecentAsync();
            //}
        }
    }
}