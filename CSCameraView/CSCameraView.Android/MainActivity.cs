using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content.Res;
using Android.Support.V4.Content;
using Android;
using Plugin.FirebasePushNotification;
using Android.Content;
using Plugin.CurrentActivity;
using Acr.UserDialogs;
using CSCameraView.Classes;
using System.Collections.Generic;
using Android.Support.V4.App;
using System.Threading.Tasks;
using CSCameraView.Dependency;
using Xamarin.Forms;
using CSCameraView.Droid.Services;
using Android.App.Job;
using CSCameraView.Droid.JobSchedular;
using Android.Telephony;
using Firebase.Iid;
using Android.Util;
using Android.Text;

namespace CSCameraView.Droid
{
   
    [Activity(Label = "SEVA - Select Entry Video Access", Icon = "@mipmap/New_RIcon",MainLauncher =true, ScreenOrientation = ScreenOrientation.Portrait, ExcludeFromRecents =true,LaunchMode =LaunchMode.SingleTop,ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )] 
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        internal static readonly string CHANNEL_ID = "SESChannelName";
        internal static readonly int NOTIFICATION_ID = 888;
     
        public static MainActivity Current { get; internal set; }
        public static readonly int PickImageId = 1000;

        FirebasePushNotificationManager notificationManager = new FirebasePushNotificationManager();

        protected override void OnCreate(Bundle savedInstanceState)
        {
          
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            notificationManager.OnTokenRefresh += NotificationManager_OnTokenRefresh;

            base.OnCreate(savedInstanceState);

            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
          
            AssetManager assets = Assets;

            var phoneCallWatcherIntent = new Intent(ApplicationContext, typeof(PhoneCallWatcher));
            SendBroadcast(phoneCallWatcherIntent);

            Settings.IsServiceRunning= IsServiceRunning(typeof(LinePhoneService1));

            var body = Intent.GetStringExtra("body");
            if (body == null)
            {
                CSCameraView.Classes.Helper.WriteLog($"Extra body = null");
                //set the this property to false when user opens the app first time it shows the Main Page always and when if call is received and app is in killed state then open the App will shows the WaiPage Screen instead of MainPage
                //Check this property at time of app open


                if (Intent.GetBooleanExtra("FROM_BOOT", false))
                {
                    CSCameraView.Classes.Helper.WriteLog($"Load Application from Boot");

                    //Set Global IsStartedFromBoot = true to notify the service that application has auto started on phone boot.
                    //
                    Settings.IsStartedFromBoot = true; 
                }
                App app = new App(this.Handle);
                LoadApplication(app);
                FirebasePushNotificationManager.ProcessIntent(this, Intent);
                
            }
            else if(Intent.GetStringExtra("NotificationType") == "MISSEDCALL")
            {
                CSCameraView.Classes.Helper.WriteLog($"LoadApplication From Missed Call Notification");
                App app = new App(page: "CallHistoryPage");
                LoadApplication(app);
                FirebasePushNotificationManager.ProcessIntent(this, Intent);
            }
            else
            {
                //call screen scenario
                CSCameraView.Classes.Helper.WriteLog($"Extra body = {body}");
                CSCameraView.Classes.Helper.WriteLog($"LoadApplication Else Scenario");

                //set these flags because the call process page is needed on lockscreen incase on ongoing call
                this.Window.AddFlags(WindowManagerFlags.DismissKeyguard | WindowManagerFlags.ShowWhenLocked| WindowManagerFlags.KeepScreenOn | WindowManagerFlags.TurnScreenOn);
                App app = new App(this.Handle,IsTaskRoot);
                LoadApplication(app);
                FirebasePushNotificationManager.ProcessIntent(this, Intent);
            }
            Current = this;

            DependencyService.Register<IAndroidService, CSCameraView.Droid.Dependency.AndroidServiceHelper>();
            Permissions();      

        }

        private void NotificationManager_OnTokenRefresh(object source, Plugin.FirebasePushNotification.Abstractions.FirebasePushNotificationTokenEventArgs e)
        {
            string token = e.Token;
        }

