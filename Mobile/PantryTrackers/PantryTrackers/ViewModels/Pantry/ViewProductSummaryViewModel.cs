using System.Collections.ObjectModel;
using System.Linq;
using PantryTrackers.Models;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class ViewProductSummaryViewModel: ViewModelBase
    {
        private int _productId;
        private PantryService _pantry { get; }
        private Command _getInventoryCommand { get; set; }

        public ObservableCollection<PantryTransaction> ProductInventory { get; } =
            new ObservableCollection<PantryTransaction>();

        public string Message = "Hello, world!";

        public Command GetInventoryCommand => _getInventoryCommand ??= new Command(async () =>
        {
            if(_productId <= default(int))
            {
                return;
            }

            IsNetworkBusy = true;
            var currentInventory = (await _pantry.GetProductSummary(_productId))
                .SelectMany(grouping => grouping.Elements);

            ProductInventory.Clear();
            foreach(var product in currentInventory)
            {
                ProductInventory.Add(product);
            }

            Message = $"There are {currentInventory.Count()} entries.";
            RaisePropertyChanged(nameof(Message));
            IsNetworkBusy = false;
        }, CanExecute);

        public ViewProductSummaryViewModel(INavigationService navigationService, PantryService pantry):
            base(navigationService, null)
        {
            _pantry = pantry;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters?.ContainsKey("ProductId") ?? false && parameters["ProductId"].GetType() == typeof(int))
            {
                GetInventoryCommand.Execute(null);
            }
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();
            GetInventoryCommand.ChangeCanExecute();
        }
    }
}
