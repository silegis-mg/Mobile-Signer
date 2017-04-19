using AssinadorMobile.iOS;
using AssinadorMobile;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Almg.MobileSigner.Components;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace AssinadorMobile.iOS
{
    public class CustomWebViewRenderer : WebViewRenderer
    {

		public CustomWebViewRenderer() {
			this.ScrollView.ScrollEnabled = true;
		}
        
        public override void LoadHtmlString(string s, NSUrl baseUrl)
        {
			//Workaround for WebView bug
            baseUrl = new NSUrl(Path.Combine(NSBundle.MainBundle.BundlePath, "Content/pdf/web/"), true);
            base.LoadHtmlString(s, baseUrl);
        }

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) { 
				Delegate = new CustomWebViewDelegate(); 
			}

			var webView = this;

			//webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			webView.ScalesPageToFit = true;
			webView.ContentMode = UIViewContentMode.ScaleAspectFill;
		}
    }

	internal class CustomWebViewDelegate : UIWebViewDelegate
	{
		public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			return true;
		}

		public override void LoadFailed(UIWebView webView, NSError error)
		{

		}

		public override void LoadingFinished(UIWebView webView)
		{

		}
	}
}
