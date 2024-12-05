using System;
using System.Globalization;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Converters
{
    public class UnixDatetimeConverter : JsonConverter
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override bool CanConvert(Type objectType) => objectType == typeof(long) || objectType == typeof(long?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime returnVal = DateTime.MinValue;

            if (reader.TokenType != JsonToken.Null)
            {
                var value = serializer.Deserialize<long>(reader);

                if (value > 0)
                {
                    returnVal = _epoch.AddMilliseconds(value).ToLocalTime();
                }
            }

            return returnVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var dateTime = ((DateTime)value - epoch);

            serializer.Serialize(writer, dateTime.TotalMilliseconds + "000");

            return;
        }

    }

}