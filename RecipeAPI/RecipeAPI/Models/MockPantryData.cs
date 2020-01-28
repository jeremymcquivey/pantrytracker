using System.Collections.Generic;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Products;
using System;

namespace RecipeAPI.MockData
{
    public class MockPantryData : List<PantryTransaction>
    {
        public MockPantryData()
        {
            AddRange(new List<PantryTransaction>
            {
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 44,
                        Name = "mozzarella cheese"
                    },
                    ProductId = 44,
                    Unit = "lb",
                    Quantity = 2,
                    UserId = Guid.Parse(""),
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 44,
                        Name = "mozzarella cheese"
                    },
                    ProductId = 44,
                    Unit = "oz",
                    Quantity = 16,
                    UserId = Guid.Parse(""),
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 44,
                        Name = "mozzarella cheese"
                    },
                    ProductId = 44,
                    UserId = Guid.Parse(""),
                    Unit = "oz",
                    Quantity = -8,
                    TransactionType = PantryTransactionType.Usage
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 37,
                        Name = "pizza sauce"
                    },
                    ProductId = 37,
                    UserId = Guid.Parse(""),
                    Unit = "fl oz",
                    Quantity = 1,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    ProductId = 44,
                    Product = new Product
                    {
                        Id = 44,
                        Name = "mozzarella cheese"
                    },
                    UserId = Guid.Parse(""),
                    Unit = "oz",
                    Quantity = 12,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 33,
                        Name = "italian sausage"
                    },
                    ProductId = 33,
                    UserId = Guid.Parse(""),
                    Unit = "lbs",
                    Quantity = 25,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 33,
                        Name = "onion"
                    },
                    ProductId = 13,
                    UserId = Guid.Parse(""),
                    Unit = "",
                    Quantity = 5,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 34,
                        Name = "rice"
                    },
                    ProductId = 34,
                    UserId = Guid.Parse(""),
                    Unit = "lbs",
                    Quantity = 10,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 36,
                        Name = "parmesan cheese"
                    },
                    ProductId = 36,
                    UserId = Guid.Parse(""),
                    Unit = "cup",
                    Quantity = 2,
                    TransactionType = PantryTransactionType.Addition
                },
                new PantryTransaction
                {
                    Product = new Product
                    {
                        Id = 44,
                        Name = "mozzarella cheese"
                    },
                    ProductId = 44,
                    UserId = Guid.Parse(""),
                    Unit = "oz",
                    Quantity = 20,
                    TransactionType = PantryTransactionType.Addition
                }
            });
        }
    }
}
