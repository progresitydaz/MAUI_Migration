using System;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Controls
{
    public class ExtendedButton : Button
    {
        private Style _normalStyle;
        private Style _disabledStyle;

        public static readonly BindableProperty IsEnabledCustomProperty =
        BindableProperty.Create("IsEnabledCustom",
                                typeof(bool),
                                typeof(ExtendedButton),
                                false,
                                BindingMode.OneWay,
                                propertyChanged: IsEnabledCustomPropertyChanged);

        public bool IsEnabledCustom
        {
            get
            {
                return (bool)GetValue(IsEnabledCustomProperty);
            }
            set
            {
                SetValue(IsEnabledCustomProperty, value);
            }
        }

        public ExtendedButton()
        {
            var resources = Application.Current.Resources;

            _normalStyle = (Style)resources["FormButtonStyle"];

            _disabledStyle = (Style)resources["FormButtonDisabledStyle"];

            PropertyChanged += ExtendedButton_PropertyChanged;

            SetStyle(IsEnabledCustom);

        }

        private void SetStyle(bool isEnabledCustom)
        {
            if (isEnabledCustom)
            {
                Style = _normalStyle;
            }
            else
            {
                Style = _disabledStyle;
            }
        }

        private void ExtendedButton_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsEnabledCustom))
            {
                SetStyle(IsEnabledCustom);
            }
        }

        private static void IsEnabledCustomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }
    }
}
