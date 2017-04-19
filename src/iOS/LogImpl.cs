using System;
using Almg.MobileSigner.Services;
using AssinadorMobile.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(LogImpl))]
namespace AssinadorMobile.iOS
{
	public class LogImpl : ILog
	{
		public void WriteLine(string message)
		{
			Console.WriteLine("[Almg.MobileSigner.iOS] " + message);
		}
	}
}
