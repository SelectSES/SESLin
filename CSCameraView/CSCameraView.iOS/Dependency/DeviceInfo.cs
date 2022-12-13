using CSCameraView.Dependency;
using CSCameraView.iOS.Dependency;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfo))]
namespace CSCameraView.iOS.Dependency
{
    public class DeviceInfo: IDeviceInfo
    {

        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.ToString();
        }
    }
}