using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CSCameraView.Classes;
using CSCameraView.Extensions;
using CSCameraView.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);
        //    try
        //    {
        //        contentGrid.AddSafeMargin(this);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Username.Text))
            {
                try
                {
                    WebAPI webApi = new WebAPI();
                    Models.ResetPassword resetPassword = new Models.ResetPassword
                    {
                        UserName = Username.Text
                    };
                    UserDialogs.Instance.ShowLoading("Loading... Please Wait.");
                    var response = await webApi.ForgotPassword(resetPassword);
                    if (response.result == "OTP sent successfully")
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert(Constants.Success, "You will receive a 7 digit code as a text message which you will need to enter in the next screen to reset your password", Constants.OK);
                        await Navigation.PushAsync(new ResetPassword(Username.Text));
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Alert", response.result, "ok");
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Alert", ex.Message, "ok");
                }
                
            }
            else
            {
                await DisplayAlert("Alert", "Please Enter Username", "ok");
            }
            
        }
    }
}