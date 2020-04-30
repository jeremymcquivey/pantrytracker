using PantryTracker.Model.Products;
using System;

namespace PantryTracker.Model.Grocery
{
    public class ListItem
    {
        public int Id { get; set; }

        public string PantryId { get; set; } //For now, this is user id until we support multiple 'pantries'.

        public double Quantity { get; set; } = 1;

        public string Unit { get; set; } = "";

        public string Size { get; set; } = "1";

        public int ProductId { get; set; }

        public int? VarietyId { get; set; }

        public ListItemStatus Status { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductVariety Variety { get; set; }
    }
}
