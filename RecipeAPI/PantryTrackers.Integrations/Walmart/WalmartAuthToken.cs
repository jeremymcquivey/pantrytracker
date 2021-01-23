namespace PantryTrackers.Integrations.Walmart
{
    public class WalmartAuthToken
    {
        public string Signature { get; set; }

        public long TimeStamp { get; set; }

        public long ValidUntil { get; set; }

        public string ConsumerId { get; set; }
    }
}
