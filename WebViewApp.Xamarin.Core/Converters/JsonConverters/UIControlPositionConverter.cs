using System;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;

namespace WebViewApp.Xamarin.Core.Converters
{
    public class UIControlPositionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = (string)reader.Value;

            switch (stringValue)
            {
                case "BOTTOM":
                    return UIControlPosition.BOTTOM;
                case "CENTER":
                    return UIControlPosition.CENTER;
                case "TOP":
                    return UIControlPosition.TOP;
                case "LEFT":
                    return UIControlPosition.LEFT;
                case "RIGHT":
                    return UIControlPosition.RIGHT;
                default:
                    return UIControlPosition.NONE;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UIControlPosition controlPosition = (UIControlPosition)value;

            switch (controlPosition)
            {
                case UIControlPosition.BOTTOM:
                    writer.WriteValue("BOTTOM");
                    break;
                case UIControlPosition.CENTER:
                    writer.WriteValue("CENTER");
                    break;
                case UIControlPosition.TOP:
                    writer.WriteValue("TOP");
                    break;
                case UIControlPosition.LEFT:
                    writer.WriteValue("LEFT");
                    break;
                case UIControlPosition.RIGHT:
                    writer.WriteValue("RIGHT");
                    break;
                case UIControlPosition.NONE:
                    writer.WriteValue("NONE");
                    break;
            }
        }
    }
}
