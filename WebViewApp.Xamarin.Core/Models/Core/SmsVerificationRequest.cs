using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class SmsVerificationRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Token1 { get; set; }

        public string Token2 { get; set; }

        public string DeviceToken { get; set; }

        public string DevicePlatform { get; set; }
    }
}
