using Prism.Mvvm;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PantryTrackers.Models
{
    public class BindableCollection<T> : BindableBase
    {
        public Command<T> ItemSelectedCommand { set; get; }

        public IList<T> Collection { get; private set; }
        
        public BindableCollection(IList<T> collection)
        {
            Collection = collection;
        }
    }
}
