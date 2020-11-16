using PantryTrackers.Services;
using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    public class MenuPlanPageViewModel: ViewModelBase
    {
        public MenuPlanPageViewModel(INavigationService navigationService)
            : base(navigationService, null)
        { 
        }
    }
}
