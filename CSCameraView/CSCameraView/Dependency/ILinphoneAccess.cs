using System;
using System.Collections.Generic;
using System.Text;


namespace CSCameraView.Dependency
{
  public  interface ILinphoneAccess
    {
        event Action OnIncomingCall;
        event Action OnRegisterSuccess;
        event Action OnRegisterFailed;
        event Action OnCallTerminated;
        event Action OnIncomingReceived;
        event Action OnCoreCallTerminated;

        bool IsManagerAvailable();

        void AnswerCall();
        void TerminateCall();
       void StopRinging();

        void DeclineCall();

        void InitializeLinphone(IntPtr context);
        void RegisterSIPUser(string username, string password, string domain,bool ISfirstRegistrationfromIOS);
        void RefreshSIPRegistration();

       

        void UnRegisterSIPUser123(string username, string password, string domain);
        void StartLinphoneManager();

        void DialDTMF(SByte KeyCode);
        void DialDTMF_2(SByte KeyCode, int Duration);
        void PlayDTMF(SByte KeyCode, int Duration);
     
    }
}
