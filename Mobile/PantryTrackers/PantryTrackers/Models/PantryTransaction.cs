namespace PantryTrackers.Models
{
    public class PantryTransaction
    {
        public string ProductCode { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; }

        public string Quantity { get; set; }

        public string Size { get; set; }

        public string Unit { get; set; }
    }
}
