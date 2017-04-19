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
using Xamarin.Forms;
using Almg.MobileSigner.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(UpdateAppImpl))]
namespace Almg.MobileSigner.Droid
{
    public class UpdateAppImpl : IUpdateApp
    {
        public string GetVersion()
        {
            return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionName;
        }

        public void Update()
        {
            Forms.Context.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + Forms.Context.PackageName)));
        }
    }
}