using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Droid.Dependency;
using WebViewApp.Xamarin.Droid.Helpers;
using Xamarin.Forms;
using WebViewApp.Xamarin.Core.Extensions;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using System.IO;
using Android.Widget;
using Android.OS;
using Android.Provider;
using AndroidX.Lifecycle;
using static Android.Provider.MediaStore;

[assembly: Dependency(typeof(PlatformManager))]
namespace WebViewApp.Xamarin.Droid.Dependency
{
    public class PlatformManager : IPlatformManager
    {
        public PlatformManager()
        {

        }

        public string GetIPAddress()
        {
            string ipAddress = string.Empty;

            try
            {
                IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

                if (adresses != null && adresses[0] != null)
                {
                    ipAddress = adresses[0].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("GetIPAddress - Error", ex);
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress;

        }

        public async Task<bool> CheckPushEnabled(bool isForceSettings)
        {
            bool enabled = false;

            try
            {
                var nm = NotificationManagerCompat.From(Android.App.Application.Context);

                enabled = nm.AreNotificationsEnabled();

                if (!enabled && isForceSettings)
                {
                    OpenPushSettings();
                }
            }
            catch (Exception ex)
            {
                await ShowToast(ex.Message);

                LogHelper.LogException("CheckPushEnabled-Exception occured", ex);
            }

            return enabled;
        }

        private void OpenPushSettings()
        {
            var context = Android.App.Application.Context;
            var appInfo = context.PackageManager.GetApplicationInfo(context.PackageName, 0);

            string packageName = context.PackageName;

            var uri = Android.Net.Uri.Parse("package:" + packageName);

            Intent settingIntent = new Intent(Android.Provider.Settings.ActionAppNotificationSettings);

            //for Android 5-7
            settingIntent.PutExtra("app_package", packageName);

            if (appInfo != null)
            {
                settingIntent.PutExtra("app_uid", appInfo.Uid);
            }

            // for Android 8 and above
            settingIntent.PutExtra("android.provider.extra.APP_PACKAGE", packageName);

            settingIntent.AddFlags(ActivityFlags.NewTask);

            context.StartActivity(settingIntent);
        }

        protected async Task ShowToast(string message)
        {
            var dialogService = AppDependencyResolver.Resolve<IDialogService>();
            await dialogService.ShowToast(message);
        }

        public async Task UpdateBadgeCountAndNotificationCenter(int badgeCount)
        {
            try
            {
                var context = Android.App.Application.Context;

                bool isPushEnabled = await CheckPushEnabled(false);

                if (isPushEnabled)
                {
                    var notificationManager = NotificationManager.FromContext(context);

                    var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

                    string title = renderService.Translate("AppName");
                    string message = renderService.Translate("YouHaveUnreadMessages");

                    string formattedMessage = string.Format(message, badgeCount);

                    var pendingItems = notificationManager.GetActiveNotifications();

                    notificationManager.CancelAll();

                    if (badgeCount > 0)
                    {
                        PushHelper.SendNotification(formattedMessage, title, context, badgeCount);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("UpdateBadgeCount-Exception occured", ex);

                await ShowToast(ex.Message);
            }
        }

        public bool CheckPackageInstalled(string packageName)
        {
            bool installed = false;

            try
            {
                var context = Android.App.Application.Context;
                var packageInfo = context.PackageManager.GetPackageInfo(packageName, PackageInfoFlags.Activities);

                installed = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("CheckPackageInstalled-Exception occured", ex);
            }

            return installed;
        }

        public async Task SaveFile(string base64, string mimeType)
        {
            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            var context = Android.App.Application.Context;

            var contentResolver = context.ContentResolver;

            try
            {
                // Remove data URL scheme if present
                base64 = base64.Replace($"data:{mimeType};base64,", string.Empty);

                // Request necessary permissions
                var storageWriteStatus = await PermissionHelper.RequestPermission<Permissions.StorageWrite>();

                if (storageWriteStatus == PermissionStatus.Granted)
                {
                    // Prepare file details
                    var fileBytes = Convert.FromBase64String(base64);

                    string extension = mimeType.GetExtension();

                    string fileName = $"{Guid.NewGuid()}{extension}";

                    var values = new ContentValues();
                    values.Put(IMediaColumns.DisplayName, fileName);
                    values.Put(IMediaColumns.MimeType, mimeType);
                    values.Put(IMediaColumns.RelativePath, Android.OS.Environment.DirectoryDownloads); // Or other public directories

                    string savedPath = $"Downloads/{fileName}";

                    var uri = contentResolver.Insert(MediaStore.Downloads.ExternalContentUri, values);

                    if (uri != null)
                    {
                        using (var outputStream = contentResolver.OpenOutputStream(uri))
                        {
                            outputStream.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }

                    string fileDownloadedMessege = renderService.Translate("FileDownloadedMessege");

                    await ShowToast(string.Format(fileDownloadedMessege, savedPath));
                }
                else
                {
                    string permissionRequiredMessege = renderService.Translate("PermissionRequired");

                    await ShowToast(permissionRequiredMessege);
                }
            }
            catch (Exception ex)
            {
                // Log and handle exception

                await ShowToast(ex.Message);

                LogHelper.LogException("SaveFileAsync-Exception occurred", ex);
            }
        }

        public int GetSafeAreaInset()
        {
            return 0;
        }
    }
}
