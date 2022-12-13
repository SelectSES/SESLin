using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCameraView.Droid.JobSchedular
{
   public static class JobSchedulerHelpers
    {



        public static readonly int JobId = 110;
        public static readonly string ValueKey = "CSCamera_value";
        public static readonly string ResultKey = "CsCamera_result";
        public static readonly string JobActionKey = "job_action";

        /// <summary>
        /// Helper to initialize the JobInfo.Builder for the Fibonacci JobService, 
        /// initializing the value 
        /// </summary>
        /// <returns>The job info builder for fibonnaci calculation.</returns>
        /// <param name="context">Context.</param>
        /// <param name="value">Value.</param>
        public static JobInfo.Builder CreateJobInfoBuilder(this Context context, int value)
        {
            var component = context.GetComponentNameForJob<CSCameraJobSchedular>();
            JobInfo.Builder builder = new JobInfo.Builder(JobId, component)
                                                 .SetFibonacciValue(value);
            return builder;
        }

        public static ComponentName GetComponentNameForJob<T>(this Context context) where T : JobService
        {
            Type t = typeof(T);
            Class javaClass = Class.FromType(t);
            return new ComponentName(context, javaClass);
        }

        public static JobInfo.Builder SetFibonacciValue(this JobInfo.Builder builder, int value)
        {
            var extras = new PersistableBundle();
            extras.PutLong(ValueKey, value);
            builder.SetExtras(extras);
            return builder;
        }

    }
}