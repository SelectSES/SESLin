using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.Res;
using Android.Graphics;
using CSCameraView.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CSCameraView.Controls;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(CustomEntryRenderer))]
namespace CSCameraView.Droid.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;

                var lp = new MarginLayoutParams(Control.LayoutParameters);
                lp.SetMargins(0, 0, 0, 0);
                LayoutParameters = lp;
                Control.LayoutParameters = lp;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}