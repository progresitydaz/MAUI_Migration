using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class HeaderView : ContentView
    {
        public string TitleText
        {
            get
            {
                return (string)GetValue(TitleTextProperty);
            }
            set
            {
                SetValue(TitleTextProperty, value);
            }
        }

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                         "TitleText",
                                                         typeof(string),
                                                         typeof(HeaderView),
                                                         string.Empty,
                                                         defaultBindingMode: BindingMode.OneWay);

        public HeaderView()
        {
            InitializeComponent();

            PropertyChanged += HeaderView_PropertyChanged;
        }

        private void HeaderView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TitleText))
            {
                AppTitleLabel.Text = TitleText;
            }
        }

        private void OnDrawerButtonTapped(object sender, EventArgs args)
        {
            var mainPage = Application.Current.MainPage as MainView;

            mainPage.IsPresented = !mainPage.IsPresented;
        }
    }
}
