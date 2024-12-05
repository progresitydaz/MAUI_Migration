using System;
using Newtonsoft.Json;

namespace WebViewApp.Xamarin.Core.Models
{
    public class CheckListItem
    {
        public string Id { get; set; }

        public string ItemType { get; set; }

        public string ItemDescription { get; set; }

        public bool IsChecked { get; set; }

        [JsonIgnore]
        public string ItemValue { get; set; }

        [JsonIgnore]
        public string Image
        {
            get
            {
                return IsChecked ? "checked.png" : "unchecked.png";
            }
        }

        public CheckListItem()
        {

        }
    }
}
