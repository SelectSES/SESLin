using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCameraView.Classes;

namespace CSCameraView.Droid.Services
{
    [BroadcastReceiver()]
    [IntentFilter(new[] { "android.intent.action.PHONE_STATE", "android.intent.action.NEW_OUTGOING_CALL" }, Priority = (int)IntentFilterPriority.HighPriority)]
    [IntentFilter(new[] { "android.intent.action.PHONE_STATE", "android.intent.action.NEW_INCOMING_CALL" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class PhoneCallWatcher : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // ensure there is information
            if (intent.Extras != null)
            {
                if (intent.Action.Equals(Intent.ActionNewOutgoingCall))
                {
                   Settings.ISPhoneBusy = true;
                    //outgoing call
                }

                // get the incoming call state
                string state = intent.GetStringExtra(TelephonyManager.ExtraState);

                // check the current state
                if (state == TelephonyManager.ExtraStateRinging)
                {
                   

                    var telephonyManager = (TelephonyManager)Android.App.Application.Context.GetSystemService(Context.TelephonyService);
                 
                    
                    Settings.ISPhoneBusy = true;
                    // read the incoming call telephone number...
                    string telephone = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);
                    // check the reade telephone
                    if (string.IsNullOrEmpty(telephone))
                        telephone = string.Empty;
                }
                else if (state == TelephonyManager.ExtraStateOffhook)
                {
                    Settings.ISPhoneBusy = true;
                    // incoming call answer
                }
                else if (state == TelephonyManager.ExtraStateIdle)
                {
                    Settings.ISPhoneBusy = false;
                    // incoming call end
                }
            }
        }
    }
}