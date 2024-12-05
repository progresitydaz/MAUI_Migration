using System;
using System.Drawing;
using CoreGraphics;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    var view = (RoundedEntry)Element;

                    Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                    Control.LeftViewMode = UITextFieldViewMode.Always;

                    Control.ReturnKeyType = UIReturnKeyType.Done;

                    Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                    Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                    Control.Layer.BorderWidth = view.BorderWidth;
                    Control.ClipsToBounds = true;
                    Control.ReturnKeyType = UIReturnKeyType.Next;

                    if (this.Element.Keyboard == Keyboard.Numeric)
                    {
                        RenderUtil.AddDoneAndNextButton(Control, view);
                    }
                }
            }
        } 
    }
}