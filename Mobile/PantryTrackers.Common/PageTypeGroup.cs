using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PantryTrackers.Common
{
    public class PageTypeGroup<T> : ObservableCollection<T>
    {
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Subtitle { get; set; }

        public PageTypeGroup(IEnumerable<T> elements) :
            base(elements) { }
    }
}
