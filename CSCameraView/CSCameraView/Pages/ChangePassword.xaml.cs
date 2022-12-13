using Acr.UserDialogs;
using CSCameraView.Classes;
using CSCameraView.Extensions;
using CSCameraView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {
        public ChangePassword(bool resetPassword = false)
        {
            try
            {
                InitializeComponent();
                if (resetPassword)
                {
                    txtCurrentPassword.IsVisible = false;
                    currentpasswordbtn.IsVisible = false;
                    NavigationPage.SetHasBackButton(this, false);
                }

            }
            catch(Exception Ex)
            {

            }
          
        }

        private async void ChangePassword_Clicked1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text) && !string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                if (CheckValidCurrentPassword())
                {
                  
                    if (txtNewPassword.Text == txtConfirmPassword.Text)
                    {
                        if (CheckValidPassword(txtConfirmPassword.Text))
                        {
                            try
                            {
                                WebAPI webApi = new WebAPI();
                                Users users = new Users
                                {
                                    UserId = Settings.UserId,
                                    Password = txtNewPassword.Text,
                                    IsFirstLogin = false
                                };

                                UserDialogs.Instance.ShowLoading("Saving Password...");
                                 ResponseModel responseModel = await webApi.ChangePassword(users);
                                UserDialogs.Instance.HideLoading();
                               
                                if (responseModel.result == "Password changed successfully")
                                {
                                        await DisplayAlert(Constants.Success, Constants.PasswordSuccess, Constants.OK);
                                         Settings.Password = txtNewPassword.Text;
                                         await Navigation.PushAsync(new CallWaitPage());
                                }
                                else
                                {
                                    await  DisplayAlert("Password Error", "The password must be a minimum of 12 characters, contain at least 1 digit, and have mixed case (e.g. 'CameraOps2020).", "OK");
                                }


                            }
                            catch (Exception ex)
                            {
                                UserDialogs.Instance.HideLoading();
                                CSCameraView.Classes.Helper.WriteLog("ChangePasswordCommandExecute:" + ex.Message + "\n" + ex.StackTrace);
                               await   DisplayAlert("Password Error", "The password must be a minimum of 12 characters, contain at least 1 digit, and have mixed case (e.g. 'CameraOps2020).", "OK");
                            }
                        }
                        else
                        {
                             await DisplayAlert("Password Error", "The password must be a minimum of 12 characters, contain at least 1 digit, and have mixed case (e.g. 'CameraOps2020).",  "OK");
                        }


                    }
                    else
                    {
                       await  DisplayAlert("Password Error", "Passwords do not Match.", "OK");
                    }
                }
                else
                {
                    await  DisplayAlert("Password Error", "CurrentPassword do not Match.", "OK");
                }

            }
            else
            {
                await DisplayAlert("Password Error", "Password field can not be empty.", "ok");
            }
        }


        public bool CheckValidCurrentPassword()
        {
            
                if (txtCurrentPassword.IsVisible==true && txtCurrentPassword.Text==Settings.Password)
                {
                    return true;
                }
               else if(txtCurrentPassword.IsVisible == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
        }

        public bool CheckValidPassword(string Password)
        {
            if (Password != null && Password != "")
            {
                if (Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,30}$").Success)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
     

        public Task<bool> DisplayMessageAsync(string title, string message, string accept, string cancel)
        {
             return App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        private void passwordbtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                txtCurrentPassword.IsPassword = txtCurrentPassword.IsPassword == true ? false : true;
            }
        }

        private void newpasswordbtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewPassword.Text))
            {
                txtNewPassword.IsPassword = txtNewPassword.IsPassword == true ? false : true;
            }
        }

        private void confimpasswordbtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                txtConfirmPassword.IsPassword = txtConfirmPassword.IsPassword == true ? false : true;
            }
        }
    }
}