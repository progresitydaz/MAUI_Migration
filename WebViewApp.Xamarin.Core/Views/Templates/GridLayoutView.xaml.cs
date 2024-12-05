using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Models;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class GridLayoutView : ContentView
    {
        private readonly int _numColumns;

        public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create("DeleteCommand",
                         typeof(ICommand),
                         typeof(GridLayoutView),
                         null,
                         BindingMode.OneWay);

        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand)GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }

        public static readonly BindableProperty DataSourceProperty =
        BindableProperty.Create("DataSource",
                             typeof(ObservableCollection<GalleryListItemModel>),
                             typeof(GridLayoutView),
                             null,
                             BindingMode.OneWay);

        public ObservableCollection<GalleryListItemModel> DataSource
        {
            get
            {
                return (ObservableCollection<GalleryListItemModel>)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }

        public GridLayoutView()
        {
            InitializeComponent();

            PropertyChanged += Component_PropertyChanged;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                _numColumns = 4;
            }
            else
            {
                _numColumns = 2;
            }
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataSource))
            {
                RenderGrid(DataSource);
            }
        }

        private void RenderGrid(IEnumerable<GalleryListItemModel> dataSource)
        {
            List<GalleryListItemModel> dataSourceToRender = dataSource.ToList();

            ItemContainer.Children.Clear();

            foreach (GalleryListItemModel item in dataSourceToRender)
            {
                GridImageView imageView = new GridImageView();
                imageView.Margin = new Thickness(5,5);

                imageView.SetBinding(GridImageView.ImageSourceProperty, new Binding(".", source: item));
                imageView.SetBinding(GridImageView.DeleteCommandProperty, new Binding("DeleteCommand", source: this));

                imageView.BindingContext = item;
                ItemContainer.Children.Add(imageView);
            }
        }
    }
}
