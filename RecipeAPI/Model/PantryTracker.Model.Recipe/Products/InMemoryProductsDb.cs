using System.Collections.Generic;

namespace PantryTracker.Model.Products
{
    public class InMemoryProductsDb : List<Product>
    {
        public InMemoryProductsDb()
        {
            this.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Soy Sauce", OwnerId = null },
                new Product { Id = 2, Name = "Vegetable Oil", OwnerId = null },
                new Product { Id = 3, Name = "Canola Oil", OwnerId = null },
                new Product { Id = 4, Name = "Olive Oil", OwnerId = null },
                new Product { Id = 6, Name = "Water", OwnerId = null },
                new Product { Id = 9, Name = "Distilled Water", OwnerId = null },
                new Product { Id = 10, Name = "Salt", OwnerId = null },
                new Product { Id = 11, Name = "Powdered Milk", OwnerId = null },
                new Product { Id = 13, Name = "Corn Flour", OwnerId = null },
                new Product { Id = 14, Name = "Wheat Flour", OwnerId = null },
                new Product { Id = 16, Name = "Flour", OwnerId = null },
                new Product { Id = 17, Name = "Margarine", OwnerId = null },
                new Product { Id = 18, Name = "Salted Butter", OwnerId = null },
                new Product { Id = 19, Name = "Unsalted Butter", OwnerId = null },
                new Product { Id = 20, Name = "Shortening", OwnerId = null },
                new Product { Id = 21, Name = "Italian Seasoning", OwnerId = null },
                new Product { Id = 22, Name = "Minced Garlic", OwnerId = null },
                new Product { Id = 23, Name = "Garlic Clove", OwnerId = null },
                new Product { Id = 25, Name = "Pepper Flakes", OwnerId = null },
                new Product { Id = 26, Name = "Black Pepper", OwnerId = null },
                new Product { Id = 28, Name = "Cheddar Cheese", OwnerId = null },
                new Product { Id = 29, Name = "Yeast", OwnerId = null },
                new Product { Id = 32, Name = "Cream Cheese", OwnerId = null },
                new Product { Id = 33, Name = "Marshmallow", OwnerId = null },
                new Product { Id = 34, Name = "Marshmallow Cream", OwnerId = null },
                new Product { Id = 35, Name = "Honey", OwnerId = null },
                new Product { Id = 36, Name = "Egg", OwnerId = null },
                new Product { Id = 37, Name = "Rice", OwnerId = null },
                new Product { Id = 38, Name = "Brown Rice", OwnerId = null },
                new Product { Id = 39, Name = "Parmesan Cheese", OwnerId = null },
                new Product { Id = 40, Name = "Mozzarella Cheese", OwnerId = null },
                new Product { Id = 41, Name = "Onion", OwnerId = null },
                new Product { Id = 42, Name = "Red Pepper", OwnerId = null },
                new Product { Id = 43, Name = "Green Pepper", OwnerId = null },
                new Product { Id = 44, Name = "Bell Pepper", OwnerId = null },
                new Product { Id = 45, Name = "Garlic", OwnerId = null },
                new Product { Id = 46, Name = "Ginger", OwnerId = null },
                new Product { Id = 47, Name = "Jalapeno", OwnerId = null },
                new Product { Id = 48, Name = "Jalapeno Potato Chip", OwnerId = null },
                new Product { Id = 49, Name = "Lime", OwnerId = null },
                new Product { Id = 50, Name = "Pork Chop", OwnerId = null },
                new Product { Id = 51, Name = "Sugar", OwnerId = null },
                new Product { Id = 52, Name = "Italian Sausage", OwnerId = null },
                new Product { Id = 53, Name = "Pizza Sauce", OwnerId = null },
                new Product { Id = 54, Name = "Spaghetti Sauce", OwnerId = null },
                new Product { Id = 55, Name = "Raw Honey", OwnerId = null },
                new Product { Id = 56, Name = "Beef Sirloin", OwnerId = null },
                new Product { Id = 57, Name = "Butterfinger", OwnerId = null },
                new Product { Id = 58, Name = "Paprika", OwnerId = null }
            });
        }
    }
}