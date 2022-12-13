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
using CSCameraView.Droid.Services;
using CSCameraView.Droid.JobSchedular;

namespace CSCameraView.Droid.Services
{

    [BroadcastReceiver(Enabled = true)]
    
    public  class AlarmReceiver : WakefulBroadcastReceiver
    {
        private static string TAG = "LL24";
        public override void OnReceive(Context context, Intent intent)
        {
           
            Util.scheduleJob(context);
            if (!MainActivity.Current.IsServiceRunning(typeof(LinePhoneService1)))
            {
                Intent background = new Intent(context, typeof(LinePhoneService1));
               
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    if((!String.IsNullOrEmpty(Settings.UserName)) && (!String.IsNullOrEmpty(Settings.Password)))
                    {
                        context.StartForegroundService(background);
                    }
                        
                 
                }
                else
                {
                    context.StartService(background);
                }
            }
            
        }
    }

}