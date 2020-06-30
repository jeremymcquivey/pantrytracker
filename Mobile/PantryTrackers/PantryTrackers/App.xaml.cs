using Prism;
using Prism.Ioc;
using PantryTrackers.ViewModels;
using PantryTrackers.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PantryTrackers.Security;
using Prism.Unity;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PantryTrackers
{
    public partial class App
    {
        private AuthenticationService _authService;
        private bool _isAuthenticating;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("/MainPage");
        }

        protected override async void OnStart()
        {
            Startup.Init(Container.GetContainer());
            _authService = Container.Resolve<AuthenticationService>();
            _authService.SuccessfulAuthentication += SuccessfulAuthentication;
            _authService.UnsuccessfulAuthentication += UnsuccessfulAuthentication;
            _authService.SystemAuthenticationError += Error;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _authService.Authenticate();
            _isAuthenticating = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO: What to do here? Good place for a breakpoint.
        }

        private void SuccessfulAuthentication(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
            {
                _isAuthenticating = false;
                await NavigationService.NavigateAsync("/Menu");
            });
        }

        private void UnsuccessfulAuthentication(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
            {
                _isAuthenticating = false;
                await NavigationService.NavigateAsync("/Unauthorized");
            });
        }

        private void Error(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
            {
                _isAuthenticating = false;
                await NavigationService.NavigateAsync("/Error");
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<Unauthorized, UnauthorizedViewModel>();
            containerRegistry.RegisterForNavigation<Error, ErrorViewModel>();
            containerRegistry.RegisterForNavigation<Views.Menu>();

            containerRegistry.Register<AuthenticationService>();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            if(!_isAuthenticating)
            {
                //TODO: Check session validity and authorize if needed.
            }
        }
    }
}
