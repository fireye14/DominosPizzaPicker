using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using DominosPizzaPicker.Client.Droid.CustomRenderers;
using DominosPizzaPicker.Client.Helpers.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace DominosPizzaPicker.Client.Droid.CustomRenderers
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        #region Constructors

        public CustomNavigationPageRenderer(Context context) : base(context)
        {
        }

        #endregion

        #region Overrides

        // always push and pop without animation
        protected override Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            return base.OnPopViewAsync(page, false);
        }

        protected override Task<bool> OnPushAsync(Page page, bool animated)
        {
            return base.OnPushAsync(page, false);
        }

        #endregion
    }
}