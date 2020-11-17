using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using PantryTrackers.Config;
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

            // 3rd party initializations
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
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

