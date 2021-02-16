using PantryTrackers.ViewModels;
using Prism;
using Prism.Navigation;
using Xamarin.Forms;
using ZXing;

namespace PantryTrackers.Controls.ViewModel
{
    public class BarcodeScannerPageViewModel: ViewModelBase
    {
        private readonly INavigationService _navService;
        private Command _manualScanResultCommand;
        private Command<Result> _barcodeScanResultCommand;
        
        public string ManualTextInput { get; set; } = string.Empty;

        public Command<Result> BarcodeScanResultCommand => _barcodeScanResultCommand ??= new Command<Result>((result) =>
        {
            Device.InvokeOnMainThreadAsync(async () =>
            {
                IsNetworkBusy = true;
                await _navService.GoBackAsync(new NavigationParameters
                {
                    { "BarcodeScanResult", result.Text }
                });
                IsNetworkBusy = false;
            });
        }, _ => CanExecute());

        public Command ManualScanResultCommand => _manualScanResultCommand ??= new Command(() =>
        {
            if (ManualTextInput.Length < 10)
            {
                return;
            }

            if(BarcodeScanResultCommand.CanExecute(null))
            {
                BarcodeScanResultCommand.Execute(new Result(ManualTextInput, new byte[0], null, BarcodeFormat.UPC_A));
            }
        }, CanExecute);

        public BarcodeScannerPageViewModel(INavigationService navigationService):
            base(navigationService, null)
        {
            _navService = navigationService;
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            ManualScanResultCommand.ChangeCanExecute();
            BarcodeScanResultCommand.ChangeCanExecute();
        }
    }
}
