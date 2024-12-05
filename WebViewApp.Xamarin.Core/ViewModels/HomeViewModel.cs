using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Actions

        #endregion

        #region Bindables

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
                RaisePropertyChanged(() => Url);
            }
        }

        string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                RaisePropertyChanged(() => PageTitle);
            }
        }

        #endregion

        #region Commands

        #endregion

        public HomeViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, ISettingsService settingsService, IApplicationDataService applicationDataService,
            IUserService userService, IAnalyticsService analyticsService,
            ILocalDbContextService localDbContext, IPlatformManager platformManager)
          : base(connectionService, navigationService, dialogService, settingsService, applicationDataService,
                userService, analyticsService, localDbContext, platformManager)
        {

        }

        public override async Task Initialize(object navigationData)
        {
            WebViewArgs webViewArgs = navigationData as WebViewArgs;

            if (webViewArgs != null)
            {
                Url = webViewArgs.Url;

                PageTitle = webViewArgs.PageTitle;
            }
            else
            {
                PageTitle = AppResources.Home;
                Url = "https://kosmetik-romy.ch";
            }

            await Task.FromResult(true);
        }

        public async override Task OnAppearing()
        {
            await Task.FromResult(true);
        }

        public async override Task OnDisappearing()
        {
            await Task.FromResult(true);
        }
    }
}
