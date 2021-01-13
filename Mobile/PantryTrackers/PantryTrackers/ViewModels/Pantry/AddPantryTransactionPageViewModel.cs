using PantryTrackers.Controls;
using PantryTrackers.Models;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class AddPantryTransactionPageViewModel : ViewModelBase
    {
        private PantryTransaction _transaction;
        private Command _launchBarcodeScannerCommand;
        private readonly INavigationService _navService;

        public Command LaunchBarcodeScannerCommand => _launchBarcodeScannerCommand ??=
            new Command(async () => 
            { 
                await _navService.NavigateAsync(nameof(BarcodeScannerPage)); 
            });

        public PantryTransaction Transaction 
        { 
            get => _transaction; 
            set 
            { 
                _transaction = value; 
                RaisePropertyChanged(nameof(Transaction)); 
            } 
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("BarcodeScanResult"))
            {
                // We have a barcode result
            }
        }

        public AddPantryTransactionPageViewModel(INavigationService navigationService):
            base(navigationService, null)
        {
            _navService = navigationService;
        }
    }
}
