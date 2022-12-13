using Acr.UserDialogs;
using CSCameraView.Classes;
using CSCameraView.Dependency;
using CSCameraView.Models;
using IncomingCall.Dependency;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace CSCameraView
{
    public partial class App : Application
    {
        public static string ConfigFilePath { get; set; }
        public static string FactoryFilePath { get; set; }

        public static ILinphoneAccess DependencyInstance => DependencyService.Get<ILinphoneAccess>(DependencyFetchTarget.GlobalInstance);

        public static ISystemCallHelper DependencySystemCallInstance => DependencyService.Get<ISystemCallHelper>(DependencyFetchTarget.GlobalInstance);

        public static IAndroidService DependencyInstanceAndroid => DependencyService.Get<IAndroidService>(DependencyFetchTarget.GlobalInstance);

        public static IntPtr BaseContext;

        public App(IntPtr context)
        {

            try
            {
                InitializeComponent();
                SubscribeToAppEvent();
                InitializeLinphoneEvents();
                 BaseContext = context;
                if(Device.RuntimePlatform==Device.iOS)
                {
                    DependencyInstance.InitializeLinphone(context);
                    DependencyService.Get<ILinphoneAccess>().StartLinphoneManager();

                }
                SetStartUpPage();
                CheckPermissions();
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingCall:." + Ex.Message);
            }
            

        }
      
        protected   void SetStartUpPage()
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering SetStartUpPage Method." + DateTime.Now);
             
                //if user login first time fo to if
                
                if ((string.IsNullOrEmpty(Settings.UserName)) || (string.IsNullOrEmpty(Settings.Password)))
                {
                    Application.Current.MainPage = new NavigationPage(new Pages.LoginPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new Pages.AuthenticationPage());
                }
            }
            catch (Exception ex)
            {
                MainPage.DisplayAlert("Error - SetStartUpPage", ex.Message, "OK");
            }
        }

        public async Task<Object> GetCurrentUserDetails(Users users)
        {
            try
            {
                WebAPI webApi = new WebAPI();
               var usertoken1 = await webApi.AccountLoginV1(users).ConfigureAwait(true);
                return usertoken1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeLinphoneEvents()
        {
            App.DependencyInstance.OnIncomingReceived -= DependencyInstance_OnIncomingReceived;
            App.DependencyInstance.OnIncomingCall -= DependencyInstance_OnIncomingCall;
            App.DependencyInstance.OnCoreCallTerminated -= DependencyInstance_OnCoreCallTerminated;


            App.DependencyInstance.OnIncomingReceived += DependencyInstance_OnIncomingReceived;
            App.DependencyInstance.OnIncomingCall += DependencyInstance_OnIncomingCall;
            App.DependencyInstance.OnCoreCallTerminated += DependencyInstance_OnCoreCallTerminated;

        }

        /// <summary>
        /// Execute only in Android
        /// when user clicks the Answer from IncomingActivity Need to connect  the Call and shows the call Process Page
        /// so after ans click user navigates directly on call proecess page and from callprocess constructor connect the linephone call and call speaker
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ISTaskRoot"></param>
       
        public App(IntPtr context, bool ISTaskRoot)
        {
            try
            {
                InitializeComponent();
                InitializeLinphoneEvents();             
                MainPage = new NavigationPage(new Pages.CallProcess(false));
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App:App entering open the App after start  Mainactivity from  incomingactivity after answer the call " + ex.Message + "StackTrace\n" + ex.StackTrace);

            }
        
        }

        public App(string page)
        {
            if (page == "CallHistoryPage")
            {
                MainPage = new NavigationPage(new Pages.CallHistoryPage());
            }
            
        }

        private void SubscribeToAppEvent()
            {

                Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
           

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Opened");
                    foreach (var data in p.Data)
                    {
                        if (data.Key == "NotificationType")
                        {
                            if (data.Value.ToString() == "MISSEDCALL")
                            {
                                MainPage = new NavigationPage(new Pages.CallHistoryPage());
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(p.Identifier))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {

                        });
                    }
                    else if (p.Data.ContainsKey("color"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                        });

                    }
                    else if (p.Data.ContainsKey("aps.alert.title"))
                    {

                    }
            };
        }
        /// <summary>
        /// if call is disconnect without Receiving or from majic jack ,and user not in callprocess page need to close the System call UI
        /// Close Activity In Android and Callkit UI In IOS and clear call Properties
        /// </summary>
       
     
        private async void DependencyInstance_OnIncomingReceived()
        {            
            try
            {
                CSCameraView.Classes.Helper.WriteLog("----------------------------------------------------------------------");
                CSCameraView.Classes.Helper.WriteLog("start the incoming Received block" + DateTime.Now);

                if (Device.RuntimePlatform == Device.iOS)
                {
                   
                    //This is only for IOs Because when user Accept the Call within Sec before getting the Core Call object call not able to connect from ProviderDelegate
                    //then Answer()  Need to call for  connect the call again based on  getting Incoming Call Received Event  from Core Call Object
                    CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingReceived:." + "IsCallConnected :" + Settings.IsLinePhoneCoreCallConnected + " \n" + "ISCallReceivedfromCallkitUI: " + Settings.ISCallReceivedfromCallkitUI);
                    if (!Settings.IsLinePhoneCoreCallConnected && Settings.ISCallReceivedfromCallkitUI)
                    {

                        Xamarin.Forms.DependencyService.Get<INativeHelper>().Speaker();
                        App.DependencyInstance.AnswerCall();


                    }
                }
                if (Device.RuntimePlatform == Device.Android)
                {

                   var tuple = DependencyService.Get<INativeHelper>().GetSystemDefaultVolume();
                    Settings.SystemDefaultMusicVolume = tuple.Item1;
                    Settings.SystemDefaultVoiceCallVolume = tuple.Item2;
                   
                    App.DependencySystemCallInstance.ShowAnswerCallUI();
                    CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingReceived with CallNumber Call"+DateTime.Now);
                    if (!string.IsNullOrEmpty(Settings.CallerNumber))
                    {
                        CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingReceived with CallerNumber not equals null"+DateTime.Now);
                        WebAPI webAPI = new WebAPI();
                        string CallerNumber = Settings.CallerNumber.Replace("+", "").Trim();
                        SesCallerModel sesCallerModel =await  webAPI.GetCallerDetailsByNumber(CallerNumber);
                        if (sesCallerModel != null)
                        {
                            CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingReceived with all details fetch from Api"+DateTime.Now);

                            Settings.videoUrl = sesCallerModel.videoURL;
                            Settings.AutoTerminateCall = sesCallerModel.autoTerminateCall.ToString();
                            Settings.DTMFDigit = sesCallerModel.dtmfDigitForDoorUnloack.ToString();

                        }
                    }
                }
            }
            catch(Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App:App entering DependencyInstance_OnIncomingReceived:." + Ex.StackTrace);

            }
        }
        private void DependencyInstance_OnIncomingCall()
        {

            //Whan app enters in Active State or allreday active navigate the CallProcess Page only when callstate is connect in LinePhone Access
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering DependencyInstance_OnIncomingCall Method." + DateTime.Now);
                if (Device.RuntimePlatform == Device.Android)
                {

                }
                if (Device.RuntimePlatform == Device.iOS)
                {

                        MainPage = new NavigationPage(new Pages.CallProcess());
                }

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("DependencyInstance_OnIncomingCall Error :App entering DependencyInstance_OnIncomingCall:." + ex.Message);
            }
        }
        /// <summary>
        /// if user disconnect the call from majic jack and call receiver still in Callkit system  UI window then need to close the system window and clear all the object 
        /// </summary>
        private   void DependencyInstance_OnCoreCallTerminated()
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("DependencyInstance_OnCoreCallTerminated:" + DateTime.Now);
                Settings.IsLinePhoneCoreCallConnected = false;
                Settings.ISCallReceivedfromCallkitUI = false;
                Settings.CurrentCallState = String.Empty;
                Settings.AutoTerminateCall = string.Empty;
                Settings.videoUrl = String.Empty;
                Settings.DTMFDigit = string.Empty;
                Settings.CallerNumber = string.Empty;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    App.DependencySystemCallInstance.TerminateCall();
                    App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName, Settings.Password, Settings.Domain);
                }
                if (((NavigationPage)Application.Current.MainPage).CurrentPage is Pages.CallWaitPage)
                {
                    return;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => {

                        Application.Current.MainPage = new NavigationPage(new Pages.CallWaitPage());
                    });
                  

                }

            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("DependencyInstance_OnCoreCallTerminated:" + Ex.StackTrace);
            }
        }

        private async void Current_OnTokenRefresh(object source, Plugin.FirebasePushNotification.Abstractions.FirebasePushNotificationTokenEventArgs e)
        {
            var isDone = false;
            if (!string.IsNullOrWhiteSpace(e.Token))
            {
               var Token = e.Token;
            }
        }

        protected  override void OnStart()
        {

         
        }

        protected override void OnSleep()
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnSleep:.");
            }
            catch (Exception Ex)
            {

            }           
  
        }

        protected override void OnResume()
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnResume:");
            }
            catch (Exception Ex)
            {

            }
                     
        }


        public async void CheckPermissions()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Phone>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Phone>();
                }
                status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Microphone>();
                }
            }
            catch (Exception ex)
            {


            }
        }

        
    }
}
