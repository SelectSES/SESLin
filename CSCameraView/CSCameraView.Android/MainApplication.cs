using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Mtp;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;

namespace CSCameraView.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CreateNotificationChannel();
          
            var resourceId = Resources.GetIdentifier("AcceptCall", "drawable", PackageName);
            if (resourceId > 0)
            {
                FirebasePushNotificationManager.IconResource = resourceId;
            }

            //If debug you should reset the token each time.
#if DEBUG

            FirebasePushNotificationManager.Initialize(this, true);

#else
            FirebasePushNotificationManager.Initialize(this, false, true, false);
#endif
            }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
        void showNotification(IDictionary<string, Object> data)
		{
			try
			{
                CSCameraView.Classes.Helper.WriteLog("___________________ShowNotification Notification Received_______________:" + "\n");

                string Title = string.Empty;
				string Body = string.Empty;
				string imageReceived = string.Empty;
				foreach (KeyValuePair<string, object> ele2 in data)
				{
					Console.WriteLine("{0} and {1}", ele2.Key, ele2.Value);
					if (ele2.Key.Equals("title"))
					{
						Title = ele2.Value.ToString();
					}
					else if (ele2.Key.Equals("image"))
					{
						imageReceived = ele2.Value.ToString();
					}
					else if (ele2.Key.Equals("body"))
					{
						Body = ele2.Value.ToString();
					}

				}

				var intent = new Intent(this, typeof(MainActivity));
				intent.AddFlags(ActivityFlags.ClearTop);


				System.Random random = new System.Random();
				int pendingIntentId = random.Next(9999 - 1000) + 1000; //for multiplepushnotifications


				var pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

				var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
										 .SetSmallIcon(Resource.Drawable.AcceptCall)
										 .SetContentTitle(Title)
										 .SetContentText(Body)
										 .SetAutoCancel(true)
										 .SetPriority(1)
										 .SetVibrate(new long[] { 1000, 1000, 1000, 1000, 1000 })
										 .SetSound(Android.Media.RingtoneManager.GetDefaultUri(Android.Media.RingtoneType.Notification))
										 .SetContentIntent(pendingIntent);

				
					NotificationCompat.BigTextStyle bigTextStyle = new NotificationCompat.BigTextStyle();
					bigTextStyle.BigText(Body);
					notificationBuilder.SetStyle(bigTextStyle);
				

				var notificationManager = NotificationManagerCompat.From(this);
				notificationManager.Notify(pendingIntentId, notificationBuilder.Build());
			}
			catch (Exception EX)
			{

				

				//throw;
			}
		}
		void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var DefaultNotificationChannelId = MainActivity.CHANNEL_ID;
                var DefaultNotificationChannelName = "CSCameraFirebasePushNotificationChannel123";
                var channel = new NotificationChannel(DefaultNotificationChannelId, DefaultNotificationChannelName, NotificationImportance.High)
                {
                    Description = string.Empty
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

    }

    class MyNotificationHandler : IPushNotificationHandler
    {
        public void OnError(string error)
        {
            throw new NotImplementedException();
        }

        public void OnOpened(NotificationResponse response)
        {

        }

        public void OnReceived(IDictionary<string, object> parameters)
        {

        }
    }
}