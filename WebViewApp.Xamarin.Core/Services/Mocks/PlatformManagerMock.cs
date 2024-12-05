using System;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Dependency;

namespace WebViewApp.Xamarin.Core.Services.Mocks
{
    public class PlatformManagerMock : IPlatformManager
    {
        public async Task<bool> CheckPushEnabled(bool isForceSettings)
        {
            return await Task.FromResult(true);
        }

        public string GetIPAddress()
        {
            return "127.0.0.1";
        }

        public int GetSafeAreaInset()
        {
            return 0;
        }

        public Task SaveFile(string base64, string mimeType)
        {
            return Task.FromResult("");
        }

        public async Task UpdateBadgeCountAndNotificationCenter(int badgeCount)
        {
            await Task.FromResult(true);
        }
    }
}
