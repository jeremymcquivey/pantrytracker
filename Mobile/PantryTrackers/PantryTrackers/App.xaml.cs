using Prism;
using Prism.Ioc;
using PantryTrackers.ViewModels;
using PantryTrackers.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PantryTrackers.Common.Security;
using Prism.Unity;
using System;
using Prism.Navigation;
using PantryTrackers.Models.Meta;
using PantryTrackers.Views.Recipes;
using System.Net.Http;
using PantryTrackers.Services;
using PantryTrackers.Views.MenuPlan;
using PantryTrackers.Views.Pantry;
using PantryTrackers.ViewModels.Pantry;
using PantryTrackers.Views.NavMenu;
using PantryTrackers.ViewModels.NavMenu;
using PantryTrackers.Views.Errors;
using PantryTrackers.ViewModels.Errors;
using PantryTrackers.Controls;
using PantryTrackers.Controls.ViewModel;
using PantryTrackers.Views.Admin;
using PantryTrackers.ViewModels.Admin;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PantryTrackers
{
    public partial class App
    {
        private AuthenticationService _authService;
        private MetadataService _metadataService;
        private bool _isAuthenticating;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync($"/{nameof(Unauthorized)}");
        }

        protected override async void OnStart()
        {
            Startup.Init(Container.GetContainer());
            _authService = Container.Resolve<AuthenticationService>();
            _authService.SuccessfulAuthentication += SuccessfulAuthentication;
            _authService.UnsuccessfulAuthentication += UnsuccessfulAuthentication;
            _authService.SystemAuthenticationError += Error;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _metadataService = Container.Resolve<MetadataService>();
            _metadataService.VersionIncompatible += (src, evt) =>
            {
                Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
                {
                    _isAuthenticating = false;
                    var navParams = new NavigationParameters
                    {
                        {
                            "Stuff",
                            new VersionMetadata
                            {
                                MinVersionCode = 125,
                                MinVersionName = "5.0"
                            }
                        }
                    };
                    await NavigationService.NavigateAsync("/Upgrade", navParams);
                });
            };

            _metadataService.VersionCompatible += (src, evt) =>
            {
                _authService.Authenticate();
                _isAuthenticating = true;
            };
            await  _metadataService.CheckVersion();
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
                await NavigationService.NavigateAsync($"/{nameof(NavMenuPage)}");
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
            containerRegistry.RegisterForNavigation<BarcodeScannerPage, BarcodeScannerPageViewModel>();

            containerRegistry.RegisterForNavigation<Unauthorized, UnauthorizedViewModel>();
            containerRegistry.RegisterForNavigation<Error, ErrorViewModel>();
            containerRegistry.RegisterForNavigation<NavMenuPage, NavMenuPageViewModel>();
            containerRegistry.RegisterForNavigation<Upgrade, UpgradeViewModel>();

            containerRegistry.RegisterForNavigation<RecipeDetailPage, RecipeDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<RecipeListPage, RecipeListPageViewModel>();

            containerRegistry.RegisterForNavigation<MenuPlanPage, MenuPlanPageViewModel>();

            containerRegistry.RegisterForNavigation<PantryMainPage, PantryMainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPantryTransactionPage, AddPantryTransactionPageViewModel>();

            containerRegistry.RegisterForNavigation<AddBarcodePage, AddBarcodePageViewModel>();
            containerRegistry.RegisterForNavigation<ProductSearchPage, ProductSearchPageViewModel>();
            containerRegistry.RegisterForNavigation<AddProductPage, AddProductPageViewModel>();

            containerRegistry.Register<AuthenticationService>();
            containerRegistry.Register<MetadataService>();

            containerRegistry.RegisterSingleton<RestClient>();
            containerRegistry.RegisterSingleton<ProductService>();
            containerRegistry.RegisterInstance(new HttpClient());
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
