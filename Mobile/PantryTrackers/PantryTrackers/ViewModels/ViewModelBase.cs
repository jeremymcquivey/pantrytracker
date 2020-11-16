using PantryTrackers.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    public class ViewModelBase : BindableBase, INavigatedAware
    {
        protected INavigationService NavigationService { get; private set; }

        protected RestClient Client { get; private set; }

        public ViewModelBase(INavigationService navigationService, RestClient client)
        {
            NavigationService = navigationService;
            Client = client;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (Client != default)
            {
                Client.OnBadRequestErrorOccurred -= OnNetworkErrorOccurred;
                Client.OnForbiddenResponse -= OnNetworkErrorOccurred;
                Client.OnNotFoundResponse -= OnNetworkErrorOccurred;
                Client.OnServerErrorOccurred -= OnNetworkErrorOccurred;
                Client.OnServiceUnavailable -= OnNetworkErrorOccurred;
                Client.OnUnauthorizedResponse -= OnNetworkErrorOccurred;
            }
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Client != default)
            {
                Client.OnBadRequestErrorOccurred += OnNetworkErrorOccurred;
                Client.OnForbiddenResponse += OnNetworkErrorOccurred;
                Client.OnNotFoundResponse += OnNetworkErrorOccurred;
                Client.OnServerErrorOccurred += OnNetworkErrorOccurred;
                Client.OnServiceUnavailable += OnNetworkErrorOccurred;
                Client.OnUnauthorizedResponse += OnNetworkErrorOccurred;
            }
        }

        protected virtual void OnNetworkErrorOccurred(object sender, System.Net.Http.HttpResponseMessage e)
        {
            // TODO: Handle bad requests here.
        }
    }
}
