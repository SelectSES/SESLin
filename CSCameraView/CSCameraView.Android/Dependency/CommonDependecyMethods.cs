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

[assembly: Dependency(typeof(CommonDependecyMethods))]
namespace CSCameraView.Droid.Dependency
{
   public  class CommonDependecyMethods : ICommonDependecyMethodsInterface
    {
        public string GetDeviceId()
        {
            return Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
    }
}