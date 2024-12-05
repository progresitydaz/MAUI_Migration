using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.CloudMessaging;
using Firebase.InstanceID;
using Foundation;
using WebViewApp.Xamarin.Core;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.iOS.Helpers;
using UIKit;
using UserNotifications;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace WebViewApp.Xamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        private IApplicationDataService _applicationDataService;

        private ISettingsService _settingsService;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

#if ENABLE_TEST_CLOUD
            Calabash.Start();
#endif

            global::Xamarin.Forms.Forms.Init();

            ConfigureFirebaseServices();

            LoadApplication(new App());

            _applicationDataService = AppDependencyResolver.Resolve<IApplicationDataService>();

            _settingsService = AppDependencyResolver.Resolve<ISettingsService>();

            RegisterForRemoteNotifications(app);

            return base.FinishedLaunching(app, options);
        }

        private void RegisterForRemoteNotifications(UIApplication app)
        {
            try
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    UNUserNotificationCenter.Current.Delegate = this;

                    UNUserNotificationCenter.Current.RequestAuthorization(
                      UNAuthorizationOptions.Alert |
                      UNAuthorizationOptions.Badge |
                      UNAuthorizationOptions.Sound, (approved, err) =>
                      {
                          if (!approved)
                          {
                              //TODO log
                          }
                      });
                }
                else
                {
                    var settings = UIUserNotificationSettings.GetSettingsForTypes(
                                UIUserNotificationType.Alert |
                                UIUserNotificationType.Badge |
                                UIUserNotificationType.Sound,
                                new NSSet());

                    app.RegisterUserNotificationSettings(settings);
                }

                app.RegisterForRemoteNotifications();

                Messaging.SharedInstance.Delegate = this;
                InstanceId.SharedInstance.GetInstanceId(InstanceIdResultHandler);

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
        }

        private async void InstanceIdResultHandler(InstanceIdResult result, NSError error)
        {
            if (error != null)
            {
                LogHelper.LogHandledException(nameof(InstanceIdResultHandler), $"Error: {error.LocalizedDescription}");
            }
            else
            {
                await SendRegistrationTokenToNotificationHub(result.Token);
            }
        }

        private void ConfigureFirebaseServices()
        {
            try
            {
                Firebase.Core.App.Configure();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private async Task SendRegistrationTokenToNotificationHub(string token)
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

        public async override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            IDictionary<string, string> data = PushHelper.GetPushNotificationDictionary(userInfo);

            LogHelper.LogPushEvent("ReceivedRemoteNotification", data, _settingsService.UserName);

            try
            {
                await ProcessNotification(userInfo, false);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        //Notification tapping when the app is in the foreground mode.
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action
                    completionHandler)
        {
            var userInfo = response?.Notification?.Request?.Content?.UserInfo;

            if (response.IsDismissAction)
            {
                //TODO
            }
            else
            {
                if (userInfo != null)
                {
                    PushNotificationData pushNotification = PushHelper.GetPushNotificationData(userInfo);

                    if (pushNotification != null)
                    {
                        if (pushNotification.IsNewMessage)
                        {
                            GlobalSetting.Instance.NewMessagesNotification = pushNotification;
                        }
                    }
                }
            }

            completionHandler();
        }

        public async override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            if (userInfo != null)
            {
                await ProcessNotification(userInfo, false);
            }
        }

        public async Task ProcessNotification(NSDictionary userInfo, bool isFromClick)
        {
            try
            {
                bool isForground = UIApplication.SharedApplication?.ApplicationState == UIApplicationState.Active;

                PushNotificationData pushNotification = PushHelper.GetPushNotificationData(userInfo);

                if (pushNotification != null)
                {
                    if (isForground)
                    {
                        IDialogService dialogService = AppDependencyResolver.Resolve<IDialogService>();

                        await dialogService.ShowDialog(pushNotification.Body, pushNotification.Title, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void SetStatusBar()
        {
            try
            {
                var statusBarColor = Color.FromHex("#011237").ToUIColor();

                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                    statusBar.BackgroundColor = statusBarColor;
                    UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                }
                else
                {
                    UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                    if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                    {
                        statusBar.BackgroundColor = statusBarColor;
                        statusBar.TintColor = UIColor.White;
                        UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            SetStatusBar();
        }

        private void LogException(Exception ex)
        {
            LogHelper.LogException("ApplicationDelegate-Exception occured", ex);
        }
    }
}
