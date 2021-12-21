using PantryTrackers.Common.Security;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Errors
{
    public class UnauthorizedViewModel : ViewModelBase
    {
        //private AuthenticationService _authService;
        private Command _loginCommand;
        private bool _loginSuccessful;

        public bool LoginSuccessful
        {
            get => _loginSuccessful;
            private set
            {
                _loginSuccessful = value;
                RaisePropertyChanged(nameof(LoginSuccessful));
            }
        }
    
        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(() =>
        {
            IsNetworkBusy = true;
            LoginSuccessful = true;
            //_authService.Authenticate();
            IsNetworkBusy = false;
        }, CanExecute));
        
        public UnauthorizedViewModel(INavigationService navService)
            : base(navService, null)
        {
            //_authService = App.Current.Container.Resolve(typeof(AuthenticationService)) as AuthenticationService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
            if(parameters.ContainsKey("SuccessfulLogin"))
            {
                LoginSuccessful = (bool)parameters["SuccessfulLogin"];
            }
            else
            {
                LoginSuccessful = true;
            }
        }
    }
}
