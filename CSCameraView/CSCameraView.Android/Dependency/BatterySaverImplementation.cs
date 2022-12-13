using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Dependency;
using CSCameraView.Droid.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(BatterySaverImplementation))]

namespace CSCameraView.Droid.Dependency
{
    public class BatterySaverImplementation: BaterySaverPermission
    {
        public bool CheckIsEnableBatteryOptimizations()
        {

            PowerManager pm = (PowerManager)Android.App.Application.Context.GetSystemService(Context.PowerService);
            //enter you package name of your application
            bool result = pm.IsIgnoringBatteryOptimizations("com.rhealtech.CSCameraOperation");
            return result;
        }

        public void StartSetting()
        {
            Intent intent = new Intent();

            intent.SetAction(Android.Provider.Settings.ActionIgnoreBatteryOptimizationSettings);
            Forms.Context.StartActivity(intent);
        }
    }
}