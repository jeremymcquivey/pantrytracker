using Prism.Mvvm;
using Xamarin.Forms;

namespace PantryTrackers.Models
{
    public partial class Recipe: BindableBase
    {
        private Command _cardSelectedCommand;

        public Command CardSelectedCommand
        {
            get => _cardSelectedCommand ??= new Command((recipe) => { });
            set => _cardSelectedCommand = value;
        }

        public void RaisePropertyChanges()
        {
            RaisePropertyChanged(nameof(Description));
            RaisePropertyChanged(nameof(Tags));
        }
    }
}
