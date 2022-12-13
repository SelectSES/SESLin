using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AVFoundation;
using CallKit;
using CoreFoundation;
using CSCameraView.Classes;
using CSCameraView.Models;
using CSCameraView.Pages;
using Foundation;
using IncomingCall.Dependency;
using Linphone;
using LinphoneShared;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;

using PushKit;
using UIKit;
using UserNotifications;
using Xamarin.Forms;


namespace CSCameraView.iOS
{
    [Register("AppDelegate")]

    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IPKPushRegistryDelegate
    {
        public static AppDelegate Instance;
        public static NSUuid activeCallUuid;


        object locker = new object(); // class level private field
        public static string fromNO { get; set; }

        public static ActiveCallManager CallManager;
        public  LinphoneAccess access;
        
        public static ProviderDelegate CallProviderDelegate { get; set; }

        string deviceToken;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            //set the this property to false when user opens the app first time it shows the Main Page always and when if call is received and app is in killed state then open the App will shows the WaiPage Screen instead of MainPage
            //Check this property at time of app open
           // Settings.CallInProcess = false;
            Instance = this;
            global::Xamarin.Forms.Forms.Init();
           
            LoadApplication(new App(IntPtr.Zero));
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                  UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                app.RegisterUserNotificationSettings(notificationSettings);
            }
            RegisterVoip();
            CallManager = new ActiveCallManager();
            CallProviderDelegate = new ProviderDelegate(CallManager);
           
            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// When you start the App first Time it creates the APNS Token from here ,and its' default Method fires evry Time 
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="credentials"></param>
        /// <param name="type"></param>
      
        public void DidUpdatePushCredentials(PKPushRegistry registry, PKPushCredentials credentials, string type)
        {
            if (credentials != null && credentials.Token != null)
            {
                var fullToken = credentials.Token.DebugDescription;
                deviceToken = fullToken.Trim('<').Trim('>').Replace(" ", string.Empty);
                CSCameraView.Classes.Helper.WriteLog("App entering DidUpdatePushCredentials state:-." + deviceToken);
                Settings.NotificationToken = deviceToken;
            }
        }
        /// <summary>
        /// DidReceiveIncomingPush is fired when we get the APNs Notification from Server and this Method fires in IOS which OS version is less than 12 which doen not contains the action parameter 
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="payload"></param>
        /// <param name="type"></param>

        public void DidReceiveIncomingPush(PKPushRegistry registry, PKPushPayload payload, string type)
        {
            ShowCallKitUI(registry, payload, null);
        }
     
        /// <summary>
        /// methods fires when APNS gets from Server for showing the callkit UI when app in locked or killed state and fires in IOS which OS version is greater than 12
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="payload"></param>
        /// <param name="type"></param>
        /// <param name="completion"></param>
        [Export("pushRegistry:didReceiveIncomingPushWithPayload:forType:withCompletionHandler:")]
        public void DidReceiveIncomingPush(PKPushRegistry registry, PKPushPayload payload, string type, Action completion)
        {

            ShowCallKitUI(registry, payload, completion);

         
        }

