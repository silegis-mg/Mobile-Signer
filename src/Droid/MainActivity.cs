using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Xamarin.Forms.Platform.Android;
using System.Xml;
using Acr.UserDialogs;
using Almg.MobileSigner;
using Xamarin.Forms;
using Android.Support.V4.Content;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Android.Gms.Common;
using Almg.MobileSigner.Model;

namespace Almg.MobileSigner.Droid
{

    [Activity (Label = "SILEGIS - MG", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
    {

        private const string HockeyID = Config.HockeyAppIdAndroid;

        protected override void OnCreate (Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CrashManager.Register(this, HockeyID, new CustomCrashListener());
            MetricsManager.Register(Application, HockeyID);

            UserDialogs.Init(this);
			MessagingCenter.Subscribe<ShareViewModel>(this, "Share", Share, null);

			LoadApplication(new App());

			if (CheckPlayServices())
			{
				var intent = new Intent(this, typeof(RegistrationIntentService));
				StartService(intent);
			}
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {

        }

        const int PLAY_SERVICES_RESOLUTION_REQUEST = 9000;

		private bool CheckPlayServices()
		{
			GoogleApiAvailability googleAPI = GoogleApiAvailability.Instance;
			int result = googleAPI.IsGooglePlayServicesAvailable(this);
			if (result != ConnectionResult.Success)
			{
				if (googleAPI.IsUserResolvableError(result))
				{
					googleAPI.GetErrorDialog(this, result,
							PLAY_SERVICES_RESOLUTION_REQUEST).Show();
				}

				return false;
			}

			return true;
		}

        private void ShowErrorMessage(string message)
        {
            new AlertDialog.Builder(this)
                .SetIcon(Android.Resource.Drawable.IcDialogAlert)
                .SetTitle("Erro")
                .SetMessage(message)
                .SetPositiveButton("Ok", (s,d) =>
                    {
                        this.Finish();
                    })
                .Show();
        }

        private const int SHARE = 1;

        async void Share(ShareViewModel source)
        {
            Java.IO.File file = new Java.IO.File(source.FileName);
            var intent = new Intent(Intent.ActionSend);
            var fileUri = FileProvider.GetUriForFile(
                            this,
                            "br.gov.almg.mobilesigner.fileprovider",
                            file);

            intent.SetType(source.MimeType);
            intent.PutExtra(Intent.ExtraStream, fileUri);

            var intentChooser = Intent.CreateChooser(intent, "Compartilhar com");
            StartActivityForResult(intentChooser, SHARE);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if(requestCode== SHARE)
            {
                if(resultCode==Result.Canceled)
                {
                    Toast.MakeText(this.ApplicationContext, "O arquivo não foi compartilhado.", ToastLength.Long).Show();
                }
			} else if (requestCode == PLAY_SERVICES_RESOLUTION_REQUEST)
			{
				var intent = new Intent(this, typeof(RegistrationIntentService));
				StartService(intent);
			}
			else {
				base.OnActivityResult(requestCode, resultCode, data);
			}
        }
    }
}

