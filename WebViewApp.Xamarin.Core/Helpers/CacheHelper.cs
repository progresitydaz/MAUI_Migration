using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Models;
using System.Linq;

namespace WebViewApp.Xamarin.Core.Helpers
{
    public class CachedItem
    {
        public object Data { get; set; }

        public TimeSpan Expires { get; set; }

        public string Key { get; set; }

        public DateTime AddedTime { get; set; }

        public CachedItem(object data, string key, TimeSpan expires, DateTime addedTime)
        {
            AddedTime = addedTime;
            Key = key;
            Expires = expires;
            Data = data;
        }
    }

    public static class CacheHelper
    {
        public static List<CachedItem> CachedItems { get; set; } = new List<CachedItem>();

        public static T Get<T>() where T : BaseResponse
        {
            string key = typeof(T).Name;

            T data = null;

            CachedItem cachedItem = CachedItems?.FirstOrDefault(x => x.Key == key);

            if (cachedItem != null)
            {
                DateTime now = DateTime.Now;

                if ((now - cachedItem.AddedTime) < cachedItem.Expires)
                {
                    data = cachedItem.Data as T;
                }
            }

            return data;
        }

        public static void Add<T>(T data, TimeSpan expires) where T : BaseResponse
        {
            if (data != null && data.IsSuccessful)
            {
                string key = typeof(T).Name;

                CachedItem cachedItem = CachedItems?.FirstOrDefault(x => x.Key == key);

                if (cachedItem != null)
                {
                    CachedItems.Remove(cachedItem);
                }

                CachedItem newItem = new CachedItem(data, key, expires, DateTime.Now);

                CachedItems.Add(newItem);
            }
        }

        public static void Reset()
        {
            CachedItems = new List<CachedItem>();
        }
    }
}
