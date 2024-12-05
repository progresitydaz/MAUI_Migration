using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Models;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Views
{
    public partial class TabBarView : ContentView
    {
        public static readonly BindableProperty TabItemTappedCommandProperty =
         BindableProperty.Create("TabItemTappedCommand",
                              typeof(ICommand),
                              typeof(TabBarView),
                              null,
                              BindingMode.OneWay);

        public ICommand TabItemTappedCommand
        {
            get
            {
                return (ICommand)GetValue(TabItemTappedCommandProperty);
            }
            set
            {
                SetValue(TabItemTappedCommandProperty, value);
            }
        }

        public static readonly BindableProperty DataSourceProperty =
        BindableProperty.Create("DataSource",
                             typeof(ObservableCollection<ListItemModel>),
                             typeof(TabBarView),
                             null,
                             BindingMode.OneWay);

        public ObservableCollection<ListItemModel> DataSource
        {
            get
            {
                return (ObservableCollection<ListItemModel>)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }

        public TabBarView()
        {
            InitializeComponent();

            PropertyChanged += Component_PropertyChanged;
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataSource))
            {
                RenderItems(DataSource);
            }
        }

        private void RenderItems(ObservableCollection<ListItemModel> dataSource)
        {
            ButtonPanel.Children.Clear();

            var resources = Application.Current.Resources;

            var tabButtonStype = (Style)resources["TabButtonStyle"];

            if (dataSource != null)
            {
                foreach (var item in dataSource)
                {
                    TabButtonView button = new TabButtonView();

                    button.Style = tabButtonStype;

                    button.Margin = new Thickness(0, 5, 0, 0);
                    button.HorizontalOptions = LayoutOptions.FillAndExpand;
                    button.VerticalOptions = LayoutOptions.CenterAndExpand;
                    button.BindingContext = item;
                    button.SetBinding(TabButtonView.IsSelectedCustomProperty, new Binding("IsSelected", source: item));

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) =>
                    {
                        SetTabBar(s, e);
                    };

                    button.GestureRecognizers.Add(tapGestureRecognizer);

                    ButtonPanel.Children.Add(button);
                }
            }
        }

        private void SetTabBar(object s, EventArgs e)
        {
            TabButtonView sender = s as TabButtonView;

            ListItemModel model = sender.BindingContext as ListItemModel;

            if (model != null)
            {
                foreach (var item in model.Siblings)
                {
                    item.IsSelected = model == item;
                }
            }

            TabItemTappedCommand?.Execute(model);
        }
    }
}
