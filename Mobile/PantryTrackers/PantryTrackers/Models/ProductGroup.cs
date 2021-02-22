using System.Collections.Generic;

namespace PantryTrackers.Models
{
    public class ProductGroup
    {
        public string Header { get; set; }

        public string Total { get; set; }

        public int DisplayMode { get; set; }

        public IEnumerable<PantryTransaction> Elements { get; set; }
    }
}
