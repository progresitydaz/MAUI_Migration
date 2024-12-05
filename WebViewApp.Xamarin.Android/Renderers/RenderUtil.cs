using System;
using Android.Content;
using Android.Util;
using Android.Views.InputMethods;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;

namespace WebViewApp.Xamarin.Droid.Renderers
{
    public static class RenderUtil
    {
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }

        public static void AddDoneAndNextButton(FormsEditText control, RoundedEntry entry)
        {
            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            string nextString = renderService.Translate("Next");
            string doneString = renderService.Translate("Done");

            if (entry.NextTextField != null)
            {
                control.ImeOptions = ImeAction.Next;
                control.SetImeActionLabel(nextString, ImeAction.Next);
            }
            else
            {
                control.ImeOptions = ImeAction.Done;
                control.SetImeActionLabel(doneString, ImeAction.Done);
            }
        }

        public static void AddDoneButton(FormsEditText control)
        {
            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            string doneString = renderService.Translate("Done");

            control.ImeOptions = ImeAction.Done;
            control.SetImeActionLabel(doneString, ImeAction.Done);
        }
    }
}
