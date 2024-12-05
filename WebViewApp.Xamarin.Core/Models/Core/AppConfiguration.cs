using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;

namespace WebViewApp.Xamarin.Core.Models
{
    public class AppConfiguration
    {
        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("enableReset")]
        public bool EnableReset { get; set; }

        [JsonProperty("enableChat")]
        public bool EnableChat { get; set; }

        [JsonProperty("enableChatAttachments")]
        public bool EnableChatAttachments { get; set; }

        public AppConfiguration()
        {

        }
    }
}
