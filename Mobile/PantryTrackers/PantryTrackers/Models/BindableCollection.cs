using Prism.Mvvm;
using System.Collections.Generic;

namespace PantryTrackers.Models
{
    public class BindableCollection<T> : BindableBase
    {
        public IList<T> Collection { get; private set; }
        
        public BindableCollection(IList<T> collection)
        {
            Collection = collection;
        }
    }
}
