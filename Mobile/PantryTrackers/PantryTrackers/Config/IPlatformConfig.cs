namespace PantryTrackers.Config
{
    public interface IPlatformConfig
    {
        string PlatformName { get; }

        string PlatformVersion { get; }

        long VersionNumber { get; }

        string VersionName { get; }
    }
}
