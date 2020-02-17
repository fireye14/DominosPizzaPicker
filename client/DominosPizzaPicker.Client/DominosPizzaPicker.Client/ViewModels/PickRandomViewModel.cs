using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using System;
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
        #endregion

        #region View Properties

        private string _pizzaText;
        public virtual string PizzaText
        {
            get { return _pizzaText; }
            set { SetProperty(ref _pizzaText, value); }
        }

        private Pizza _generatedPizza;
        public virtual Pizza GeneratedPizza
        {
            get { return _generatedPizza; }
            set
            {
                SetProperty(ref _generatedPizza, value);
                new Task(async () => PizzaText = _generatedPizza == null ? string.Empty : await _generatedPizza.ToStringAsync()).Start();
            }
        }

        #endregion

        #region Command Properties

        private bool _canGenerate;
        public bool CanGenerate
        {
            get { return _canGenerate; }
            set { SetProperty(ref _canGenerate, value); }
        }

        #endregion

        #region Constructors
        public PickRandomViewModel()
        {
            CanGenerate = true;
            SetRandomUneatenPizzaTextCommand = new Command(
                execute: async () =>
                {
                    // called when button is clicked
                    CanGenerate = false;
                    await SetRandomUneatenPizzaText();
                    CanGenerate = true;
                },
                canExecute: () =>
                {
                    // called when binding is first defined in View and when CanExecuteChanged is fired (fired when calling ChangeCanExecute())
                    return CanGenerate;
                });

            UpdateEatenStatusCommand = new Command(
                execute: async () =>
                {
                    GeneratedPizza.Eaten = true;
                    GeneratedPizza.DateEaten = DateTime.Today;
                    await pizzaMan.SavePizzaAsync(GeneratedPizza);
                    // Send Successful Update message to the View
                    MessagingCenter.Send(this, nameof(Messages.SuccessfulUpdate));
                    GeneratedPizza = null;
                },
                canExecute: () =>
                {
                    return GeneratedPizza != null && CanGenerate;
                });
        }

        #endregion

        #region Overrides
        /// <summary>
        /// </summary>
        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CanGenerate))
            {
                (SetRandomUneatenPizzaTextCommand as Command).ChangeCanExecute();
                (UpdateEatenStatusCommand as Command).ChangeCanExecute();
            }
            else if (e.PropertyName == nameof(GeneratedPizza))
            {
                (UpdateEatenStatusCommand as Command).ChangeCanExecute();
            }

            base.OnViewModelPropertyChanged(e);
        }
        #endregion

        #region Commands

        public ICommand SetRandomUneatenPizzaTextCommand { get; private set; }
        public ICommand UpdateEatenStatusCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Look up a random pizza with Eaten = 0 and set PizzaText
        /// </summary>
        public async Task SetRandomUneatenPizzaText()
        {
            GeneratedPizza = await pizzaMan.GetRandomUneatenPizza();
        }

        #endregion

        #region Event Handlers

        #endregion

    }
}
