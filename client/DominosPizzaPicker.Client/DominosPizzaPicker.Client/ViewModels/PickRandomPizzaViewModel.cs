using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Binbin.Linq;
using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class PickRandomPizzaViewModel : CustomViewModel
    {
        #region Fields

        private readonly SauceManager sauceMan;
        private readonly ToppingManager toppingMan;
        private readonly PizzaManager pizzaMan;
        private readonly PizzaViewManager pizzaViewMan;

        private const string RandomText = "Random";
        private const string MeatText = "Any Meat";
        private const string NonMeatText = "Any Non-Meat";
        private const string CheeseText = "Any Cheese";

        #endregion

        #region Properties and Backing Fields

        //public List<string> SauceList => _sauceList ?? (_sauceList = new List<string>());
        public List<string> SauceList { get; set; }

        private string _selectedSauce;
        public string SelectedSauce
        {
            get => _selectedSauce;
            set => SetProperty(ref _selectedSauce, value);
        }

        public List<string> ToppingList { get; set; }

        public ObservableCollection<string> SelectedToppings { get; set; }

        private List<string> _nonSpecificToppingOptions;
        public List<string> NonSpecificToppingOptions
        {
            get
            {
                if (_nonSpecificToppingOptions == null)
                {
                    _nonSpecificToppingOptions = new List<string> { RandomText, MeatText, NonMeatText, CheeseText };
                }

                return _nonSpecificToppingOptions;
            }
        }

        private Dictionary<string, List<Func<PizzaView, bool>>> _nonSpecificToppingFuncs;

        public Dictionary<string, List<Func<PizzaView, bool>>> NonSpecificToppingFuncs
        {
            get
            {
                return _nonSpecificToppingFuncs ?? (_nonSpecificToppingFuncs =
                    new Dictionary<string, List<Func<PizzaView, bool>>>
                    {
                        {
                            MeatText,
                            new List<Func<PizzaView, bool>>
                                { p => p.Topping1IsMeat, p => p.Topping2IsMeat, p => p.Topping3IsMeat }
                        },
                        {
                            NonMeatText,
                            new List<Func<PizzaView, bool>>
                                { p => !p.Topping1IsMeat, p => !p.Topping2IsMeat, p => !p.Topping3IsMeat }
                        },
                        {
                            CheeseText,
                            new List<Func<PizzaView, bool>>
                                { p => p.Topping1IsCheese, p => p.Topping2IsCheese, p => p.Topping3IsCheese }
                        }
                    });
            }
        }

        private string _pizzaText;

        public virtual string PizzaText
        {
            get => _pizzaText;
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "\n\n\n";
                SetProperty(ref _pizzaText, value);
            }
        }
        
        public List<Picker> ToppingPickerList { get; set; }

        private PizzaView _generatedPizza;
        public virtual PizzaView GeneratedPizza
        {
            get => _generatedPizza;
            set => SetProperty(ref _generatedPizza, value);
        }

        #endregion

        #region Constructor

        public PickRandomPizzaViewModel()
        {
            sauceMan = SauceManager.DefaultManager;
            toppingMan = ToppingManager.DefaultManager;
            pizzaMan = PizzaManager.DefaultManager;
            pizzaViewMan = PizzaViewManager.DefaultManager;

            PizzaText = null;
        }

        #endregion

        #region Overrides

        protected override void InitializeCommands()
        {
            base.InitializeCommands();


            GenerateCommand = this.CreateCommand(async () =>
            {
                Expression<Func<PizzaView, bool>> exp = p => p.IsRandomlyGenerated && !p.Eaten;

                if (!string.IsNullOrEmpty(SelectedSauce) && !NonSpecificToppingOptions.Contains(SelectedSauce))
                    exp = exp.And(p => p.Sauce == SelectedSauce);

                foreach (var t in SelectedToppings)
                {
                    if (!string.IsNullOrEmpty(t) && !NonSpecificToppingOptions.Contains(t))
                        exp = exp.And(p => p.Topping1 == t || p.Topping2 == t || p.Topping3 == t);
                }


                // list of specific toppings selected that already have filters added for them; we don't want our non specific filters to include these
                var specificToppingList = SelectedToppings.Where(t => !NonSpecificToppingOptions.Contains(t)).ToList();

                if (specificToppingList.Count < 3)
                {
                    // Find selections that are topping types (non specific) and add to expression accordingly


                    #region Add Meat Expressions

                    var count = SelectedToppings.Count(x => x.Equals(MeatText));


                    switch (count)
                    {

                        case 1:
                            // at least 1 topping needs to be meat

                            // can be up to 2 specific selected toppings

                            // if only 1, make sure our 1 random meat doesn't equal it
                            if (specificToppingList.Count == 1)
                            {
                                exp = exp.And(p =>
                                    (p.Topping1IsMeat && p.Topping1 != specificToppingList[0]) ||
                                    (p.Topping2IsMeat && p.Topping2 != specificToppingList[0]) ||
                                    (p.Topping3IsMeat && p.Topping3 != specificToppingList[0]));
                            }
                            // if 2, make sure our 1 random meat doesn't equal both
                            else if (specificToppingList.Count == 2)
                            {
                                exp = exp.And(p =>
                                    (p.Topping1IsMeat && p.Topping1 != specificToppingList[0] && p.Topping1 != specificToppingList[1]) ||
                                    (p.Topping2IsMeat && p.Topping2 != specificToppingList[0] && p.Topping2 != specificToppingList[1]) ||
                                    (p.Topping3IsMeat && p.Topping3 != specificToppingList[0] && p.Topping3 != specificToppingList[1]));
                            }
                            // if 0, we don't care
                            else
                            {
                                exp = exp.And(p => p.Topping1IsMeat || p.Topping2IsMeat || p.Topping3IsMeat);
                            }

                            break;

                        case 2:
                            // any combination of 2 toppings needs to be meat (or all 3)


                            // can be at most 1 specific selected topping


                            // if it exists, also make sure that one isn't included in our 2 meat selections
                            if (specificToppingList.Count > 0)
                            {
                                exp = exp.And(p =>
                                        (p.Topping1IsMeat && p.Topping1 != specificToppingList[0] &&
                                        p.Topping2IsMeat && p.Topping2 != specificToppingList[0])
                                    || (p.Topping1IsMeat && p.Topping1 != specificToppingList[0] &&
                                        p.Topping3IsMeat && p.Topping3 != specificToppingList[0])
                                    || (p.Topping2IsMeat && p.Topping2 != specificToppingList[0] &&
                                        p.Topping3IsMeat && p.Topping3 != specificToppingList[0]));
                            }

                            // otherwise we don't care about the 3rd topping selection here
                            else
                            {
                                exp = exp.And(p => (p.Topping1IsMeat && p.Topping2IsMeat)
                                                        || (p.Topping1IsMeat && p.Topping3IsMeat)
                                                        || (p.Topping2IsMeat && p.Topping3IsMeat));
                            }

                            break;

                        case 3:
                            // all 3 toppings need to be meat
                            exp = exp.And(p => p.Topping1IsMeat && p.Topping2IsMeat && p.Topping3IsMeat);
                            break;
                    }

                    #endregion Meat 


                    #region Add Non-Meat Expressions

                    count = SelectedToppings.Count(x => x.Equals(NonMeatText));

                    switch (count)
                    {

                        case 1:
                            // at least 1 topping needs to be non meat

                            // can be up to 2 specific selected toppings

                            // if only 1, make sure our 1 random non meat doesn't equal it
                            if (specificToppingList.Count == 1)
                            {
                                exp = exp.And(p =>
                                    (!p.Topping1IsMeat && p.Topping1 != specificToppingList[0]) ||
                                    (!p.Topping2IsMeat && p.Topping2 != specificToppingList[0]) ||
                                    (!p.Topping3IsMeat && p.Topping3 != specificToppingList[0]));
                            }
                            // if 2, make sure our 1 random non meat doesn't equal both
                            else if (specificToppingList.Count == 2)
                            {
                                exp = exp.And(p =>
                                    (!p.Topping1IsMeat && p.Topping1 != specificToppingList[0] && p.Topping1 != specificToppingList[1]) ||
                                    (!p.Topping2IsMeat && p.Topping2 != specificToppingList[0] && p.Topping2 != specificToppingList[1]) ||
                                    (!p.Topping3IsMeat && p.Topping3 != specificToppingList[0] && p.Topping3 != specificToppingList[1]));
                            }
                            // if 0, we don't care
                            else
                            {
                                exp = exp.And(p => !p.Topping1IsMeat || !p.Topping2IsMeat || !p.Topping3IsMeat);
                            }

                            break;

                        case 2:
                            // any combination of 2 toppings needs to be non meat (or all 3)


                            // can be at most 1 specific selected topping


                            // if it exists, also make sure that one isn't included in our 2 non meat selections
                            if (specificToppingList.Count > 0)
                            {
                                exp = exp.And(p =>
                                        (!p.Topping1IsMeat && p.Topping1 != specificToppingList[0] &&
                                        !p.Topping2IsMeat && p.Topping2 != specificToppingList[0])
                                    || (!p.Topping1IsMeat && p.Topping1 != specificToppingList[0] &&
                                        !p.Topping3IsMeat && p.Topping3 != specificToppingList[0])
                                    || (!p.Topping2IsMeat && p.Topping2 != specificToppingList[0] &&
                                        !p.Topping3IsMeat && p.Topping3 != specificToppingList[0]));
                            }

                            // otherwise we don't care about the 3rd topping selection here
                            else
                            {
                                exp = exp.And(p => (!p.Topping1IsMeat && !p.Topping2IsMeat)
                                                        || (!p.Topping1IsMeat && !p.Topping3IsMeat)
                                                        || (!p.Topping2IsMeat && !p.Topping3IsMeat));
                            }

                            break;

                        case 3:
                            // all 3 toppings need to be non meat
                            exp = exp.And(p => !p.Topping1IsMeat && !p.Topping2IsMeat && !p.Topping3IsMeat);
                            break;
                    }

                    #endregion Non-Meat


                    #region Add Cheese Expressions

                    count = SelectedToppings.Count(x => x.Equals(CheeseText));

                    switch (count)
                    {

                        case 1:
                            // at least 1 topping needs to be cheese

                            // can be up to 2 specific selected toppings

                            // if only 1, make sure our 1 random cheese doesn't equal it
                            if (specificToppingList.Count == 1)
                            {
                                exp = exp.And(p =>
                                    (p.Topping1IsCheese && p.Topping1 != specificToppingList[0]) ||
                                    (p.Topping2IsCheese && p.Topping2 != specificToppingList[0]) ||
                                    (p.Topping3IsCheese && p.Topping3 != specificToppingList[0]));
                            }
                            // if 2, make sure our 1 random cheese doesn't equal both
                            else if (specificToppingList.Count == 2)
                            {
                                exp = exp.And(p =>
                                    (p.Topping1IsCheese && p.Topping1 != specificToppingList[0] && p.Topping1 != specificToppingList[1]) ||
                                    (p.Topping2IsCheese && p.Topping2 != specificToppingList[0] && p.Topping2 != specificToppingList[1]) ||
                                    (p.Topping3IsCheese && p.Topping3 != specificToppingList[0] && p.Topping3 != specificToppingList[1]));
                            }
                            // if 0, we don't care
                            else
                            {
                                exp = exp.And(p => p.Topping1IsCheese || p.Topping2IsCheese || p.Topping3IsCheese);
                            }

                            break;

                        case 2:
                            // any combination of 2 toppings needs to be cheese (or all 3)


                            // can be at most 1 specific selected topping


                            // if it exists, also make sure that one isn't included in our 2 cheese selections
                            if (specificToppingList.Count > 0)
                            {
                                exp = exp.And(p =>
                                        (p.Topping1IsCheese && p.Topping1 != specificToppingList[0] &&
                                         p.Topping2IsCheese && p.Topping2 != specificToppingList[0])
                                    || (p.Topping1IsCheese && p.Topping1 != specificToppingList[0] &&
                                        p.Topping3IsCheese && p.Topping3 != specificToppingList[0])
                                    || (p.Topping2IsCheese && p.Topping2 != specificToppingList[0] &&
                                        p.Topping3IsCheese && p.Topping3 != specificToppingList[0]));
                            }

                            // otherwise we don't care about the 3rd topping selection here
                            else
                            {
                                exp = exp.And(p => (p.Topping1IsCheese && p.Topping2IsCheese)
                                                        || (p.Topping1IsCheese && p.Topping3IsCheese)
                                                        || (p.Topping2IsCheese && p.Topping3IsCheese));
                            }

                            break;

                        case 3:
                            // all 3 toppings need to be cheese
                            exp = exp.And(p => p.Topping1IsCheese && p.Topping2IsCheese && p.Topping3IsCheese);
                            break;
                    }

                    #endregion Cheese

                }

                PizzaText = string.Empty;

                GeneratedPizza = await pizzaViewMan.GetRandomUneatenPizzaWithCondition(exp);
                if (GeneratedPizza == null)
                {
                    MessagingCenter.Send(this, nameof(Messages.NoPizzasExist));
                }
                else
                    PizzaText = GeneratedPizza.ToStringNewLine();

            });

            UpdateEatenStatusCommand = this.CreateCommand(async () =>
                {
                    GeneratedPizza.Eaten = true;
                    GeneratedPizza.DateEaten = DateTime.Today;
                    await pizzaMan.SavePizzaAsync(GeneratedPizza);
                    await CachedData.AddToEatenPizzaCache(GeneratedPizza);
                    // Send Successful Update message to the View
                    MessagingCenter.Send(this, nameof(Messages.SuccessfulUpdate));
                    GeneratedPizza = null;
                },
                () => GeneratedPizza != null, new List<string> { nameof(GeneratedPizza) });

        }

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {

            base.OnViewModelPropertyChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(SelectedToppings != null)
                    SelectedToppings.CollectionChanged -= SelectedToppings_ListChanged;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Commands

        public Command GenerateCommand { get; private set; }

        public Command UpdateEatenStatusCommand { get; private set; }

        #endregion

        #region Methods

        public async void LoadLists()
        {

            // Sauces            
            SauceList = (await sauceMan.GetSaucesAsync(true)).OrderBy(x => x.Name).Select(x => x.Name).ToList();
            SauceList.Insert(0, RandomText);
            OnPropertyChanged(nameof(SauceList));
            SelectedSauce = SauceList[0];

            // Toppings
            ToppingList = (await toppingMan.GetToppingsAsync()).OrderBy(x => x.Name).Select(x => x.Name).ToList();
            ToppingList.InsertRange(0, NonSpecificToppingOptions);
            OnPropertyChanged(nameof(ToppingList));

            // Initialize selected toppings as all Random
            SelectedToppings = new ObservableCollection<string> { ToppingList[0], ToppingList[0], ToppingList[0] };
            OnPropertyChanged(nameof(SelectedToppings));

            SelectedToppings.CollectionChanged -= SelectedToppings_ListChanged;
            SelectedToppings.CollectionChanged += SelectedToppings_ListChanged;
        }

        #endregion

        #region Events

        private void SelectedToppings_ListChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            try
            {
                SelectedToppings.CollectionChanged -= SelectedToppings_ListChanged;
                OnSelectedToppingsCollectionChanged(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                SelectedToppings.CollectionChanged += SelectedToppings_ListChanged;
            }
        }

        protected virtual void OnSelectedToppingsCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var newVal = SelectedToppings[e.NewStartingIndex];
                if (!string.IsNullOrEmpty(newVal) && !NonSpecificToppingOptions.Contains(newVal) && SelectedToppings.Count(x => x.Equals(newVal)) >= 2)
                {
                    // with a replace action type, the OldItems list only contains a single object
                    SelectedToppings[e.NewStartingIndex] = e.OldItems[0].ToString();
                    //MessagingCenter.Send(this, nameof(Messages.DuplicateToppingNotAllowed));

                    if (ToppingPickerList[e.NewStartingIndex] is Picker pck)
                    {
                        if(pck.IsFocused)
                            pck.Unfocus();

                        pck.Focus();
                    }

                    OnPropertyChanged(nameof(SelectedToppings));
                }
            }

        }

        #endregion
    }
}
