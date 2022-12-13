
using CSCameraView.Dependency;
using Foundation;

using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using System.Text;
using UIKit;
using CSCameraView.iOS;
using CSCameraView.iOS.Dependency;
using LinphoneShared;
using CSCameraView.Classes;
using CallKit;
using Linphone;
using System.Threading.Tasks;

[assembly: Dependency(typeof(CallManager))]
namespace CSCameraView.iOS.Dependency
{
    public class CallManager : ISystemCallHelper
    {

        /// <summary>
        /// This is for Terminate the System Callkit Call
        /// </summary>

        public void TerminateCall()
        {
            try
            {

                AppDelegate.CallManager.EndCall();

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallManager:App entering system TerminateCall state:-." + ex.Message);
            }


        }
        /// <summary>
        /// This is for Start the System Callkit Call also when we connect the linephone Call
        /// </summary>
        public void AcceptCall()
        {
            try
            {
                AppDelegate.CallManager.StartCall();

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallManager:App entering system AcceptCall state:-." + ex.Message);
            }



        }
        public static LinphoneAccess access;
        public void ShowAnswerCallUI()
        {
            try
            {
                Task.Run(() =>
                {

                    App.DependencyInstance.InitializeLinphone(App.BaseContext);
                    CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush InitializeLinphone.");

                    App.DependencyInstance.RegisterSIPUser(Settings.UserName, Settings.Password, Settings.Domain, false);
                    CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush after RegisterSIPUser");


                    App.DependencyInstance.StartLinphoneManager();
                    CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush after StartLinphoneManager.");
                }).Wait(5000);


                var update = new CXCallUpdate();
                update.HasVideo = true;

                update.LocalizedCallerName = "Voip Calling";

                update.SupportsDtmf = true;
                update.SupportsHolding = false;
                update.SupportsGrouping = false;
                update.SupportsUngrouping = false;

                var randomGuid = Guid.NewGuid();
                string randomGuid1 = randomGuid.ToString();
                AppDelegate.activeCallUuid = new NSUuid(randomGuid1);
                AppDelegate.fromNO = "Voip Calling";


                CSCameraView.Classes.Helper.WriteLog("App entering ShowAnswerCallkitUI ReportIncomingCall Method with call and registration success object.");
                AppDelegate.CallProviderDelegate.ReportIncomingCall(update, AppDelegate.activeCallUuid, AppDelegate.fromNO, "Test");


            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering ReportIncomingCall in AppDelegate." + ex.Message);
            }
        }
    }
}