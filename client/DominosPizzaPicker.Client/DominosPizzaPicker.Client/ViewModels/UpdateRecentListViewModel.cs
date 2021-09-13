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
        readonly Color evenColor = Color.White.MultiplyAlpha(0.7);
        readonly Color oddColor = Color.FromHex("#3F51B5");
        readonly Color needsUpdate = Color.FromHex("#B53F3F");

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

                    foreach (var p in CachedData.RecentEatenPizzaCache)
                    {

                        // For below, checking if cancel requested then breaking (commented out code) would probably be better than throwing an exception, but it only works if in a loop, so get used to the try/catch/finally pattern

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
                            SauceName = p.Sauce,
                            Topping1Name = p.Topping1,
                            Topping2Name = p.Topping2,
                            Topping3Name = p.Topping3,
                            DateEatenText = p.DateEaten.Date.ToString("M/d/yy"),
                            RatingText = p.Rating == 0 ? "No Rating" : p.Rating.ToString(),
                            CommentText = string.IsNullOrEmpty(p.Comment) ? "No Comment" : p.Comment,
                            //RowColor = p.Rating == 0 || string.IsNullOrEmpty(p.Comment) ? needsUpdate : (PizzaList.Count % 2 == 0 ? evenColor : oddColor)
                            RowColor = p.Rating == 0 || string.IsNullOrEmpty(p.Comment) ? needsUpdate : oddColor,

                            RowRippleColor = p.Rating == 0 || string.IsNullOrEmpty(p.Comment) ? Color.YellowGreen : Color.HotPink
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

        public void OnSelectedPizzaChanged()
        {
            if (!string.IsNullOrEmpty(SelectedPizza?.Id) && UpdateAnotherPizzaCommand.CanExecute(null))
            {
                UpdateAnotherPizzaCommand.Execute(SelectedPizza.Id);
            }
        }

        #endregion

        #region Overrides

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPizza))
            {
                OnSelectedPizzaChanged();
            }

            base.OnViewModelPropertyChanged(e);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();
            UpdateAnotherPizzaCommand = new Command<string>(
                execute: async (string pizzaId) =>
                {
                    IsBusy = true;
                    Cancellation.Cancel();
                    Page page = !string.IsNullOrEmpty(pizzaId) ? new UpdatePizza(pizzaId) as Page : new UpdateSelectSpecific() as Page;
                    await Navigation.PushAsync(page);
                    IsBusy = false;
                },
                canExecute: (string a) =>
                {
                    return !IsBusy;
                });

            Commands.Add(UpdateAnotherPizzaCommand);
            CommandCanExecuteProperties.Add(UpdateAnotherPizzaCommand, new List<string>() { nameof(IsBusy) });
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
        public Color RowRippleColor { get; set; }
    }
}
