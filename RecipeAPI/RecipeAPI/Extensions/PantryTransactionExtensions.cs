using System;
using System.Collections.Generic;
using System.Linq;
using PantryTracker.Model;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Products;
using RecipeAPI.MockData;
using PantryTracker.Model.Extensions;

namespace RecipeAPI.Extensions
{
    public static class PantryTransactionExtensions
    {
        public static IEnumerable<PantryTransaction> CalculateTotals(this IEnumerable<RecipeProduct> products)
        {
            if (products == null || !products.Any())
            {
                return new List<PantryTransaction>();
            }

            return products.CombineUnits();
        }

        public static IEnumerable<PantryTransaction> CombineUnits(this IEnumerable<PantryTransaction> entries)
        {
            return entries.GroupBy(x => new { x.ProductId, x.VarietyId, x.Unit })
                          .Select(p =>
                          new PantryTransaction
                          {
                              Product = p.First().Product,
                              Variety = p.First().Variety,
                              UserId = p.First().UserId,
                              ProductId = p.Key.ProductId,
                              VarietyId = p.Key.VarietyId,
                              Quantity = Math.Round(p.Sum(q => q.Quantity * q.Size.ToNumber()),2),
                              Size = "1",
                              Unit = p.Key.Unit
                          });
        }

        public static IEnumerable<PantryTransaction> CalculateProductTotals(this IEnumerable<PantryTransaction> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return null;
            }

            return transactions.GroupBy(transaction => new { transaction.VarietyId, transaction.Size, transaction.Unit })
                               .Select(group => new PantryTransaction
                               {
                                   VarietyId = group.Key.VarietyId,
                                   Size = group.Key.Size,
                                   Unit = group.Key.Unit,
                                   Product = group.First().Product,
                                   ProductId = group.First().ProductId,
                                   Variety = group.First().Variety,
                                   Quantity = group.Sum(trans => trans.Quantity)
                               }).OrderBy(group => group.Variety?.Description)
                                 .ThenBy(group => group.Unit)
                                 .ThenByDescending(group => group.Size);
        }

        public static IEnumerable<PantryTransaction> CalculateTotals(this IEnumerable<PantryTransaction> products, IEnumerable<Tuple<int, string>> productUnits)
        {
            if (products == null || !products.Any())
            {
                return null;
            }

            if(productUnits == default)
            {
                productUnits = products.Select(p => new Tuple<int, string>(p.ProductId, p.Product?.DefaultUnit ?? ""));
            }

            return products.SanitizeUnits()
                           .CombineUnits()
                           .ConvertUnits(productUnits.SanitizeUnits())
                           .CombineUnits();
        }

        private static IEnumerable<Tuple<int, string>> SanitizeUnits(this IEnumerable<Tuple<int, string>> units)
        {
            var sanitizedUnits = new List<Tuple<int, string>>();
            var unitAliases = new MockUnitAliasData();

            foreach (var entry in units)
            {
                var officialUnit = unitAliases.FirstOrDefault(a => a.Item2.Equals(entry.Item2, StringComparison.CurrentCultureIgnoreCase))
                                             ?.Item1;
                if (officialUnit != null)
                {
                    sanitizedUnits.Add(new Tuple<int, string>(entry.Item1, officialUnit));
                    Console.WriteLine($"2. Found {entry.Item2} to be an alternative version of {officialUnit}");
                }
                else
                {
                    sanitizedUnits.Add(entry);
                }
            }

            return sanitizedUnits;
        }

        private static IEnumerable<PantryTransaction> SanitizeUnits(this IEnumerable<PantryTransaction> entries)
        {
            var unitAliases = new MockUnitAliasData();

            foreach (var entry in entries)
            {
                var officialUnit = unitAliases.FirstOrDefault(a => a.Item2.Equals(entry.Unit, StringComparison.CurrentCultureIgnoreCase))
                                             ?.Item1;
                if (officialUnit != default)
                {
                    Console.WriteLine($"1. Found {entry.Unit} to be an alternative version of {officialUnit}");
                    entry.Unit = officialUnit;
                }
            }

            return entries;
        }

        private static IEnumerable<PantryTransaction> ConvertUnits(this IEnumerable<PantryTransaction> entries, IEnumerable<Tuple<int, string>> productUnits)
        {
            var convertedElements = new List<PantryTransaction>();
            var unitConversions = new MockUnitConversionData();

            foreach (var entry in entries)
            {
                var destinationUnit = productUnits.First(productUnit => productUnit.Item1 == entry.ProductId).Item2;

                var conversion = unitConversions.Where(convert => convert.SecondaryUnit == (entry.Unit ?? "").Replace(".", string.Empty) &&
                                                                       convert.PrimaryUnit == destinationUnit &&
                                                                       (convert.ProductId == null || convert.ProductId == entry.ProductId))
                                                     .OrderByDescending(c => c.ProductId)
                                                     .FirstOrDefault();
                ConvertUnit(entry, conversion);
                convertedElements.Add(entry);
            }

            return convertedElements;
        }

        private static void ConvertUnit(PantryTransaction entry, UnitConversionRate conversion)
        {
            if (conversion == null || conversion.ConversionScale.IsApproximatelyEqual(0, 0.05))
            {
                Console.WriteLine($"No need to convert {entry.Unit} because it's already the requested unit or has no conversion defined.");
                return;
            }

            var convertedValue = Math.Round(entry.Quantity / conversion.ConversionScale, 2);
            Console.WriteLine($"{entry.Quantity} {entry.Unit} of {entry.Product.Name} converted to {convertedValue} {conversion.PrimaryUnit} by dividing by {conversion.ConversionScale}");

            entry.Quantity = convertedValue;
            entry.Unit = conversion.PrimaryUnit;
        }
    }
}
