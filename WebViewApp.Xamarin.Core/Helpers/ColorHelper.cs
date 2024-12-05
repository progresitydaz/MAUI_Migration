using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Helpers
{
    public static class ColorHelper
    {
        public static Dictionary<string, Color> CachedColors { get; set; } = new Dictionary<string, Color>();

        public static string GetDiplayColor(string diplayColor)
        {
            Color color = Color.FromHex("#4B575F");

            try
            {
                if (CachedColors.ContainsKey(diplayColor))
                {
                    bool isPresent = CachedColors.TryGetValue(diplayColor, out color);
                }
                else
                {
                    bool canParse = int.TryParse(diplayColor, out int colorVal);

                    if (canParse)
                    {
                        int red = (colorVal & 0xFF);
                        int green = ((colorVal >> 8) & 0xFF);
                        int blue = ((colorVal >> 16) & 0xFF);

                        color = System.Drawing.Color.FromArgb(colorVal);

                        CachedColors.Add(diplayColor, color);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Exception occured - GetDiplayColor", ex);
            }

            string hex = color.ToHex();

            return hex;
        }
    }
}
