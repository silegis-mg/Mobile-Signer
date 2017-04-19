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
using System.IO;
using Xamarin.Forms;
using Java.IO;
using Almg.MobileSigner.Services;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ResourceLoaderAndroid))]
namespace Almg.MobileSigner.Droid
{
    public class ResourceLoaderAndroid: IResourceLoader
    {
        public Stream OpenFile(string name)
        {
            return Forms.Context.Assets.Open(name, Android.Content.Res.Access.Streaming);
        }

        public void SaveFile(string name, Stream stream)
        {
			Stream outputStream = Forms.Context.OpenFileOutput(name, FileCreationMode.Private);
			StreamHelper.CopyStream(stream, outputStream);
			outputStream.Flush();
			outputStream.Close();
        }
    }
}