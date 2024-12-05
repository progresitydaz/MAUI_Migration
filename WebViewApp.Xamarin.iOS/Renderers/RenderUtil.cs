using System;
using System.Drawing;
using WebViewApp.Xamarin.Core.Controls;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Services;
using UIKit;

namespace WebViewApp.Xamarin.iOS.Renderers
{
    public static class RenderUtil
    {

        public static void AddDoneButton(UIResponder control)
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            string doneString = renderService.Translate("Done");

            var doneButton = new UIBarButtonItem(doneString, UIBarButtonItemStyle.Done, delegate
            {
                control.ResignFirstResponder();
            });

            toolbar.Items = new UIBarButtonItem[] {
                new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };

            if (control is UITextField)
            {
                UITextField textField = (UITextField)control;

                textField.InputAccessoryView = toolbar;
            }
            else if (control is UITextView)
            {
                UITextView textField = (UITextView)control;

                textField.InputAccessoryView = toolbar;
            }
        }

        public static void AddDoneAndNextButton(UITextField control, RoundedEntry entry)
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

            var renderService = AppDependencyResolver.Resolve<IUIRenderService>();

            string nextString = renderService.Translate("Next");
            string doneString = renderService.Translate("Done");

            var doneButton = new UIBarButtonItem(doneString, UIBarButtonItemStyle.Done, delegate
            {
                control.ResignFirstResponder();
            });

            UIBarButtonItem nextButton = null;

            if (entry.NextTextField != null)
            {
                nextButton = new UIBarButtonItem(nextString, UIBarButtonItemStyle.Done, delegate
                  {
                      bool focuesd = entry.NextTextField.Focus();

                      if (!focuesd)
                      {
                          control.ResignFirstResponder();
                      }
                  });
            }

            if (nextButton != null)
            {
                toolbar.Items = new UIBarButtonItem[] {
                    nextButton,
                    new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                    doneButton
                };
            }
            else
            {
                toolbar.Items = new UIBarButtonItem[] {
                    new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                    doneButton
                };
            }

            control.InputAccessoryView = toolbar;
        }

    }
}
