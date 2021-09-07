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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicator">the ActivityIndicator object</param>
        /// <param name="showIndicator">true to show the indicator, false to do nothing</param>
        /// <param name="indicatorDelay">minimum amount of time, in ms, that the indicator will be shown for</param>
        public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator, int indicatorDelayMs)
        {
            this.indicator = indicator;
            this.showIndicator = showIndicator;

            if (showIndicator)
            {
                indicatorDelay = Task.Delay(indicatorDelayMs);
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
