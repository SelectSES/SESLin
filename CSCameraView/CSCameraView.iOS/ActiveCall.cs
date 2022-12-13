using AVFoundation;
using CoreFoundation;
using CSCameraView.Classes;
using Foundation;
using IncomingCall.Dependency;
using Linphone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace CSCameraView.iOS
{
  
    public class ActiveCall
    {
        #region Private Variables
        public bool isConnecting;
        public bool isConnected;
        public bool isOnhold;
        #endregion

        #region Computed Properties
       public NSUuid UUID { get; set; }

  
        public bool isOutgoing { get; set; }
        public string Handle { get; set; }

        

        public string CallStatus { get; set; }
        public DateTime StartedConnectingOn { get; set; }
        public DateTime ConnectedOn { get; set; }
        public DateTime EndedOn { get; set; }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                isConnecting = value;
                if (isConnecting) StartedConnectingOn = DateTime.Now;
                RaiseStartingConnectionChanged();
            }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                if (isConnected)
                {
                    ConnectedOn = DateTime.Now;

                }
                else
                {
                    EndedOn = DateTime.Now;
                }
                RaiseConnectedChanged();
            }
        }

        public bool IsOnHold
        {
            get { return isOnhold; }
            set
            {
                isOnhold = value;
            }
        }
        #endregion

        #region Constructors
        public ActiveCall()
        {
        }
        public ActiveCall(NSUuid uuid)
        {
            this.UUID = uuid;
        }
        public ActiveCall(NSUuid uuid, string handle, bool outgoing)
        {
            // Initialize
            this.UUID = uuid;
            this.Handle = handle;
            this.isOutgoing = outgoing;
        }

        public ActiveCall(NSUuid uuid, string handle,string callstatus)
        {
            // Initialize
            this.UUID = uuid;
            this.Handle = handle;
            this.CallStatus = callstatus;
        }

      
        #endregion

        #region Public Methods
        public void StartCall(ActiveCallbackDelegate completionHandler)
        {
            // Simulate the call starting successfully
            completionHandler(true);

            // Simulate making a starting and completing a connection
            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 3000), () => {
                // Note that the call is starting
                IsConnecting = true;

                // Simulate pause before connecting
                DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 1500), () => {
                    // Note that the call has connected
                    IsConnecting = false;
                    IsConnected = true;
                });
            });
        }

        /// <summary>
        /// Connect the LinePhone Call and System Call
        /// </summary>
        /// <param name="completionHandler"></param>
        public void AnswerCall(ActiveCallbackDelegate completionHandler)
        {
          
            CSCameraView.Classes.Helper.WriteLog("ActiveCall:App entering AnswerCall state." + DateTime.Now);
            App.DependencyInstance.AnswerCall();
            App.DependencySystemCallInstance.AcceptCall();
            IsConnected = true;
            completionHandler(true);
            
        }
        /// <summary>
        /// 1]This Method is create for when app is in Background the System DidConfigureAudio function or Method Not fires that time Need to Enables the system Background Audio 
        /// 2]Also when App is in Forground and Killed ,lock state the ConfigureAudio function is fires that time Need to mix that Background Audio with System Callkit Configure Audio
        /// </summary>
        /// <param name="completionHandler"></param>
        public void configureAudioSession(ActiveCallbackDelegate completionHandler)
        {
            EnableBackgroundAudio();
            CSCameraView.Classes.Helper.WriteLog("ActiveCall:configureAudioSession state.");
           
            completionHandler(true);


        }
        private void EnableBackgroundAudio()
        {
            CSCameraView.Classes.Helper.WriteLog("App entering EnableBackgroundAudio state.");
            var currentSession = AVAudioSession.SharedInstance();
            currentSession.SetCategory(AVAudioSessionCategory.PlayAndRecord,
              AVAudioSessionCategoryOptions.MixWithOthers);
            currentSession.SetActive(true);
        }
        public void EndCall(ActiveCallbackDelegate completionHandler)
        {
            // Simulate the call ending
            IsConnected = false;
            completionHandler(true);
        }
        #endregion

        #region Events
        public delegate void ActiveCallbackDelegate(bool successful);
        public delegate void ActiveCallStateChangedDelegate(ActiveCall call);

        public event ActiveCallStateChangedDelegate StartingConnectionChanged;
        internal void RaiseStartingConnectionChanged()
        {
            if (this.StartingConnectionChanged != null) this.StartingConnectionChanged(this);
        }

        public event ActiveCallStateChangedDelegate ConnectedChanged;
        internal void RaiseConnectedChanged()
        {
            if (this.ConnectedChanged != null) this.ConnectedChanged(this);
        }
        #endregion
    }
}