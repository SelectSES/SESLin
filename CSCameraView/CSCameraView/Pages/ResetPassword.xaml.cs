using CSCameraView.Classes;
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
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword(string username)
        {
            InitializeComponent();
            Username.Text = username;
        }

        private async void ResetPassword_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text))
            {
                try
                {
                    WebAPI webApi = new WebAPI();
                    Models.ResetPassword resetPassword = new Models.ResetPassword
                    {
                        UserName = Username.Text,
                        OTP = Password.Text
                    };
                    var response = await webApi.ResetPassword(resetPassword);
                    if(response!=null)
                    {

                        Settings.UserId = response.UserId;
                        //Settings.UserName = response.UserName;
                        
                        Settings.UserType = response.UserType;
                        Settings.Domain = response.sipDomain; //domain.Text;


                      
                        Settings.AccessToken = response.AccessToken;
                        Settings.RefreshToken = response.refreshToken;
                        Settings.refreshTokenExpiration = response.refreshTokenExpiration;
                        Settings.Name = response.Name;

                    }
                    await Navigation.PushAsync(new ChangePassword(true));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", ex.Message, "ok");
                }
                
            }
            else
            {
                await DisplayAlert("Alert", "Please fill All the Fields", "ok");
            }
        }
    }
}