using System;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;

namespace WebViewApp.Xamarin.Core.Converters
{
    public class UIControlValidationTypeConverter : JsonConverter
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
                case "ALLWAYS_ON":
                    return UIControlValidationType.ALLWAYS_ON;
                case "AllREQUIRED":
                    return UIControlValidationType.AllREQUIRED;
                case "ANYOPTIONAL":
                    return UIControlValidationType.ANYOPTIONAL;
                case "AllREQUIRED_OR_ANYOPTIONAL":
                    return UIControlValidationType.AllREQUIRED_OR_ANYOPTIONAL;
                case "AllREQUIRED_AND_ANYOPTIONAL":
                    return UIControlValidationType.AllREQUIRED_AND_ANYOPTIONAL;
                default:
                    return UIControlValidationType.NONE;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UIControlValidationType ControlValidationType = (UIControlValidationType)value;

            switch (ControlValidationType)
            {
                case UIControlValidationType.ALLWAYS_ON:
                    writer.WriteValue("ALLWAYS_ON");
                    break;
                case UIControlValidationType.AllREQUIRED:
                    writer.WriteValue("AllREQUIRED");
                    break;
                case UIControlValidationType.ANYOPTIONAL:
                    writer.WriteValue("ANYOPTIONAL");
                    break;
                case UIControlValidationType.AllREQUIRED_OR_ANYOPTIONAL:
                    writer.WriteValue("AllREQUIRED_OR_ANYOPTIONAL");
                    break;
                case UIControlValidationType.AllREQUIRED_AND_ANYOPTIONAL:
                    writer.WriteValue("AllREQUIRED_AND_ANYOPTIONAL");
                    break;
                case UIControlValidationType.NONE:
                    writer.WriteValue("NA");
                    break;
            }
        }
    }
}
