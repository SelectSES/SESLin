using CSCameraView.Extensions;
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
    public partial class CallHistoryPage : ContentPage
    {
        public CallHistoryPage()
        {
            InitializeComponent();
        }

        private void backBtn_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}