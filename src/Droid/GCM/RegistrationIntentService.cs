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
using Android.Util;
using Android.Gms.Gcm.Iid;
using Android.Gms.Gcm;
using Almg.MobileSigner.Controllers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Droid
{
	[Service(Exported = false)]
	public class RegistrationIntentService : IntentService
    {
        static object locker = new object();

        public RegistrationIntentService() : base("RegistrationIntentService") { }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");
                lock (locker)
                {
                    var instanceID = InstanceID.GetInstance(this);
                    Log.Info("RegistrationIntentService", "Instance ID = " + instanceID.Id);
                    instanceID.DeleteToken(Config.GcmSenderIDAndroid, GoogleCloudMessaging.InstanceIdScope);
                    var token = instanceID.GetToken(Config.GcmSenderIDAndroid, GoogleCloudMessaging.InstanceIdScope, null);

                    Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
                    Subscribe(token);
                    SendRegistrationToServer(token);
                }
            }
            catch (Exception e)
            {
                Log.Debug("RegistrationIntentService", "Failed to get a registration token");
                return;
            }
        }

        private void SendRegistrationToServer(string token)
        {
            NotificationController notification = new NotificationController();
            string version = Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionName;
            Task<bool> task = notification.SendNewTokenToServer(token, version);
            task.Wait();
            if(!task.Result)
            {
                Log.Debug("RegistrationIntentService", "Failed to send registration token to server");
            }
        }

        private void Subscribe(string token)
        {
            try
            {
                var pubSub = GcmPubSub.GetInstance(this);
                pubSub.Subscribe(token, "/topics/global", null);
            } catch(Exception e)
            {
                Log.Error("RegistrationIntentService", e.Message);
            }
        }
    }
}