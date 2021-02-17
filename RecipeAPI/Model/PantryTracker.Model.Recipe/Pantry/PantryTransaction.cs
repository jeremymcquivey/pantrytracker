using System;
using PantryTracker.Model.Products;

namespace PantryTracker.Model.Pantry
{
    public interface IItemQuantity
    {
        string Unit { get; set; }

        int Quantity { get; set; }

        double Size { get; set; }

        double TotalAmount { get; set; }

        int? ProductId { get; set; }

        int? VarietyId { get; set; }

        /// <summary>
        /// Intended to be virtual
        /// </summary>
        Product Product { get; set; }

        ProductVariety Variety { get; set; }
    }

    public class PantryTransaction: IItemQuantity
    {
        public PantryTransaction() { }

        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int Quantity { get; set; } = 1;

        public string Unit { get; set; } = "";

        public double Size { get; set; }

        public double TotalAmount { get; set; }

        public int? ProductId { get; set; }

        public int? VarietyId { get; set; }

        public PantryTransactionType TransactionType { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductVariety Variety { get; set; }
    }
}
