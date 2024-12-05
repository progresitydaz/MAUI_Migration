using System;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Behaviors
{
    public class EntryValidationBehavior : Behavior<Editor>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Editor)sender;

            bool isValid = true;

            if (entry.Text.Length > this.MaxLength)
            {
                isValid = false;
            }

            var setValue = isValid ? e.NewTextValue : e.OldTextValue;

            if (setValue != null)
            {
                entry.Text = setValue;
            }
            else
            {
                entry.Text = string.Empty;
            }
        }
    }
}
