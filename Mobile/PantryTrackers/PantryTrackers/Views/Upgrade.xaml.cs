using PantryTrackers.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Upgrade : ContentPage
    {
        private UpgradeViewModel _vm;

        public Upgrade()
        {
            InitializeComponent();
            _vm = BindingContext as UpgradeViewModel;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Launcher.OpenAsync(new System.Uri(_vm.UpdateUrl));
        }
    }
}