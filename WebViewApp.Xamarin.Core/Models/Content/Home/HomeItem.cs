using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class HomeItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("header")]
        public string Header { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("subHeader")]
        public string SubHeader { get; set; }
    }
}