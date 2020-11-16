using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodeScannerPage : ContentPage
    {
        public BarcodeScannerPage()
        {
            InitializeComponent();
        }
    }
}