using System;
using System.IO;
using CSCameraView;
using CSCameraView.Classes;
using CSCameraView.Dependency;
using System.Collections.Generic;
using System.Collections.Generic;

using Linphone;
using LinphoneShared;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;

[assembly: Dependency(typeof(LinphoneAccess))]
namespace LinphoneShared
{
    public class LinphoneAccess : ILinphoneAccess
    {

        public LinphoneManager Manager { get; set; }


        public Call CurrentRunningCall=new Call();

        public Core Core
        {
            get { return Manager.Core; }
        }

        #region Events
        public delegate void ActiveCallbackDelegate(bool successful);
        public delegate void ActiveCallStateChangedDelegate(LinphoneAccess call);



        public event ActiveCallStateChangedDelegate ConnectedChanged;

        #endregion


        public event Action OnIncomingCall;

        public event Action OnIncomingReceived;
        public event Action OnRegisterSuccess;
        public event Action ONUnRegister;
        public event Action OnRegisterFailed;
        public event Action OnCallTerminated;
        public event Action OnIOSRegisterSuccess;
        public event Action OnCoreCallTerminated;

        public event Action<string> OnIOSIncomingReceived;

        public bool IsManagerAvailable()
        {
            if (Manager != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InitializeLinphone(IntPtr context)
        {
            initLinphoneManager(context);
        }

        public void StopRinging()
        {
            Core.StopRinging();
        }
        
        public void RefreshSIPRegistration()
        {
            try
            {
                if (Core != null)
                {
                    Core.RefreshRegisters();
                }
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall:." + ex.StackTrace + "mESSAGE:\n" + ex.Message);
            }

        }

        /// <summary>
        /// This is the Actul Method which connects the LinePhone call object and allow to the user to cimmunicate each other
        /// </summary>
        public void AnswerCall()
        {
            try
            {
                if (Core != null)
                {
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall Core object exist:." + Core.Identity + " " + Core.InCallTimeout);

                    if (Core.CurrentCall != null)
                    {
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall after getting the Core current call object:." + Core.CurrentCall.State);
                        if (Core.CurrentCall.State == CallState.IncomingReceived)
                        {
                            Settings.IsLinePhoneCoreCallConnected = true;
                            Core.AcceptCall(Core.CurrentCall);
                        }
                    }
                    else
                    {
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall when Core.CurrentCall in null:.");
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall after getting the CurrentRunningCall from call State object:." + CurrentRunningCall.State);
                        if (CurrentRunningCall != null)
                        {
                            if (CurrentRunningCall.State == CallState.IncomingReceived)
                            {
                                Settings.IsLinePhoneCoreCallConnected = true;

                                Core.AcceptCall(CurrentRunningCall);
                            }
                        }
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall current  call allreday Disconnect from majic Jack:." + CurrentRunningCall.State);
                    }
                    if (Settings.IsVolumeBoosted)
                    {
                        Core.PlaybackGainDb = Core.PlaybackGainDb + 2;
                    }

                }
            }
            catch (Exception EX)
            {
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering AnswerCall:." + EX.StackTrace + "MESSAGE:\n" + EX.Message);
            }
        }

        /// <summary>
        ///1] Method calls only when after user connects the call press the allow entry or press the Do not allow Entry from CallProcess Page
        /// 2]when the user Call Timer is Finished it's automatically Disconnect
        /// </summary>

        public void TerminateCall()
        {

            try
            {
                
                if (Settings.IsVolumeBoosted)
                {
                    Core.PlaybackGainDb = Core.PlaybackGainDb - 2;
                }
                if (Core.CurrentCall != null)
                {
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CoreCurrentCall not null:.");
                    if (Core.CurrentCall.State == CallState.StreamsRunning)
                    {
                        CSCameraView.Classes.Helper.WriteLog($"LinphoneAccess:Jitter Status: {Core.AudioAdaptiveJittcompEnabled}");
                        Settings.CurrentCallState = Core.CurrentCall.State.ToString();
                        Core.TerminateCall(Core.CurrentCall);
                        Settings.CurrentCallState = Core.CurrentCall.State.ToString();
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CoreCurrentCall Sucessfully:.");
                        CurrentRunningCall = new Call();
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CoreCurrentCall null Cusuccessfully:.");

                    }

                }
                if (CurrentRunningCall != null)
                {
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CurrentRunningCall not null:.");
                    if (CurrentRunningCall.State == CallState.StreamsRunning)
                    {
                        Settings.CurrentCallState = CurrentRunningCall.State.ToString();
                        Core.TerminateCall(CurrentRunningCall);
                        Settings.CurrentCallState = CurrentRunningCall.State.ToString();
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CurrentRunningCall Sucessfully:.");
                        CurrentRunningCall = new Call();
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall CoreCurrentCall null Cusuccessfully:.");

                    }

                }
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering TerminateCall:."+ "\n"+ ex.StackTrace + "mESSAGE:\n" + ex.Message);
            }
        }

        /// <summary>
        /// if user terminate the Call Before call connects like from press Decline from System callkit UI or from Incoming Activity from Android
        /// </summary>
        //terminate the Call Before Connect
        public void DeclineCall()
        {
            if (Core.CurrentCall != null)
            {
                Core.TerminateCall(Core.CurrentCall);
                Settings.CurrentCallState = Core.CurrentCall.State.ToString();
                CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering DeclineCall successfully:.");
            }

        }
        public void BusyCall()
        {
            try
            {
                if (Core.CurrentCall != null)
                {
                    Core.DeclineCall(Core.CurrentCall, Reason.Busy);
                    Settings.CurrentCallState = Core.CurrentCall.State.ToString();
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering DeclineCall successfully:.");
                }
                if (CurrentRunningCall != null)
                {
                    Core.DeclineCall(CurrentRunningCall, Reason.Busy);
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering DeclineCall successfully:.");

                }
            }
            catch(Exception Ex)
            {

                CSCameraView.Classes.Helper.WriteLog("BusyCall:"+Ex.Message +"\n" +Ex.StackTrace);
            }
           


        }

        public void DialDTMF(sbyte KeyCode)
        {
            if (Core.CurrentCall.State == CallState.StreamsRunning)
            {
                Core.CurrentCall.SendDtmf(KeyCode);
            }
        }
        /// <summary>
        /// Methods fires from user press the allow entry in callProcees Page for Open the Get based on DTMF digit Value
        /// </summary>
        /// <param name="KeyCode"></param>
        /// <param name="Duration"></param>

        public void DialDTMF_2(sbyte KeyCode, int Duration)
        {
            try
            {
                Core.UseRfc2833ForDtmf = true;
                Core.UseInfoForDtmf = true;

                Core.StopDtmf();

                if (Core.CurrentCall.State == CallState.StreamsRunning)
                {
                    Core.StartDtmfStream();

                    Core.CurrentCall.SendDtmf(KeyCode);
                    Core.StopDtmf();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void PlayDTMF(sbyte KeyCode, int Duration)
        {
            Core.StartDtmfStream();
            Core.PlayDtmf(KeyCode, Duration);
        }

        #region Private Methods

        private void initLinphoneManager(IntPtr context)

        {
            string configFilePath = string.Empty;
            string factoryFilePath = string.Empty;
#if __ANDROID__
            Android.Content.Res.AssetManager assets = Android.App.Application.Context.Assets;
            string path = Android.App.Application.Context.FilesDir.AbsolutePath;
            configFilePath = path + "/default_rc";
            if (!File.Exists(configFilePath))
            {
                using (StreamReader sr = new StreamReader(assets.Open("linphonerc_default")))
                {
                    string content = sr.ReadToEnd();
                    File.WriteAllText(configFilePath, content);
                }
            }
            factoryFilePath = path + "/factory_rc";
            if (!File.Exists(factoryFilePath))
            {
                using (StreamReader sr = new StreamReader(assets.Open("linphonerc_factory")))
                {
                    string content = sr.ReadToEnd();
                    File.WriteAllText(factoryFilePath, content);
                }
            }
#endif

            Manager = new LinphoneManager();
            Manager.Init(configFilePath, factoryFilePath, context);
            Core.Listener.OnRegistrationStateChanged += OnRegistrationStateChanged;
            Core.Listener.OnCallStateChanged += OnCallStateChanged;
            Core.Listener.OnCallStatsUpdated += OnCallStatsUpdated;
            Core.Listener.OnLogCollectionUploadStateChanged += OnLogCollectionUploadStateChanged;
        }

        private void OnRegistrationStateChanged(Core lc, ProxyConfig cfg, RegistrationState cstate, string message)
        {

            CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnRegistrationStateChanged:.");
            switch (cstate)
            {
                case RegistrationState.Ok:
                    OnRegisterSuccess?.Invoke();
                    Settings.RegistrationState = RegistrationState.Ok.ToString();
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnRegistrationStateChanged:." + cstate);
                    break;
                case RegistrationState.Failed:
                    CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnRegistrationStateChanged:." + cstate);
                    OnRegisterFailed?.Invoke();
                    break;
                case RegistrationState.Cleared:

                    break;
                default:
                    break;
            }
        }



        private void OnLogCollectionUploadStateChanged(Core lc, CoreLogCollectionUploadState state, string info)
        {

        }

        private void OnCallStatsUpdated(Core lc, Call call, CallStats stats)
        {

           


        }


        private void OnCallStateChanged(Core lc, Call call, CallState cstate, string message)
        {
             int   Count = 0;
            if(Settings.ISPhoneBusy)
            {

                BusyCall();
            }
            else
            {
                switch (cstate)
                {
                    case CallState.IncomingReceived:
                        Settings.CurrentCallState = CallState.IncomingReceived.ToString();
                        Settings.CallerNumber = call.CallLog.FromAddress.Username;
                        CSCameraView.Classes.Helper.WriteLog("App entering OnCallStateChanged state." + DateTime.Now);
                        CSCameraView.Classes.Helper.WriteLog("Ringtone File: " + Core.Ring);

                        OnIncomingReceived?.Invoke();
                        CurrentRunningCall = lc.CurrentCall;

                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnCallStateChanged:." + cstate);
                        break;
                    case CallState.Connected:
                        Settings.CurrentCallState = CallState.Connected.ToString();
                        OnIncomingCall?.Invoke();
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnCallStateChanged:." + cstate);
                        break;

                    case CallState.End:
                        if ((Settings.CurrentCallState.Equals("Connected")) ||(Settings.CurrentCallState.Equals("StreamsRunning")) || (Settings.CurrentCallState.Equals("IncomingReceived")))
                        {
                        }
                        else
                        {
                            OnCallTerminated?.Invoke();
                            OnCoreCallTerminated?.Invoke();
                        }


                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnCallStateChanged End:." + Count++);
                        CSCameraView.Classes.Helper.WriteLog("LinphoneAccess:App entering OnCallStateChanged:." + cstate);
                        break;



                    default:
                        break;
                }

            }
          
             
        }

     
        public void  RegisterSIPUser(string username, string password, string domain,bool IsFirstRegistrationfromIOS)
        {
            username = username.Trim();
            password = password.Trim();

            string Result1 = string.Empty;
            
            try
            {
                if((!string.IsNullOrEmpty(username)) &&( !string.IsNullOrEmpty(password) )&& (!string.IsNullOrEmpty(domain)))
                {

                    CSCameraView.Classes.Helper.WriteLog("RegisterSIPUser." + username + "  \n " + password + " \n    " + domain);
                   
                        var authInfo = Factory.Instance.CreateAuthInfo(username, null, password, null, null, domain);
                        Core.AddAuthInfo(authInfo);
                        var proxyConfig = Core.CreateProxyConfig();
                     var identity = Factory.Instance.CreateAddress("sip:username@domain;transport=tcp");
                    identity.Username = username;
                        identity.Domain = domain;
                        identity.Transport = TransportType.Tcp;
                        proxyConfig.Edit();
                        proxyConfig.IdentityAddress = identity;
                        proxyConfig.ServerAddr = domain;
                        proxyConfig.Route = domain;
                        proxyConfig.RegisterEnabled = true;
                        proxyConfig.Done();
                        Core.AddProxyConfig(proxyConfig);
                        Core.DefaultProxyConfig = proxyConfig;
                    
                                         

                        Core.SessionExpiresValue = 3600;
                        Core.RefreshRegisters();
                        Core.SessionExpiresRefresherValue = SessionExpiresRefresher.UAC;

  

                }

            }
            catch(Exception Ex)
            {
                  App.Current.MainPage.DisplayAlert("Registration Failure", "Please check and retry again."+Ex.Message +"\n" + username , "OK");
                CSCameraView.Classes.Helper.WriteLog("Registration Failure:" + Ex.Message + "StackTrace::\n" + Ex.StackTrace);

            }
        }

        public void UnRegisterSIPUser123(string username, string password, string domain)
        {
            try
            {

                CSCameraView.Classes.Helper.WriteLog("UnRegisterSIPUser123." + username + "  \n " + password + " \n    " + domain);
                
                if(Core!=null && Core.DefaultProxyConfig!=null)
                {
                    var proxyConfig = Core.DefaultProxyConfig;
                  
                    proxyConfig.RegisterEnabled = false;

                    proxyConfig.Done();
                    Core.RemoveProxyConfig(proxyConfig);
                }

                else
                {

                }
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("UnRegisterSIPUser:" + ex.Message + "StackTrace::\n" + ex.StackTrace);
            }
        }

        


        public void StartLinphoneManager()
        {
            Manager.Start();
        }
        #endregion

    }
}
