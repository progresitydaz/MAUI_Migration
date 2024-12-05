using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using WebViewApp.Xamarin.Core;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Droid.Helpers;

namespace WebViewApp.Xamarin.Droid.Activities
{
    [Activity(Icon = "@mipmap/ic_launcher",
        Label = "@string/app_name",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            PushNotificationData pushNotification = PushHelper.GetPushNotificationData(Intent.Extras);

            if (pushNotification != null)
            {
                if (pushNotification.IsNewMessage)
                {
                    GlobalSetting.Instance.NewMessagesNotification = pushNotification;
                }
            }

            CallMainActivity();
        }

        private void CallMainActivity()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}
