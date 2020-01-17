﻿using System.Collections.Generic;
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = Guid.NewGuid(),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
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
                    UserId = new Guid(""),
                    Unit = "oz",
                    Quantity = 20,
                    TransactionType = PantryTransactionType.Addition
                }
            });
        }
    }
}
