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
using Almg.MobileSigner.Services;
using Almg.MobileSigner.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(BaseUrl_Android))]
namespace Almg.MobileSigner.Droid
{

    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/pdf/web/";
        }
    }
    
}