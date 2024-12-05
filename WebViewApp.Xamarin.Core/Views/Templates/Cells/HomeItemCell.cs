using System;
using System.Collections.Generic;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Models;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class HomeItemCell : ContentView
    {
        public static readonly BindableProperty SelectedCommandProperty =
        BindableProperty.Create("SelectedCommand",
                         typeof(ICommand),
                         typeof(HomeItemCell),
                         null,
                         BindingMode.OneWay);

        public ICommand SelectedCommand
        {
            get
            {
                return (ICommand)GetValue(SelectedCommandProperty);
            }
            set
            {
                SetValue(SelectedCommandProperty, value);
            }
        }

        public HomeItemCell()
        {
            InitializeComponent();
        }

        private void ExtendedButton_Clicked(System.Object sender, System.EventArgs e)
        {
            var item = BindingContext as GalleryListItemModel;

            SelectedCommand?.Execute(item);
        }
    }
}
