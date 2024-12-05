using System;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Localization;
using WebViewApp.Xamarin.Core.Services;
using static Xamarin.Essentials.Permissions;
using Xamarin.Essentials;

namespace WebViewApp.Xamarin.Core.Helpers
{
    public static class PermissionHelper
    {
        public async static Task<PermissionStatus> RequestPermission<T>() where T : BasePlatformPermission, new()
        {
            PermissionStatus status = PermissionStatus.Unknown;

            try
            {
                status = await Permissions.CheckStatusAsync<T>();

                if (status == PermissionStatus.Granted)
                    return status;

                if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // Prompt the user to turn on in settings
                    // On iOS once a permission has been denied it may not be requested again from the application
                    return status;
                }

                if (Permissions.ShouldShowRationale<T>())
                {
                    // Prompt the user with additional information as to why the permission is needed
                    IDialogService dialogService = AppDependencyResolver.Resolve<IDialogService>();
                    await dialogService.ShowDialog(
                        AppResources.PermissionRequired,
                        AppResources.AppName,
                        AppResources.OK);
                }

                status = await Permissions.RequestAsync<T>();

            }
            catch (Exception ex)
            {
                LogHelper.LogException("Exception occured while requestiong the permissions", ex);
            }

            return status;
        }
    }
}
