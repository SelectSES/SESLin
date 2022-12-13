using CSCameraView.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSCameraView.Classes
{
    public class WebAPI : WebAPIExtension
	{

        public async Task<List<UserMissedCalls>> GetMissedCalls(int userId)
		{
			try
			{
				return await GetAsyncAuthorized<List<UserMissedCalls>>($"Users/users/missedcalls/{userId}").ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<bool> SendLog()
        {
			return await PostFile("Log/PostLogFile", CSCameraView.Classes.Helper.GetLogContentsTOUpload()).ConfigureAwait(true);
		}

		public async Task<bool> RegisterFCMUser(Users users)
		{
			return await PostAsync<Users, bool>("account/registerFCMUser", users);
		}

		public async Task<UserToken> AccountLogin(Users users)
		{
			return await PostAsync<Users, UserToken>("account/login", users);
		}

		public async Task<bool> updateUserLogging(int userId,bool IsLoggingEnabled)
		{
			return await PostAsync<object, bool>($"account/updateUserLogging/{userId}/{IsLoggingEnabled}",null);
		}

		public async Task<Object> AccountLoginV1(Users users)
		{
			return await PostAsyncV2<Users, Object>("account/login", users);
		}
		public async Task<SesCallerModel> GetCallerDetailsByNumber(string phonenumberofcaller)
		{
			try
			{
				return await GetAsync<SesCallerModel>($"panels/panels/getbyphoneNumber/{phonenumberofcaller.ToString()}",true).ConfigureAwait(true);
			}
			
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<ResponseModel> ForgotPassword(ResetPassword resetPassword)
		{
			return await PostAsync<ResetPassword, ResponseModel>("account/forgotPassword", resetPassword);
		}

		public async Task<UserToken> ResetPassword(ResetPassword resetPassword)
		{
			return await PostAsyncV1<ResetPassword, UserToken>("account/resetPassword", resetPassword);
		}

		public async Task<ResponseModel> ChangePassword(Users users)
		{
			return await PostAsync<Users, ResponseModel>("account/changePassword", users);
		}
		public async Task<Object> Autologin(UserToken userToken)
		{
			return await PostAsyncV2<UserToken, Object>("account/autologin", userToken);
		}

	}
}
