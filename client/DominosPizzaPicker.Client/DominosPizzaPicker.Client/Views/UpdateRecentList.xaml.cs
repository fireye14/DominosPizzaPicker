using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.Models.Managers;
using DominosPizzaPicker.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateRecentList : CustomContentPage<UpdateRecentListViewModel>
    {
        #region Constructor

        public UpdateRecentList()
        {
            ViewModel.ActivityIndicator = activityIndicator;
            ViewModel.Navigation = Navigation;
        }

        #endregion

        #region Methods

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.LoadList();
        }

        #endregion
    }
}