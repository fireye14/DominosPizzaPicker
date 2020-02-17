using DominosPizzaPicker.Client.Helpers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using DominosPizzaPicker.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DominosPizzaPicker.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickRandom : CustomContentPage<PickRandomViewModel>
	{
        public PickRandom()
        {
            // Subscribe to Successful Update message
            this.Subscribe<PickRandomViewModel>(nameof(Messages.SuccessfulUpdate), MessageSubscriptionType.DisplayAlert);
        }
    }
}