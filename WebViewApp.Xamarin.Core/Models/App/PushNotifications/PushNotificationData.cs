using System;
using WebViewApp.Xamarin.Core.Constants;

namespace WebViewApp.Xamarin.Core.Models
{
    public class PushNotificationData
    {
        public string MessageId { get; set; }

        public bool IsProcessed { get; set; }

        public bool IsProcessing { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Color { get; set; }

        public string[] ChatMessageIds { get; set; }

        public bool IsNewMessage
        {
            get
            {
                bool isNewMessage = MessageId == PushNotificationKeys.CHAT_NOTIFICATION &&
                    Status == PushNotificationStatuses.MESSAGE_NEW;

                bool isNewMessageNotif = Title == "Nieuw bericht ontvangen";

                return isNewMessage || isNewMessageNotif;
            }
        }
    }
}
