using System.Collections.Generic;
using System.Linq;

namespace PantryTrackers.Models
{
    public class ProductGroup
    {
        public string Header { get; set; }
        
        public string Total { get; set; }

        public IEnumerable<PantryLine> Elements { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultUnit { get; set; }
    }

    public class PantryLine
    {


        public ProductVariety Variety { get; set; }
    }

    public class ProductVariety
    {
        public string Description { get; set; }
    }
}
