using System.Collections.Generic;

namespace PantryTrackers.Models
{
    public static class ProductDisplayMode
    {
        public const int EachUnit = 1;

        public const int PurchaseQuantity = 2;
    }

    public class ProductGroup
    {
        public string Header { get; set; }

        public string Total { get; set; }

        public int DisplayMode { get; set; }

        public IEnumerable<PantryTransaction> Elements { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultUnit { get; set; }
    }

    public class ProductVariety
    {
        public string Description { get; set; }
    }
}
