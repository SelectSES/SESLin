using CSCameraView.Classes;
using CSCameraView.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationPage : ContentPage
    {
        public AuthenticationPage()
        {
            InitializeComponent();
        }

        //Added this Page is only for if user is allreday logged in and user change their Password from web Application
        //then Need to Authenticate the User again and Navigate Back to Login if the Crediantials are not matched
        protected async override void OnAppearing()
        {
            try
            {
                CSCameraView.Classes.Helper.WriteLog("App entering AuthenticationPage: OnAppearing." + DateTime.Now);
                base.OnAppearing();
                WebAPI webApi = new WebAPI();
                UserToken userToken3 = new UserToken();
                Users users = new Users
                {
                    UserName = Settings.UserName,
                    Password = Settings.Password
                };
                var userToken2 = await webApi.AccountLoginV1(users);
                var jo = JObject.Parse(userToken2.ToString());
                userToken3 = JsonConvert.DeserializeObject<UserToken>(jo.ToString());
                if (userToken3 != null && userToken3.UserId > 0)
                {
                    Settings.UserId = userToken3.UserId;
                    Application.Current.MainPage = new NavigationPage(new Pages.CallWaitPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new Pages.LoginPage());
                }
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("AuthenticationPage:OnAppearing: " + Ex.Message + "StackTrace\n" + Ex.StackTrace);
                Application.Current.MainPage = new NavigationPage(new Pages.LoginPage());
            }
        }
    }
}