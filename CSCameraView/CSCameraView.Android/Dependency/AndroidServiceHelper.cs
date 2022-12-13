
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Dependency;
using CSCameraView.Droid.Dependency;
using CSCameraView.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;
using System.Text;
using Android.Content;
using CSCameraView.Classes;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidServiceHelper))]

namespace CSCameraView.Droid.Dependency
{
   public  class AndroidServiceHelper : IAndroidService
    {
        private static Context context = Android.App.Application.Context;

         Intent intent = new Intent(context, typeof(LinePhoneService1));
        public void StartService()
        {
            //Call only StartService() because startForgoundService Method  get Exception

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                if (!string.IsNullOrEmpty(Settings.UserName) &&(!string.IsNullOrEmpty(Settings.Password)))
                {
                    context.StartForegroundService(intent);
                }
            }
            else
            {
                context.StartService(intent);
             }
        }


       
        public void StopService()
        {
            //stop time for refresh Regiser  inside the linephone service

            context.StopService(intent);
        }
    }
}