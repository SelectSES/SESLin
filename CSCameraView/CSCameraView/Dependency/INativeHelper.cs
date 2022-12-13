using System;
using System.Collections.Generic;
using System.Text;

namespace IncomingCall.Dependency
{
	public interface INativeHelper
	{
		
        string GetExternalStorage();
		void Speaker();

		Tuple<int,int> GetSystemDefaultVolume();

		void SetSystemDefaultVolume(int defaultMusicVolume,int defaultVoiceVolume);

		void Mute();
		void UnMute();

		bool IsServiceRunning();
	}
}
