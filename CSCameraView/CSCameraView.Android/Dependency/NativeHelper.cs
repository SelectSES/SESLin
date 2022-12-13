using Android.Content;
using Android.Media;
using Android.Telephony;
using CSCameraView.Droid.Dependency;
using CSCameraView.Droid.Services;
using IncomingCall.Dependency;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeHelper))]
namespace CSCameraView.Droid.Dependency
{
    class NativeHelper : INativeHelper
	{
        public int musicOrigVol { get; private set; }

       
		public string GetExternalStorage()
        {
			
			return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;

		}

		public Tuple<int, int> GetSystemDefaultVolume()
		{
			AudioManager am = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			int Music_volume_level = am.GetStreamVolume(Stream.Music);
			int VoiceCall_volume_level1 = am.GetStreamVolume(Stream.VoiceCall);
			return new Tuple<int, int>(Music_volume_level, VoiceCall_volume_level1);

		}

		public void SetSystemDefaultVolume(int DefaultMusicVolume ,int DefaultVoiceCallVolume)
		{
			var audioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			audioManager.SetStreamVolume(Stream.Music, DefaultMusicVolume, 0);
			var audioManager1 = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			audioManager1.SetStreamVolume(Stream.VoiceCall, DefaultVoiceCallVolume, 0);
			audioManager.Mode = Mode.Normal;
			audioManager1.AdjustStreamVolume(Stream.Music, Adjust.Lower, 0);
			audioManager.AdjustStreamVolume(Stream.VoiceCall, Adjust.Lower, 0);

		}

		public void Speaker()
		{
		
			var audioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			audioManager.SetStreamVolume(Stream.Music, audioManager.GetStreamMaxVolume(Stream.Music), 0);
			audioManager.Mode = Mode.InCommunication;
			audioManager.SpeakerphoneOn = true;
	
			audioManager.AdjustStreamVolume(Stream.Music, Adjust.Raise, 0);




			AudioManager am1 = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			am1.SetStreamVolume(Stream.VoiceCall, audioManager.GetStreamMaxVolume(Stream.VoiceCall), 0);
			am1.AdjustStreamVolume(Stream.VoiceCall, Adjust.Raise, 0);
		}

		public void Mute()
        {
			var audioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			musicOrigVol = audioManager.GetStreamVolume(Stream.VoiceCall);
			var mute = 0;
			audioManager.MicrophoneMute = true;
			audioManager.SetStreamVolume(Stream.VoiceCall, mute, 0);
		}

		public void UnMute()
        {
			var audioManager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
			var volume = musicOrigVol;
			audioManager.MicrophoneMute = false;
			audioManager.SetStreamVolume(Stream.VoiceCall, volume, 0);
        }
		public bool IsServiceRunning()
        {
			if(MainActivity.Current.IsServiceRunning(typeof(LinePhoneService1)))
			{
				return true;
            }
            else
            {
				return false;
            }
        }
	}
}