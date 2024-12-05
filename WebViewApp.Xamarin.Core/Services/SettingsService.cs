using System;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface ISettingsService
    {
        string AuthRefreshToken { get; set; }
        string PushNotificationToken { get; set; }
        string NewPushNotificationToken { get; set; }
        long SelectedPollingStationId { get; set; }
        string UserName { get; set; }
        string DevicePlatform { get; }
        string AppVersionNumber { get; }
        string AuthAccessToken { get; set; }
        bool ReloadAppConfig { get; set; }
        bool ClearChatMessasges { get; set; }
        int SelectedMenuItemId { get; set; }

        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
    }

    public class SettingsService : BaseService, ISettingsService
    {
        #region Setting Constants

        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string PushNotificationTokenKey = "push_token";
        private const string NewPushNotificationTokenKey = "new_push_token";
        private const string SelectedPollingStationIdKey = "selected_polling_station_id";
        private const string UserNameKey = "user_name";
        private const string ReloadAppConfigKey = "reload_app_config";
        private const string ClearChatMessasgesKey = "clear_chat_messasges";
        private const string SelectedMenuItemIdKey = "selected_menuItem";

        #endregion

        #region Public Properties

        public string AuthRefreshToken
        {
            get => GetValueOrDefault(RefreshTokenKey, string.Empty);
            set => AddOrUpdateValue(RefreshTokenKey, value);
        }

        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessTokenKey, string.Empty);
            set => AddOrUpdateValue(AccessTokenKey, value);
        }

        public string PushNotificationToken
        {
            get => GetValueOrDefault(PushNotificationTokenKey, string.Empty);
            set => AddOrUpdateValue(PushNotificationTokenKey, value);
        }

        public string NewPushNotificationToken
        {
            get => GetValueOrDefault(NewPushNotificationTokenKey, string.Empty);
            set => AddOrUpdateValue(NewPushNotificationTokenKey, value);
        }

        public long SelectedPollingStationId
        {
            get => Convert.ToInt64(GetValueOrDefault(SelectedPollingStationIdKey, "0"));
            set => AddOrUpdateValue(SelectedPollingStationIdKey, value.ToString());
        }

        public string UserName
        {
            get => GetValueOrDefault(UserNameKey, string.Empty);
            set => AddOrUpdateValue(UserNameKey, value);
        }

        public int SelectedMenuItemId
        {
            get => Convert.ToInt32(GetValueOrDefault(SelectedMenuItemIdKey, "0"));
            set => AddOrUpdateValue(SelectedMenuItemIdKey, value.ToString());
        }

        public string DevicePlatform
        {
            get
            {
                string devicePlatform = Device.RuntimePlatform == Device.Android ? "android" : "ios";
                return devicePlatform;
            }
        }

        public string AppVersionNumber
        {
            get
            {
                string appVersionNumber = DependencyService.Get<IVersionManager>().VersionNumber();
                return appVersionNumber;
            }
        }

        public bool ReloadAppConfig
        {
            get => GetValueOrDefault(ReloadAppConfigKey, false);
            set => AddOrUpdateValue(ReloadAppConfigKey, value);
        }

        public bool ClearChatMessasges
        {
            get => GetValueOrDefault(ClearChatMessasgesKey, false);
            set => AddOrUpdateValue(ClearChatMessasgesKey, value);
        }

        #endregion

        #region Public Methods

        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;

            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Unable to save: " + key, ex);
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Unable to remove: " + key, ex);
            }
        }

        #endregion
    }
}

