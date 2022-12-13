using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Classes
{
   public  class Constants
    {

        #region Internet Connection

        public const string NoInternetConnection = "No Internet Connection.";
        public const string Failedduetonetworkerror = "Failed due to network error. Please try again when your are connected to the Internet.";

        #endregion Internet Connection

        #region LinePhoneAcesssCallState


        #endregion

        public const string OK = "OK";
        public const string Sorry = " ";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Error = "Error";
        public const string Success = "Success";
        public const string PasswordSuccess = "Successfully changed password.";

        public const int DELAY_BETWEEN_LOG_MESSAGES = 5000; // milliseconds
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string SERVICE_STARTED_KEY = "has_service_been_started";
        public const string BROADCAST_MESSAGE_KEY = "broadcast_message";
        public const string NOTIFICATION_BROADCAST_ACTION = "ServicesDemo3.Notification.Action";

        public const string ACTION_START_SERVICE = "ServicesDemo3.action.START_SERVICE";
        public const string ACTION_STOP_SERVICE = "ServicesDemo3.action.STOP_SERVICE";
        public const string ACTION_RESTART_TIMER = "ServicesDemo3.action.RESTART_TIMER";
        public const string ACTION_MAIN_ACTIVITY = "ServicesDemo3.action.MAIN_ACTIVITY";

    }

    public enum CallStates
    {

        IncomingReceived=1,
        Connected=2,
        End=3


    }
}