        protected override void OnDestroy()
        {
           
            base.OnDestroy();
        }
        protected override void OnPause()
        {
          
            base.OnPause();
        }


        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
           

        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {

            CSCameraView.Classes.Helper.WriteLog(DateTime.Now.ToString());
            CSCameraView.Classes.Helper.WriteLog("statrt TaskScheduler_UnobservedTaskException:"+e.Exception.Message +"\n" +e.Exception.StackTrace);
           
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CSCameraView.Classes.Helper.WriteLog(DateTime.Now.ToString());
            CSCameraView.Classes.Helper.WriteLog("statrt CurrentDomain_UnhandledException"+e.ExceptionObject);

        }

        protected override void OnResume()
        {
            
            base.OnResume();
           // BaseContext.UnregisterReceiver(receiver);
            OverlayPermissions();

            try
            {
                //LibVLCSharp.Shared.Core.Initialize();
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != (int)Permission.Granted)
                {
                    RequestPermissions(new string[] { Manifest.Permission.RecordAudio }, 0);
                }
              

            }
            catch (Exception ex)
            {


            }
        }

   
     
        private void OverlayPermissions()
        {
            if (!Android.Provider.Settings.CanDrawOverlays(this))
            {
                //StartActivityForResult(new Android.Content.Intent(Android.Provider.Settings.ActionManageOverlayPermission, Android.Net.Uri.Parse("package:" + PackageName)), 0);
            }
        }
        
        public bool IsServiceRunning(System.Type ClassTypeof)
        {
            ActivityManager manager = (ActivityManager)ApplicationContext.GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if(service.Service.ClassName.Contains(ClassTypeof.Name))
                {
                    return true;
                }
                
            }
            return false;
        }

        private void Permissions()
        {
            try
            {
                List<string> list = new List<string>();
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.RecordAudio);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.Camera);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.CallPhone) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.CallPhone);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadCallLog) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.ReadCallLog);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RequestIgnoreBatteryOptimizations) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.RequestIgnoreBatteryOptimizations);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.ReadExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.WriteExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadUserDictionary) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.WriteExternalStorage);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteUserDictionary) != (int)Permission.Granted)
                {
                    list.Add(Manifest.Permission.WriteExternalStorage);
                }
                string[] str = list.ToArray();
                if (str != null && str.Length != 0)
                {
                    ActivityCompat.RequestPermissions(this, str, 101);
                }
                if (!Android.Provider.Settings.CanDrawOverlays(this))
                {
                    if ("xiaomi" == Build.Manufacturer.ToLower())
                    {
                        var intent = new Intent("miui.intent.action.APP_PERM_EDITOR");
                        intent.SetClassName("com.miui.securitycenter",
                                "com.miui.permcenter.permissions.PermissionsEditorActivity");
                        intent.PutExtra("extra_pkgname", this.PackageName);
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
                        alert.SetTitle("Please Enable the Additional permissions");
                        alert.SetMessage("You will not receive notifications while the app is in background if you disable these permissions");
                        alert.SetIcon(Resource.Drawable.abc_dialog_material_background);

                        alert.SetCancelable(false);
                        alert.SetButton("Go to Settings", (c, ev) =>
                        {
                            try
                            {
                                StartActivity(intent);
                            }
                            catch (Exception ex)
                            {
                                Intent myIntent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission);
                                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                                AlertDialog alert = dialog.Create();
                                alert.SetTitle("Please Enable the \"Appear on Top\"  permission for the SEVA App.");
                                alert.SetMessage("You will not receive notifications while the app is in background if this permission is not enabled");
                                alert.SetIcon(Resource.Drawable.abc_dialog_material_background);
                                alert.SetCancelable(false);
                                alert.SetButton("Go to Settings", (c, ev) =>
                                {
                                    StartActivity(myIntent);
                                });
                                alert.Show();
                            }
                            
                        });
                        alert.Show();
                        
                    }
                    else
                    {
                        Intent myIntent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission);
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
                        alert.SetTitle("Please Enable the \"Appear on Top\"  permission for the SEVA App.");
                        alert.SetMessage("You will not receive notifications while the app is in background if this permission is not enabled");
                        alert.SetIcon(Resource.Drawable.abc_dialog_material_background);
                        alert.SetCancelable(false);
                        alert.SetButton("Go to Settings", (c, ev) =>
                        {
                            StartActivity(myIntent);
                        });
                        alert.Show();
                    }
                }
            }
            catch(Exception Ex)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Error");
                alert.SetMessage(""+Ex.Message);
                alert.Show();
            }
          
        }
    }
}