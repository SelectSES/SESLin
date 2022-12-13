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

namespace CSCameraView.Droid.Services
{
    [BroadcastReceiver(Enabled = true)]
  
    public class WakefulBroadcastReceiver : BroadcastReceiver
    {
        private static string TAG = "LL24";
        PowerManager.WakeLock screenWakeLock = null;
        public override void OnReceive(Context context, Intent intent)
        {
       
            if (screenWakeLock == null)
            {
                PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
                screenWakeLock = pm.NewWakeLock(WakeLockFlags.OnAfterRelease 
                        | WakeLockFlags.AcquireCausesWakeup | WakeLockFlags.ScreenBright | WakeLockFlags.Full, "myApp:notificationLock");
                screenWakeLock.Acquire();
            }
           
            if (screenWakeLock != null)
            screenWakeLock.Release();

        }

    }

}