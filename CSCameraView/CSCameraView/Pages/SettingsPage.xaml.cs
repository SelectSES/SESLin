using CSCameraView.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CSCameraView.Extensions;

namespace CSCameraView.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            sendLogSwitch.IsToggled = Settings.IsSendLogsBtnVisible;

            imageIcon.Source = ImageSource.FromResource("CSCameraView.Images.left.png", typeof(SettingsPage).GetTypeInfo().Assembly);
        }

        #region Overrides
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
        #endregion

        private void ChangePasswordTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }

        private void sendLogSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (sendLogSwitch.IsToggled)
            {
                Settings.IsSendLogsBtnVisible = true;
            }
            else
            {
                Settings.IsSendLogsBtnVisible = false;
            }
        }

        private void backBtn_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}