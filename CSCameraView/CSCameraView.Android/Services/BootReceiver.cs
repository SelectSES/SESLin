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
using CSCameraView.Classes;


namespace CSCameraView.Droid.Services
{
    [BroadcastReceiver(Enabled = true)]
   
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)

        {
            try
            {
                if (intent.Action.Equals(Intent.ActionBootCompleted))
                {
                    Settings.IsServiceRunning = false;
                    Intent i = new Intent(context, typeof(MainActivity));
                    i.AddFlags(ActivityFlags.NewTask);
                    i.PutExtra("FROM_BOOT", true);
                    context.StartActivity(i);
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
    }

   
}