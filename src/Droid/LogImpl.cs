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

[assembly: Xamarin.Forms.Dependency(typeof(LogImpl))]
namespace Almg.MobileSigner.Droid
{
    public class LogImpl : ILog
    {
        public void WriteLine(string message)
        {
            Android.Util.Log.Debug("Almg.MobileSigner.Droid", message);

        }
    }
}