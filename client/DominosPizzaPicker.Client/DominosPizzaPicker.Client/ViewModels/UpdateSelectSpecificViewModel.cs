using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DominosPizzaPicker.Client.Views;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class UpdateSelectSpecificViewModel : CustomViewModel
    {
        #region Fields
        private Dictionary<int, Action> TabFunctions;
        private SauceManager sauceMan;
        private ToppingManager toppingMan;
        private PizzaManager pizzaMan;
        private const uint AnimationLength = 80;
        #endregion

        #region Properties       

        public List<string> SauceList { get; set; }
        public ObservableCollection<Sauce> SauceLookup { get; set; }
        private string _selectedSauce;
        public string SelectedSauce
        {
            get { return _selectedSauce; }
            set
            {
                SetProperty(ref _selectedSauce, value);
            }
        }

        private bool _meatTabSelected;
        public bool MeatTabSelected
        {
            get { return _meatTabSelected; }
            set
            {
                SetProperty(ref _meatTabSelected, value);
                if (value)
                {
                    NonMeatTabSelected = false;
                }


                if (MeatListView == null) return;
                var tranX = value ? 0 : MeatListView.Width + 30;
                TranslateToAsync(MeatListView, tranX, 0, AnimationLength);
            }
        }

        private bool _nonMeatTabSelected;
        public bool NonMeatTabSelected
        {
            get { return _nonMeatTabSelected; }
            set
            {
                SetProperty(ref _nonMeatTabSelected, value);
                if (value)
                {
                    MeatTabSelected = false;
                }


                if (NonMeatListView == null) return;
                var tranX = value ? 0 : -1 * (NonMeatListView.Width + 30);
                TranslateToAsync(NonMeatListView, tranX, 0, AnimationLength);
            }
        }

        public ObservableCollection<NamedTopping> MeatList { get; set; }
        public ObservableCollection<NamedTopping> NonMeatList { get; set; }

        public VisualElement MeatListView { get; set; }
        public VisualElement NonMeatListView { get; set; }


        // used for selected item in both tab lists. captures tap to also change selected state of checkbox when list item is tapped
        private NamedTopping _selectedTopping;
        public NamedTopping SelectedTopping
        {
            get { return _selectedTopping; }
            set { SetProperty(ref _selectedTopping, value); }
        }


        public int SelectedToppingCount
        {
            get
            {
                if (MeatList == null || NonMeatList == null)
                    return 0;

                return MeatList.Where(x => x.IsSelected).Count() + NonMeatList.Where(x => x.IsSelected).Count();
            }
        }

        #endregion

        #region Constructor

        public UpdateSelectSpecificViewModel()
        {

            MeatTabSelected = true;

            sauceMan = SauceManager.DefaultManager;
            toppingMan = ToppingManager.DefaultManager;
            pizzaMan = PizzaManager.DefaultManager;

            // Define functions to run when a tab is selected
            TabFunctions = new Dictionary<int, Action>
            {
                {1, () => MeatTabSelected = true },
                {2, () => NonMeatTabSelected = true }
            };

            MeatList = new ObservableCollection<NamedTopping>();
            NonMeatList = new ObservableCollection<NamedTopping>();
        }

        #endregion

        #region Commands
        public Command TabTappedCommand { get; private set; }
        public Command ContinueCommand { get; private set; }
        #endregion

        #region Overrides
        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            TabTappedCommand = this.CreateCommand<object>(tabNum => TabFunctions[Convert.ToInt16(tabNum)]());

            ContinueCommand = this.CreateCommand(async () =>
                {
                    var toppingList = new List<string>();
                    toppingList.AddRange(MeatList.Where(x => x.IsSelected).Select(x => x.Id));
                    toppingList.AddRange(NonMeatList.Where(x => x.IsSelected).Select(x => x.Id));
                    var pizza = await pizzaMan.GetSinglePizza(SauceLookup.FirstOrDefault(x => x.Name == SelectedSauce).Id, toppingList);
                    await Navigation.PushAsync(new UpdatePizza(pizza.Id));
                }, () => SelectedToppingCount >= 3 && !string.IsNullOrEmpty(SelectedSauce),
                new List<string> { nameof(SelectedTopping), nameof(SelectedSauce) });
        }

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(SelectedTopping))
            {
                OnSelectedToppingChanged();
            }
            if (e.PropertyName == nameof(SelectedSauce))
            {
                //ContinueCommand.ChangeCanExecute();
            }

            base.OnViewModelPropertyChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MeatList.ToList().ForEach(x => x.PropertyChanged -= OnSelectedToppingListChanged);
                NonMeatList.ToList().ForEach(x => x.PropertyChanged -= OnSelectedToppingListChanged);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Methods

        public async Task LoadLists()
        {
            if (MeatList.Count != 0 || NonMeatList.Count != 0) return;

            // Sauces            
            SauceLookup = await sauceMan.GetSaucesAsync(true);
            SauceList = SauceLookup.Select(x => x.Name).ToList();
            OnPropertyChanged(nameof(SauceList));

            // Toppings
            var toppingList = await toppingMan.GetToppingsAsync(true);
            foreach (var t in toppingList)
            {
                var nt = new NamedTopping { Id = t.Id, Name = t.Name.Trim(), IsSelected = false, ToppingEnabled = true, IsMeat = t.IsMeat };
                nt.PropertyChanged += OnSelectedToppingListChanged;

                if (t.IsMeat)
                {
                    MeatList.Add(nt);
                    continue;
                }

                NonMeatList.Add(nt);

            }
        }

        // for when a List item is selected
        public void OnSelectedToppingListChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NamedTopping.IsSelected))
            {
                if (SelectedToppingCount >= 3)
                {
                    MeatList.ToList().ForEach(x => x.ToppingEnabled = x.IsSelected);
                    NonMeatList.ToList().ForEach(x => x.ToppingEnabled = x.IsSelected);

                }
                else
                {
                    MeatList.ToList().ForEach(x => x.ToppingEnabled = true);
                    NonMeatList.ToList().ForEach(x => x.ToppingEnabled = true);
                }

                // update continue command 
                //ContinueCommand.ChangeCanExecute();
            }
        }

        public void OnSelectedToppingChanged()
        {
            if (SelectedTopping == null) return;

            // can't select more if already at 3
            // always able to change a currently selected topping            
            if (SelectedToppingCount <= 2 || SelectedTopping.IsSelected)
            {
                SelectedTopping.IsSelected = !SelectedTopping.IsSelected;
            }

            SelectedTopping = default(NamedTopping);
        }

        // Async wrapper method
        public async void TranslateToAsync(VisualElement v, double x, double y, uint length = 250, Easing easing = null)
        {
            await v.TranslateTo(x, y, length, easing);
        }


        #endregion
    }

    // used for list view item source
    public class NamedTopping : SimpleNotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMeat { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private bool _toppingEnabled;
        public bool ToppingEnabled
        {
            get { return _toppingEnabled; }
            set { SetProperty(ref _toppingEnabled, value); }
        }
    }
}

