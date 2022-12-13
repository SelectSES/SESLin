using AVFoundation;
using CSCameraView.iOS.Dependency;
using Foundation;
using IncomingCall.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeHelper))]
namespace CSCameraView.iOS.Dependency
{
    public class NativeHelper : INativeHelper
    {

        public string GetExternalStorage()
        {

            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }

      
        public void InvokePopUp()
        {
            throw new NotImplementedException();
        }
        public void RegisterReceiver()
        {
            throw new NotImplementedException();
        }
        public void Speaker()
        {
            try
            {

                CSCameraView.Classes.Helper.WriteLog("Speaker:App Entering Native speaker function  in Dependancy.");
                NSError err;
                var session = AVAudioSession.SharedInstance();
                session.SetCategory(AVAudioSessionCategory.PlayAndRecord, AVAudioSessionCategoryOptions.MixWithOthers);
                session.SetMode(AVAudioSession.ModeVoiceChat, out err);
                session.SetActive(true);


                session.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out err);
            }
            catch(Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("Speaker:App Entering Native speaker function  in Dependancy." +ex.Message);
            }
            
        }
        public Tuple<int,int> GetSystemDefaultVolume()
        {

            return new Tuple<int, int>(0,0);

        }

        public void SetSystemDefaultVolume(int DefaultMusicVolume, int DefaultVoiceCallVolume)
        {
            
         
        }

        public void Mute()
        {
            throw new NotImplementedException();
        }
        public void UnMute()
        {
            throw new NotImplementedException();
        }

        public bool IsServiceRunning()
        {
            throw new NotImplementedException();
        }
    }
}