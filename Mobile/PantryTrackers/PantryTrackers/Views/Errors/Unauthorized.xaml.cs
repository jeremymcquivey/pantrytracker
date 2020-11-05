
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Errors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Unauthorized : ContentPage
    {
        public Unauthorized()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}