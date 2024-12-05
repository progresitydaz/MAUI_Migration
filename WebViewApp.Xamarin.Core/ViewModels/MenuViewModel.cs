using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Models.Base;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IPhoneService _phoneService;

        #region Actions

        #endregion

        #region Bindables

        private ObservableCollection<HomeMenuItem> _menuItems;
        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        #endregion

        #region Commands

        public ICommand MenuItemTappedCommand => new Command(OnMenuItemTapped);

        public ICommand SocialButtonTappedCommand => new Command(OnSocialButtonTapped);

        #endregion

        public MenuViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, ISettingsService settingsService, IApplicationDataService applicationDataService,
            IUserService userService, IAnalyticsService analyticsService, IPhoneService phoneService,
            ILocalDbContextService localDbContext, IPlatformManager platformManager)
          : base(connectionService, navigationService, dialogService, settingsService, applicationDataService,
                userService, analyticsService, localDbContext, platformManager)
        {
            _phoneService = phoneService;

            MenuItems = CreateMenuItems();
        }

        private ObservableCollection<HomeMenuItem> CreateMenuItems()
        {
            var resources = Application.Current.Resources;

            var sectionSeparatorStyle = (Style)resources["MenuSectionSeparatorStyle"];
            var menuSeparatorStyle = (Style)resources["MenuItemSeparatorStyle"];

            var menuItems = new ObservableCollection<HomeMenuItem>();

            menuItems.Add(HomeMenuItem.Create(PageType.Home));
            menuItems.Add(HomeMenuItem.Create(PageType.Contact));
            menuItems.Add(HomeMenuItem.Create(PageType.About));

            return menuItems;
        }

        public override async Task Initialize(object navigationData)
        {
            IsBusy = true;

            await Task.FromResult(true);

            IsBusy = false;
        }

        private async void OnMenuItemTapped(object menuItemTappedEventArgs)
        {
            ISettingsService settingsService = AppDependencyResolver.Resolve<ISettingsService>();

            var menuItem = ((menuItemTappedEventArgs as ItemTappedEventArgs)?.Item as HomeMenuItem);

            await NavigateToMenuItem(menuItem);
        }

        private async Task NavigateToMenuItem(HomeMenuItem menuItem)
        {
            ISettingsService settingsService = AppDependencyResolver.Resolve<ISettingsService>();

            if (menuItem != null)
            {
                _settingsService.SelectedMenuItemId = menuItem.Id;

                var type = menuItem?.ViewModel;

                object parameter = null;

                if (type == typeof(InnerWebViewModel))
                {
                    WebViewArgs webViewArgs = new WebViewArgs();

                    switch ((PageType)menuItem.Id)
                    {
                        case PageType.Home:
                            webViewArgs.PageTitle = string.Empty;
                            webViewArgs.Url = GlobalSetting.Instance.BaseGatewayEndpoint;
                            break;

                        case PageType.About:
                            webViewArgs.PageTitle = AppResources.About;
                            webViewArgs.Url = "https://www.progresity.com/binnenbeter";
                            break;

                        case PageType.Contact:
                            webViewArgs.PageTitle = AppResources.Contact;
                            webViewArgs.Url = "https://www.progresity.com/contact";
                            break;
                    }

                    parameter = webViewArgs;
                }

                await _navigationService.NavigateToAsync(type, parameter);
            }
        }

        public void OnAppConfigurationChanged(GetConfigurationResponse appConfiguration)
        {
            var resources = Application.Current.Resources;

            var sectionSeparatorStyle = (Style)resources["MenuSectionSeparatorStyle"];
            var menuSeparatorStyle = (Style)resources["MenuItemSeparatorStyle"];

            RaisePropertyChanged(() => MenuItems);
        }

        public void OnLoggedOut()
        {
            MenuItems = CreateMenuItems();

            RaisePropertyChanged(() => MenuItems);
        }

        public override void SubscribeToEvents(object viewArgs = null)
        {

        }

        public override void UnSubscribeToEvents()
        {

        }


        public async Task OnNavigateRequested(PageType pageType)
        {
            HomeMenuItem menuItem = MenuItems.FirstOrDefault(x => x.Id == (int)pageType);

            await NavigateToMenuItem(menuItem);
        }

        public async override Task OnAppearing()
        {
            await Task.FromResult(true);
        }

        public async override Task OnDisappearing()
        {
            await Task.FromResult(true);
        }

        private async void OnSocialButtonTapped(object obj)
        {
            string id = obj as string;

            string url = string.Empty;

            switch (id)
            {
                case "1":
                    url = "https://www.facebook.com/reppido";
                    break;
                case "2":
                    url = "https://www.linkedin.com/company/reppido/";
                    break;
                case "3":
                    url = "https://twitter.com/reppidoNL";
                    break;
            }

            await Launcher.OpenAsync(url);
        }
    }
}
