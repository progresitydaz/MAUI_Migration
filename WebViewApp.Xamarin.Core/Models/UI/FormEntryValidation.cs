using WebViewApp.Xamarin.Core.Constants;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class FormEntryValidation
    {
        [JsonProperty("inputTextDataType")]
        public UIValidationValueTypes InputTextDataType { get; set; }

        [JsonProperty("precision")]
        public int Precision { get; set; }

        [JsonProperty("minValue")]
        public long MinValue { get; set; }

        [JsonProperty("maxValue")]
        public long MaxValue { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("maxLength")]
        public long MaxLength { get; set; }

    }
}
