using System;
using System.Drawing;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.ViewModels;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Models
{
    public class HomeMenuItem : ExtendedBindableObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type ViewModel { get; set; }

        public Style Style { get; set; }

        string _formattedValue;
        public string FormattedValue
        {
            get
            {
                return _formattedValue;
            }
            set
            {
                _formattedValue = value;
                IsFormattedTextVisible = !string.IsNullOrEmpty(value);
                RaisePropertyChanged(() => FormattedValue);
                RaisePropertyChanged(() => IsFormattedTextVisible);
            }
        }

        bool _isFormattedTextVisible;
        public bool IsFormattedTextVisible
        {
            get
            {
                return _isFormattedTextVisible;
            }
            set
            {
                _isFormattedTextVisible = value;
                RaisePropertyChanged(() => IsFormattedTextVisible);
            }
        }


        public HomeMenuItem()
        {

        }

        public static HomeMenuItem Create(PageType pageType)
        {
            HomeMenuItem menuItem = null;

            var resources = Application.Current.Resources;

            var sectionSeparatorStyle = (Style)resources["MenuSectionSeparatorStyle"];
            var menuSeparatorStyle = (Style)resources["MenuItemSeparatorStyle"];

            switch (pageType)
            {
                case PageType.Home:
                    menuItem = new HomeMenuItem
                    {
                        Id = (int)PageType.Home,
                        Title = AppResources.Home,
                        IconSource = "home.png",
                        ViewModel = typeof(InnerWebViewModel),
                        Style = sectionSeparatorStyle
                    };
                    break;
                case PageType.About:
                    menuItem = new HomeMenuItem
                    {
                        Id = (int)PageType.About,
                        Title = AppResources.About,
                        IconSource = "about_app.png",
                        ViewModel = typeof(InnerWebViewModel),
                        Style = sectionSeparatorStyle
                    };
                    break;
                case PageType.Contact:
                    menuItem = new HomeMenuItem
                    {
                        Id = (int)PageType.Contact,
                        Title = AppResources.Contact,
                        IconSource = "contact.png",
                        ViewModel = typeof(InnerWebViewModel),
                        Style = sectionSeparatorStyle
                    };
                    break;
            }
            return menuItem;
        }
    }
}
