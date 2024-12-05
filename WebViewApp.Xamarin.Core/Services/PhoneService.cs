using System;
using Plugin.Messaging;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IPhoneService
    {
        void MakePhoneCall(string phoneNumber);
    }

    public class PhoneService : BaseService, IPhoneService
    {
        public PhoneService()
        {

        }

        public void MakePhoneCall(string phoneNumber)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;

            if (phoneDialer.CanMakePhoneCall)
            {
                phoneDialer.MakePhoneCall(phoneNumber);
            }
        }
    }
}
