using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class UpdatePizzaViewModel : CustomViewModel
    {
        #region Fields

        private PizzaViewManager pizzaViewMan;
        private PizzaManager pizzaMan;

        #endregion

        #region Properties
        public string Id { get; set; }

        private string _sauceName;
        public string SauceName
        {
            get { return _sauceName; }
            set { SetProperty(ref _sauceName, value); }
        }

        private string _topping1Name;
        public string Topping1Name
        {
            get { return _topping1Name; }
            set { SetProperty(ref _topping1Name, value); }
        }

        private string _topping2Name;
        public string Topping2Name
        {
            get { return _topping2Name; }
            set { SetProperty(ref _topping2Name, value); }
        }

        private string _topping3Name;
        public string Topping3Name
        {
            get { return _topping3Name; }
            set { SetProperty(ref _topping3Name, value); }
        }

        private bool _eatenStatus;
        public bool EatenStatus
        {
            get { return _eatenStatus; }
            set { SetProperty(ref _eatenStatus, value); }
        }

        private DateTime _dateEaten;
        public DateTime DateEaten
        {
            get { return _dateEaten; }
            set { SetProperty(ref _dateEaten, value); }
        }

        private float _rating;
        public float Rating
        {
            get { return _rating; }
            set
            {
                // Validate only if value changed
                if (SetProperty(ref _rating, value))
                    Validate(nameof(Rating));
            }
        }

        private string _ratingErrorText;
        public string RatingErrorText
        {
            get { return _ratingErrorText; }
            set { SetProperty(ref _ratingErrorText, value); }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public PizzaInputData OriginalPizzaData { get; set; }


        public bool IsDirty =>
            EatenStatus != OriginalPizzaData.EatenStatus || DateEaten != OriginalPizzaData.DateEaten ||
            Math.Abs(Rating - OriginalPizzaData.Rating) > 0f || Comment != OriginalPizzaData.Comment;

        #endregion

        #region Constructor

        public UpdatePizzaViewModel()
        {
            pizzaViewMan = PizzaViewManager.DefaultManager;
            pizzaMan = PizzaManager.DefaultManager;
        }

        #endregion

        #region Methods
        // void method called from View constructor (can't be awaited)
        public async void LoadPizza(string id)
        {
            await LoadPizzaAsync(id);
        }

        public async Task LoadPizzaAsync(string id)
        {
            Id = id;
            var pizza = await pizzaViewMan.GetSinglePizza(Id);
            SauceName = pizza.Sauce;
            Topping1Name = pizza.Topping1;
            Topping2Name = pizza.Topping2;
            Topping3Name = pizza.Topping3;
            EatenStatus = pizza.Eaten;
            DateEaten = DateTime.Compare(pizza.DateEaten, DateTime.Parse("01/01/1901")) <= 0 ? DateTime.Today.Date : pizza.DateEaten.Date;
            Rating = pizza.Rating;
            Comment = pizza.Comment;

            if(OriginalPizzaData == null)
                OriginalPizzaData = new PizzaInputData(EatenStatus, DateEaten, Rating, Comment);
            else
            {
                OriginalPizzaData.EatenStatus = EatenStatus;
                OriginalPizzaData.DateEaten = DateEaten;
                OriginalPizzaData.Rating = Rating;
                OriginalPizzaData.Comment = Comment;
            }

            UpdateCommand?.ChangeCanExecute();
        }

        /// <summary>
        /// Rating cannot be greater than 10 or less than 0
        /// </summary>
        protected bool ValidateRating()
        {
            if (Rating > 10)
            {
                RatingErrorText = "Cannot be greater than 10";
                return false;
            }

            if (Rating < 0)
            {
                RatingErrorText = "Cannot be less than 0";
                return false;
            }

            RatingErrorText = null;
            return true;
        }
        #endregion

        #region Overrides

        protected override void AddValidations()
        {
            base.AddValidations();
            Validations.Add(nameof(Rating), ValidateRating);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            RefreshCommand = this.CreateCommand(async () => await LoadPizzaAsync(Id));

            UpdateCommand = this.CreateCommand(async () =>
            {
                if (Validate())
                {
                    var pizza = await pizzaViewMan.GetSinglePizza(Id);
                    pizza.Eaten = EatenStatus;
                    pizza.DateEaten = EatenStatus ? DateEaten : (DateTime)SqlDateTime.MinValue;
                    pizza.Rating = Rating;
                    pizza.Comment = Comment.Trim();
                    await pizzaMan.SavePizzaAsync(pizza);

                    if (pizza.Eaten)
                    {
                        await CachedData.AddToEatenPizzaCache(pizza);
                    }
                    else
                    {
                        await CachedData.RemoveFromEatenPizzaCache(pizza);
                    }

                    MessagingCenter.Send(this, nameof(Messages.SuccessfulUpdate));
                    RefreshCommand.Execute(null);
                }
            }, () => IsValid && IsDirty, new List<string> { nameof(IsValid), nameof(IsDirty) });

        }

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            // Check IsDirty status
            if (e.PropertyName == nameof(EatenStatus) || e.PropertyName == nameof(DateEaten) ||
                e.PropertyName == nameof(Rating) || e.PropertyName == nameof(Comment))
            {
                UpdateCommand.ChangeCanExecute();
            }

            base.OnViewModelPropertyChanged(e);
        }

        #endregion

        #region Commands
        public Command RefreshCommand { get; private set; }
        public Command UpdateCommand { get; private set; }
        #endregion
    }

    public class PizzaInputData
    {
        public bool EatenStatus;
        public DateTime DateEaten;
        public float Rating;
        public string Comment;

        public PizzaInputData(bool eatenStatus, DateTime dateEaten, float rating, string comment)
        {
            EatenStatus = eatenStatus;
            DateEaten = dateEaten;
            Rating = rating;
            Comment = comment;
        }
    }
}
