using System;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace WebViewApp.Xamarin.iOS.Renderers
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    var view = (ExtendedEditor)Element;

                    Control.Layer.CornerRadius = Convert.ToSingle(0f);
                    Control.Layer.BorderWidth = Convert.ToSingle(0f);
                    Control.ClipsToBounds = true;

                    if (view.NextTextField != null)
                    {
                        Control.ReturnKeyType = UIReturnKeyType.Next;
                    }
                    else
                    {
                        Control.ReturnKeyType = UIReturnKeyType.Done;
                    }

                    RenderUtil.AddDoneButton(Control);
                }
            }
        }
    }
}