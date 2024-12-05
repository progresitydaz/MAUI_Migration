using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;
using WebViewApp.Xamarin.Core;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Droid.Activities;

namespace WebViewApp.Xamarin.Droid.Helpers
{
    public static class PushHelper
    {
        public static PushNotificationData GetPushNotificationData(IDictionary<string, string> data)
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

                if (data.ContainsKey("messageId"))
                {
                    messageId = data["messageId"] as string;
                }

                if (data.ContainsKey("status"))
                {
                    status = data["status"] as string;
                }

                if (data.ContainsKey("title"))
                {
                    title = data["title"] as string;
                }

                if (data.ContainsKey("body"))
                {
                    body = data["body"] as string;
                }

                if (data.ContainsKey("color"))
                {
                    color = data["color"] as string;
                }

                if (data.ContainsKey("chatMessageIds"))
                {
                    string list = data["chatMessageIds"] as string;
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

        public static PushNotificationData GetPushNotificationData(Bundle extras)
        {
            PushNotificationData pushData = null;

            if (extras != null)
            {
                var dataDictionary = PushHelper.GetDataDictionary(extras);

                pushData = GetPushNotificationData(dataDictionary);
            }

            return pushData;
        }

        public static IDictionary<string, string> GetDataDictionary(Bundle userInfo)
        {
            var d = new Dictionary<string, string>();

            var keyset = userInfo.KeySet();

            foreach (var key in keyset)
            {
                d.Add(key, userInfo.Get(key)?.ToString());
            }

            return d;
        }

        public static void SendNotification(string alertBody, string messageTitle, Context context, int count = 0)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("messageId", PushNotificationKeys.CHAT_NOTIFICATION);
            intent.PutExtra("status", PushNotificationStatuses.MESSAGE_NEW);

            Bundle notificationCenterExtras = new Bundle();
            notificationCenterExtras.PutString("messageId", PushNotificationKeys.CHAT_NOTIFICATION);
            notificationCenterExtras.PutString("status", PushNotificationStatuses.MESSAGE_NEW);

            Random random = new Random();
            int pushCount = random.Next(9999 - 1000) + 1000;

            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, pushCount, intent, PendingIntentFlags.Immutable);

            var notificationBuilder = new NotificationCompat.Builder(context, PushNotificationConstants.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.splash)
                .SetContentTitle(messageTitle)
                .SetContentText(alertBody)
                .SetContentIntent(pendingIntent)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(false)
                .SetExtras(notificationCenterExtras)
                .SetNumber(count);

            var notificationManager = NotificationManager.FromContext(context);

            // Since android Oreo notification channel is needed.

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(PushNotificationConstants.CHANNEL_ID,
                                                  PushNotificationConstants.CHANNEL_NAME,
                                                   NotificationImportance.Default)
                {

                    Description = "Firebase Cloud Messages appear in this channel"
                };

                notificationManager.CreateNotificationChannel(channel);
            }

            var notification = notificationBuilder.Build();

            notificationManager.Notify(pushCount, notification);
        }
    }
}
