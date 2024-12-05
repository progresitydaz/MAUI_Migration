using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class AuthenticationResponse : BaseResponse
    {
        [JsonProperty("loginOk")]
        public bool IsLoginOk { get; set; }

        [JsonProperty("sendSMSOk")]
        public bool IsSendSmsOk { get; set; }

        [JsonProperty("enableVoiceMessage")]
        public bool EnableVoiceMessage { get; set; }

        [JsonProperty("httpStatusCode")]
        public long HttpStatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("skipSMSValidation")]
        public bool SkipSMSValidation { get; set; }

        [JsonProperty("hasDeviceToken")]
        public bool HasDeviceToken { get; set; }

        [JsonProperty("hasDevicePlatform")]
        public bool HasDevicePlatform { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }

        public AuthenticationResponse()
        {
        }
    }
}
