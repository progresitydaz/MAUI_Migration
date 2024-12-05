using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using WebViewApp.Xamarin.Core;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Droid.Activities;
using WebViewApp.Xamarin.Droid.Helpers;
using Xamarin.Forms;
using static Android.App.ActivityManager;

namespace WebViewApp.Xamarin.Droid.PushNotifications
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        const string TAG = "FirebaseNotificationService";

        private IApplicationDataService _applicationDataService;

        private ISettingsService _settingsService;

        public FirebaseNotificationService()
        {
            _applicationDataService = AppDependencyResolver.Resolve<IApplicationDataService>();

            _settingsService = AppDependencyResolver.Resolve<ISettingsService>();
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            var refreshedToken = p0;
            SendRegistrationTokenToNotificationHub(refreshedToken);
        }

        public async override void OnMessageReceived(RemoteMessage message)
        {
            LogHelper.LogPushEvent("ReceivedRemoteNotification", message.Data, _settingsService.UserName);

            try
            {
                bool isForground = GetIsForground();

                PushNotificationData pushNotification = PushHelper.GetPushNotificationData(message.Data);

                if (pushNotification != null)
                {
                    if (isForground)
                    {
                        IDialogService dialogService = AppDependencyResolver.Resolve<IDialogService>();

                        await dialogService.ShowDialog(pushNotification.Body, pushNotification.Title, "OK");
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.LogException("Error occured when pushing the notification", ex);
            }
        }

        private bool GetIsForground()
        {
            RunningAppProcessInfo myProcess = new RunningAppProcessInfo();

            GetMyMemoryState(myProcess);

            bool isInBackground = myProcess.Importance != Android.App.Importance.Foreground;

            return !isInBackground;
        }

        private async void SendRegistrationTokenToNotificationHub(string token)
        {
            _settingsService.PushNotificationToken = token;
            _settingsService.NewPushNotificationToken = token;

            if (!string.IsNullOrEmpty(_settingsService.AuthRefreshToken))
            {
                UpdateDeviceTokenRequest request = new UpdateDeviceTokenRequest()
                {
                    DevicePlatform = _settingsService.DevicePlatform,
                    DeviceToken = token,
                };

                var response = await _applicationDataService.UpdateDeviceToken(request);

                if (response != null && response.IsSuccessful && response.IsSaved)
                {
                    _settingsService.NewPushNotificationToken = string.Empty;
                }
                else
                {
                    LogHelper.LogEvent("SendRegTokenToNotificationHub- Error", string.Empty);
                }
            }

            System.Diagnostics.Debug.WriteLine(token);
        }
    }
}
