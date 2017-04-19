using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Webkit;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Droid;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace Almg.MobileSigner.Droid
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                CustomWebView webView = (CustomWebView)sender;
                //Control.SetWebChromeClient(new WebChromeClient());
                //Control.SetWebViewClient(new WebViewClient());
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.BuiltInZoomControls = webView.OverviewMode;
                Control.Settings.LoadWithOverviewMode = webView.OverviewMode;
                Control.Settings.UseWideViewPort = webView.OverviewMode;
                Control.SetInitialScale(0);
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}