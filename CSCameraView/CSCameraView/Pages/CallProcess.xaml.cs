using CSCameraView.Classes;
using CSCameraView.Dependency;
using CSCameraView.Models;
using CSCameraView.ViewModels;
using IncomingCall.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CSCameraView.Extensions;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallProcess : ContentPage
    {
        private bool IsClickAvailable;
        private TimeSpan timeout { get; set; }
        private bool timerrunning;
        System.Timers.Timer sendDTMFBeforeCallDisconnectTimer=new Timer();

        System.Timers.Timer CheckVideoUrlagaintimer = new Timer();
        private static int ApiResponseProcessingCounter = 0;
        public string Timeout { get; set; }


        #region Constructor

        //In IOS Call  user connect the call from callkit ProviderDelegates and then only need the shows the UI of Ur Media Player and buttons
        public CallProcess()
        {

            try
            {
                InitializeComponent();
                InitializeLinphoneEvents();
                IsClickAvailable = true;
                InitializeWebView();
                Linphone_OnIncomingCall();

                if (string.IsNullOrEmpty(Settings.videoUrl))
                {
                    CheckVideoUrlagaintimer = new Timer(5000);
                    CheckVideoUrlagaintimer.Start();
                    CheckVideoUrlagaintimer.Elapsed += CheckVideoUrlagaintimer_Elapsed;
                }

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering Callprocees pagefor android to connect the Page from AnswerCall Method");
            }
        }

        //temporary constructor for testing purposes
        public CallProcess(string param)
        {
            try
            {
                InitializeComponent();
                videoWebView.IsVisible = true;
                lblVideoUrlMessage.IsVisible = false;

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("Testing Constructor error");
            }
        }

        public CallProcess(bool ISRootTast)
        {
            IsClickAvailable = true;
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering Callprocees page from AnswerCall Method");
                InitializeComponent();
                Xamarin.Forms.DependencyService.Get<INativeHelper>().Speaker();
                App.DependencyInstance.AnswerCall();
                InitializeLinphoneEvents();
                InitializeWebView();
                Linphone_OnIncomingCall();


                if (string.IsNullOrEmpty(Settings.videoUrl))
                {
                    CheckVideoUrlagaintimer = new Timer(5000);
                    CheckVideoUrlagaintimer.Start();
                    CheckVideoUrlagaintimer.Elapsed += CheckVideoUrlagaintimer_Elapsed;
                }

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering CallProcess Constructor" + ex.Message);

            }
        }

        #endregion

        #region Overrides
        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                if (Settings.IsVideoPortrait)
                {
                    //set the video webview height and width to fit 3:4 aspect ratio
                    videoWebView.WidthRequest = width;
                    videoWebView.HeightRequest = width * 1.34;
                }
                else
                {
                    //set the video webview height and width to fit 4:3 aspect ratio
                    videoWebView.WidthRequest = width;
                    videoWebView.HeightRequest = width * 0.75;
                }

                base.OnSizeAllocated(width, height);

            }
            catch (Exception ex)
            {

            }
            
        }
        #endregion

        #region Private Methods

        private async void CheckVideoUrlagaintimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ApiResponseProcessingCounter += 5000;
                if (ApiResponseProcessingCounter >= 30000)
                {
                    videoWebView.IsVisible = false;
                    lblVideoUrlMessage.Text = "Video Unavailable";
                    lblVideoUrlMessage.TextColor = Color.White;
                    lblVideoUrlMessage.IsVisible = true;
                }
                if (!string.IsNullOrEmpty(Settings.videoUrl))
                {
                    CheckVideoUrlagaintimer.Stop();
                }
                else
                {
                    CheckVideoUrlagaintimer.Stop();
                    WebAPI webAPI = new WebAPI();
                    string CallerNumber = Settings.CallerNumber.Replace("+", "").Trim();
                    SesCallerModel sesCallerModel = await webAPI.GetCallerDetailsByNumber(CallerNumber);
                    if (sesCallerModel != null)
                    {
                        CheckVideoUrlagaintimer.Stop();
                        Settings.videoUrl = sesCallerModel.videoURL;
                        Settings.AutoTerminateCall = sesCallerModel.autoTerminateCall.ToString();
                        Settings.DTMFDigit = sesCallerModel.dtmfDigitForDoorUnloack.ToString();
                    }
                    else
                    {
                        CheckVideoUrlagaintimer.Start();
                    }
                }
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App:App entering CheckVideoUrlagaintimer_Elapsed:." + Ex.StackTrace);

            }

        }

        private void InitializeWebView()
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.videoUrl))
                {
                    CSCameraView.Classes.Helper.WriteLog($"Video URL: {Settings.videoUrl}");
                    videoWebView.Source = new Uri(Settings.videoUrl);
                    videoWebView.IsVisible = true;
                    lblVideoUrlMessage.IsVisible = false;
                }               
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("Initialize WebView." + ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        
        private void InitializeLinphoneEvents()
        {
            App.DependencyInstance.OnCallTerminated -= Linphone_OnCallTerminated;
            App.DependencyInstance.OnCallTerminated += Linphone_OnCallTerminated;
        }

        private async void AllowEntry(CallProcessViewModel callProcessViewModel)
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked");
                btnAllowEntry.BackgroundColor = Color.GreenYellow;
                if (!String.IsNullOrEmpty(Settings.DTMFDigit))
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked with DTMF not NUll");
                    char textChar = Convert.ToChar(Settings.DTMFDigit);

                    App.DependencyInstance.PlayDTMF(Convert.ToSByte(textChar), 1000);

                    for (int i = 0; i < 15; i++)
                    {
                        App.DependencyInstance.DialDTMF_2(Convert.ToSByte(textChar), 100);
                        await Task.Delay(200);
                    }
                }

            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
            }
            finally
            {

                if (Device.RuntimePlatform == Device.Android)
                {
                    sendDTMFBeforeCallDisconnectTimer = new Timer(3000);
                    sendDTMFBeforeCallDisconnectTimer.Start();
                    sendDTMFBeforeCallDisconnectTimer.Elapsed += sendDTMFBeforeCallDisconnectTimer_Elapsed;

                }
                else
                {
                    Task.Delay(3000);
                    DisConnectCall();
                }

            }
        }

        private async void DenyEntry(CallProcessViewModel callProcessViewModel)
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering btnDonotallowEntry_Clicked");

                btnDonotallowEntry.BackgroundColor = Color.OrangeRed;

                char textChar = Convert.ToChar("0");

                App.DependencyInstance.PlayDTMF(Convert.ToSByte(textChar), 1000);
                for (int i = 0; i < 15; i++)
                {
                    App.DependencyInstance.DialDTMF_2(Convert.ToSByte(textChar), 100);
                    await Task.Delay(200);
                }
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering btnDonotallowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
            }
            finally
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    sendDTMFBeforeCallDisconnectTimer = new Timer(3000);
                    sendDTMFBeforeCallDisconnectTimer.Start();
                    sendDTMFBeforeCallDisconnectTimer.Elapsed += sendDTMFBeforeCallDisconnectTimer_Elapsed;

                }
                else
                {
                    Task.Delay(3000);
                    DisConnectCall();
                }
            }
        }
        #region LinphoneEvents
        /// <summary>
        /// if the call is terminated by system and App Current Page in CallInProcess App Fires this Event 
        /// </summary>
        private async void Linphone_OnCallTerminated()
        {
            try
                  
            {
                //StopMedia();
                ApiResponseProcessingCounter = 0;
                              
            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering Linphone_OnCallTerminated state." + ex.Message);
            }

        }

        /// <summary>
        /// if Core Object connects the Call ans call status in Connected need to show the KEyPad and MediaPlayer App fires this Event
        /// </summary>
        private void Linphone_OnIncomingCall()
        {
            try
            {
                StartTimer();

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering DidActivateAudioSession state." + ex.Message);
            }
        }

        internal void ResetTimer()
        {
            timerrunning = true;
            if (!string.IsNullOrEmpty(Settings.AutoTerminateCall))
            {
                Double AutoTerminateCallSec = Convert.ToDouble(Settings.AutoTerminateCall);
                timeout = TimeSpan.FromSeconds(AutoTerminateCallSec);
            }
            else
            {
                timeout = TimeSpan.FromSeconds(55);
            }


            UpdateTimerUI();
        }

        internal void StartTimer()
        {
            ResetTimer();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateTimerUI();
                CheckTimeout();
                return timerrunning;
            });
        }

        internal void CheckTimeout()
        {
            if (timeout <= new TimeSpan())
            {
                StopTimer();
            }
        }

        internal void StopTimer()
        {
         
             if(timerrunning)
            {
                timerrunning = false;
                DisConnectCall();
            }
      

        }

        internal void UpdateTimerUI()
        {

            timeout = timeout.Subtract(TimeSpan.FromSeconds(1));
            lblTimeCounter.Text = timeout.ToString(@"mm\:ss");

        }
        #endregion

        /// <summary>
        /// if Call is terminated by Receiver Decline Button Click in Call in Process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        public void DisConnectCall()
        {
            try
            {
                
                CSCameraView.Classes.Helper.WriteLog("App entering Disconnect function");
                    timerrunning = false;

                    ApiResponseProcessingCounter = 0;
                   
                    Settings.IsLinePhoneCoreCallConnected = false;
                    Settings.ISCallReceivedfromCallkitUI = false;
                    Settings.AutoTerminateCall = string.Empty;
                    Settings.videoUrl = String.Empty;
                    Settings.DTMFDigit = string.Empty;
                    Settings.CurrentCallState = String.Empty;


                    Page currentPage = Navigation.NavigationStack.LastOrDefault();
                    if (currentPage != null && currentPage is Pages.CallWaitPage)
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering Current Page as CallwaitPage");
                        return;
                    }
                    else
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering set the Current Page as Callwait Page");

                        Device.BeginInvokeOnMainThread(() => {

                            //remove activity flags from MainActivity on android. before navigating to call wait page.
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                DependencyService.Get<IAndroidActivityFlagsHelper>().clearFlags();
                            }
                            App.Current.MainPage = new NavigationPage(new Pages.CallWaitPage());
                        });
                        

                    }

                    //No Need to set the flag again ,because we set this flag on CallTerminate Event
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        App.DependencyInstance.TerminateCall();
                        DependencyService.Get<INativeHelper>().SetSystemDefaultVolume(Settings.SystemDefaultMusicVolume, Settings.SystemDefaultVoiceCallVolume);
                    }
                    else
                    {
                        App.DependencySystemCallInstance.TerminateCall();
                        App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName, Settings.Password, Settings.Domain);
                        App.DependencyInstance.TerminateCall();
                    }
                sendDTMFBeforeCallDisconnectTimer.Stop();

                CheckVideoUrlagaintimer.Stop();

            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering DidActivateAudioSession state." + Ex.Message);
            }
        }

        private void ChangePassword_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }



        private async void btnAllowEntry_Clicked(object sender, EventArgs e)
        {
            if (IsClickAvailable)
            {
                IsClickAvailable = false;
                try
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked");
                    btnAllowEntry.BackgroundColor = Color.GreenYellow;
                    if (!String.IsNullOrEmpty(Settings.DTMFDigit))
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked with DTMF not NUll");
                        char textChar = Convert.ToChar(Settings.DTMFDigit);


                        App.DependencyInstance.PlayDTMF(Convert.ToSByte(textChar), 1000);

                        for (int i = 0; i < 15; i++)
                        {
                            App.DependencyInstance.DialDTMF_2(Convert.ToSByte(textChar), 100);
                            await Task.Delay(200);
                        }

                    }

                }
                catch (Exception Ex)
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering btnAllowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
                }
                finally
                {

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        sendDTMFBeforeCallDisconnectTimer = new Timer(3000);
                        sendDTMFBeforeCallDisconnectTimer.Start();
                        sendDTMFBeforeCallDisconnectTimer.Elapsed += sendDTMFBeforeCallDisconnectTimer_Elapsed;

                    }
                    else
                    {
                        Task.Delay(3000);
                        DisConnectCall();
                    }

                }
            }           
        }

        private async void btnDonotallowEntry_Clicked(object sender, EventArgs e)
        {
            if (IsClickAvailable)
            {
                IsClickAvailable = false;
                try
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering btnDonotallowEntry_Clicked");

                    btnDonotallowEntry.BackgroundColor = Color.OrangeRed;

                    char textChar = Convert.ToChar("0");

                    App.DependencyInstance.PlayDTMF(Convert.ToSByte(textChar), 1000);
                    for (int i = 0; i < 15; i++)
                    {
                        App.DependencyInstance.DialDTMF_2(Convert.ToSByte(textChar), 100);
                        await Task.Delay(200);
                    }
                }
                catch (Exception Ex)
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering btnDonotallowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
                }
                finally
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        sendDTMFBeforeCallDisconnectTimer = new Timer(3000);
                        sendDTMFBeforeCallDisconnectTimer.Start();
                        sendDTMFBeforeCallDisconnectTimer.Elapsed += sendDTMFBeforeCallDisconnectTimer_Elapsed;

                    }
                    else
                    {
                        Task.Delay(3000);
                        DisConnectCall();
                    }
                }
            }           
        }


        private void sendDTMFBeforeCallDisconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sendDTMFBeforeCallDisconnectTimer.Stop();
            Device.BeginInvokeOnMainThread(() =>
            {
                DisConnectCall();
            });

        }
        #endregion


        public void InitializeMessageCenterMethods()
        {
            try
            {
                MessagingCenter.Subscribe<CallProcessViewModel>(this, "AllowEntry", AllowEntry);
                MessagingCenter.Subscribe<CallProcessViewModel>(this, "DenyEntry", DenyEntry);

            }
            catch (Exception ex)
            {
                DisplayAlert("Error - DashboardPage", ex.Message, "OK");
            }
        }


        private async Task DependencyDTMF(char input, int seconds = 2)
        {
            for (int i = 0; i < seconds; i++)
            {
                await Task.Delay(1000);

            }
            
        }
    }
}