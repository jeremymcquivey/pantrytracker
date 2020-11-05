using PantryTrackers.Models;
using Prism.Navigation;

namespace PantryTrackers.ViewModels.Pantry
{
    public class AddPantryTransactionPageViewModel: ViewModelBase
    {
        private PantryTransaction _transaction;

        public PantryTransaction Transaction { get => _transaction; set { _transaction = value; RaisePropertyChanged(nameof(Transaction)); } }

        public AddPantryTransactionPageViewModel(INavigationService navigationService):
            base(navigationService)
        {

        }
    }
}
