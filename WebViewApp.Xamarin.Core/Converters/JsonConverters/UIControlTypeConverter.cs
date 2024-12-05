using System;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;

namespace WebViewApp.Xamarin.Core.Converters
{
    public class UIControlTypeConverter : JsonConverter
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
                case "TEXT_INPUT":
                    return UIControlTypes.TEXT_INPUT;
                case "CHECKBOX_INPUT":
                    return UIControlTypes.CHECKBOX_INPUT;
                case "SUBMIT_BUTTON":
                    return UIControlTypes.SUBMIT_BUTTON;
                case "NEXT_BUTTON":
                    return UIControlTypes.NEXT_BUTTON;
                case "PREVIOUS_BUTTON":
                    return UIControlTypes.PREVIOUS_BUTTON;
                case "LOGOUT_BUTTON":
                    return UIControlTypes.LOGOUT_BUTTON;
                case "TEXT_LABEL":
                    return UIControlTypes.TEXT_LABEL;
                case "HIGHLIGHTED_LARGE_LABEL":
                    return UIControlTypes.HIGHLIGHTED_LARGE_LABEL;
                case "HIGHLIGHTED_SMALL_LABEL":
                    return UIControlTypes.HIGHLIGHTED_SMALL_LABEL;
                case "IMAGE":
                    return UIControlTypes.IMAGE;
                case "FORM_TEXT_INPUT":
                    return UIControlTypes.FORM_TEXT_INPUT;
                case "NAME_VALUE":
                    return UIControlTypes.NAME_VALUE;
                case "DATE_INPUT":
                    return UIControlTypes.DATE_INPUT;
                case "RADIO_INPUT":
                    return UIControlTypes.RADIO_INPUT;
                default:
                    return UIControlTypes.NONE;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UIControlTypes controlType = (UIControlTypes)value;

            switch (controlType)
            {
                case UIControlTypes.CHECKBOX_INPUT:
                    writer.WriteValue("CHECKBOX_INPUT");
                    break;
                case UIControlTypes.TEXT_INPUT:
                    writer.WriteValue("TEXT_INPUT");
                    break;
                case UIControlTypes.SUBMIT_BUTTON:
                    writer.WriteValue("SUBMIT_BUTTON");
                    break;
                case UIControlTypes.NEXT_BUTTON:
                    writer.WriteValue("NEXT_BUTTON");
                    break;
                case UIControlTypes.PREVIOUS_BUTTON:
                    writer.WriteValue("PREVIOUS_BUTTON");
                    break;
                case UIControlTypes.LOGOUT_BUTTON:
                    writer.WriteValue("LOGOUT_BUTTON");
                    break;
                case UIControlTypes.TEXT_LABEL:
                    writer.WriteValue("TEXT_LABEL");
                    break;
                case UIControlTypes.HIGHLIGHTED_LARGE_LABEL:
                    writer.WriteValue("HIGHLIGHTED_LARGE_LABEL");
                    break;
                case UIControlTypes.HIGHLIGHTED_SMALL_LABEL:
                    writer.WriteValue("HIGHLIGHTED_SMALL_LABEL");
                    break;
                case UIControlTypes.IMAGE:
                    writer.WriteValue("IMAGE");
                    break;
                case UIControlTypes.FORM_TEXT_INPUT:
                    writer.WriteValue("FORM_TEXT_INPUT");
                    break;
                case UIControlTypes.NAME_VALUE:
                    writer.WriteValue("NAME_VALUE");
                    break;
                case UIControlTypes.DATE_INPUT:
                    writer.WriteValue("NAME_VALUE");
                    break;
                case UIControlTypes.RADIO_INPUT:
                    writer.WriteValue("RADIO_INPUT");
                    break;
                case UIControlTypes.NONE:
                    writer.WriteValue("NONE");
                    break;

            }
        }
    }
}
