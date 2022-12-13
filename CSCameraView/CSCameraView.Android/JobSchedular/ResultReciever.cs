using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Droid.JobSchedular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCameraView.Droid
{

    public partial class MainActivity
    {
        [BroadcastReceiver(Enabled = true, Exported = false)]
        protected internal class ResultReciever : BroadcastReceiver
        {
            MainActivity activity;

            public ResultReciever()
            {
              
            }

            public ResultReciever(MainActivity activity)
            {
                this.activity = activity;
            }

            public override void OnReceive(Context context, Intent intent)
            {

                if (activity == null)
                {
                }
                else
                {
                    long result = intent.Extras.GetLong(JobSchedulerHelpers.ResultKey, -1);
                    if (result > -1)
                    {

                    }
                    else
                    {
                    }
                }
            }
        }
    }
}