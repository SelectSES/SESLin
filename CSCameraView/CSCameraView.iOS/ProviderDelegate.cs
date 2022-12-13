using CallKit;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;
using IncomingCall.Dependency;
using LinphoneShared;
using CSCameraView.Dependency;
using AVFoundation;
using CSCameraView.Classes;
using System.Timers;

namespace CSCameraView.iOS
{

    public class ProviderDelegate : CXProviderDelegate
    {
        #region Computed Properties
     public  ActiveCallManager CallManager { get; set; }
        public CXProviderConfiguration Configuration { get; set; }
        public static CXProvider Provider { get; set; }

        public CXCallUpdate callupdate { get; set; }

        System.Timers.Timer t2;
        #endregion

        #region Constructors
        public ProviderDelegate(ActiveCallManager callManager)
        {
            // Save connection to call manager
           CallManager = callManager;

            // Define handle types
            var handleTypes = new[] { (NSNumber)(int)CXHandleType.Generic };

            // Get Image Template
           var templateImage = UIImage.FromFile("endcall.png");
          
            // Setup the initial configurations
            Configuration = new CXProviderConfiguration("CSCameraOperation")
            {
                MaximumCallsPerCallGroup = 1,
                SupportedHandleTypes = new NSSet<NSNumber>(),
                IconTemplateImageData = templateImage.AsPNG(),
                SupportsVideo = true,
               RingtoneSound = "musicloop01.wav",
               
            };

            // Create a new provider
            Provider = new CXProvider(Configuration);


            ////Attach this delegate
            Provider.SetDelegate(this, null);

        }
        #endregion

        #region Override Methods
        //TODO:not fires ever need to checek,handling calls clear directly in call end action 
        public override void DidReset(CXProvider provider)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering DidReset state.");
            CallManager.Calls.Clear();
        }


        public override void DidBegin(CXProvider provider)
        {
            CSCameraView.Classes.Helper.WriteLog("App entering DidBegin state.");
        }
        /// <summary>
        /// The Method which actul connects the Call 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="action"></param>

