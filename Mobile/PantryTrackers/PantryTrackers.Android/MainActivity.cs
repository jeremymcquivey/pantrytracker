using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using PantryTrackers.Config;
using PantryTrackers.Droid.Notifications;
using Prism;
using Prism.Ioc;

namespace PantryTrackers.Droid
{
    [Activity(Label = "PantryTrackers", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Context Context => Application.Context;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            IsPlayServicesAvailable();
            CreateNotificationChannel();

            // 3rd party initializations
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
        }

        public bool IsPlayServicesAvailable()
        {
            var tag = nameof(MainActivity);
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(tag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(tag, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(tag, "Google Play Services is available.");
            return true;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }


            var channel = new NotificationChannel(NotificationConstants.ChannelId, NotificationConstants.ChannelName, NotificationImportance.Default)
            {
                Description = string.Empty
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPlatformConfig, PlatformConfig>();
        }
    }
}

