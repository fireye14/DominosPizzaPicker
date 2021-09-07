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
        }

        #endregion

        #region Overrides
        /// <summary>
        /// </summary>
        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CanGenerate))
            {
                //(SetRandomUneatenPizzaTextCommand as Command).ChangeCanExecute();
                //(UpdateEatenStatusCommand as Command).ChangeCanExecute();
            }
            else if (e.PropertyName == nameof(GeneratedPizza))
            {
                //(UpdateEatenStatusCommand as Command).ChangeCanExecute();

                PizzaText = GeneratedPizza == null ? string.Empty : GeneratedPizza.ToString();
                //CanGenerate = true;
                //t.Start();
                // Run synchronously so the SetProperty of CanGenerate doesn't error
                //t.RunSynchronously();
                //t.Wait();
                //CanGenerate = true;
                //t.ContinueWith(x => CanGenerate = true);
            }

            base.OnViewModelPropertyChanged(e);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            SetRandomUneatenPizzaTextCommand = new Command(
                execute: async () =>
                {
                    // called when button is clicked
                    IsBusy = true;
                    await GenerateRandomUneatenPizza();
                    IsBusy = false;
                },
                canExecute: () =>
                {
                    // called when binding is first defined in View and when CanExecuteChanged is fired (fired when calling ChangeCanExecute())
                    //return CanGenerate;
                    return !IsBusy;
                });

            UpdateEatenStatusCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    GeneratedPizza.Eaten = true;
                    GeneratedPizza.DateEaten = DateTime.Today;
                    await pizzaMan.SavePizzaAsync(GeneratedPizza);
                    CachedData.AddToEatenPizzaCache(GeneratedPizza);
                    // Send Successful Update message to the View
                    MessagingCenter.Send(this, nameof(Messages.SuccessfulUpdate));
                    GeneratedPizza = null;
                    IsBusy = false;
                },
                canExecute: () =>
                {
                    return GeneratedPizza != null && !IsBusy;
                });

            CommandCanExecuteProperties.Add(SetRandomUneatenPizzaTextCommand, new List<string> { nameof(IsBusy) });
            CommandCanExecuteProperties.Add(UpdateEatenStatusCommand, new List<string> { nameof(IsBusy), nameof(GeneratedPizza) });
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
