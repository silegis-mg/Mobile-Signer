using System;
using AssinadorMobile.iOS;
using System.IO;
using Foundation;
using Almg.MobileSigner.Services;
using Almg.MobileSigner.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(ResourceLoaderiOS))]
namespace AssinadorMobile.iOS
{
	public class ResourceLoaderiOS : IResourceLoader
	{
		public Stream OpenFile(string name)
		{
			string localFileName = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/" + name);
			return new FileStream (localFileName, FileMode.Open, FileAccess.Read);
		}
	}
}

