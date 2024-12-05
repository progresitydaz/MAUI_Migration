using System;
using System.Globalization;

namespace WebViewApp.Xamarin.Core.Extensions
{
    public static class NumberExtension
    {
        public static string FormatInvariant(this double value)
        {
            string formattedValue = string.Empty;

            formattedValue = value.ToString(CultureInfo.InvariantCulture);

            return formattedValue;
        }

        public static string ToPrice(this double val)
        {
            string formattedText = string.Empty;

            var culture = CultureInfo.CurrentCulture;

            formattedText = val.ToString(culture);

            return $"{formattedText} LKR";
        }

        public static string ToNumber(this double val)
        {
            string formattedText = string.Empty;

            var culture = CultureInfo.InvariantCulture;

            formattedText = val.ToString(culture);

            return formattedText;
        }
    }
}
