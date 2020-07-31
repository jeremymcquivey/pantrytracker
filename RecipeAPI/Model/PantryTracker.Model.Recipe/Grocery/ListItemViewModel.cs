using PantryTracker.Model.Products;
using System;

namespace PantryTracker.Model.Grocery
{
    /// <summary>
    /// This is a temporary class meant to leverage AutoMapper to bridge the gap between string and numeric quantity conversions
    /// until I get around to updating the UI to do it the right way.
    /// </summary>
    public class ListItemViewModel
    {
        public int Id { get; set; }

        public string PantryId { get; set; } //For now, this is user id until we support multiple 'pantries'.

        public string Quantity { get; set; }

        public string Unit { get; set; } = "";

        public string Size { get; set; }

        public string FreeformText { get; set; }

        public int? ProductId { get; set; }

        public int? VarietyId { get; set; }

        public ListItemStatus Status { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductVariety Variety { get; set; }
    }
}
