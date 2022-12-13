using CSCameraView.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;


namespace CSCameraView.Classes
{
   public  class Settings
    {


        #region Setting Constants

        private static ISettings AppSettings => CrossSettings.Current;

       
        private const string TransportKey = "Transport_Key";
        private const string NotificationTokenKey = "NotificationToken_Key";

        private const string CallInProcessKey = "CallInProcess_Key";

        private const string CurrentCallStateKey = "CurrentCallState_Key";

        private const string IsAnswerKey = "IsAnswer_Key";

        private const string RegistrationStateKey = "RegistrationState_Key";

        private const string videoUrlKey = "videoUrl_Key";

        private const string DTMFKey = "DTMF_Key";

        private const string AutoTerminateCallKey = "AutoTerminateCall_Key";


        private const string IsUserLoggedInKey = "RemberMe_Key";


        private const string IsServiceRunningKey = "IsServiceRunning_Key";

        private const string CallerNumberKey = "CallerNumber_Key";
        private const string ISCallConnectedKey = "ISCallConnected_Key";


        private const string ISCallReceivedfromCallkitUIKey = "ISCallReceivedfromCallkitUI_Key";


        private const string UserNameKey = "UserName_Key";
        private const string PasswordKey = "Password_Key";
        private const string DomainKey = "Domain_Key";

        private const string AccessTokenKey = "AccessToken_Key";
         private const string UserIdKey = "UserId_Key";
        private const string NameKey = "Name_Key";

        private const string IsFirstLoginKey = "IsFirstLogin_Key";
        private const string refreshTokenKey = "refreshToken_Key";
        private const string refreshTokenExpirationKey = "refreshTokenExpiration_Key";

        private const string UserTypeKey = "UserType_Key";


        private const string IPAddressKey = "IP_Address";


        private const string SystemDefaultMusicVolumeKey = "SystemDefaultMusicVolume_Key";
        private const string SystemDefaultVoiceCallVolumeKey = "SystemDefaultVoiceCallVolume_Key";

        private const string ISPhoneBusyKey = "IS_Phone_busy";

        private const string UrlSelectionModeKey = "Url_Selection_Mode";

        private const string IsSendLogsBtnVisibleKey = "IsSendLogsBtnVisible_Key";

        private const string IsStartedFromBootKey = "IsStartedFromBoot_Key";

        private const string IsVideoPortraitKey = "IsVideoPortrait_Key";

        private const string IsVolumeBoostedKey = "IsVolumeBoosted_Key";
        #endregion


        public static bool IsVolumeBoosted
        {
            get => AppSettings.GetValueOrDefault(IsVolumeBoostedKey, true);
            set => AppSettings.AddOrUpdateValue(IsVolumeBoostedKey, value);
        }
        
        public static bool IsVideoPortrait
        {
            get => AppSettings.GetValueOrDefault(IsVideoPortraitKey, true);
            set => AppSettings.AddOrUpdateValue(IsVideoPortraitKey, value);
        }

        public static bool ISCallReceivedfromCallkitUI
        {
            get => AppSettings.GetValueOrDefault(ISCallReceivedfromCallkitUIKey, false);
            set => AppSettings.AddOrUpdateValue(ISCallReceivedfromCallkitUIKey, value);
        }

        public static bool ISPhoneBusy
        {
            get => AppSettings.GetValueOrDefault(ISPhoneBusyKey, false);
            set => AppSettings.AddOrUpdateValue(ISPhoneBusyKey, value);
        }



        #region UserToken Details

