using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class ShoppingDataResponse : BaseResponse
    {
        [JsonProperty("categories")]
        public List<ShopCategory> Categories { get; set; }
    }
}
