
using IncomingCall.Dependency;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using CSCameraView.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSCameraView.Classes
{
    public static class Helper
    {
      static string filePath= Path.Combine(DependencyService.Get<INativeHelper>().GetExternalStorage(), "CSCameraLog.txt");

		
		public static   async void WriteLog(string message, [CallerMemberName] string memberName = "")
        {
            
            try
            {
			
				FileInfo file = new FileInfo(filePath);
               
                if (!file.Exists)
                {
                    file.Create().Close();
                }

                using (StreamWriter w = File.AppendText(filePath))
				{
					Log(message, memberName, w);
				}

			}
			catch(Exception ex)
            {

			}
        }



        public static void Log(string logMessage, string memberName, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("Method Name : " + memberName);
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }


        public static MultipartFormDataContent GetLogContentsTOUpload()
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "File", "Log.txt");
            return content;
        }


        public static async void CheckTokenNeedToRefresh()
        {
            //CHECK IF USER HAVE KEPT RememberMe
            try
            {
                if (Settings.IsUserLoggedIn && Settings.refreshTokenExpiration != null)
                {
                    if (Settings.refreshTokenExpiration < DateTime.UtcNow)
                    {
                        UserToken userToken = new UserToken
                        {
                            UserId = Settings.UserId,

                            AccessToken = Settings.AccessToken,
                            refreshToken = Settings.RefreshToken,
                            refreshTokenExpiration = Settings.refreshTokenExpiration,
                        };

                        WebAPI webApi = new WebAPI();
                        var userToken1 = await webApi.Autologin(userToken);
                        if (userToken1.Equals("SessionExpired"))
                        {
                            App.Current.MainPage = new NavigationPage(new Pages.LoginPage());
                        }

                        else
                        {
                            var jo = JObject.Parse(userToken1.ToString());
                            UserToken userToken2 = JsonConvert.DeserializeObject<UserToken>(jo.ToString());
                            if (userToken2 != null)
                            {
                                Settings.UserId = userToken2.UserId;
                                Settings.AccessToken = userToken2.AccessToken;
                                Settings.RefreshToken = userToken2.refreshToken;
                                Settings.refreshTokenExpiration = userToken2.refreshTokenExpiration;
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //IGNORE
            }
        }




    }


}
