
using System;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Plugin.Permissions;
using WebViewApp.Xamarin.Core;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Droid.Helpers;
using XE = Xamarin.Essentials;

namespace WebViewApp.Xamarin.Droid.Activities
{
    [Activity(Exported = true, Label = "@string/app_name",
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/LaunchTheme",
        MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation |
        ConfigChanges.ScreenSize | ConfigChanges.KeyboardHidden,
        ScreenOrientation = ScreenOrientation.User)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public int RequestCameraAndStorageId { get; set; } = 500;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            XE.Platform.Init(this, savedInstanceState);

            UserDialogs.Init(this);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            var context = Android.App.Application.Context;

            PushNotificationData pushNotification = PushHelper.GetPushNotificationData(Intent.Extras);

            if (pushNotification != null)
            {
                if (pushNotification.IsNewMessage)
                {
                    GlobalSetting.Instance.NewMessagesNotification = pushNotification;
                }
            }

            App stemburaeauApp = new App();

            LoadApplication(stemburaeauApp);

            RequestPermissions();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterReceiver();
        }

        private void UnRegisterReceiver()
        {
            try
            {

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UnRegister Receiver" + ex.Message);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            XE.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
        }
        private void LogException(Exception ex)
        {
            LogHelper.LogException("Exception occured", ex);
        }

        void RequestPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Manifest.Permission.Camera,
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage
                }, RequestCameraAndStorageId);
            }
        }
    }
}