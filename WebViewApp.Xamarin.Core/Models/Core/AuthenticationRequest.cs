using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class AuthenticationRequest
    {
        public string Password { get; set; }

        public string UserName { get; set; }

        public string AppVersion { get; set; }

        public string DeviceToken { get; set; }

        public string DevicePlatform { get; set; }
    }
}
