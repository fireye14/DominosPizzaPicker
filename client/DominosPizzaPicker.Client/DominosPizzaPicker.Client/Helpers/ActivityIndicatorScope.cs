using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers
{
    // Usage of this class, where syncIndicator is the name of an ActivityIndicator in the xaml:
    //
    //    using (var scope = new ActivityIndicatorScope(syncIndicator, showIndicator: true))
    //    {
    //        pizzaList.ItemsSource = await pizzaMan.Get10RecentAsync();
    //    }



    public class ActivityIndicatorScope : IDisposable
    {
        private bool showIndicator;
        public ActivityIndicator indicator;
        private Task indicatorDelay;

        private bool IndicatorActivity
        {
            set
            {
                if (indicator == null) return;
                indicator.IsVisible = value;
                indicator.IsRunning = value;
            }
        }

        public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
        {
            this.indicator = indicator;
            this.showIndicator = showIndicator;

            if (showIndicator)
            {
                indicatorDelay = Task.Delay(1000);
                IndicatorActivity = true;
            }
            else
            {
                indicatorDelay = Task.FromResult(0);
            }
        }

        public void Dispose()
        {
            if (showIndicator)
            {
                indicatorDelay.ContinueWith(t => IndicatorActivity = false, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
