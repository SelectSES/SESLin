using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]

namespace CSCameraView.Droid.Renderers
{
    public class CustomEntryRenderer: EntryRenderer
	{

		public CustomEntryRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null || e.NewElement == null)
			{
				return;
			}

			Control.SetPadding(15, Control.PaddingTop, 15, Control.PaddingBottom);
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
			}
			else
			{
				Control.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcAtop);
			}
		}
	}
}