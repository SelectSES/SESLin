using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;

using Android.Views;
using Android.Widget;
using CSCameraView.Classes;

using Firebase.Messaging;
using Java.Net;
using Android;
using CSCameraView.Droid;
using static Android.Provider.SyncStateContract;
using CSCameraView.Droid.Services;

using LinphoneShared;
using CSCameraView.Dependency;
using Xamarin.Forms;
using RelativeLayout = Xamarin.Forms.RelativeLayout;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Android.Support.V4.App;

namespace CSCameraView.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]


	//added this class  for display the andrion Notification with OS 10
	public class FirebaseMessaging : FirebaseMessagingService
	{
     
        public string Title = "Title";
        public string Body = "Body";

        public override void OnMessageReceived(RemoteMessage message)
         {
            try
            { 
                var body = message.GetNotification() == null ? string.Empty : message.GetNotification().Body;
                var title = message.GetNotification() == null ? string.Empty : message.GetNotification().Title;
                string NotificationType = message.Data.ContainsKey("NotificationType") ? message.Data["NotificationType"] : String.Empty; 
           
                if (NotificationType.ToUpper()=="FORCELOGOUT")
                {

                    App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName, Settings.Password, Settings.Domain);

                    App.DependencyInstanceAndroid.StopService();
                    Settings.UserName = string.Empty;
                    Settings.Password = string.Empty;
                    Settings.Domain = string.Empty;
                    Settings.RegistrationState = string.Empty;
                    Settings.IsUserLoggedIn = false;
                    Settings.NotificationToken = string.Empty;
                    Settings.UrlSelectionMode = string.Empty;

                    Device.BeginInvokeOnMainThread(() => {

                        App.Current.MainPage = new NavigationPage(new Pages.LoginPage());
                    });
                }
                if (NotificationType.ToUpper() == "MISSEDCALL")
                {
                    SendMissedCallNotification(body, message.Data);
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                  CSCameraView.Classes.Helper.WriteLog("OnMessageReceived:" + ex.Message + "StackTrace\n" + ex.StackTrace);
            }
        }
       
        public override void HandleIntent(Intent intent)
        {
            try
            {
                if (intent.Extras != null)
                {
                    var builder = new RemoteMessage.Builder("FirebaseMessaging");

                    foreach (string key in intent.Extras.KeySet())
                    {
                        builder.AddData(key, intent.Extras.Get(key).ToString());
                    }

                    this.OnMessageReceived(builder.Build());
                }
                else
                {
                    base.HandleIntent(intent);
                }
            }
            catch (Exception)
            {
                base.HandleIntent(intent);
            }
        }

        void SendNotification(string messageBody, IDictionary<string, string> data, string notificationTitle = "FCM")
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this,
                                                          MainActivity.NOTIFICATION_ID,
                                                          intent,
                                                          PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.MissedCall)
                                      .SetContentTitle(notificationTitle)
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());
        }
        void SendMissedCallNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this,
                                                          MainActivity.NOTIFICATION_ID,
                                                          intent,
                                                          PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.MissedCall)
                                      .SetContentTitle("SEVA - MISSED CALL")
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());
        }
    }

}