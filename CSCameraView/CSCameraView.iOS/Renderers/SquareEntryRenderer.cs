using CoreGraphics;
using CSCameraView.Controls;
using CSCameraView.iOS.Renderers;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(SquareEntry), typeof(SquareEntryRenderer))]

namespace CSCameraView.iOS.Renderers
{
    public class SquareEntryRenderer: EntryRenderer
	{

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Element != null && Control != null)
			{
                // Create a custom border with square corners
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 0f;
                Control.Layer.BorderWidth = 0f;
                Control.Layer.BorderColor = UIColor.FromRGB(200, 200, 200).CGColor;
                Control.Layer.BackgroundColor = UIColor.White.CGColor;

                // Invisible views create padding at the beginning and end
                Control.LeftView = new UIView(new CGRect(0, 0, 18, Control.Frame.Height));
                Control.RightView = new UIView(new CGRect(0, 0, 18, Control.Frame.Height));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightViewMode = UITextFieldViewMode.Always;
            }
		}
	}
}