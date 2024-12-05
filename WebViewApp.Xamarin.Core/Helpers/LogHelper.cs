using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.AppCenter.Crashes;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;

namespace WebViewApp.Xamarin.Core.Helpers
{
    public static class LogHelper
    {
        public static void LogException(string messege, Exception ex)
        {
            try
            {
                Crashes.TrackError(ex);

                Debug.WriteLine($"{0} exception {1}", messege, ex);
            }
            catch (Exception exInner)
            {
                Debug.WriteLine($"Exception occured while logging the exception {0}", exInner);
            }
        }

        public static void LogHandledException(string category, string message)
        {
            ISettingsService settingsService = AppDependencyResolver.Resolve<ISettingsService>();
            IAnalyticsService analyticsService = AppDependencyResolver.Resolve<IAnalyticsService>();

            string devicePlatform = settingsService.DevicePlatform;
            string appVersionNumber = settingsService.AppVersionNumber;
            string userName = settingsService.UserName;

            var loggedData = new Dictionary<string, string>
            {
                { "username", userName },
                { "appVersion", appVersionNumber },
                { "devicePlatform", devicePlatform },
                { "category", category },
                { "exception", message },
            };

            analyticsService.TrackEvent(EventNames.RESTAPIError, loggedData);

        }

        public static void LogPushEvent(string eventName, IDictionary<string, string> data, string userName)
        {
            try
            {
                data.Add("userName", userName);
                data.Add("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                IAnalyticsService analyticsService = AppDependencyResolver.Resolve<IAnalyticsService>();

                string dataString = string.Empty;

                StringBuilder stringBuilder = new StringBuilder();

                if (data != null && data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        stringBuilder.Append($"{item.Key}:{item.Value},");
                    }
                }

                dataString = stringBuilder.ToString();

                Console.WriteLine(dataString);
            }
            catch (Exception exInner)
            {
                LogException("LogPushEvent-Error", exInner);
            }
        }

        public static void LogEvent(string eventName, string data)
        {
            try
            {
                ISettingsService settingsService = AppDependencyResolver.Resolve<ISettingsService>();

                string userName = settingsService.UserName;

                IAnalyticsService analyticsService = AppDependencyResolver.Resolve<IAnalyticsService>();

                var loggedData = new Dictionary<string, string>
                {
                    { "username", userName },
                    { "data", data },
                    { "time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                };

                analyticsService.TrackEvent(eventName, loggedData);
            }
            catch (Exception exInner)
            {
                Debug.WriteLine(exInner);
            }
        }
    }
}
