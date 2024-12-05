using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using XF = Xamarin.Forms;
using AndroidSpecific = Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WebViewApp.Xamarin.Core
{
    public partial class App : XF.Application
    {
        public App()
        {
            InitializeComponent();

            InitApp();

            AndroidSpecific.Application.SetWindowSoftInputModeAdjust(this, AndroidSpecific.WindowSoftInputModeAdjust.Resize);
        }

        private void InitApp()
        {

#if DEBUG

            GlobalSetting.Instance.BaseGatewayEndpoint = ApiConstants.CaretakerHomePilotEndPoint;
#endif

#if RELEASE
            GlobalSetting.Instance.BaseGatewayEndpoint = ApiConstants.CaretakerHomePilotEndPoint;
#endif

            AppDependencyResolver.RegisterComponents();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await InitNavigation();

            SetupAppCenter();

            base.OnResume();
        }

        private void SetupAppCenter()
        {
            try
            {
                AppCenter.Start("ios=f65fb3fd-7e55-41fa-8bd1-3750a3b745cf;" +
                                "android=8b48621f-4022-4133-bfbb-463eb9c88c94;",
                                typeof(Analytics), typeof(Crashes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured while starting the app center {0}", ex);
            }
        }

        private Task InitNavigation()
        {
            var navigationService = AppDependencyResolver.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        private Task ResumeNavigation()
        {
            var navigationService = AppDependencyResolver.Resolve<INavigationService>();

            return navigationService.ResumeAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected async override void OnResume()
        {
            try
            {
                await ResumeNavigation();
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Exception occured while resuming the app", ex);
            }
        }
    }
}
