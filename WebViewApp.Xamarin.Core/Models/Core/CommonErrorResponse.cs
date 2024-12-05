using System;
using WebViewApp.Xamarin.Core.Localization;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class CommonErrorResponse
    {
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }

        [JsonProperty("result_type")]
        public string ResultType { get; set; }

        [JsonProperty("result_title")]
        public string ResultTitle { get; set; }

        [JsonProperty("result_msg_body")]
        public string ResultMsgBody { get; set; }

        [JsonProperty("detailed_msg")]
        public string DetailedMsg { get; set; }

        public static CommonErrorResponse CreateErrorResponseFor(Exception exception)
        {
            CommonErrorResponse response = new CommonErrorResponse()
            {
                ResultCode = "1",
                ResultTitle = AppResources.AppName,
                ResultType = "1",
                ResultMsgBody = exception.Message,
                DetailedMsg = exception.StackTrace
            };

            return response;
        }
    }
}
