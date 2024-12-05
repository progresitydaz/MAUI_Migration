using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class TabButtonView : ContentView
    {
        public static readonly BindableProperty IsSelectedCustomProperty =
        BindableProperty.Create("IsSelected",
                               typeof(bool),
                               typeof(TabButtonView),
                               false,
                               BindingMode.OneWay);

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedCustomProperty);
            }
            set
            {
                SetValue(IsSelectedCustomProperty, value);
            }
        }

        public TabButtonView()
        {
            InitializeComponent();
            PropertyChanged += Component_PropertyChanged;
            SetStyle(IsSelected);
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsSelected))
            {
                SetStyle(IsSelected);
            }
        }

        public void SetStyle(bool isSelected)
        {
            var resources = Application.Current.Resources;

            var normalLabelStyle = (Style)resources["TabButtonLabelStyle"];

            var selectedLabelStyle = (Style)resources["TabButtonLabelSelectedStyle"];

            CaptionLabel.Style = isSelected ? selectedLabelStyle : normalLabelStyle;
        }
    }
}
