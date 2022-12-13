using CSCameraView.Controls;
using CSCameraView.iOS.Dependency;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace CSCameraView.iOS.Dependency
{
    public class HybridWebViewRenderer : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var view = Element as HybridWebView;
            if (view == null || NativeView == null)
            {
                return;
            }
            this.SizeToFit();
            this.BackgroundColor = UIColor.Black;
            UIDelegate = new ExtendedUIWebViewDelegate(this);
        }

    }


    public class ExtendedUIWebViewDelegate : UIWebViewDelegate , IWKUIDelegate
    {
        HybridWebViewRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(HybridWebViewRenderer _webViewRenderer = null)
        {
            webViewRenderer = _webViewRenderer ?? new HybridWebViewRenderer();
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            var wv = webViewRenderer.Element as HybridWebView;
            if (wv != null)
            {
                await System.Threading.Tasks.Task.Delay(100); // wait here till content is rendered
                wv.HeightRequest = (double)webView.ScrollView.ContentSize.Height;
            }
        }
    }

}