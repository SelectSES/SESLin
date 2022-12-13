using Acr.UserDialogs;
using CSCameraView.Classes;
using CSCameraView.Dependency;
using CSCameraView.Extensions;
using CSCameraView.Models;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        Timer t1;

        public ObservableCollection<MenuPopUp> Items { get; set; }
        public string Version => DependencyService.Get<IVersionInfo>().VersionName;
        long lastPress;
        private DateTime? LastTap = null;
        private int NumberOfTaps = 0;
        private const int NumberOfTapsRequired = 4;
        private const long ToleranceInMs = 1000;
        public LoginPage()
        {
            try
            {
                InitializeComponent();
               // IniTialiZePopupMenuData();
                if (Device.RuntimePlatform == Device.iOS)
                {
                    App.DependencyInstance.OnRegisterSuccess -= Linphone_OnRegisterSuccess;
                    App.DependencyInstance.OnRegisterSuccess += Linphone_OnRegisterSuccess;
                }
                Version_Number.Text = "Version: " + Version;
                Settings.UrlSelectionMode = string.Empty;

                var logoTapRecognizer = new TapGestureRecognizer();
                logoTapRecognizer.Tapped += async (s, e) =>
                     {
                         long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                         if (currentTime - lastPress > ToleranceInMs)
                         {
                             lastPress = currentTime;
                             NumberOfTaps = 1;
                         }
                         else
                         {
                             NumberOfTaps++;
                             if (NumberOfTaps == NumberOfTapsRequired)
                                 //LogoImage_Tapped();
                             
                             toggleOptionsMenu();
                         }
                     };
                Logoicon.GestureRecognizers.Add(logoTapRecognizer);


            }
            catch (Exception Ex)
            {

            }
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

        private void IniTialiZePopupMenuData()
        {
            var data = new ObservableCollection<MenuPopUp>();

            data.Add(new MenuPopUp { MenuType = "Live", Link = "http://seva.selectses.com/api/api"});
            data.Add(new MenuPopUp { MenuType = "Dev", Link = "http://sevadev.selectses.com/api/api" });
            data.Add(new MenuPopUp { MenuType = "QA", Link = "http://sevaqa.selectses.com/api/api" });
           

            this.Items = data;

            //this.DeleteItemCommand = new Command(this.DeleteItemCommandExecuted);
          //  MenuCollectionView.ItemsSource = data;
            BindingContext = this;
        }

        private void toggleOptionsMenu()
        {
            popupGrid.IsVisible = !popupGrid.IsVisible;
            switch (Settings.UrlSelectionMode)
            {
               
                case "Live":

                    DevLinkframe.BackgroundColor = Color.LightGray;
                    QAlinkframe.BackgroundColor = Color.LightGray;
                    Livelinkframe.BackgroundColor = Color.Aqua;

                    break;

                case "Dev":

                    DevLinkframe.BackgroundColor = Color.Aqua;
                    QAlinkframe.BackgroundColor = Color.LightGray;
                    Livelinkframe.BackgroundColor = Color.LightGray;

                    break;
                case "QA":
                    DevLinkframe.BackgroundColor = Color.LightGray;
                    QAlinkframe.BackgroundColor = Color.Aqua;
                    Livelinkframe.BackgroundColor = Color.LightGray;


                    break;

                default:
                    DevLinkframe.BackgroundColor = Color.LightGray;
                    QAlinkframe.BackgroundColor = Color.LightGray;
                    Livelinkframe.BackgroundColor = Color.Aqua;

                    break;




            }
           


        
    }
        private void Linphone_OnRegisterSuccess()
        {
            try
            {
                //Settings.UserName = username.Text;
                //Settings.Password = password.Text;
                //Application.Current.MainPage = new NavigationPage(new CallWaitPage());
                Page currentPage = Navigation.NavigationStack.LastOrDefault(); //this is the instance of the ContentPage we are currently at. if (currentPage == this) { //I'm last page. }

                //if (((NavigationPage)Application.Current.MainPage).CurrentPage is Pages.CallWaitPage || ((NavigationPage)Application.Current.MainPage).CurrentPage is Pages.ChangePassword)
                //{
                //    return;
                //}
                if (currentPage != null && currentPage is Pages.CallWaitPage || currentPage is Pages.ChangePassword)
                {
                    return;
                }
                else
                {
                    if (Settings.IsFirstLogin)
                        Navigation.PushAsync(new ChangePassword(true));
                    else
                        //  App.Current.MainPage = new NavigationPage(new CallWaitPage());
                        Navigation.PushAsync(new CallWaitPage());
                }
            }
            catch (Exception Ex)
            {

            }

        }
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(username.Text) && !string.IsNullOrEmpty(password.Text))
            {
                try
                {
                    RegistrationModel registrationModel = new RegistrationModel();
                    WebAPI webApi = new WebAPI();
                    CSCameraView.ViewModels.BaseViewModel baseViewModel = new ViewModels.BaseViewModel();

                    if (await baseViewModel.HaveInternet())
                    {
                        UserDialogs.Instance.ShowLoading("Loading... Please Wait.");

                        username.Text = username.Text.Trim();
                        password.Text = password.Text.Trim();

                        Users users = new Users
                        {
                            UserName = username.Text,
                            Password = password.Text
                        };
                        var userToken = await webApi.AccountLogin(users);
                        if (userToken != null && userToken.UserId > 0)
                        {

                            Settings.UserId = userToken.UserId;
                            Settings.UserName = username.Text;
                            Settings.Password = password.Text;
                            Settings.UserType = userToken.UserType;
                            Settings.Domain = userToken.sipDomain; //domain.Text;
                            Settings.IsSendLogsBtnVisible = userToken.IsLoggingEnabled;

                            Settings.IsFirstLogin = userToken.IsFirstLogin;
                            Settings.AccessToken = userToken.AccessToken;
                            Settings.RefreshToken = userToken.refreshToken;
                            Settings.refreshTokenExpiration = userToken.refreshTokenExpiration;
                            Settings.Name = userToken.Name;


                            Settings.IsUserLoggedIn = true;
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                users = new Users()
                                {
                                    UserId = userToken.UserId,
                                    DeviceType = Device.RuntimePlatform,
                                  DeviceId= DependencyService.Get<ICommonDependecyMethodsInterface>().GetDeviceId(),
                                FCMToken = CrossFirebasePushNotification.Current.Token

                                };
                                if (!string.IsNullOrEmpty(users.FCMToken))
                                {
                                    Helper.WriteLog("Token------------------------: " + DateTime.Now);
                                    Helper.WriteLog("Token: " + users.FCMToken);
                                    Settings.NotificationToken = users.FCMToken;
                                    var apiResponse = await webApi.RegisterFCMUser(users);

                                    if (Settings.IsFirstLogin)
                                        await Navigation.PushAsync(new ChangePassword(true));
                                    else
                                        //  App.Current.MainPage = new NavigationPage(new CallWaitPage());
                                        await Navigation.PushAsync(new CallWaitPage());

                                }
                                else
                                {
                                    await DisplayAlert("Message", "Unable to Register please try again", "OK");
                                }
                            }
                            else
                            {
                                users = new Users()
                                {
                                    UserId = userToken.UserId,
                                    DeviceType = Device.RuntimePlatform,
                                    DeviceId = DependencyService.Get<ICommonDependecyMethodsInterface>().GetDeviceId(),
                                    FCMToken = Settings.NotificationToken
                                };

                                if (!string.IsNullOrEmpty(users.FCMToken))
                                {
                                    using (Acr.UserDialogs.UserDialogs.Instance.Loading("Registering details..."))
                                    {
                                        var apiResponse = await webApi.RegisterFCMUser(users);
                                        if (Settings.IsFirstLogin)
                                            await Navigation.PushAsync(new ChangePassword(true));
                                        else
                                            //  App.Current.MainPage = new NavigationPage(new CallWaitPage());
                                            await Navigation.PushAsync(new CallWaitPage());


                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Message", "Unable to Register please try again", "OK");
                                }

                            }
                        }
                        UserDialogs.Instance.HideLoading();

                    }
                }
                catch (Exception Ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Authentication Failure", "The username/password combination is incorrect. Please check and retry again.", "OK");
                    CSCameraView.Classes.Helper.WriteLog("App entering OnRegisterClicked" + Ex.Message + "StackTrace\n" + Ex.StackTrace);
                }
            }
            else
            {
                await DisplayAlert("Alert", "Please fill All the Fields", "ok");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPassword());
        }

        private void passwordBtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(password.Text))
            {
                password.IsPassword = password.IsPassword == true ? false : true;
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

        private async void Permissions_Clicked(object sender, EventArgs e)
        {
            if (DependencyService.Get<BaterySaverPermission>().CheckIsEnableBatteryOptimizations())
            {


            }
            else
            {
                var action = await Application.Current.MainPage.DisplayAlert("open battery settings", "please close battery optimization", "yes", "No");

                if (action)
                {

                    DependencyService.Get<BaterySaverPermission>().StartSetting();
                }
                else
                {

                }
            }
        }

        private void passwordBtn_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void LogoImage_Tapped()
        {

            try
            {
                WebAPIExtension webAPIExtension = new WebAPIExtension();
                var actionSheet = await DisplayActionSheet("choose environment", "Cancel", null, "Live-", "Dev", "Qa");

                switch (actionSheet)
                {
                    case "Cancel":
                        break;

                    case "Live":
                        Settings.UrlSelectionMode = "Live";


                        break;

                    case "Dev":
                        Settings.UrlSelectionMode = "Dev";


                        break;
                    case "Qa":
                        Settings.UrlSelectionMode = "Qa";


                        break;

                }
            }
            catch (Exception Ex)
            {



            }
        }

        private void ClosePopUp_Tapped(object sender, EventArgs e)
        {
            toggleOptionsMenu();
        }

        private void QALink_Tapped(object sender, EventArgs e)
        {
            Settings.UrlSelectionMode = "QA";
            DevLinkframe.BackgroundColor = Color.LightGray;
            QAlinkframe.BackgroundColor = Color.Aqua;
            Livelinkframe.BackgroundColor = Color.LightGray;
            toggleOptionsMenu();
        }

        private void DevLink_Tapped(object sender, EventArgs e)
        {
            Settings.UrlSelectionMode = "Dev";
            DevLinkframe.BackgroundColor = Color.Aqua;
            QAlinkframe.BackgroundColor = Color.LightGray;
            Livelinkframe.BackgroundColor = Color.LightGray;
            toggleOptionsMenu();
        }

        private void Livelink_Tapped(object sender, EventArgs e)
        {
            Settings.UrlSelectionMode = "Live";
            DevLinkframe.BackgroundColor = Color.LightGray;
            QAlinkframe.BackgroundColor = Color.LightGray;
            Livelinkframe.BackgroundColor = Color.Aqua;
            toggleOptionsMenu();
        }

       

      
    }
    public class MenuPopUp
    {
        public string MenuType { get; set; }
        public string Link { get; set; }
        public Boolean IsSelected { get; set; }
      
    }
}