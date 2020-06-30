using PantryTracker.Model.Products;
using System;

namespace PantryTracker.Model.Grocery
{
    public class ListItem
    {
        public int Id { get; set; }

        public string PantryId { get; set; } //For now, this is user id until we support multiple 'pantries'.

        public string Quantity { get; set; }

        public string Unit { get; set; } = "";

        public string Size { get; set; }

        public int ProductId { get; set; }

        public int? VarietyId { get; set; }

        public ListItemStatus Status { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductVariety Variety { get; set; }
    }
}
