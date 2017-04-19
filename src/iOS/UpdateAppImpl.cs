using System;
using Almg.MobileSigner.Services;
using Foundation;
using MobileSigner.iOS;
using UIKit;
using Almg.MobileSigner;

[assembly: Xamarin.Forms.Dependency(typeof(UpdateAppImpl))]
namespace MobileSigner.iOS
{
	public class UpdateAppImpl : IUpdateApp
	{
		private static string installUrl = "itms-services://?action=download-manifest&amp;url=" + Config.BaseUrl + "/aplicativos/silegis.plist";


        public string GetVersion()
		{
			return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
		}

		public void Update()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(installUrl));
		}
	}

}
