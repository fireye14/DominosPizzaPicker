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
	public partial class UpdateSelectSpecific : CustomContentPage<UpdateSelectSpecificViewModel>
	{
		public UpdateSelectSpecific ()
		{
            ViewModel.Navigation = Navigation;
        }

        #region Overrides

        protected async override void OnAppearing()
        {
            ViewModel.MeatListView = MeatList;
            ViewModel.NonMeatListView = NonMeatList;
            await ViewModel.LoadLists();
        }

        #endregion
    }
}