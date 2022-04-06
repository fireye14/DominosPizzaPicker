using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickRandomPizza : CustomContentPage<PickRandomPizzaViewModel>
    {
        public PickRandomPizza()
        {
            
            ViewModel.ToppingPickerList = new List<Picker> { pckTopping1, pckTopping2, pckTopping3 };
            ViewModel.LoadLists();
            
            this.Subscribe<PickRandomPizzaViewModel>(nameof(Messages.DuplicateToppingNotAllowed), MessageSubscriptionType.DisplayAlert);
            this.Subscribe<PickRandomPizzaViewModel>(nameof(Messages.NoPizzasExist), MessageSubscriptionType.DisplayAlert);
            this.Subscribe<PickRandomPizzaViewModel>(nameof(Messages.SuccessfulUpdate), MessageSubscriptionType.DisplayAlert);
        }

    }
}