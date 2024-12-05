using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class ActivityIndicatorView : ContentView
    {
        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
                                                         "IsBusy",
                                                         typeof(bool),
                                                         typeof(ActivityIndicatorView),
                                                         false,
                                                         defaultBindingMode: BindingMode.OneWay);

        public ActivityIndicatorView()
        {
            InitializeComponent();

            PropertyChanged += ActivityView_PropertyChanged;

        }

        void ActivityView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsBusy))
            {
                //BusyIndicator.is = IsBusy;

                AutomationProperties.SetHelpText(BusyIndicator, IsBusy ? "System ist beschäftigt" : "System ist nicht belegt");
            }
        }
    }
}
