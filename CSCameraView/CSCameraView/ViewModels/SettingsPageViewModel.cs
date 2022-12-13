using CSCameraView.Classes;
using CSCameraView.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CSCameraView.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Constructor
        public SettingsPageViewModel()
        {
            SaveCommand = new Command(SaveCommandExecute);
          
        }


        #endregion

        #region Properties
        private bool _IsSendLogsToggled;
        public bool IsSendLogsToggled
        {
            get
            {
                return Settings.IsSendLogsBtnVisible;
            }
            set
            {
                Settings.IsSendLogsBtnVisible = value;
               
                RaisePropertyChanged("IsSendLogsToggled");
            }
        }

        private bool _IsVideoPortraitToggled;
        public bool IsVideoPortraitToggled
        {
            get
            {
                return Settings.IsVideoPortrait
                    ;
            }
            set
            {
                Settings.IsVideoPortrait = value;

                RaisePropertyChanged("IsVideoPortraitToggled");
            }
        }

        private bool _IsVolumeBoostToggled;
        public bool IsVolumeBoostToggled
        {
            get
            {
                return Settings.IsVolumeBoosted;
            }
            set
            {
                Settings.IsVolumeBoosted = value;

                RaisePropertyChanged("IsVolumeBoostToggled");
            }
        }

        private async void SaveCommandExecute(object obj)
        {
            try
            {

                switch (Settings.UrlSelectionMode)
                {
                    case "Cancel":



                        break;

                    case "Live":
                        await App.Current.MainPage.DisplayAlert("Message", "Log flag update successfully.", "OK");
                        await Application.Current.MainPage.Navigation.PushAsync(new CallWaitPage());

                        break;

                    case "Dev":
                        WebAPI webApi = new WebAPI();
                        var apiResponse = await webApi.updateUserLogging(Settings.UserId, IsSendLogsToggled);
                        if (apiResponse)
                        {

                            await App.Current.MainPage.DisplayAlert("Message", "Log flag update successfully.", "OK");
                            await Application.Current.MainPage.Navigation.PushAsync(new CallWaitPage());
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", "Log flag update successfully.", "OK");
                        }



                        break;
                    case "QA":
                        await App.Current.MainPage.DisplayAlert("Message", "Log flag update successfully.", "OK");
                        await Application.Current.MainPage.Navigation.PushAsync(new CallWaitPage());


                        break;

                    default:
                        await App.Current.MainPage.DisplayAlert("Message", "Log flag update successfully.", "OK");
                        await Application.Current.MainPage.Navigation.PushAsync(new CallWaitPage());

                        break;




                }

              

            }
            catch (Exception Ex)
            {

                await App.Current.MainPage.DisplayAlert("Exception", Ex.Message, "OK");

            }
        }

    
        #endregion

        #region Commands

        public ICommand SaveCommand{ get; set; }
        #endregion

        #region Command Methods
        #endregion

        #region Methods
        #endregion
    }
}
