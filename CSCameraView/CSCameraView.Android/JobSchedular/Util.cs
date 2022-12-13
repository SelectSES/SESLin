using Android.App;
using Android.App.Job;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCameraView.Droid.JobSchedular
{
    public class Util
    {

        public static void scheduleJob(Context context)
        {

            try
            {
                JobScheduler jobScheduler =
   (JobScheduler)MainActivity.Current.GetSystemService(Context.JobSchedulerService);


                JobInfo.Builder builder = MainActivity.Current.CreateJobInfoBuilder(0)

                    .SetPersisted(false)
                    .SetMinimumLatency(1000)    // Wait at least 1 second
                    .SetOverrideDeadline(10000)  // But no longer than 10 seconds
                    .SetRequiredNetworkType(NetworkType.Any);

                int result = jobScheduler.Schedule(builder.Build());
                if (result == JobScheduler.ResultSuccess)
                {

                }
                else
                {
                }

            }

            catch (Exception Ex)
            {
            }
        }
    }
}