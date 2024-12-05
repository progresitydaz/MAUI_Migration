using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class HomeDataResponse: BaseResponse
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("images")]
        public List<HomeItem> Images { get; set; }

        [JsonProperty("items")]
        public List<HomeItem> HomeItems { get; set; }

        [JsonProperty("registrations")]
        public List<RegisteredUser> Registrations { get; set; }
    }
}
