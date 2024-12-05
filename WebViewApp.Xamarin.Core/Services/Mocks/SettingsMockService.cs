using System;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Services;

namespace WebViewApp.Xamarin.Core.Services.Mocks
{
    public class SettingsMockService : ISettingsService
    {
        public string PushNotificationToken { get; set; }
        public string NewPushNotificationToken { get; set; }
        public long SelectedPollingStationId { get; set; }
        public string AuthAccessToken { get; set; }
        public bool ReloadAppConfig { get; set; }
        public int SelectedMenuItemId { get; set; }
        public bool ClearChatMessasges { get; set; }
        #region Implemented

        private static string _authRereshTokenInternal { get; set; }
        public string AuthRefreshToken
        {
            get
            {
                return _authRereshTokenInternal;
            }
            set
            {
                _authRereshTokenInternal = value;
            }
        }
         
        private static string _devicePlatformInternal { get; set; }
        public string DevicePlatform
        {
            get
            {
                return _devicePlatformInternal;
            }
        }

        private static string _appVersionNumberInternal { get; set; }
        public string AppVersionNumber
        {
            get
            {
                return _appVersionNumberInternal;
            }
        }

        private static string _userNameInternal { get; set; }
        public string UserName
        {
            get
            {
                return _userNameInternal;
            }
            set
            {
                _userNameInternal = value;
            }
        }

        #endregion

        public static void SetMockValues()
        {
            _appVersionNumberInternal = "1.0.27";
            _userNameInternal = "lex";
            _devicePlatformInternal = "ios";
        }

        public static void SetToken(string tokenString)
        {
            _authRereshTokenInternal = tokenString;
        }

        #region Not Implemented

        public Task AddOrUpdateValue(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool GetValueOrDefault(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetValueOrDefault(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
