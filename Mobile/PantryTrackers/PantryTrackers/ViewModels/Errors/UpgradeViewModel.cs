using PantryTrackers.Config;
using Prism.Navigation;
using PantryTrackers.Models.Meta;
using Xamarin.Forms;
using Xamarin.Essentials;
using PantryTrackers.Services;

namespace PantryTrackers.ViewModels.Errors
{
    internal class UpgradeViewModel : ViewModelBase, INavigatedAware
    {
        private readonly IPlatformConfig _config;
        private string _minVersionName;

        public Command UpdateCommand => new Command(async () =>
        {
            await Launcher.OpenAsync(new System.Uri(UpdateUrl));
        });

        public string CurrentVersion => $"{_config.VersionNumber}";

        public string CurrentVersionName => _config.VersionName;

        public string MinimumVersion { get; private set; }

        public string UpdateUrl { get; private set; }

        public string MinimumVersionName
        {
            get => _minVersionName ?? string.Empty;
            set 
            {
                _minVersionName = value;
                RaisePropertyChanged(nameof(MinimumVersionName));
            }
        }

        public UpgradeViewModel(INavigationService navigationService, IPlatformConfig config)
            : base(navigationService, null) 
        {
            _config = config;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters) { base.OnNavigatedTo(parameters); }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.ContainsKey("Stuff"))
            {
                var stuff = parameters["Stuff"] as VersionMetadata;
                MinimumVersion = $"{stuff.MinVersionCode}";
                MinimumVersionName = stuff.MinVersionName;
                UpdateUrl = "market://details?id=com.jeremymcquivey.alphatime";
            }
        }
    }
}
