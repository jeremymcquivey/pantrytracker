using Prism.Navigation;

namespace PantryTrackers.ViewModels.Errors
{
    public class UnauthorizedViewModel: ViewModelBase
    {
        public UnauthorizedViewModel(INavigationService navService)
            : base(navService, null) { }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
