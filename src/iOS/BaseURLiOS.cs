using AssinadorMobile.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Almg.MobileSigner.Services;

[assembly: Xamarin.Forms.Dependency(typeof(BaseUrl_iOS))]
namespace AssinadorMobile.iOS
{

    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            string fileName = "Content/pdf/web"; // remember case-sensitive
            string localHtmlUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);
            return localHtmlUrl;
        }
    }

}