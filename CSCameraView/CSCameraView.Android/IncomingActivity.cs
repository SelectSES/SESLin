using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Content.PM;
using Android;
using Android.Net;
using Android.Views;
using System;
using CSCameraView.Dependency;

using Java.Lang;
using CSCameraView.Classes;
using System.Threading.Tasks;
using Xamarin.Essentials;
using IncomingCall.Dependency;
using Android.Media;
using System.Timers;

namespace CSCameraView.Droid
{
	[Activity(Label = "SEVA - Select Entry Video Access IncomingCall", LaunchMode =Android.Content.PM.LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait)]
	public class IncomingActivity : Activity, View.IOnClickListener
    {
        public static IncomingActivity CurrentIncomingActivity { get; internal set; }

        TextView tips;
        ImageView objbtnDecline, objbtnAnswer;
        public event Action OnAcceptCallingSuccess;
        long mSessionid;
        private System.Timers.Timer timer;
        System.Timers.Timer t2;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.DismissKeyguard | WindowManagerFlags.ShowWhenLocked | WindowManagerFlags.LayoutNoLimits | WindowManagerFlags.Fullscreen | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.TurnScreenOn);

            SetContentView(Resource.Layout.CustomView);

            objbtnDecline = (ImageView)FindViewById(Resource.Id.btnDecline);
            objbtnAnswer = (ImageView)FindViewById(Resource.Id.btnAnswer);


            FindViewById(Resource.Id.btnAnswer).SetOnClickListener(this);
            FindViewById(Resource.Id.btnDecline).SetOnClickListener(this);


            CurrentIncomingActivity = this;
            t2 = new Timer(30000);
            t2.Start();
            t2.Elapsed += Timer2_Elapsed;


            Android.Media.AudioManager am = (Android.Media.AudioManager)GetSystemService(Context.AudioService);
            
            switch (am.RingerMode)
            {
                case RingerMode.Normal:
          
                    break;
                case RingerMode.Silent:
                    App.DependencyInstance.StopRinging();

                    break;
                case RingerMode.Vibrate:
                    App.DependencyInstance.StopRinging();


                    break;
            }
        }
        /// <summary>
        /// set this timer because ,if user call rceived ,and showing Incmoing Activity only ,and call disconnect from jack then linephoneAccess does not fires  the terminate event ,and call still connected which shows the incoming Activity Continusly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            t2.Stop();
            ActivityManager activityManager = (ActivityManager)this.GetSystemService(Context.ActivityService);
            var recentTasks = activityManager.GetRunningTasks(999);
            for (int i = 0; i < recentTasks.Count; i++)
            {
                if (recentTasks[i].BaseActivity.ToShortString().IndexOf("IncomingActivity") > -1)
                {
                   
                    Finish();
                }
            }
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeUp)
            {
                App.DependencyInstance.StopRinging();
            }
            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeDown)
            {
                App.DependencyInstance.StopRinging();
            }
            return base.OnKeyDown(keyCode, e);
        }


        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);   
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    
        public void OnClick(View v)
        {
            try
            {

                var notificationManager = (NotificationManager)MainActivity.Current.GetSystemService(Context.NotificationService);
                switch (v.Id)
                {
                     case Resource.Id.btnAnswer:
                        //remove the incoming call notification
                        notificationManager.Cancel(2);

                        ActivityManager activityManager = (ActivityManager)this.GetSystemService(Context.ActivityService);
                        var recentTasks = activityManager.GetRunningTasks(999);
                        for (int i = 0; i < recentTasks.Count; i++)
                        {
                            if (recentTasks[i].BaseActivity.ToShortString().IndexOf("IncomingActivity") > -1)
                            {
                                Finish();
                                
                                using (var h = new Handler(Looper.MainLooper))
                                    h.Post(() =>
                                    {
                                        Intent fullScreenIntent = new Intent(MainActivity.Current, typeof(CallConnectingActivity));
                                        Bundle bundle = new Bundle();
                                        bundle.PutString("body", "body");
                                        bundle.PutString("title", "Title");
                                        fullScreenIntent.PutExtras(bundle);
                                    
                                        fullScreenIntent.AddFlags(ActivityFlags.NewTask 
                                            | ActivityFlags.ClearTask |ActivityFlags.SingleTop  );
                                        StartActivity(fullScreenIntent);
                                    });

                                break;
                            }
                            if (recentTasks[i].BaseActivity.ToShortString().IndexOf("MainActivity") > -1)
                            {
                                Finish();
                                Intent CallScreenIntent = new Intent(this, typeof(MainActivity));
                                Bundle bundle = new Bundle();
                                bundle.PutString("body", "body");
                                CallScreenIntent.PutExtras(bundle);
                                StartActivity(CallScreenIntent);
                                
                                break;
                            }
                        }
                        break;

                    case Resource.Id.btnDecline:
                        //remove the incoming call notification
                        notificationManager.Cancel(2);
                        Finish();
         
                        App.DependencyInstance.DeclineCall();
                        Settings.CurrentCallState = string.Empty;          
                        
                        break;
                      
                }
            }
            catch(System.Exception ex)
            {

                CSCameraView.Classes.Helper.WriteLog("IncomingActivity:OnClick" + ex.Message + "StackTrace\n" + ex.StackTrace);

            }
        }
       

    }
}


