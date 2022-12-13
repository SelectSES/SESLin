using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Dependency;
using CSCameraView.Droid.Dependency;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSCameraView.Classes;
using Android.Support.V4.App;

[assembly: Xamarin.Forms.Dependency(typeof(CallManager))]
namespace CSCameraView.Droid.Dependency
{
   public class CallManager: ISystemCallHelper
    {
        public string Title = "Title";
        public string Body = "Body";
        public void TerminateCall()
        {
            try
            {

                 IncomingActivity.CurrentIncomingActivity.Finish();
                 App.DependencyInstance.DeclineCall();

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallManager:App entering system TerminateCall state:-." + ex.Message);
            }


        }
        //Addded This Method only for declare the Defination becuase we inheryts the ISystemCallHelper interface no need to write any code for Android
        public void AcceptCall()
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallManager:App entering system AcceptCall state:-." + ex.Message);
            }



        }

        public void ShowAnswerCallUI()
        {
            try
            {
                    using (var h = new Handler(Looper.MainLooper))
                        h.Post(() =>
                        {
                            Intent fullScreenIntent = new Intent(MainActivity.Current, typeof(IncomingActivity));
                            Bundle bundle = new Bundle();
                            bundle.PutString("body", Body);
                            bundle.PutString("title", Title);
                            fullScreenIntent.PutExtras(bundle);
                            fullScreenIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.FromBackground | ActivityFlags.ClearTask);
                            MainActivity.Current.StartActivity(fullScreenIntent);

                            PendingIntent pendingIntent = PendingIntent.GetActivity(MainActivity.Current, 002, fullScreenIntent, PendingIntentFlags.UpdateCurrent);

                            var channelName = "Incoming_Call_Notification";
                            var channelDescription = "Incoming Call Notification Description";
                            var channel = new NotificationChannel("002", channelName, NotificationImportance.Default)
                            {
                                Description = channelDescription
                            };
                            var notificationManager = (NotificationManager)MainActivity.Current.GetSystemService(Context.NotificationService);
                            notificationManager.CreateNotificationChannel(channel);

                            // Instantiate the builder and set notification elements:
                            NotificationCompat.Builder builder = new NotificationCompat.Builder(MainActivity.Current, "002")
                                .SetContentIntent(pendingIntent)
                                .SetContentTitle("Incoming Call")
                                .SetContentText("You have an incoming call")
                                .SetSmallIcon(Resource.Drawable.Logo)
                                .SetOngoing(true);

                            // Build the notification:
                            Notification notification = builder.Build();

                            // Publish the notification:
                            const int notificationId = 2;
                            notificationManager.Notify(notificationId, notification);
                            
                        });
                
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering ReportIncomingCall in AppDelegate." + ex.Message);
            }
        }


    }
}