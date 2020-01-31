namespace PantryTracker.Model.Products
{
    public class ProductCode
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public string Code { get; set; }

        public string Size { get; set; }

        public string Unit { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
