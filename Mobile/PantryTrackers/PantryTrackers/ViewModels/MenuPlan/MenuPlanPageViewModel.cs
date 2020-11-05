using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    public class MenuPlanPageViewModel: ViewModelBase
    {
        private readonly INavigationService _navService;

        public MenuPlanPageViewModel(INavigationService navigationService)
            : base(navigationService)
        { 
            _navService = navigationService;
        }
    }
}
