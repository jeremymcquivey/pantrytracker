using System.Collections.Generic;

namespace PantryTrackers.Models.Products
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultUnit { get; set; }

        public int QuantityDisplayMode { get; set; }

        public IEnumerable<ProductVariety> Varieties { get; set; }
    }
}
