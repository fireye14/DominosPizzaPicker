using System;
using System.ComponentModel;
using Android.Media.Session;
using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models.Managers;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class PizzaStatsMainViewModel : CustomViewModel
    {
        #region Fields

        private readonly PizzaViewManager pizzaViewMan;

        #endregion

        #region Properties and Backing Fields

        private long _totalPizzaCount;
        public long TotalPizzaCount
        {
            get => _totalPizzaCount;
            set => SetProperty(ref _totalPizzaCount, value);
        }

        private long _eatenPizzaCount;
        public long EatenPizzaCount
        {
            get => _eatenPizzaCount;
            set => SetProperty(ref _eatenPizzaCount, value);
        }

        public string TotalEatenPizzaText => TotalPizzaCount == 0
            ? string.Empty
            : $"{EatenPizzaCount} of {TotalPizzaCount} eaten ({EatenPizzaCount / (float)TotalPizzaCount:N10}%)";

        #endregion

        #region Constructor

        public PizzaStatsMainViewModel()
        {
            pizzaViewMan = PizzaViewManager.DefaultManager;
        }

        #endregion

        #region Overrides

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(EatenPizzaCount) || e.PropertyName == nameof(TotalPizzaCount))
                OnPropertyChanged(nameof(TotalEatenPizzaText));

            base.OnViewModelPropertyChanged(e);
        }

        #endregion

        #region Methods

        public async void LoadStats()
        {
            TotalPizzaCount = await pizzaViewMan.GetTotalPizzaCount();
            EatenPizzaCount = await pizzaViewMan.GetEatenPizzaCount();
        }

        #endregion
    }
}