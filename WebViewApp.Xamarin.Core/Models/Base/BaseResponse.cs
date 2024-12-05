using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class BaseResponse
    {
        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        [JsonIgnore]
        public bool IsSessionExpired { get; set; }

        [JsonIgnore]
        public bool IsSaveSreenError { get; set; }

        [JsonProperty("error")]
        public CommonErrorResponse Error { get; set; }

        public bool IsTaskCanceled { get; set; }

        public bool IsUrlError { get; set; }

        public BaseResponse()
        {
        }
    }
}
