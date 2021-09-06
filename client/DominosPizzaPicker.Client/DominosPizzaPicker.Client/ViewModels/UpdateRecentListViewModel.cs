using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using DominosPizzaPicker.Client.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class UpdateRecentListViewModel : CustomViewModel
    {
        #region Fields
        PizzaViewManager pizzaViewMan;
        // for some reason, background colors appear on top of the tap animation. make semi-transparent to be able to see it sort of
        Color evenColor = Color.White.MultiplyAlpha(0.7);
        Color oddColor = Color.FromHex("#3F51B5");
        Color needsUpdate = Color.FromHex("#B53F3F");
        #endregion


        #region Properties

        private NamedPizza _selectedPizza;
        public virtual NamedPizza SelectedPizza
        {
            get { return _selectedPizza; }
            set { SetProperty(ref _selectedPizza, value); }
        }        

        public ObservableCollection<NamedPizza> PizzaList { get; set; }

        #endregion

        #region Constructor
        public UpdateRecentListViewModel()
        {
            pizzaViewMan = PizzaViewManager.DefaultManager;
            PizzaList = new ObservableCollection<NamedPizza>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get most recently eaten pizzas 
        /// </summary>
        public void LoadList()
        {
            if (CachedData.RecentEatenPizzaCache == null || CachedData.RecentEatenPizzaCache.Count == 0)
                return;

            // init token source and token
            Cancellation.Init();

            try
            {
                using (var scope = new ActivityIndicatorScope(ActivityIndicator, showIndicator: true, indicatorDelayMs: 500))
                {
                    PizzaList.Clear();
                    //var pizzaList = await pizzaViewMan.GetRecentAsync();
                    //var pizzaList = CachedData.RecentEatenPizzaCache;

                    foreach (var p in CachedData.RecentEatenPizzaCache)
                    {

                        // For below, checking if cancel requested then breaking would probably be better (commented out code) than throwing an exception, but it only works if in a loop, so get used to the try/catch/finally pattern

                        // throw exception if there has been a request to cancel
                        Cancellation.ThrowIfCancellationRequested();

                        //if (Cancellation.IsCancellationRequested)
                        //{
                        //    PizzaList.Clear();
                        //    break;
                        //}

                        PizzaList.Add(new NamedPizza
                        {
                            Id = p.Id,
                            //SauceName = await p.GetSauceName(),
                            //Topping1Name = await p.GetTopping1Name(),
                            //Topping2Name = await p.GetTopping2Name(),
                            //Topping3Name = await p.GetTopping3Name(),
                            SauceName = p.Sauce,
                            Topping1Name = p.Topping1,
                            Topping2Name = p.Topping2,
                            Topping3Name = p.Topping3,
                            DateEatenText = p.DateEaten.Date.ToString("M/d/yy"),
                            RatingText = p.Rating == 0 ? "No Rating" : p.Rating.ToString(),
                            CommentText = string.IsNullOrEmpty(p.Comment) ? "No Comment" : p.Comment,
                            RowColor = p.Rating == 0 || string.IsNullOrEmpty(p.Comment) ? needsUpdate : (PizzaList.Count % 2 == 0 ? evenColor : oddColor)
                        });
                    }
                }
            }
            catch (OperationCanceledException)
            {
                PizzaList.Clear();
            }
            finally
            {
                Cancellation.Dispose();                
            }
        }

        public async void OnSelectedPizzaChanged()
        {
            Cancellation.Cancel();
            await Navigation.PushAsync(new UpdatePizza(SelectedPizza.Id));
        }

        #endregion

        #region Overrides

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPizza))
            {
                OnSelectedPizzaChanged();
            }
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();
            UpdateAnotherPizzaCommand = new Command(
                execute: async () =>
                {
                    Cancellation.Cancel();
                    await Navigation.PushAsync(new UpdateSelectSpecific());
                },
                canExecute: () =>
                {
                    return true;
                });

            Commands.Add(UpdateAnotherPizzaCommand);
        }

        #endregion

        #region Commands

        public Command UpdateAnotherPizzaCommand { get; private set; }
        #endregion
    }

    // used for list view item source
    public class NamedPizza
    {
        public string Id { get; set; }
        public string SauceName { get; set; }
        public string Topping1Name { get; set; }
        public string Topping2Name { get; set; }
        public string Topping3Name { get; set; }
        public string DateEatenText { get; set; }
        public string RatingText { get; set; }
        public string CommentText { get; set; }
        public Color RowColor { get; set; }
    }
}
