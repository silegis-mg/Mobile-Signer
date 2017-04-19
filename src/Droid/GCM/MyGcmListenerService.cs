using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Gcm;
using Android.Util;
using Android.Support.V4.App;
using Android.Graphics;

namespace Almg.MobileSigner.Droid
{
	[Service(Exported = false), IntentFilter(new[] { "com.google.android.c2dm.intent.RECEIVE" })]
	public class MyGcmListenerService : GcmListenerService
	{
		const string TAG = "MyGcmListenerService";

		public override void OnMessageReceived(string from, Bundle data)
		{
			var message = data.GetString("message");
            var body = data.GetString("body");
            Log.Debug(TAG, "From: " + from);
			Log.Debug(TAG, "Message: " + message);
            Log.Debug(TAG, "Body: " + body);

            SendNotification(message!=null?message:body);
		}

		void SendNotification(string message)
		{
			var intent = new Intent(this, typeof(MainActivity));
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            Bitmap appIcon = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.icon);

            var bigTextStyle = new Notification.BigTextStyle().BigText(message).SetBigContentTitle("SILEGIS");

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetDefaults(NotificationDefaults.Sound)
                .SetVibrate(new long[] { 100, 250, 100, 250, 100, 250 })
                .SetLights(Color.Yellow, 500, 500)
                .SetContentTitle("SILEGIS")
                .SetStyle(bigTextStyle)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetContentText(message);

			var notificationManager = (NotificationManager) GetSystemService(Context.NotificationService);
			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}