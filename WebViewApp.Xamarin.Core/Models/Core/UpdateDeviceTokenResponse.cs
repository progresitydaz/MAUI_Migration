using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class UpdateDeviceTokenResponse : BaseResponse
    {
        [JsonProperty("saved")]
        public bool IsSaved { get; set; }
      
    }
}
