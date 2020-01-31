namespace PantryTrackers.Integrations.Kroger
{
    internal class KrogerProduct
    {
        public string ProductId { get; set; }

        public string Description { get; set; }

        public string UPC { get; set; }

        public string Brand { get; set; }

        public KrogerItem[] Items { get; set; }
    }
}
