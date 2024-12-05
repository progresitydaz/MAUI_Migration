
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly IConnectionService _connectionService;
        protected readonly INavigationService _navigationService;
        protected readonly IDialogService _dialogService;
        protected readonly ISettingsService _settingsService;
        protected readonly IAnalyticsService _analyticsService;
        protected readonly IApplicationDataService _applicationDataService;
        protected readonly IUserService _userService;
        protected readonly ILocalDbContextService _localDbContext;
        protected readonly IPlatformManager _platformManager;

        public ViewModelBase(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, ISettingsService settingsService, IApplicationDataService applicationDataService,
            IUserService userService, IAnalyticsService analyticsService,
            ILocalDbContextService localDbContext, IPlatformManager platformManager)
        {
            _connectionService = connectionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _settingsService = settingsService;
            _analyticsService = analyticsService;
            _localDbContext = localDbContext;
            _platformManager = platformManager;
            _applicationDataService = applicationDataService;
            _userService = userService;

            IsNotificationAvailable = false;
        }

        #region Commands

        public ICommand ActionItemTappedCommand { protected set; get; }

        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsLogin
        {
            get
            {
                return GlobalSetting.Instance.IsLoggedIn;
            }
        }

        private bool _isNotificationAvailable;
        public bool IsNotificationAvailable
        {
            get
            {
                return _isNotificationAvailable;
            }

            set
            {
                _isNotificationAvailable = value;
                RaisePropertyChanged(() => IsNotificationAvailable);
            }
        }

        private PushNotificationData _notification;
        public PushNotificationData Notification
        {
            get
            {
                return _notification;
            }

            set
            {
                _notification = value;
                IsNotificationAvailable = value != null;
                RaisePropertyChanged(() => Notification);
            }
        }

        private bool _isAttachmentEnabled;
        public bool IsAttachmentEnabled
        {
            get
            {
                return _isAttachmentEnabled;
            }

            set
            {
                _isAttachmentEnabled = value;
                RaisePropertyChanged(() => IsAttachmentEnabled);
            }
        }

        private string _activityMessage;
        public string ActivityMessage
        {
            get
            {
                return _activityMessage;
            }

            set
            {
                _activityMessage = value;
                RaisePropertyChanged(() => ActivityMessage);
            }
        }

        private bool _isImageGalleryShowing;
        public bool IsImageGalleryShowing
        {
            get
            {
                return _isImageGalleryShowing;
            }

            set
            {
                _isImageGalleryShowing = value;
                RaisePropertyChanged(() => IsImageGalleryShowing);
            }
        }

        ObservableCollection<ListItemModel> _actionListItems;
        public ObservableCollection<ListItemModel> ActionListItems
        {
            get
            {
                return _actionListItems;
            }
            set
            {
                _actionListItems = value;
                RaisePropertyChanged(() => ActionListItems);
            }
        }

        private bool _isActionsheetVisible;
        public bool IsActionsheetVisible
        {
            get
            {
                return _isActionsheetVisible;
            }

            set
            {
                _isActionsheetVisible = value;
                RaisePropertyChanged(() => IsActionsheetVisible);
            }
        }

        public bool IsPickIntentOn
        {
            get
            {
                return GlobalSetting.Instance.IsPickIntentOn;
            }

            set
            {
                GlobalSetting.Instance.IsPickIntentOn = value;
            }
        }


        private bool _isFullScreenPopupVisible;
        public bool IsFullScreenPopupVisible
        {
            get
            {
                return _isFullScreenPopupVisible;
            }

            set
            {
                _isFullScreenPopupVisible = value;
                RaisePropertyChanged(() => IsFullScreenPopupVisible);
            }
        }

        private GalleryListItemModel _selectedFullScreenPopupItem;
        public GalleryListItemModel SelectedFullScreenPopupItem
        {
            get
            {
                return _selectedFullScreenPopupItem;
            }

            set
            {
                _selectedFullScreenPopupItem = value;
                RaisePropertyChanged(() => SelectedFullScreenPopupItem);
            }
        }

        protected async Task ShowToast(string message)
        {
            await _dialogService.ShowToast(message);
        }

        public string AppVersionNumber => _settingsService.AppVersionNumber;

        public string DevicePlatform => _settingsService.DevicePlatform;

        public ViewModelBase(IConnectionService connectionService)
        {
            _navigationService = AppDependencyResolver.Resolve<INavigationService>();
        }

        public virtual Task Initialize(object navigationData)
        {
            return Task.FromResult(false);
        }

        public virtual void SubscribeToEvents(object viewArgs = null)
        {

        }

        public virtual void UnSubscribeToEvents()
        {

        }

        public virtual Task OnAppearing()
        {
            return Task.FromResult(true);
        }

        public virtual Task OnDisappearing()
        {
            return Task.FromResult(true);
        }

        public virtual Task NavigateToBack()
        {
            return Task.FromResult(true);
        }

        public virtual Task DisplayCameraActionsheet()
        {
            return Task.FromResult(true);
        }

        public async Task ValidateTokenExpiry(BaseResponse response)
        {
            if (response.IsSessionExpired)
            {
                await _navigationService.PerformLogout();
            }
        }

        protected EnvironmentSetting GetCurrentEnvironmentSetting()
        {
            List<EnvironmentSetting> environmentSettings = _localDbContext.GetEntityList<EnvironmentSetting>();

            EnvironmentSetting environmentSetting = environmentSettings.FirstOrDefault();

            if (environmentSetting == null)
            {
                environmentSetting = EnvironmentSetting.GetDefault();

                _localDbContext.SaveEntity<EnvironmentSetting>(environmentSetting);

                environmentSettings = _localDbContext.GetEntityList<EnvironmentSetting>();

                environmentSetting = environmentSettings.First();
            }

            return environmentSetting;
        }

        public async Task NavigateToMenu(PageType pageType)
        {
            await _navigationService.NavigateToMenu(pageType);
        }

        public void TrackView()
        {
            var viewType = this.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");

            var loggedData = new Dictionary<string, string>
            {
                { "username", _settingsService.UserName },
                { "appVersion", AppVersionNumber },
                { "devicePlatform", DevicePlatform },
            };

            _analyticsService.TrackEvent(viewName, loggedData);
        }

        public async Task InvokeErrorMessage(BaseResponse response, string defaultErrorTitle, string defaultErrorMsg, bool isLogin = false)
        {
            if (response == null)
            {
                await _dialogService.ShowDialog(
                        defaultErrorMsg,
                        defaultErrorTitle,
                        AppResources.OK);

                return;
            }

            if (response.IsTaskCanceled)
                return;

            if (response.IsSessionExpired && !isLogin)
            {
                await OnLogout(false);

                return;
            }

            CommonErrorResponse error = response.Error;

            await ShowError(error, defaultErrorTitle, defaultErrorMsg);
        }

        private Task OnLogout(bool v)
        {
            return Task.FromResult(true);
        }

        protected async Task ShowError(CommonErrorResponse error, string defaultErrorTitle, string defaultErrorMsg)
        {
            string errorTitle = error != null && !string.IsNullOrEmpty(error.ResultTitle) ?
                                                        error.ResultTitle : defaultErrorTitle;
            string errorMsg = string.Empty;

            errorMsg = error != null && !string.IsNullOrEmpty(error.ResultMsgBody) ?
                                                                   error.ResultMsgBody : defaultErrorMsg;
            if (errorMsg.Contains("<!DOCTYPE html"))
            {
                errorMsg = defaultErrorMsg;
            }

            await _dialogService.ShowDialog(
                errorMsg,
                errorTitle,
                AppResources.OK);
        }
    }
}