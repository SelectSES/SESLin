using IncomingCall.Dependency;
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
    public partial class CallConnectingPage : ContentPage
    {
        /// <summary>
        /// shows the this when user Receive the call immediately within sec based on APN Callkit UI in IOS ,and that time linephone call are not get then need to shows the connecting page insted of callprocess page
        /// </summary>
        public CallConnectingPage()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception EX)
            {

            }
            

         
        }
    }
}