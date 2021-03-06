﻿using System.Collections.Generic;

namespace PantryTracker.Model.Products
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultUnit { get; set; }

        public string OwnerId { get; set; }

        public int QuantityDisplayMode { get; set; }

        public virtual IEnumerable<ProductVariety> Varieties { get; set; }

        public virtual IEnumerable<ProductCode> Codes { get; set; }
    }
}
