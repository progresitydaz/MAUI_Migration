using System;
using System.Collections.Generic;
using Foundation;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using UIKit;

namespace WebViewApp.Xamarin.iOS.Helpers
{
    public static class PushHelper
    {
        public static PushNotificationData GetPushNotificationData(NSDictionary data)
        {
            PushNotificationData pushData = null;

            if (data != null)
            {
                string messageId = string.Empty;
                string status = string.Empty;
                string title = string.Empty;
                string body = string.Empty;
                string color = string.Empty;
                string[] chatMessageIds = null;

                if (data.ContainsKey(new NSString("messageId")))
                {
                    messageId = (data[new NSString("messageId")] as NSString).ToString();
                }

                if (data.ContainsKey(new NSString("status")))
                {
                    status = (data[new NSString("status")] as NSString).ToString();
                }

                if (data.ContainsKey(new NSString("title")))
                {
                    title = (data[new NSString("title")] as NSString).ToString();
                }

                if (data.ContainsKey(new NSString("body")))
                {
                    body = (data[new NSString("body")] as NSString).ToString();
                }

                if (data.ContainsKey(new NSString("color")))
                {
                    color = (data[new NSString("color")] as NSString).ToString();
                }

                if (data.ContainsKey(new NSString("chatMessageIds")))
                {
                    string list = (data[new NSString("chatMessageIds")] as NSString).ToString();
                    chatMessageIds = list.Split(',');
                }

                if (!string.IsNullOrEmpty(messageId) || !string.IsNullOrEmpty(title))
                {
                    pushData = new PushNotificationData()
                    {
                        Body = body,
                        Title = title,
                        MessageId = messageId,
                        Status = status,
                        Color = color,
                        ChatMessageIds = chatMessageIds,
                    };
                }
            }

            return pushData;
        }

        public static void SendNotification(string alertBody, string title, int badgeCount, NSDictionary userInfo)
        {
            // create the notification
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(0);

            // configure the alert
            notification.AlertAction = title;
            notification.AlertBody = alertBody;

            // modify the badge
            if (badgeCount > 0)
            {
                notification.ApplicationIconBadgeNumber = badgeCount;
            }

            notification.UserInfo = userInfo;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }

        public static IDictionary<string, string> GetPushNotificationDictionary(NSDictionary userInfo)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();

            try
            {
                if (userInfo != null)
                {
                    foreach (NSString key in userInfo.Keys)
                    {
                        if (key == "aps")
                        {
                            var apsDict = userInfo.ValueForKey(key) as NSDictionary;

                            if (apsDict != null)
                            {
                                foreach (var apsKey in apsDict)
                                {
                                    dic.Add(apsKey.Key.ToString(), apsKey.Value.ToString());
                                }
                            }
                        }

                        string val = userInfo.ValueForKey(key).ToString();

                        dic.Add(key, val);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("GetPushNotificationDictionary", ex);
            }

            return dic;
        }
    }
}
