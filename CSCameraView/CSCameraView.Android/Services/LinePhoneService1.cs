using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Xamarin.Forms;
using CSCameraView.Classes;
using LinphoneShared;
using Android.Net.Wifi;
using Android.Text.Format;
using Android.Support.V4.App;
using CSCameraView.Droid.JobSchedular;
using Android.App.Job;

namespace CSCameraView.Droid.Services
{
    [Service]
 
    class LinePhoneService1 : Service
    {
    
       // public static LinphoneAccess access;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        Timer timer_for_RefreshRegistration=new Timer();
        public const int ServiceRunningNotifID = 9000;
        static readonly string TAG = typeof(MainActivity).FullName;
        JobScheduler jobScheduler;
        private Context mContext;
        public const string URGENT_CHANNEL = "CustomChanel";


        private static string foregroundChannelId = "9001";
        private static Context context = global::Android.App.Application.Context;

        public override void OnCreate()
        {
            base.OnCreate();
            mContext = ApplicationContext;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
          
            CSCameraView.Classes.Helper.WriteLog("OnStartCommand." + DateTime.Now);
            try
            {
                jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
                Notification Notif=ReturnNotif();
                StartForeground(ServiceRunningNotifID, Notif);

                AlarmManager manager = (AlarmManager)GetSystemService(AlarmService);
                long triggerAtTime = SystemClock.ElapsedRealtime() + (10 * 60 * 1000);
                Intent alarmintent = new Intent(this, typeof(AlarmReceiver));

                PendingIntent pendingintent = PendingIntent.GetBroadcast(this, 0, alarmintent, 0);
                if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    manager.Cancel(pendingintent);
                    manager.SetAndAllowWhileIdle(AlarmType.ElapsedRealtimeWakeup, triggerAtTime, pendingintent);
            

                }
                else if (Android.OS.Build.VERSION.SdkInt == BuildVersionCodes.Kitkat || Android.OS.Build.VERSION.SdkInt == BuildVersionCodes.Lollipop)
                {
                    manager.Cancel(pendingintent);
                    manager.SetExact(AlarmType.ElapsedRealtimeWakeup, triggerAtTime, pendingintent);
                }


                if (!App.DependencyInstance.IsManagerAvailable())
                {
                    App.DependencyInstance.InitializeLinphone(App.BaseContext);
                    App.DependencyInstance.StartLinphoneManager();
                }
                App.DependencyInstance.RegisterSIPUser(Settings.UserName, Settings.Password, Settings.Domain,false);



                BootReceiver bootReceiver = new BootReceiver();
                IntentFilter filter = new IntentFilter(Intent.ActionBootCompleted)
                {
                    Priority = (int)IntentFilterPriority.HighPriority
                };
                RegisterReceiver(bootReceiver, filter);


                timer_for_RefreshRegistration = new Timer(600000);
                timer_for_RefreshRegistration.Elapsed += timer_for_RefreshRegistration_Elapsed;
                timer_for_RefreshRegistration.Start();


                if (Settings.IsStartedFromBoot)
                {
                    if (MainActivity.Current != null)
                    {
                        Settings.IsStartedFromBoot = false;
                        CSCameraView.Classes.Helper.WriteLog($"MoveTaskToBack from Service onStartCommand: {MainActivity.Current.MoveTaskToBack(true)}");
                    }
                }
            }
            catch(Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("LinePhoneService1:OnStartCommand:" + Ex.Message + "StackTrace\n" + Ex.StackTrace);
            }
            return StartCommandResult.Sticky;
        }
     

     

    
        private void timer_for_RefreshRegistration_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer_for_RefreshRegistration.Stop();
                if(!string.IsNullOrEmpty(Settings.UserName) && !String.IsNullOrEmpty(Settings.Password))
                {
                     App.DependencyInstance.RefreshSIPRegistration();
                  
                }
                timer_for_RefreshRegistration.Start();
            }
            catch(Exception ex)
            {
              
            }
        }

        public override void OnDestroy()
        {
            CSCameraView.Classes.Helper.WriteLog("OnDestroy." + DateTime.Now);
            Intent broadcastIntent = new Intent(this,typeof(LinePhoneService1));
             SendBroadcast(broadcastIntent);
            base.OnDestroy();

        }
        PowerManager.WakeLock screenWakeLock = null;

        public override bool StopService(Intent name)
        {
            timer_for_RefreshRegistration.Stop();

            return base.StopService(name);
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
        }

      
   
        public Notification ReturnNotif()
        {
            // Building intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
                 .SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(Resources.GetString(Resource.String.notification_text))
                .SetSmallIcon(Resource.Drawable.ic_NewRIcon)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            // Building channel if API verion is 26 or above
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "SEVA", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }

        /// <summary>
        /// Builds a PendingIntent that will display the main activity of the app. This is used when the 
        /// user taps on the notification; it will take them to the main activity of the app.
        /// </summary>
        /// <returns>The content intent.</returns>
        PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }

       

     
       


    }
}