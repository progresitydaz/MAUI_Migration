using System;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Controls
{
    public class ExtendedEditor : Editor
    {
        public static readonly BindableProperty NextTextFieldProperty =
           BindableProperty.Create(nameof(NextTextField),
                                   typeof(InputView), typeof(ExtendedEditor), null);

        public InputView NextTextField
        {
            get => (InputView)GetValue(NextTextFieldProperty);
            set => SetValue(NextTextFieldProperty, value);
        }

        public static readonly BindableProperty IsMultilineProperty =
           BindableProperty.Create(nameof(IsMultiline),
                                   typeof(bool), typeof(ExtendedEditor), false);

        public bool IsMultiline
        {
            get => (bool)GetValue(IsMultilineProperty);
            set => SetValue(IsMultilineProperty, value);
        }

        public ExtendedEditor()
        {

        }
    }
}