        public static string UrlSelectionMode
        {
            get => AppSettings.GetValueOrDefault(UrlSelectionModeKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(UrlSelectionModeKey, value);
        }


        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(AccessTokenKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(AccessTokenKey, value);
        }

        public static string IPAddress
        {
            get => AppSettings.GetValueOrDefault(IPAddressKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(IPAddressKey, value);
        }

        public static string RefreshToken
        {
            get => AppSettings.GetValueOrDefault(refreshTokenKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(refreshTokenKey, value);
        }

        public static bool IsFirstLogin
        {
            get => AppSettings.GetValueOrDefault(IsFirstLoginKey, false);
            set => AppSettings.AddOrUpdateValue(IsFirstLoginKey, value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(UserNameKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(UserNameKey, value);
        }
        public static DateTime refreshTokenExpiration
        {
            get => AppSettings.GetValueOrDefault(refreshTokenExpirationKey, new DateTime(1960, 7, 1, 00, 00, 00, DateTimeKind.Utc));
            set
            {
                var temp = value;
                temp = DateTime.SpecifyKind(temp, DateTimeKind.Utc);
                AppSettings.AddOrUpdateValue(refreshTokenExpirationKey, temp);
            }
        }

        public static string Name
        {
            get => AppSettings.GetValueOrDefault(NameKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(NameKey, value);
        }
        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PasswordKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(PasswordKey, value);
        }

        public static string Domain
        {
            get => AppSettings.GetValueOrDefault(DomainKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(DomainKey, value);
        }

        public static string UserType
        {
            get => AppSettings.GetValueOrDefault(UserTypeKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(UserTypeKey, value);
        }

        public static int UserId
        {
            get => AppSettings.GetValueOrDefault(UserIdKey, 0);
            set => AppSettings.AddOrUpdateValue(UserIdKey, value);
        }


        #endregion

        public static string CallerNumber
        {
            get => AppSettings.GetValueOrDefault(CallerNumberKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(CallerNumberKey, value);
        }
     

     

        public static int Transport
        {
            get => AppSettings.GetValueOrDefault(TransportKey, 0);
            set => AppSettings.AddOrUpdateValue(TransportKey, value);
        }

       
        public static string DTMFDigit
        {
            get => AppSettings.GetValueOrDefault(DTMFKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(DTMFKey, value);
        }

        public static string AutoTerminateCall
        {
            get => AppSettings.GetValueOrDefault(AutoTerminateCallKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(AutoTerminateCallKey, value);
        }

        public static string NotificationToken
        {
            get => AppSettings.GetValueOrDefault(NotificationTokenKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(NotificationTokenKey, value);
        }

        public static bool IsUserLoggedIn
        {
            get => AppSettings.GetValueOrDefault(IsUserLoggedInKey, false);
            set => AppSettings.AddOrUpdateValue(IsUserLoggedInKey, value);
        }

        public static bool IsServiceRunning
        {
            get => AppSettings.GetValueOrDefault(IsServiceRunningKey, false);
            set => AppSettings.AddOrUpdateValue(IsServiceRunningKey, value);
        }

        public static bool IsLinePhoneCoreCallConnected
        {
            get => AppSettings.GetValueOrDefault(ISCallConnectedKey, false);
            set => AppSettings.AddOrUpdateValue(ISCallConnectedKey, value);
        }


        public static string CurrentCallState
        {
            get => AppSettings.GetValueOrDefault(CurrentCallStateKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(CurrentCallStateKey, value);
        }
        public static bool IsAnswer
        {
            get => AppSettings.GetValueOrDefault(IsAnswerKey, false);
            set => AppSettings.AddOrUpdateValue(IsAnswerKey, value);
        }

        public static string RegistrationState
        {
            get => AppSettings.GetValueOrDefault(RegistrationStateKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(RegistrationStateKey, value);
        }

        public static string videoUrl
        {
            get => AppSettings.GetValueOrDefault(videoUrlKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(videoUrlKey, value);
        }


        public static int  SystemDefaultMusicVolume
        {
            get => AppSettings.GetValueOrDefault(SystemDefaultMusicVolumeKey, 0);
            set => AppSettings.AddOrUpdateValue(SystemDefaultMusicVolumeKey, value);
        }


        public static int SystemDefaultVoiceCallVolume
        {
            get => AppSettings.GetValueOrDefault(SystemDefaultVoiceCallVolumeKey, 0);
            set => AppSettings.AddOrUpdateValue(SystemDefaultVoiceCallVolumeKey, value);
        }

        public static bool IsSendLogsBtnVisible
        {
            get => AppSettings.GetValueOrDefault(IsSendLogsBtnVisibleKey, false);
            set => AppSettings.AddOrUpdateValue(IsSendLogsBtnVisibleKey, value);
        }

        public static bool IsStartedFromBoot
        {
            get => AppSettings.GetValueOrDefault(IsStartedFromBootKey, false);
            set => AppSettings.AddOrUpdateValue(IsStartedFromBootKey, value);
        }
    }
}
