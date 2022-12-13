using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCameraView.Droid
{
    //[Activity(Label = "SplashActivity")]
    [Activity(Theme = "@style/MyTheme.Splash", Icon = "@drawable/NewRIconTransparent", NoHistory = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {

        string body = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            var body = Intent.GetStringExtra("body");
            if(body==null)
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
            else
            {
                using (var h = new Handler(Looper.MainLooper))
                    h.Post(() =>
                    {
                        Intent fullScreenIntent = new Intent(MainActivity.Current, typeof(MainActivity));
                        Bundle bundle = new Bundle();
                        bundle.PutString("body", "body");
                        bundle.PutString("title", "Title");
                        fullScreenIntent.PutExtras(bundle);
                     
                        fullScreenIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.FromBackground
                            | ActivityFlags.ClearTask | ActivityFlags.SingleTop | ActivityFlags.ReceiverForeground | ActivityFlags.LaunchedFromHistory);
                        StartActivity(fullScreenIntent);
                    });

            }

           

        }


    }
}