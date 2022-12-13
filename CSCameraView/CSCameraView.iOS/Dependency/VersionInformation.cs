using CSCameraView.Dependency;
using CSCameraView.iOS.Dependency;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionInformation))]

namespace CSCameraView.iOS.Dependency
{
    public class VersionInformation: IVersionInfo
    {

        public string VersionName => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();

        public string VersionCode => NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();

    }
}