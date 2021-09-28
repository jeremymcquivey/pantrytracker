using PantryTrackers.ViewModels.Pantry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Pantry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPantryTransactionPage : ContentPage
    {
        private AddPantryTransactionPageViewModel _vm;

        public AddPantryTransactionPage()
        {
            InitializeComponent();
        }

        void Entry_Completed(object sender, System.EventArgs e)
        {
            if(_vm == null)
            {
                return;
            }

            _vm.Validate();
        }

        protected override void OnBindingContextChanged()
        {
            _vm = (AddPantryTransactionPageViewModel)BindingContext;
            base.OnBindingContextChanged();
        }
    }
}