namespace PantryTrackers.Models
{
    public class PantryTransaction
    {
        string ProductCode { get; set; }

        string ProductId { get; set; }

        string ProductName { get; set; }

        string Quantity { get; set; }

        decimal ContainerSize { get; set; }

        string Unit { get; set; }
    }
}
