using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.ViewModels
{
    public class PickRandomViewModel : CustomViewModel
    {
        #region Fields
        private static readonly PizzaManager pizzaMan = PizzaManager.DefaultManager;
        private static readonly PizzaViewManager pizzaViewMan = PizzaViewManager.DefaultManager;
        #endregion

        #region View Properties

        private string _pizzaText;
        public virtual string PizzaText
        {
            get { return _pizzaText; }
            set { SetProperty(ref _pizzaText, value); }
        }

        private PizzaView _generatedPizza;
        public virtual PizzaView GeneratedPizza
        {
            get { return _generatedPizza; }
            set { SetProperty(ref _generatedPizza, value); }
        }

        #endregion

        #region Constructors
        public PickRandomViewModel()
        {
        }

        #endregion

        #region Overrides
        /// <summary>
        /// </summary>
        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GeneratedPizza))
            {

                PizzaText = GeneratedPizza == null ? string.Empty : GeneratedPizza.ToString();
            }

            base.OnViewModelPropertyChanged(e);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            SetRandomUneatenPizzaTextCommand = this.CreateCommand(async () => await GenerateRandomUneatenPizza());

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

        #endregion

        #region Commands

        public Command SetRandomUneatenPizzaTextCommand { get; private set; }
        public Command UpdateEatenStatusCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Look up a random pizza with Eaten = 0 and set PizzaText
        /// </summary>
        public async Task GenerateRandomUneatenPizza()
        {
            GeneratedPizza = await pizzaViewMan.GetRandomUneatenPizza();
        }

        #endregion

        #region Event Handlers

        #endregion

    }
}
