using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class ImageGalleryView : ContentView
    {
        public static readonly BindableProperty DataSourceProperty =
        BindableProperty.Create("DataSource",
                           typeof(ObservableCollection<GalleryListItemModel>),
                           typeof(ImageGalleryView),
                           null,
                           BindingMode.OneWay);

        public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create("IsReadOnly",
                          typeof(bool),
                          typeof(ImageGalleryView),
                          false,
                          BindingMode.OneWay);

        public static readonly BindableProperty ClickedCommandProperty =
        BindableProperty.Create("ClickedCommand",
                          typeof(ICommand),
                          typeof(ImageGalleryView),
                          null,
                          BindingMode.OneWay);

        public static readonly BindableProperty IsFullScreenProperty =
        BindableProperty.Create("IsFullScreen",
                         typeof(bool),
                         typeof(ImageGalleryView),
                         false,
                         BindingMode.OneWay);

        public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create("SelectedItem",
                         typeof(GalleryListItemModel),
                         typeof(ImageGalleryView),
                         null,
                         BindingMode.OneWay);

        public ICommand ClickedCommand
        {
            get
            {
                return (ICommand)GetValue(ClickedCommandProperty);
            }
            set
            {
                SetValue(ClickedCommandProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        public bool IsFullScreen
        {
            get
            {
                return (bool)GetValue(IsFullScreenProperty);
            }
            set
            {
                SetValue(IsFullScreenProperty, value);
            }
        }

        public GalleryListItemModel SelectedItem
        {
            get
            {
                return (GalleryListItemModel)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

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

        public int CurrentIndex { get; set; }

        public ImageGalleryView()
        {
            InitializeComponent();

            if (!IsFullScreen)
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

                double imageHeightRequest = mainDisplayInfo.Height / (3 * mainDisplayInfo.Density);

                double imageHeight = Device.Idiom == TargetIdiom.Tablet ? 400 : imageHeightRequest;

                ImageView.HeightRequest = imageHeight;
            }

            PropertyChanged += View_PropertyChanged;
            CloseButton.IsVisible = false;
        }

        private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataSource))
            {
                SetDataSource();
            }
            else if (e.PropertyName == nameof(IsReadOnly))
            {
                DeleteButton.IsVisible = !IsReadOnly;
            }
            else if (e.PropertyName == nameof(IsFullScreen))
            {
                CloseButton.IsVisible = IsFullScreen;
                DeleteButton.IsVisible = !IsFullScreen;
            }
            else if (e.PropertyName == nameof(SelectedItem))
            {
                SelectImage(SelectedItem);
            }
        }

        private void SetDataSource()
        {
            DotContainer.Children.Clear();

            if (DataSource.Any())
            {
                GalleryListItemModel firstItem = DataSource.FirstOrDefault();

                SelectImage(firstItem);
            }
        }

        private void SelectImage(GalleryListItemModel galleryItem)
        {
            if (galleryItem != null)
            {
                ImageView.Source = galleryItem.ImageSource;
                ImageView.BindingContext = galleryItem;

                CurrentIndex = DataSource.IndexOf(galleryItem);

                DrawDots(DataSource);
            }
        }

        private void DrawDots(ObservableCollection<GalleryListItemModel> dataSource)
        {
            DotContainer.Children.Clear();

            int i = 0;

            foreach (var item in dataSource)
            {
                var image = new Image()
                {
                    Margin = new Thickness(-5, 2),
                    BackgroundColor = Color.Transparent,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 24,
                    WidthRequest = 24,
                    BindingContext = item,
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    Image tappedImage = s as Image;

                    GalleryListItemModel galleryItem = tappedImage.BindingContext as GalleryListItemModel;

                    SelectImage(galleryItem);
                };

                image.GestureRecognizers.Add(tapGestureRecognizer);

                if (i == CurrentIndex)
                    image.Source = "Progress_dot_filled.png";
                else
                    image.Source = "Progress_dot_light.png";

                DotContainer.Children.Add(image);

                i++;
            }
        }

        private void Delete_ButtonClcked(object sender, System.EventArgs e)
        {
            if (DataSource != null)
            {
                GalleryListItemModel item = DataSource[CurrentIndex];
            }
        }

        private void Close_ButtonClcked(object sender, System.EventArgs e)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            var vm = BindingContext as ViewModelBase;

            vm.IsFullScreenPopupVisible = false;
        }

        protected void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left)
            {
                int nextIndex = CurrentIndex + 1;

                if (nextIndex < DataSource.Count)
                {
                    GalleryListItemModel nextitem = DataSource[nextIndex];

                    SelectImage(nextitem);
                }
            }
            else if (e.Direction == SwipeDirection.Right)
            {
                int prevIndex = CurrentIndex - 1;

                if (prevIndex >= 0)
                {
                    GalleryListItemModel nextitem = DataSource[prevIndex];

                    SelectImage(nextitem);
                }
            }
            else if (e.Direction == SwipeDirection.Down)
            {
                if (IsFullScreen)
                {
                    ClosePopup();
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image tappedImage = sender as Image;

            GalleryListItemModel galleryItem = tappedImage.BindingContext as GalleryListItemModel;

            ClickedCommand?.Execute(galleryItem);
        }
    }
}
