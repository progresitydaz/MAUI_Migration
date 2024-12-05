using System;
using System.Globalization;

namespace WebViewApp.Xamarin.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static string FormatDate(this DateTimeOffset? dateTime)
        {
            string format = "dd-MM-yyyy";

            CultureInfo culture = GlobalSetting.Instance.CurrentCulture;

            string formattedDate = string.Empty;

            if (dateTime.HasValue)
            {
                formattedDate = dateTime.Value.ToString(format, culture);
            }
            else
            {
                formattedDate = "N.A";
            }

            return formattedDate;
        }

        public static string FormatDateTime(this DateTimeOffset? dateTime)
        {
            string format = "dd-MM-yyyy HH:mm:ss";

            CultureInfo culture = GlobalSetting.Instance.CurrentCulture;

            string formattedDate = string.Empty;

            if (dateTime.HasValue)
            {
                formattedDate = dateTime.Value.ToString(format, culture);
            }
            else
            {
                formattedDate = "N.A";
            }

            return formattedDate;
        }

        public static DateTimeOffset DateTimeOffset(this DateTime dateTime)
        {
            DateTimeOffset formattedDate = DateTime.Now;

            formattedDate = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return formattedDate;
        }

        public static long ToUnixTimeMilliseconds(this DateTimeOffset? dateTime)
        {
            long timeVal;

            if (dateTime.HasValue)
            {
                timeVal = dateTime.Value.ToUnixTimeMilliseconds();
            }
            else
            {
                timeVal = 0;
            }

            return timeVal;
        }

    }
}
