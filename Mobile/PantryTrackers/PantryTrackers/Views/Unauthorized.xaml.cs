
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views
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