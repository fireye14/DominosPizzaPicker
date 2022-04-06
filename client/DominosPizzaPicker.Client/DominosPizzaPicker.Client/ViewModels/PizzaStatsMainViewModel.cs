using System;
using System.ComponentModel;
using System.Linq;
using Android.Media.Session;
using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models.Managers;
using Java.Util;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class PizzaStatsMainViewModel : CustomViewModel
    {
        #region Fields

        private readonly PizzaViewManager pizzaViewMan;

        private const byte precision = 6;

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
            ? "  of  eaten "
            : $"{EatenPizzaCount} of {TotalPizzaCount} eaten ({GetPercentage(EatenPizzaCount, TotalPizzaCount)}%)";


        private string _highestRatedPizzaText;
        public string HighestRatedPizzaText
        {
            get => _highestRatedPizzaText;
            set => SetProperty(ref _highestRatedPizzaText, value);
        }

        private long _distinctSauceCount;
        public long DistinctSauceCount
        {
            get => _distinctSauceCount;
            set => SetProperty(ref _distinctSauceCount, value);
        }

        private long _distinctToppingCount;
        public long DistinctToppingCount
        {
            get => _distinctToppingCount;
            set => SetProperty(ref _distinctToppingCount, value);
        }

        private long _alfredoSauceEatenCount;
        public long AlfredoSauceEatenCount
        {
            get => _alfredoSauceEatenCount;
            set => SetProperty(ref _alfredoSauceEatenCount, value);
        }

        public string AlfredoSauceEatenText => DistinctSauceCount == 0
            ? "  of  eaten "
            : $"{AlfredoSauceEatenCount} of {DistinctSauceCount} eaten ({GetPercentage(AlfredoSauceEatenCount, DistinctSauceCount)}%)";


        private long _premiumChickenEatenCount;
        public long PremiumChickenEatenCount
        {
            get => _premiumChickenEatenCount;
            set => SetProperty(ref _premiumChickenEatenCount, value);
        }

        public string PremiumChickenEatenText => DistinctToppingCount == 0
            ? "  of  eaten "
            : $"{PremiumChickenEatenCount} of {DistinctToppingCount} eaten ({GetPercentage(PremiumChickenEatenCount, DistinctToppingCount)}%)";



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

            else if (e.PropertyName == nameof(AlfredoSauceEatenCount) || e.PropertyName == nameof(DistinctSauceCount))
                OnPropertyChanged(nameof(AlfredoSauceEatenText));

            else if (e.PropertyName == nameof(PremiumChickenEatenCount) || e.PropertyName == nameof(DistinctToppingCount))
                OnPropertyChanged(nameof(PremiumChickenEatenText));

            base.OnViewModelPropertyChanged(e);
        }

        #endregion

        #region Methods

        // called from view constructor
        public async void LoadStats()
        {
            TotalPizzaCount = await pizzaViewMan.GetTotalPizzaCount();
            EatenPizzaCount = await pizzaViewMan.GetEatenPizzaCount();

            var highestRatedPizza = await pizzaViewMan.GetHighestRatedPizza();
            HighestRatedPizzaText = $"{highestRatedPizza.Rating} : {highestRatedPizza.ToString()}";

            // How many pizzas have Alfredo Sauce. Same number for all sauces
            DistinctSauceCount =
                await pizzaViewMan.GetCountWithCondition(x => x.IsRandomlyGenerated && x.Sauce == "Alfredo Sauce");

            // How many pizzas have Premium Chicken. Same number for all individual toppings
            DistinctToppingCount =
                await pizzaViewMan.GetCountWithCondition(x =>
                    x.IsRandomlyGenerated && (x.Topping1 == "Premium Chicken" || x.Topping2 == "Premium Chicken" ||
                                              x.Topping3 == "Premium Chicken"));



            AlfredoSauceEatenCount =
                await pizzaViewMan.GetCountWithCondition(x => x.IsRandomlyGenerated && x.Eaten && x.Sauce == "Alfredo Sauce");


            PremiumChickenEatenCount =
                await pizzaViewMan.GetCountWithCondition(x =>
                    x.IsRandomlyGenerated && x.Eaten && (x.Topping1 == "Premium Chicken" || x.Topping2 == "Premium Chicken" ||
                                              x.Topping3 == "Premium Chicken"));

            // Example of getting eaten/total pizzas with Black Olives
            // If I end up doing this for every topping/sauce, I could save time by only doing the total count query once for each
            //TotalPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && (x.Topping1 == "Black Olives" || x.Topping2 == "Black Olives" ||
            //                                         x.Topping3 == "Black Olives"));
            //EatenPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Eaten && (x.Topping1 == "Black Olives" || x.Topping2 == "Black Olives" ||
            //                                         x.Topping3 == "Black Olives"));

            // Example of getting eaten/total pizzas with Alfredo Sauce
            //TotalPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Sauce == "Alfredo Sauce");
            //EatenPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Eaten && x.Sauce == "Alfredo Sauce");


            // Example of getting eaten/total pizzas with Alfredo Sauce and Premium Chicken
            //TotalPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Sauce == "Alfredo Sauce" && (x.Topping1 == "Premium Chicken" ||
            //                                                            x.Topping2 == "Premium Chicken" ||
            //                                                            x.Topping3 == "Premium Chicken"));
            //EatenPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Eaten && x.Sauce == "Alfredo Sauce" && (x.Topping1 == "Premium Chicken" ||
            //        x.Topping2 == "Premium Chicken" || x.Topping3 == "Premium Chicken"));


            // Example of getting eaten/total pizzas with alfredo sauce and chicken, but no olives
            //TotalPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Sauce == "Alfredo Sauce" &&
            //    (x.Topping1 == "Premium Chicken" || x.Topping2 == "Premium Chicken" ||
            //     x.Topping3 == "Premium Chicken") && !x.Topping1.Contains("Olives") && !x.Topping2.Contains("Olives") &&
            //    !x.Topping3.Contains("Olives"));

            //EatenPizzaCount = await pizzaViewMan.GetCountWithCondition(x =>
            //    x.IsRandomlyGenerated && x.Eaten && x.Sauce == "Alfredo Sauce" &&
            //    (x.Topping1 == "Premium Chicken" || x.Topping2 == "Premium Chicken" ||
            //     x.Topping3 == "Premium Chicken") && !x.Topping1.Contains("Olives") && !x.Topping2.Contains("Olives") &&
            //    !x.Topping3.Contains("Olives"));
        }

        protected virtual double GetPercentage(float num, float ofTotal, byte prec = precision, MidpointRounding roundingType = MidpointRounding.AwayFromZero)
        {
            // precision in Math.Round() can't be less than 0 or greater than 15
            prec = Math.Max(Math.Min(prec, (byte)15), (byte)0);
            return Math.Round(100 * num / ofTotal, prec, roundingType);
        }

        #endregion
    }
}