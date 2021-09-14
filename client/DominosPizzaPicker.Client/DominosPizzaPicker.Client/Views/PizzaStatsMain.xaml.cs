using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PizzaStatsMain : CustomContentPage<PizzaStatsMainViewModel>
    {
        public PizzaStatsMain()
        {
            ViewModel.LoadStats();
        }
    }
}