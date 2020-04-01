﻿using Microsoft.EntityFrameworkCore;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI
{
    public class ProductService
    {
        private readonly Func<Dictionary<Tuple<int, int?>, string[]>>  _productDelegate;
        private readonly RecipeContext _database;
        private readonly ICacheManager _cache;

        public ProductService(ICacheManager cache, RecipeContext database)
        {
            _database = database;
            _cache = cache;

            _productDelegate = () => GetProductBreakdowns(ownerId: null);
        }

        public Product GetById(int id)
        {
            return _database.Products.SingleOrDefault(product => product.Id == id);
        }

        public ProductVariety GetVariety(int? id)
        {
            if(id.HasValue)
            {
                return _database.Varieties.SingleOrDefault(variety => variety.Id == id.Value);
            }

            return null;
        }

        public KeyValuePair<Tuple<int, int?>, string[]> MatchProduct(string description)
        {
            var products = _cache.Get("AllProducts", _productDelegate, TimeSpan.FromDays(1));

            return products.Where(list => list.Value.All(q => description.Contains(q, StringComparison.CurrentCultureIgnoreCase)))
                           .OrderByDescending(p => p.Value.Length)
                           .FirstOrDefault();
        }

        private Dictionary<Tuple<int, int?>, string[]> GetProductBreakdowns(string ownerId)
        {
            var varieties = _database.Varieties.Include(v => v.Product)
                                               .ToList();

            return _database.Products.Where(p => p.OwnerId == ownerId || p.OwnerId == null)
                                     .Select(p => new ProductVariety
                                     {
                                         Product = p,
                                         ProductId = p.Id,
                                         Description = null,
                                         Id = 0
                                     }).ToList()
                                     .Union(varieties)
                                     .ToDictionary(variety => new Tuple<int, int?>(variety.ProductId, variety.Id), p => ((p.Description?.Split(" ", StringSplitOptions.None) ?? new string[0])
                                                                                                                                        .Concat(p.Product.Name.Split(" ", StringSplitOptions.None)))
                                                                                                                                        .ToArray());
        }
    }
}