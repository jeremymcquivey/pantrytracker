using PantryTrackers.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        private MenuViewModel _vm;

        public Menu()
        {
            InitializeComponent();
            _vm = BindingContext as MenuViewModel;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            _vm.DoSomething();
        }
    }
}