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

[assembly: Xamarin.Forms.Dependency(typeof(AndroidActivityFlagsHelper))]
namespace CSCameraView.Droid.Dependency
{
    public class AndroidActivityFlagsHelper : IAndroidActivityFlagsHelper
    {
        //Method to clear activity flags that keep the activity alive on lock screen and keep the screen on.
        public void clearFlags()
        {
            try
            {
                MainActivity.Current.Window.ClearFlags(WindowManagerFlags.DismissKeyguard | WindowManagerFlags.ShowWhenLocked | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.TurnScreenOn);
                CSCameraView.Classes.Helper.WriteLog($"Flags Cleared");
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog($"Exception in clearFlags: {ex.StackTrace}");
            }
        }
    }
}