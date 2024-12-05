using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class ShopCategory
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<ShopItem> Items { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}