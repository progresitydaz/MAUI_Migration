using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;
using PP = Plugin.Permissions;

namespace WebViewApp.Xamarin.Core.ViewModels
{
    public class InnerWebViewModel : ViewModelBase
    {
        #region Fields

        public bool IsFirstTime { get; set; } = true;

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

        public InnerWebViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, ISettingsService settingsService, IApplicationDataService applicationDataService,
            IUserService userService, IAnalyticsService analyticsService,
            ILocalDbContextService localDbContext, IPlatformManager platformManager)
          : base(connectionService, navigationService, dialogService, settingsService, applicationDataService,
                userService, analyticsService, localDbContext, platformManager)
        {
            Url = GlobalSetting.Instance.BaseGatewayEndpoint;
        }

        public override async Task Initialize(object navigationData)
        {
            WebViewArgs webViewArgs = navigationData as WebViewArgs;

            if (webViewArgs != null)
            {
                Url = webViewArgs.Url;

                PageTitle = webViewArgs.PageTitle;
            }

            await Task.FromResult(true);
        }

        public async override Task OnAppearing()
        {
            await Task.FromResult(true);

            TrackView();

            await CheckRunTimePermission();
        }

        public async override Task OnDisappearing()
        {
            await Task.FromResult(true);
        }

        public async override Task NavigateToBack()
        {
            await _navigationService.NavigateToBack();
        }

        public virtual void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (IsFirstTime)
            {
                IsFirstTime = false;

                IsBusy = true;
            }
        }

        public virtual void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
        }

        private async Task CheckRunTimePermission()
        {
            try
            {
                var locationStatus = await PermissionHelper.RequestPermission<Permissions.LocationWhenInUse>();
                var storageReadStatus = await PermissionHelper.RequestPermission<Permissions.StorageRead>();
                var storageWriteStatus = await PermissionHelper.RequestPermission<Permissions.StorageWrite>();
                var cameraStatus = await PermissionHelper.RequestPermission<Permissions.Camera>();
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Exception occured while requestiong the permissions", ex);
            }
        }
    }
}
