using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class GetConfigurationResponse : BaseResponse
    {
        [JsonProperty("appConfig")]
        public AppConfiguration AppConfig { get; set; }

        public GetConfigurationResponse()
        {
        }
    }
}
