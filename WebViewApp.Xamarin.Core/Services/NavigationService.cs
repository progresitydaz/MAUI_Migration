using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.ViewModels;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using WebViewApp.Xamarin.Core.Views;
using WebViewApp.Xamarin.Core.Views.Base;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task ResumeAsync();

        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type type, object parameter = null);

        Task ClearBackStack();

        Task PerformLogout();

        void OnAppConfigurationChanged(GetConfigurationResponse appConfiguration);

        Task NavigateToMenu(PageType pageType);

        void NotifyNewMessage(PushNotificationData args);

        Task NavigateToBack(bool isRoot = false);

        ViewModelBase GetCurrentViewModel();
    }

    public class NavigationService : BaseService, INavigationService
    {
        private readonly ISettingsService _settingsService;

        protected Application CurrentApplication => Application.Current;

        public NavigationService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public Task ResumeAsync()
        {
            var vm = GetCurrentViewModel();

            if (vm is InnerWebViewModel)
            {
                return Task.CompletedTask;
            }
            else
            {
                return NavigateToAsync<InnerWebViewModel>();
            }
        }

        private async void CheckForPushEnabled()
        {
            var platformManager = DependencyService.Get<IPlatformManager>();

            bool isEnabled = await platformManager.CheckPushEnabled(true);
        }


        public Task InitializeAsync()
        {
            return NavigateToAsync<MainViewModel>();
        }

        public Task NavigateToAsync(Type type, object parameter = null)
        {
            return InternalNavigateToAsync(type, parameter);
        }

        public Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else
            {
                var mainPage = CurrentApplication.MainPage as MainView;

                mainPage.Detail = page;

                mainPage.IsPresented = false;
            }

            await (page.BindingContext as ViewModelBase).Initialize(parameter);
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = null;

            page = Activator.CreateInstance(pageType) as Page;

            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        public async Task ClearBackStack()
        {
            await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        public async Task PerformLogout()
        {
            GlobalSetting.Instance.LoggedOut();

            var mainPage = CurrentApplication.MainPage as MainView;

            var menuPage = mainPage.Flyout as MenuView;

            var menuViewModel = menuPage.BindingContext as MenuViewModel;

            menuViewModel.OnLoggedOut();

            await NavigateToAsync<InnerWebViewModel>();
        }

        public void OnAppConfigurationChanged(GetConfigurationResponse appConfiguration)
        {
            MenuViewModel vm = GetMenuViewModel();

            if (vm != null)
            {
                vm.OnAppConfigurationChanged(appConfiguration);
            }
        }

        public async Task NavigateToMenu(PageType pageType)
        {
            MenuViewModel vm = GetMenuViewModel();

            if (vm != null)
            {
                await vm.OnNavigateRequested(pageType);
            }
        }

        public void NotifyNewMessage(PushNotificationData args)
        {
            ViewModelBase vm = GetCurrentViewModel();

            if (vm != null)
            {
                vm.Notification = args;

                //if (vm is ChatViewModel)
                //{
                //    await vm.OnAppearing();
                //}
            }
        }

        private MenuViewModel GetMenuViewModel()
        {
            MenuViewModel vm = null;

            var mainPage = CurrentApplication.MainPage as MainView;

            if (mainPage != null)
            {
                MenuView menuView = mainPage.Flyout as MenuView;

                vm = menuView?.BindingContext as MenuViewModel;
            }

            return vm;
        }

        public ViewModelBase GetCurrentViewModel()
        {
            ViewModelBase vm = null;

            var currentPage = CurrentApplication.MainPage;

            var mainPage = currentPage as MainView;

            if (mainPage != null)
            {
                ContentPage view = mainPage.Detail as ContentPage;

                vm = view?.BindingContext as ViewModelBase;
            }
            else
            {
                vm = currentPage?.BindingContext as ViewModelBase;
            }

            return vm;
        }

        public async Task NavigateToBack(bool isRoot = false)
        {
            try
            {
                var navigationPage = GetNavigationPage();

                if (isRoot)
                {
                    await navigationPage?.PopToRootAsync();
                }
                else
                {
                    await navigationPage?.PopAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException($"Exception occured - NavigateToBack {0}", ex);
            }
        }

        private NavigationPage GetNavigationPage()
        {
            NavigationPage navigationPage = null;

            if (CurrentApplication.MainPage is NavigationPage)
            {
                navigationPage = CurrentApplication.MainPage as NavigationPage;
            }

            return navigationPage;
        }
    }
}
