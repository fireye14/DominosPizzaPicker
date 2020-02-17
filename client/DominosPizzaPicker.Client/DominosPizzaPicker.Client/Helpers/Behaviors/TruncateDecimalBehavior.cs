using System.Linq;
using Xamarin.Forms;

namespace DominosPizzaPicker.Client.Helpers.Behaviors
{
    public class TruncateDecimalBehavior : Behavior<Entry>
    {
        public int NumDecimals { get; set; }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var input = e.NewTextValue;
            if (!string.IsNullOrEmpty(input))
            {
                if (!(sender is Entry entry)) return;                

                // If there are more than numDec digits after the decimal, take off the last digit
                if (input.Contains(".") && (NumDecimals == 0 || input.Substring(input.IndexOf(".") + 1).Length > NumDecimals))
                {
                    entry.Text = input.Substring(0, input.Length - 1);
                }
            }
        }
    }
}
