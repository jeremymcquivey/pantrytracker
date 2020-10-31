using Android.Content;
using PantryTrackers.Config;
using PantryTrackers.Droid;

public class PlatformConfig : IPlatformConfig
{
    public string PlatformName => "Android";

    public string PlatformVersion
    {
        get
        {
            return "7.0";
        }
    }

    public long VersionNumber
    {
        get
        {
            var context = MainActivity.Context;
            return context.PackageManager.GetPackageInfo(context.PackageName, 0).LongVersionCode;
        }
    }

    public string VersionName
    {
        get
        {
            var context = MainActivity.Context;
            return $"{context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName}";
        }
    }

    public ScreenResolution ScreenResolution =>
        new ScreenResolution()
        {
            Height = MainActivity.Context.Resources.DisplayMetrics.HeightPixels,
            Width = MainActivity.Context.Resources.DisplayMetrics.WidthPixels,
            DPI = (double)MainActivity.Context.Resources.DisplayMetrics.DensityDpi
        };
}