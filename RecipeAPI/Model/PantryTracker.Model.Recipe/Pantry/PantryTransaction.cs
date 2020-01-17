using System;
using PantryTracker.Model.Products;

namespace PantryTracker.Model.Pantry
{
    public class PantryTransaction
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }

        public string Size { get; set; }

        public int ProductId { get; set; }

        public PantryTransactionType TransactionType { get; set; }

        public virtual Product Product { get; set; }
    }
}
