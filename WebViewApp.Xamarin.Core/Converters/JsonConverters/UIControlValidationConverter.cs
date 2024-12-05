using System;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;

namespace WebViewApp.Xamarin.Core.Converters
{
    public class UIControlValidationConverter : JsonConverter
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
                case "TEXT":
                    return UIValidationValueTypes.TEXT;
                case "INTEGER":
                    return UIValidationValueTypes.INTEGER;
                case "INTEGER_POS":
                    return UIValidationValueTypes.INTEGER_POS;
                case "INTEGER_NEG":
                    return UIValidationValueTypes.INTEGER_NEG;
                case "DOUBLE":
                    return UIValidationValueTypes.DOUBLE;
                case "DOUBLE_POS":
                    return UIValidationValueTypes.DOUBLE_POS;
                case "DOUBLE_NEG":
                    return UIValidationValueTypes.DOUBLE_NEG;
                case "DATE":
                    return UIValidationValueTypes.DATE;
                case "TIME":
                    return UIValidationValueTypes.TIME;
                case "TIMESTAMP":
                    return UIValidationValueTypes.TIMESTAMP;
                case "PHONENUMBER":
                    return UIValidationValueTypes.PHONENUMBER;
                case "EMAILADDRESS":
                    return UIValidationValueTypes.EMAILADDRESS;
                case "URL":
                    return UIValidationValueTypes.URL;
                default:
                    return UIValidationValueTypes.NONE;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UIValidationValueTypes controlType = (UIValidationValueTypes)value;

            switch (controlType)
            {
                case UIValidationValueTypes.TEXT:
                    writer.WriteValue("TEXT");
                    break;
                case UIValidationValueTypes.INTEGER:
                    writer.WriteValue("INTEGER");
                    break;
                case UIValidationValueTypes.INTEGER_POS:
                    writer.WriteValue("INTEGER_POS");
                    break;
                case UIValidationValueTypes.INTEGER_NEG:
                    writer.WriteValue("INTEGER_NEG");
                    break;
                case UIValidationValueTypes.DOUBLE:
                    writer.WriteValue("DOUBLE");
                    break;
                case UIValidationValueTypes.DOUBLE_POS:
                    writer.WriteValue("CHECKBOX_INPUT");
                    break;
                case UIValidationValueTypes.DOUBLE_NEG:
                    writer.WriteValue("DOUBLE_NEG");
                    break;
                case UIValidationValueTypes.DATE:
                    writer.WriteValue("DATE");
                    break;
                case UIValidationValueTypes.TIME:
                    writer.WriteValue("TIME");
                    break;
                case UIValidationValueTypes.TIMESTAMP:
                    writer.WriteValue("TIMESTAMP");
                    break;
                case UIValidationValueTypes.PHONENUMBER:
                    writer.WriteValue("PHONENUMBER");
                    break;
                case UIValidationValueTypes.EMAILADDRESS:
                    writer.WriteValue("EMAILADDRESS");
                    break;
                case UIValidationValueTypes.URL:
                    writer.WriteValue("URL");
                    break;
                case UIValidationValueTypes.NONE:
                    writer.WriteValue("NONE");
                    break;
            }

        }
    }
}
