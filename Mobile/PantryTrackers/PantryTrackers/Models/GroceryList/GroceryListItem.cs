using System;
using PantryTrackers.Models.Products;

namespace PantryTrackers.Models.GroceryList
{
    public class GroceryListItem
    {
        public bool IsFreeform => !string.IsNullOrEmpty(FreeformText);

        public bool HasQuantity => Quantity > 0;

        public string SizeText
        {
            get
            {
                if(IsFreeform)
                {
                    return FreeformText;
                }

                var sizeValue = !string.IsNullOrEmpty(Unit) && Size > 0 ? $" ({Size} {Unit})" : "";
                return $"{DisplayName}{sizeValue}";
            }
        }

        public int Id { get; set; }

        public Guid PantryId { get; set; }

        public int? Quantity { get; set; }

        public string Unit { get; set; }

        public decimal? Size { get; set; }

        public decimal? TotalAmount { get; set; }

        public string DisplayName { get; set; }

        public string FreeformText { get; set; }

        public int? ProductId { get; set; }

        public int? VarietyId { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public GroceryListItemStatus Status { get; set; }

        public Product Product { get; set; }

        public ProductVariety Variety { get; set; }
    }
}
