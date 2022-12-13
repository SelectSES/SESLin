using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCameraView.Droid.JobSchedular
{
   
    [Service(Name = "CSCameraView.Droid.JobSchedular.CSCameraJobSchedular", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class CSCameraJobSchedular : JobService
    {
        public static readonly string TAG = typeof(CSCameraJobSchedular).FullName;
        long fibonacciValue;

        JobParameters parameters;

        public CSCameraJobSchedular()
        {
        }

        public override bool OnStartJob(JobParameters @params)
        {
            

            Intent broadcastIntent = new Intent(this, typeof(AlarmReceiver));
            SendBroadcast(broadcastIntent);
            Util.scheduleJob(MainActivity.Current.BaseContext); // reschedule the job
                return true;
           
        }

        public override bool OnStopJob(JobParameters @params)
        {

            Intent broadcastIntent = new Intent(this, typeof(AlarmReceiver));
            SendBroadcast(broadcastIntent);

            return true; // reschedule the job.
        }


        /// <summary>
        /// Broadcast the result of the Fibonacci calculation.
        /// </summary>
        /// <param name="result">Result.</param>
        void BroadcastResults(long result)
        {
            Intent i = new Intent(JobSchedulerHelpers.JobActionKey);
            i.PutExtra(JobSchedulerHelpers.ResultKey, result);
            BaseContext.SendBroadcast(i);
        }


    }
}