        private void ShowCallKitUI(PKPushRegistry registry, PKPushPayload payload, Action completion)
        {
            try
            {

                CSCameraView.Classes.Helper.WriteLog("-----------------------------------------------------------------------------------------------------------------------------");
                CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush state.");
                var aps = payload.DictionaryPayload.ObjectForKey(new NSString("aps")) as NSDictionary;

                CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush state." + aps);
                Settings.IsLinePhoneCoreCallConnected = false;
                Settings.ISCallReceivedfromCallkitUI = false;
                if (aps != null)
                {


                    NSString alertKey = new NSString("alert");
                    var alert = aps.ObjectForKey(alertKey) as NSString;

                    NSString NotificationTypeKey = new NSString("NotificationType");
                    var NotificationType = aps.ObjectForKey(NotificationTypeKey) as NSString;
                    string Message = string.Empty;
                    if (NotificationType!=null)
                    {
                         Message = NotificationType.ToString();
                    }
                
                    if (!String.IsNullOrEmpty(Message) && Message.ToUpper().ToString()== "FORCELOGOUT")
                    {

                        App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName, Settings.Password, Settings.Domain);
                        Settings.UserName = string.Empty;
                        Settings.Password = string.Empty;
                        Settings.Domain = string.Empty;
                        Settings.RegistrationState = string.Empty;
                        Settings.IsUserLoggedIn = false;
                        Settings.NotificationToken = string.Empty;
                        Settings.UrlSelectionMode = string.Empty;

                        //  Navigation.PushAsync(new LoginPage());

                        Device.BeginInvokeOnMainThread(() => {

                            App.Current.MainPage = new NavigationPage(new Pages.LoginPage());
                        });

                    }
                    else
                    {
                        NSString fromPhNoKey = new NSString("fromPhNo");
                        var fromPhNo = aps.ObjectForKey(fromPhNoKey) as NSString;

                        NSString CallStatusKey = new NSString("callStatus");
                        var CallStatus = (aps.ObjectForKey(CallStatusKey) as NSString).ToString();

                        NSString videoUrlKey = new NSString("videoUrl");
                        Settings.videoUrl = aps.ObjectForKey(videoUrlKey) as NSString;


                        NSString autoTerminateCallKey = new NSString("autoTerminateCall");
                        NSNumber AutoTerminateCall = aps.ObjectForKey(autoTerminateCallKey) as NSNumber;
                        Settings.AutoTerminateCall = AutoTerminateCall.StringValue;


                        NSString dtMFDigitForDoorUnloackKey = new NSString("dtmfDigitForDoorUnlock");
                        NSNumber DTMFDigit = aps.ObjectForKey(dtMFDigitForDoorUnloackKey) as NSNumber;
                        Settings.DTMFDigit = DTMFDigit.StringValue;
                        CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush after getting apns response" + Settings.videoUrl + "\n" + Settings.AutoTerminateCall + "\n" + Settings.DTMFDigit);



                        Task.Run(() =>
                        {
                            App.DependencyInstance.InitializeLinphone(App.BaseContext);
                            CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush InitializeLinphone.");

                            App.DependencyInstance.StartLinphoneManager();
                            CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush after StartLinphoneManager.");

                            App.DependencyInstance.RegisterSIPUser(Settings.UserName, Settings.Password, Settings.Domain, false);
                            CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush after RegisterSIPUser");



                        }).Wait(5000);
                        //}

                        var update = new CXCallUpdate();

                        update.HasVideo = true;

                        update.LocalizedCallerName = fromPhNo;

                        update.SupportsDtmf = true;
                        update.SupportsHolding = false;
                        update.SupportsGrouping = false;
                        update.SupportsUngrouping = false;

                        var randomGuid = Guid.NewGuid();
                        string randomGuid1 = randomGuid.ToString();
                        activeCallUuid = new NSUuid(randomGuid1);
                        fromNO = fromPhNo;

                        //Handle the flag for when user is in active state need shows the callkit UI based on ,if APNs is not Received shows the UI based on CallState Method IncommingReceived Event
                        //or if APNS is received shows from here instead of events
                        CSCameraView.Classes.Helper.WriteLog("App entering ShowAnswerCallkitUI ReportIncomingCall Method with call and registration success object.");
                        AppDelegate.CallProviderDelegate.ReportIncomingCall(update, AppDelegate.activeCallUuid, AppDelegate.fromNO, "Test");

                        completion();
                    }

                   
                }
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering DidReceiveIncomingPush state." + ex.Message);
            }
        }

       

        public override void OnActivated(UIApplication application)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering WillEnterForeground state." + DateTime.Now + "IsCallConnected:" + Settings.IsLinePhoneCoreCallConnected + "ISCallReceivedfromCallkitUI:" + Settings.ISCallReceivedfromCallkitUI);
            
        }
        
        public override void WillEnterForeground(UIApplication application)
        {


        }


        public override void OnResignActivation(UIApplication application)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering OnResignActivation state.");
        }

        public override void DidEnterBackground(UIApplication application)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering DidEnterBackground state.");
        }

        // [not guaranteed that this will run]
        public override void WillTerminate(UIApplication application)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering WillTerminate state.");

        }
        void RegisterVoip()
        {
            var mainQueue = DispatchQueue.MainQueue;
            PKPushRegistry voipRegistry = new PKPushRegistry(mainQueue);
            voipRegistry.Delegate = this;
            voipRegistry.DesiredPushTypes = new NSSet(new string[] { PushKit.PKPushType.Voip });
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {

            CSCameraView.Classes.Helper.WriteLog("App entering TaskScheduler_UnobservedTaskException state." + e.Exception);
            CSCameraView.Classes.Helper.WriteLog("App entering TaskScheduler_UnobservedTaskException state." + e.Observed);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering TaskScheduler_UnobservedTaskException state." + e.ExceptionObject);

        }

    }


}
