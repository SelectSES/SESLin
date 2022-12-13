using Android.App;
using Android.Content;
using Android.Content.PM;
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



[assembly: Dependency(typeof(VersionInformation))]
namespace CSCameraView.Droid.Dependency
{
   public  class VersionInformation: IVersionInfo
	{

		private PackageInfo packageInfo;

		public string VersionName
		{
			get
			{
				if (packageInfo == null)
				{
					InitializePackageInfo();
				}

				return packageInfo.VersionName;
			}
		}

		public string VersionCode
		{
			get
			{
				if (packageInfo == null)
				{
					InitializePackageInfo();
				}

				return packageInfo.VersionCode.ToString();
			}
		}

		// Initialize package information variable.
		private void InitializePackageInfo()
		{
			var context = Android.App.Application.Context;
			packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
		}
	}
}