using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DominosPizzaPicker.Client.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DominosPizzaPicker.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // The root page of the application
            //MainPage = new SauceList();
            //MainPage = new ToppingList();
            //MainPage = new PizzaList();
            //MainPage = new NavigationPage(new MainMenu());
            //MainPage = new NavigationPage(new SplashLoading());
            MainPage = new SplashLoading();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}