        public override void PerformAnswerCallAction(CXProvider provider, CXAnswerCallAction action)
        {
            try
            {
                Settings.ISCallReceivedfromCallkitUI = true;
                t2.Stop();
                CSCameraView.Classes.Helper.WriteLog("App entering PerformAnswerCallAction state. "+ DateTime.Now);
                    //Find requested call
                    var call = CallManager.FindCall(action.CallUuid);

                    // Found?
                    if (call == null)
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering PerformAnswerCallAction with call status null. " + DateTime.Now);
                        // No, inform system and exit
                        action.Fail();
                        return;
                    }
                    else
                    {
                       //need to enable the Audio in background before call connect or answer the call 
                        call.configureAudioSession((successful) =>
                        {
                            // Was the call successfully answered?
                            if (successful)
                            {
                                CSCameraView.Classes.Helper.WriteLog("App entering configureAudioSession method  Success to inform the System." + DateTime.Now);
                                // Yes, inform system
                                action.Fulfill();
                            }
                            else
                            {
                                // No, inform system
                                action.Fail();
                            }
                        });


                        // Attempt to answer call, connect the system and call 
                        call.AnswerCall((successful) =>
                        {
                            // Was the call successfully answered?
                            if (successful)
                            {
                                CSCameraView.Classes.Helper.WriteLog("App entering PerformAnswerCallAction Success to inform the System.  " + DateTime.Now);
                                // Yes, inform system
                                action.Fulfill();
                            }
                            else
                            {
                                // No, inform system
                                action.Fail();
                            }
                        });
                    }
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering PerformAnswerCallAction:: "   +Ex.Message);
            }
           
        }
        /// <summary>
        /// Method fires after Perform Callkit Call Action and Active the Audio
        /// Enables the System Audio when App is in Locked state,and mix with configure audio method
        /// if user Receive the call from locked state then it shows the system callkit window at that time we need to connect the Syetem Audio insted on Custom Audio
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="audioSession"></param>

        public override void DidActivateAudioSession(CXProvider provider, AVFoundation.AVAudioSession audioSession)
        {
            // Start the calls audio session here
            try
            {
                NSError err;
                CSCameraView.Classes.Helper.WriteLog("App entering DidActivateAudioSession state.");
                var session = audioSession;
                session.SetCategory(AVAudioSessionCategory.PlayAndRecord, AVAudioSessionCategoryOptions.MixWithOthers);
                audioSession.SetMode(AVAudioSession.ModeVoiceChat, out err);
                session.SetActive(true);
                session.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out err);
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering DidActivateAudioSession state." + Ex.Message);
            }
        }
        /// <summary>
        /// Fires Automatically when you Discoonect the Callkit call using EndCall function
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="audioSession"></param>

        public override void DidDeactivateAudioSession(CXProvider provider, AVFoundation.AVAudioSession audioSession)
        {
            NSError err;
            var session = audioSession;
            session.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetMode(AVAudioSession.ModeVoiceChat, out err);
            session.SetActive(false);

            CSCameraView.Classes.Helper.WriteLog("App entering DidDeactivateAudioSession state.");
            // End the calls audio session and restart any non-call
            // related audio
        }

        /// <summary>
        /// fire Situations
        /// fires when user decline click from callkit Decline button action
        /// fires from when user performes the decline action after call finished ,from caller and receiver side
        /// override from  CallManager for if user Decline the system call need to end the System call also 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="action"></param>
       
        public override void PerformEndCallAction(CXProvider provider, CXEndCallAction action)
        {

            try
            {
                t2.Stop();
                CSCameraView.Classes.Helper.WriteLog("App entering PerformEndCallAction state.");
               
                //   Find requested call
                var call = CallManager.FindCall(action.CallUuid);
            
                Settings.CurrentCallState = String.Empty;
                // Found?
                if (call == null)
                {
                    CSCameraView.Classes.Helper.WriteLog("App entering PerformEndCallAction call is null state.");
                    
                    // No, inform system and exi
                    action.Fail();
                  return;
                }

                // // Attempt to answer call
                call.EndCall((successful) =>
                {
                    // Was the call successfully answered?
                    if (successful)
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering PerformEndCallAction call find and trying to remove from callmanager.");
                        // Remove call from manager's queue
                        CallManager.Calls.Remove(call);
                        CallManager.Calls.Clear();
                        // ActiveCallManager.Calls.Remove(call);

                        //Call the Decline Method if Receiver Press the Decline without connect the call then need to inform the core call object call is discoonect the Receiver without connect
                        App.DependencyInstance.DeclineCall();
                        App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName,Settings.Password,Settings.Domain);
                        // Yes, inform system
                        action.Fulfill();
                    }
                    else
                    {
                        CSCameraView.Classes.Helper.WriteLog("App entering PerformEndCallAction call fail action");
                        // No, inform system
                        action.Fail();
                    }
                });

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering PerformEndCallAction state." + ex.Message);
            }
        }

        /// <summary>
        /// The Method which Shows the Callkit UI ,means Reports the user that call is Coming ant any state link locked and App is in Background or killed state
        /// </summary>
        /// <param name="update"></param>
        /// <param name="uuid"></param>
        /// <param name="handle"></param>
        /// <param name="callstatus"></param>
        public void ReportIncomingCall(CXCallUpdate update, NSUuid uuid, string handle,string callstatus)
        {
            try
            { 
                CSCameraView.Classes.Helper.WriteLog("App entering ProviderDelegates ReportIncomingCall state.");
                    CallManager.Calls.Clear();
                        CSCameraView.Classes.Helper.WriteLog("App entering ProviderDelegates ReportIncomingCall after reg state ok and callstate connected find");
                        Provider.ReportNewIncomingCall(uuid, update, (error) =>
                            {
                                //Was the call accepted
                                if (error == null)
                                {
                                    //Yes, report to call manager
                                    CallManager.Calls.Add(new ActiveCall(uuid, handle, callstatus));
                                }
                                else
                                {
                                    //Report error to user here
                                    Console.WriteLine("Error: {0}", error);
                                }
                            });

                t2 = new Timer(60000);
                t2.Start();

                t2.Elapsed += Timer2_Elapsed;
            }
            catch (Exception EX)
            {
                CSCameraView.Classes.Helper.WriteLog("App entering ReportIncomingCall state."+ EX.Message +"Stacktrace:\n" + EX.StackTrace);
            }
        }

        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                t2.Stop();
                if (!Settings.ISCallReceivedfromCallkitUI)
                {
                    
                    CXCallEndedReason cXCallEndedReason = CXCallEndedReason.Unanswered;
                    Provider.ReportCall(AppDelegate.activeCallUuid, null, cXCallEndedReason);
                }
            }
            catch(Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("Timer2_Elapsed" + Ex.Message + "Stacktrace:\n" + Ex.StackTrace);
            }
           
        }

        
        #endregion
    }
}
