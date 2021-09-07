using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePizza : CustomContentPage<UpdatePizzaViewModel>
    {
        public UpdatePizza(string Id)
        {
            ViewModel.LoadPizza(Id);
            // Subscribe to Successful Update message
            this.Subscribe<UpdatePizzaViewModel>(nameof(Messages.SuccessfulUpdate), MessageSubscriptionType.DisplayAlert);
        }
    }
}