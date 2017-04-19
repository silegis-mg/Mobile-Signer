using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Almg.MobileSigner;
using HockeyApp.iOS;
using Firebase.Analytics;
using Firebase.CloudMessaging;
using UserNotifications;
using Firebase.InstanceID;
using Almg.MobileSigner.Controllers;
using System.Threading.Tasks;
using ToastIOS;
using Xamarin.Forms;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;

namespace AssinadorMobile.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IMessagingDelegate, IUNUserNotificationCenterDelegate
	{

		private const string HockeyID = Config.HockeyAppIdiOS;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new Almg.MobileSigner.App());

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure(HockeyID);
			manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
			manager.StartManager();

			RegisterFirebaseNotification();
			MessagingCenter.Subscribe<ShareViewModel>(this, "Share", Share, null);

			return base.FinishedLaunching(app, options);
		}

		#region Share File
		private void Share(ShareViewModel source)
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			var subviews = window.Subviews;
			var view = subviews.Last();
			var frame = view.Frame;

			var uidic = UIDocumentInteractionController.FromUrl(new NSUrl(source.FileName, true));

			if (!uidic.PresentPreview(true))
			{
				var rc = uidic.PresentOpenInMenu(frame, view, true);
				if (!rc)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = AppResources.FAILED_TO_OPEN_ATTACHMENT,
						Message = AppResources.ATTACHMENT_FILE_TYPE_NOT_SUPPORTED
					};
					alert.AddButton(AppResources.OK);
					alert.Show();
				}
			}

		}
		#endregion

		#region Firebase Notification
		private void RegisterFirebaseNotification()
		{
			InstanceId.Notifications.ObserveTokenRefresh(async (sender, e) =>
			{
				// This callback will be fired everytime a new token is generated, including the first
				// time.
				var refreshedToken = InstanceId.SharedInstance.Token;
				ConnectToFCM();

				NotificationController notifCtrl = new NotificationController();
				bool ok = await notifCtrl.SendNewTokenToServer(refreshedToken, NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString());
				if (!ok)
				{
					Console.WriteLine("Failed to send token to server");
				}

			});

			// Register your app for remote notifications.
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// iOS 10 or later
				var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
				UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
				{
					Console.WriteLine(granted);
				});

				// For iOS 10 display notification (sent via APNS)
				UNUserNotificationCenter.Current.Delegate = this;

				// For iOS 10 data message (sent via FCM)
				Messaging.SharedInstance.RemoteMessageDelegate = this;
			}
			else
			{
				// iOS 9 or before
				var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			Firebase.Analytics.App.Configure();

			ConnectToFCM();
		}

		private void ConnectToFCM()
		{
			Messaging.SharedInstance.Connect(error =>
				{
					if (error != null)
					{
						Console.WriteLine("Error connecting to FCM: " + error);
					}
					else
					{
						Console.WriteLine("Connected to FCM");
						Messaging.SharedInstance.Subscribe("/topics/global");
					}
				});
		}

		// To receive notifications in foregroung on iOS 9 and below.
		// To receive notifications in background in any iOS version
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			if (userInfo.ContainsKey((NSString)"message"))
			{
				string message = (string)userInfo["message"].ToString();
				ShowNotification(message);
			}
		}

		// Receive data message on iOS 10 devices.
		public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
		{
			if (remoteMessage.AppData.ContainsKey((NSString)"message"))
			{
				string message = (string)remoteMessage.AppData["message"].ToString();
				ShowNotification(message);
			}
		}

		private void ShowNotification(string message)
		{
			if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
			{
				Toast.MakeText(message, 4000).SetCornerRadius(0.5f).SetGravity(ToastGravity.Top).Show();
			}
			else 
			{
				var notification = new UILocalNotification();
				notification.FireDate = NSDate.FromTimeIntervalSinceNow(1);
				notification.AlertAction = "Silegis";
				notification.AlertBody = message;
				notification.SoundName = UILocalNotification.DefaultSoundName;
				UIApplication.SharedApplication.ScheduleLocalNotification(notification);
			}

			MessagingCenter.Send<string>(message, "Notification");
		}

		public override void DidEnterBackground(UIApplication application)
		{
			Messaging.SharedInstance.Disconnect();
			Console.WriteLine("Disconnected from FCM");
		}

		[Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
		public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			System.Console.WriteLine(notification.Request.Content.UserInfo);
		}

		[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
		public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
		}

		#endregion
	}
}

