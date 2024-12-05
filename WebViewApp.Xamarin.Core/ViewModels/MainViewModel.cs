using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Essentials;

namespace WebViewApp.Xamarin.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Actions

        #endregion

        #region Bindables

        #endregion

        #region Commands

        #endregion

        public MainViewModel(IConnectionService connectionService, INavigationService navigationService,
            IDialogService dialogService, ISettingsService settingsService, IApplicationDataService applicationDataService,
            IUserService userService, IAnalyticsService analyticsService,
            ILocalDbContextService localDbContext, IPlatformManager platformManager)
          : base(connectionService, navigationService, dialogService, settingsService, applicationDataService,
                userService, analyticsService, localDbContext, platformManager)
        {

        }

        public override async Task Initialize(object navigationData)
        {
            IsBusy = true;

            await SetTokenToClipBoard();

            await _navigationService.NavigateToAsync<InnerWebViewModel>();

            IsBusy = false;
        }

        private async Task SetTokenToClipBoard()
        {
            try
            {
                await Clipboard.SetTextAsync(_settingsService.PushNotificationToken);
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowToast(ex.Message);
            }
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
