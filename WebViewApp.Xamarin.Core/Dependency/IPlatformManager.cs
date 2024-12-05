using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Models;

namespace WebViewApp.Xamarin.Core.Dependency
{
    public interface IPlatformManager
    {
        string GetIPAddress();

        Task<bool> CheckPushEnabled(bool isForceSettings);

        Task UpdateBadgeCountAndNotificationCenter(int badgeCount);

        Task SaveFile(string base64, string mimeType);

        int GetSafeAreaInset();
    }
}
