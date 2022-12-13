using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IncomingCall.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CSCameraView.Droid
{
    //[Activity(Label = "CallConnecting") ]
    [Activity(Label = "CallConnecting")]
    public class CallConnectingActivity : Activity
    {
        private System.Timers.Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.DismissKeyguard | WindowManagerFlags.ShowWhenLocked | WindowManagerFlags.LayoutNoLimits | WindowManagerFlags.Fullscreen | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.TurnScreenOn);

            SetContentView(Resource.Layout.layout1);

            timer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = 1
            };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
          
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    Intent fullScreenIntent = new Intent(MainActivity.Current, typeof(MainActivity));
                    Bundle bundle = new Bundle();
                    bundle.PutString("body", "body");
                    bundle.PutString("title", "Title");
                    fullScreenIntent.PutExtras(bundle);
                
                    fullScreenIntent.AddFlags(ActivityFlags.NewTask 
                        | ActivityFlags.ClearTask | ActivityFlags.SingleTop);
                    StartActivity(fullScreenIntent);
                });
        }

        
    }
}