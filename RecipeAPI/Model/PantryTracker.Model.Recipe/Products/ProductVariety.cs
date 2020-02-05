namespace PantryTracker.Model.Products
{
    public class ProductVariety
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }
    }
}
