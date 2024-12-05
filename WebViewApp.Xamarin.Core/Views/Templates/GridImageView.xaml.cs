using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Models;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class GridImageView : ContentView
    {
        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create("ImageSource",
                           typeof(GalleryListItemModel),
                           typeof(GridImageView),
                           null,
                           BindingMode.OneWay);

        public GalleryListItemModel ImageSource
        {
            get
            {
                return (GalleryListItemModel)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create("DeleteCommand",
                          typeof(ICommand),
                          typeof(GridImageView),
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

        public GridImageView()
        {
            InitializeComponent();
            PropertyChanged += Component_PropertyChanged;
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ImageSource))
            {
                SetImageSource(ImageSource);
            }
        }

        private void SetImageSource(GalleryListItemModel imageSource)
        {
            if (imageSource != null)
            {
                if (!string.IsNullOrEmpty(ImageSource.ImageBase64))
                {
                    ImageView.Source = ImageSource.ImageSource;
                }
                else if (!string.IsNullOrEmpty(ImageSource.IconSource))
                {
                    ImageView.Source = ImageSource.IconSource;
                }

                DeleteButton.IsVisible = !ImageSource.IsDefault;
                
            }
            else
            {
                ImageView.Source = "Add_a_photo_blue.png";

                DeleteButton.IsVisible = false;
            }
        }

        private void Delete_ButtonClcked(object sender, System.EventArgs e)
        {
            var item = BindingContext as GalleryListItemModel;

            DeleteCommand?.Execute(item);
        }
    }
}
