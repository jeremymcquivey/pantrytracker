using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.Model.Products
{
    public class InMemoryProductsDb : List<Product>
    {
        public InMemoryProductsDb()
        {
            this.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Vegetable Oil" },
                new Product { Id = 2, Name = "soy sauce" },
                new Product { Id = 3, Name = "Canola Oil" },
                new Product { Id = 4, Name = "olive oil" },
                new Product { Id = 5, Name = "water" },
                new Product { Id = 6, Name = "distilled water" },
                new Product { Id = 7, Name = "marshmallow" },
                new Product { Id = 8, Name = "marshmallow cream" },
                new Product { Id = 9, Name = "cream cheese" },
                new Product { Id = 10, Name = "flour" },
                new Product { Id = 11, Name = "wheat flour" },
                new Product { Id = 12, Name = "corn flour" },
                new Product { Id = 13, Name = "onion" },
                new Product { Id = 14, Name = "red pepper" },
                new Product { Id = 15, Name = "green pepper" },
                new Product { Id = 16, Name = "bell pepper" },
                new Product { Id = 17, Name = "garlic" },
                new Product { Id = 18, Name = "ginger" },
                new Product { Id = 19, Name = "black pepper" },
                new Product { Id = 20, Name = "cheddar cheese" },
                new Product { Id = 21, Name = "egg" },
                new Product { Id = 22, Name = "jalapeno" },
                new Product { Id = 23, Name = "jalapeno potato chip" },
                new Product { Id = 24, Name = "lime" },
                new Product { Id = 25, Name = "pork chop" },
                new Product { Id = 26, Name = "margarine" },
                new Product { Id = 27, Name = "butter" },
                new Product { Id = 28, Name = "unsalted butter" },
                new Product { Id = 29, Name = "powdered milk" },
                new Product { Id = 30, Name = "salt" },
                new Product { Id = 31, Name = "sugar" },
                new Product { Id = 32, Name = "yeast" },
                new Product { Id = 33, Name = "italian sausage" },
                new Product { Id = 34, Name = "rice" },
                new Product { Id = 35, Name = "brown rice" },
                new Product { Id = 36, Name = "parmesan cheese" },
                new Product { Id = 37, Name = "pizza sauce" },
                new Product { Id = 38, Name = "spaghetti sauce" },
                new Product { Id = 39, Name = "shortening" },
                new Product { Id = 40, Name = "honey" },
                new Product { Id = 41, Name = "raw honey" },
                new Product { Id = 42, Name = "beef sirloin"},
                /*new Product { Id = 43, Name = "butterfinger", OwnerId = 12},
                new Product { Id = 44, Name = "italian seasoning", OwnerId = 32},
                new Product { Id = 44, Name = "mozzarella cheese", OwnerId = 32},
                new Product { Id = 98, Name = "butterfinger", OwnerId = 32},*/
                new Product { Id = 112, Name = "butterfinger"},
            });
        }
    }
}