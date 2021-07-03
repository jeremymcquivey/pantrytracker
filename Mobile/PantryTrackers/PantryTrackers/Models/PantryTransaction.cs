using PantryTrackers.Models.Meta.Enums;
using PantryTrackers.Models.Products;

namespace PantryTrackers.Models
{
    public class PantryTransaction
    {
        public string ProductCode { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public string Quantity { get; set; }

        public string Container { get; set; }

        public string Size { get; set; }

        public string Unit { get; set; }

        public double TotalAmount { get; set; }

        public int TransactionType { get; set; } = TransactionTypes.Addition;

        public string ToString(int displayMode) 
        {
            switch(displayMode)
            {
                case ProductDisplayMode.EachUnit:
                    return $"{ Variety?.Description ?? ProductName } {Quantity} { Container ?? "ct" }";
                case ProductDisplayMode.PurchaseQuantity:
                default:
                    return $"{ Variety?.Description ?? ProductName } {TotalAmount} { Unit } {(!string.IsNullOrEmpty(Container) ? $"({Quantity} {Container})": "")}";
            }
        }

        public ProductVariety Variety { get; set; }
    }
}
