using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using StembureauApp.Xamarin.Core.Services;
using StembureauApp.Xamarin.iOS.Dependency;
using Xamarin.Forms;

[assembly: Dependency(typeof(AnalyticsService))]
namespace StembureauApp.Xamarin.iOS.Dependency
{
    public class AnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventId)
        {
            LogEvent(eventId, (IDictionary<string, string>)null);
        }

        public void LogEvent(string eventId, string paramName, string value)
        {
            LogEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void LogEvent(string eventId, IDictionary<string, string> parameters)
        {
            try
            {
                if (parameters == null)
                {
                    LogEvent(eventId, null);
                    return;
                }

                var keys = new List<NSString>();
                var values = new List<NSString>();
                foreach (var item in parameters)
                {
                    keys.Add(new NSString(item.Key));
                    values.Add(new NSString(item.Value));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
