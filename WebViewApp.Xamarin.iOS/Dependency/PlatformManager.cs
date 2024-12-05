using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.iOS.Dependency;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using UserNotifications;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.iOS.Helpers;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Extensions;
using WebViewApp.Xamarin.Core.Constants;
using Xamarin.Forms.Internals;
using System.IO;
using WebViewApp.Xamarin.iOS.Delegates;

[assembly: Dependency(typeof(PlatformManager))]
namespace WebViewApp.Xamarin.iOS.Dependency
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
                var netInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var netInterface in netInterfaces)
                {
                    if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddress = addrInfo.Address.ToString();

                                if (!string.IsNullOrEmpty(ipAddress))
                                    break;
                            }
                        }
                    }
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
            UNNotificationSettings settings = null;

            bool enabled = false;

            try
            {
                settings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync();

                enabled = (settings?.AlertSetting == UNNotificationSetting.Enabled);

                if (!enabled & isForceSettings)
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("CheckPushEnabled - Error", ex);

                await ShowToast(ex.Message);
            }

            return enabled;
        }

        protected async Task ShowToast(string message)
        {
            var dialogService = AppDependencyResolver.Resolve<IDialogService>();
            await dialogService.ShowToast(message);
        }

        public async Task UpdateBadgeCountAndNotificationCenter(int badgeCount)
        {
            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            await DeletePendingNotifications();

            bool isPushEnabled = await CheckPushEnabled(false);

            if (UIApplication.SharedApplication != null)
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeCount;
            }

            if (isPushEnabled)
            {
                if (badgeCount > 0)
                {
                    string titleString = renderService.Translate("AppName");
                    string messageString = renderService.Translate("YouHaveUnreadMessages");

                    string body = string.Format(messageString, badgeCount);

                    NSMutableDictionary userDic = new NSMutableDictionary();
                    userDic.SetValueForKey(NSObject.FromObject(PushNotificationKeys.CHAT_NOTIFICATION), new NSString("messageId"));
                    userDic.SetValueForKey(NSObject.FromObject(PushNotificationStatuses.MESSAGE_NEW), new NSString("status"));
                    userDic.SetValueForKey(NSObject.FromObject(titleString), new NSString("title"));
                    userDic.SetValueForKey(NSObject.FromObject(body), new NSString("body"));
                    userDic.SetValueForKey(NSObject.FromObject(true), new NSString("isLocal"));

                    PushHelper.SendNotification(body, titleString, badgeCount, userDic);
                }
            }
        }

        private async Task DeletePendingNotifications()
        {
            try
            {
                var pendingItems = await UNUserNotificationCenter.Current.GetDeliveredNotificationsAsync();

                if (pendingItems != null)
                {
                    foreach (var pendingItem in pendingItems)
                    {
                        string[] ids = new string[] { pendingItem.Request.Identifier };

                        UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("DeletePendingNotifications-Exception occured", ex);
            }
        }

        public bool CheckPackageInstalled(string packageName)
        {
            bool installed = false;

            try
            {
                NSUrl url = NSUrl.FromString(packageName);
                installed = UIApplication.SharedApplication.CanOpenUrl(url);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("CheckPackageInstalled-Exception occured", ex);
            }

            return installed;
        }

        public async Task SaveFile(string base64, string mimeType)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64);

                var decoded = System.Text.Encoding.UTF8.GetString(bytes);

                string extension = mimeType.GetExtension();

                string fileName = $"{new Guid().ToString()}{extension}";

                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                var filePath = Path.Combine(documentsPath, fileName);

                File.WriteAllBytes(filePath, bytes);

                var previewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(filePath));

                previewController.Delegate = new UIDocumentInteractionControllerDelegateHandler(UIApplication.SharedApplication.KeyWindow.RootViewController);

                Device.BeginInvokeOnMainThread(() =>
                {
                    previewController.PresentPreview(true);
                });
            }
            catch (Exception ex)
            {
                var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

                string message = renderService.Translate("InstallDocViewerMessage");

                await ShowToast(message);

                LogHelper.LogException("SaveFile-Exception occured", ex);
            }

            await Task.FromResult(true);
        }

        public int GetSafeAreaInset()
        {
            int top = 0;
            try
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets; // Can't use KeyWindow this early

                    top = (int)insets.Top;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("GetSafeAreaInset-Exception occured", ex);
            }

            return top;
        }
    }
}
