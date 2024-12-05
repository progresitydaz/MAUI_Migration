using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using WebViewApp.Xamarin.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(ExtendedDatePickerRenderer))]
namespace WebViewApp.Xamarin.Droid.Renderers
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (DatePicker)Element;

                // creating gradient drawable for the curved background
                var gradientBackground = new GradientDrawable();
                gradientBackground.SetShape(ShapeType.Rectangle);
                gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                // Thickness of the stroke line
                gradientBackground.SetStroke(1, Color.FromHex("#8f9fad").ToAndroid());

                // Radius for the curves
                gradientBackground.SetCornerRadius(
                         RenderUtil.DpToPixels(this.Context,
                             Convert.ToSingle(0)));

                // set the background of the label
                Control.SetBackground(gradientBackground);
                // Set padding for the internal text from border
                Control.SetPadding(
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(10)),
                    15,
                    (int)RenderUtil.DpToPixels(this.Context, Convert.ToSingle(10)),
                    15);
            }
        }
    }
}
