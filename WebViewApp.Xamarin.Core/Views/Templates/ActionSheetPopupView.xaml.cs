using System;
using System.Collections.Generic;
using System.ComponentModel;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class ActionSheetPopupView : ContentView
    {
        public static readonly BindableProperty DataSourceProperty =
        BindableProperty.Create("DataSource",
                           typeof(List<ListItemModel>),
                           typeof(ActionSheetPopupView),
                           null,
                           BindingMode.OneWay);

        public List<ListItemModel> DataSource
        {
            get
            {
                return (List<ListItemModel>)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }

        public ActionSheetPopupView()
        {
            InitializeComponent();
            PropertyChanged += Cell_PropertyChanged;
        }

        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataSource))
            {
                //TODO PAMODA PopupListView.ItemsSource = DataSource.ToObservableCollection();
            }
        }

        private void OnCloseButtonTapped(object sender, EventArgs args)
        {
            var vm = BindingContext as ViewModelBase;

            vm.IsActionsheetVisible = false;
        }
    }
}
