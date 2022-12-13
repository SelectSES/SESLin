using CSCameraView.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class RegistrationModel : ImplPropertyNotifier
	{

		
		private string _userName;

		public string UserName
		{
			get => _userName;
			set => SetProperty(ref _userName, value);
		}

		private string _Password;

		public string Password
		{
			get => _Password;
			set => SetProperty(ref _Password, value);
		}


		private string _deviceId;

		public string DeviceId
		{
			get => _deviceId;
			set => SetProperty(ref _deviceId, value);
		}

		private string _NotitficationToken;

		public string NotitficationToken
		{
			get => _NotitficationToken;
			set => SetProperty(ref _NotitficationToken, value);
		}

		private string _DeviceType;

		public string DeviceType
		{
			get => _DeviceType;
			set => SetProperty(ref _DeviceType, value);
		}

		//SET the Property for when user is allready logined in, then Get the Chapter Setting again on Dashboard Refresh
		private bool _ResetLogin = false;

		public bool ResetLogin
		{
			get => _ResetLogin;
			set => SetProperty(ref _ResetLogin, value);
		}

	}
}
