using CSCameraView.Classes;
using CSCameraView.Dependency;
using CSCameraView.Models;
using CSCameraView.ViewModels;
using IncomingCall.Dependency;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Essentials;
using System.IO;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallWaitPage : ContentPage
    {
        private System.Timers.Timer timer;
        public string Version => DependencyService.Get<IVersionInfo>().VersionName;
        public string Build => DependencyService.Get<IVersionInfo>().VersionCode;
        public bool IsOptionsPopupVisible { get; set; }
        public CallWaitPage()
        {
            try
            {
                InitializeComponent();

                IsOptionsPopupVisible = false;

                Version_Number.Text = "Version: " +  Version + "B" + Build;
                if(Device.RuntimePlatform == Device.Android)
                {
                    if (Settings.RegistrationState.Equals("Ok"))
                    {
                        InitmsgLabel.IsVisible = false;
                        waitingmsgLabel.IsVisible = true;
                    }
                    else
                    {
                        InitmsgLabel.IsVisible = true;
                        waitingmsgLabel.IsVisible = false;
                    }

                }
                else
                {
                    InitmsgLabel.IsVisible = false;
                    waitingmsgLabel.IsVisible = true;
                }
               
                if ((Device.RuntimePlatform == Device.Android))
                {
                     Settings.IsServiceRunning= Xamarin.Forms.DependencyService.Get<INativeHelper>().IsServiceRunning();
                    if(!Settings.IsServiceRunning)
                    {
                     
                        App.DependencyInstanceAndroid.StartService();
                        Settings.IsServiceRunning = true;
                        App.DependencyInstance.OnRegisterSuccess -= Linphone_OnRegisterSuccess;
                        App.DependencyInstance.OnRegisterSuccess += Linphone_OnRegisterSuccess;

                    }
          
                }
            }
            catch (Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallWaitPage " + Ex.Message + "StackTrace\n" + Ex.StackTrace);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((CallWaitPageViewModel)this.BindingContext).IsSendLogsVisible = Settings.IsSendLogsBtnVisible;
            Device.BeginInvokeOnMainThread(() => InvalidateMeasure());
        }

        private void Linphone_OnRegisterSuccess()
        {
            waitingmsgLabel.IsVisible = true;
            InitmsgLabel.IsVisible = false;
        }

        private  void LogOut_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    Settings.IsServiceRunning = false;
                    App.DependencyInstanceAndroid.StopService();
                }
                App.DependencyInstance.UnRegisterSIPUser123(Settings.UserName, Settings.Password, Settings.Domain);
                
                //Settings.userToken.IsFirstLogin = false;
                Settings.UserName = string.Empty;
                Settings.Password = string.Empty;
                Settings.Domain = string.Empty;
                Settings.RegistrationState = string.Empty;
                Settings.IsUserLoggedIn = false;
                Settings.NotificationToken = string.Empty;
                Settings.UrlSelectionMode = string.Empty;
                Settings.IsSendLogsBtnVisible = false;
                Navigation.PushAsync(new LoginPage());

            }
            catch (Exception ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallWaitPage:Logout: " + ex.Message + "StackTrace\n" + ex.StackTrace);
            }
        }
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                await new Classes.WebAPI().SendLog();
            }
            catch (Exception ex)
            {
                DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        private void ChangePassword_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            WarningControl.IsVisible = false;
        }

        private void toggleOptionsMenu()
        {
            popupGrid.IsVisible = !popupGrid.IsVisible;
        }

        private void optionsBtnTap_Tapped(object sender, EventArgs e)
        {
            toggleOptionsMenu();
        }

        private void sendToBackBtnTap_Tapped(object sender, EventArgs e)
        {
            //TODO: send app to background code here
        }

        private void changePasswordBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }

        private async void signOutBtn_Clicked(object sender, EventArgs e)
        {
            try
            {

                bool answer = await DisplayAlert("Sign Out?", "Are you sure you want to deactivate camera services?", "Yes", "No");
                if (answer)
                {
                    LogOut_Tapped(sender, e);
                }
                else
                {
                    toggleOptionsMenu();
                }
            }
            catch(Exception Ex)
            {
                CSCameraView.Classes.Helper.WriteLog("CallWaitPage:Logout: " + Ex.Message + "StackTrace\n" + Ex.StackTrace);
            }
          
            
        }

        private void callHistoryBtn_Clicked(object sender, EventArgs e)
        {
            toggleOptionsMenu();
            Navigation.PushAsync(new CallHistoryPage());
        }

        private void settingsBtn_Clicked(object sender, EventArgs e)
        {
            toggleOptionsMenu();
            Navigation.PushAsync(new SettingsPage());
        }

        private async void sendLogsBtn_Clicked(object sender, EventArgs e)
        {

            try
            {
                await SendEmail("CSCamera Logs","CSCamera Logs",new List<string>() { "sup@ses.com" });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }

        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                EmailAttachment emailAttachment = new EmailAttachment(Path.Combine(DependencyService.Get<INativeHelper>().GetExternalStorage(), "CSCameraLog.txt"));
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    Attachments = new List<EmailAttachment>() { emailAttachment }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
            }
            catch (Exception ex)
            {
            }
        }

        private void tempBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new CallProcess(param: "testing"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}