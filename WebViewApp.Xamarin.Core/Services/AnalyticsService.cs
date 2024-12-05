using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AppCenter.Analytics;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IAnalyticsService
    {
        void TrackEvent(string eventName, IDictionary<string, string> properties = null);
    }

    public class AnalyticsService : IAnalyticsService
    {
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            try
            {
                Analytics.TrackEvent(eventName, properties);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured while logging the analytics {0}", ex);
            }
        }
    }
}
