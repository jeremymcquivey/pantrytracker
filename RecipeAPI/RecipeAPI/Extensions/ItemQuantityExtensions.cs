using System;
using System.Collections.Generic;
using System.Linq;
using PantryTracker.Model;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Extensions;
using PantryTracker.Model.Meta;

namespace RecipeAPI.Extensions
{
    public static class ItemQuantityExtensions
    {
        public static IEnumerable<T> CalculateTotals<T> (this IEnumerable<T> products) where T : IItemQuantity
        {
            if (products == null || !products.Any())
            {
                return new List<T>();
            }

            return products.CombineUnits();
        }

        public static IEnumerable<T> CombineUnits<T> (this IEnumerable<T> entries) where T : IItemQuantity 
        {
            return entries.GroupBy(x => new { x.ProductId, x.VarietyId, x.Unit })
                          .Select(p =>
                          {
                              var item = (T)Activator.CreateInstance(typeof(T));
                              item.Product = p.First().Product;
                              item.Variety = p.First().Variety;
                              item.ProductId = p.Key.ProductId;
                              item.VarietyId = p.Key.VarietyId;
                              item.Unit = p.Key.Unit;
                              item.Quantity = Math.Round(p.Sum(q => q.Quantity * q.Size.ToNumber(1)), 2);
                              item.Size = string.Empty;
                              return item;
                          });
        }

        public static IEnumerable<T> CalculateProductTotals<T>(this IEnumerable<T> transactions) where T : IItemQuantity
        {
            if (transactions == null || !transactions.Any())
            {
                return null;
            }

            return transactions.GroupBy(transaction => new { transaction.VarietyId, transaction.Size, transaction.Unit })
                               .Select(group =>
                               {
                                   var newTransaction = (T)Activator.CreateInstance(typeof(T));
                                   newTransaction.VarietyId = group.Key.VarietyId;
                                   newTransaction.Size = group.Key.Size;
                                   newTransaction.Unit = group.Key.Unit;
                                   newTransaction.Product = group.First().Product;
                                   newTransaction.ProductId = group.First().ProductId;
                                   newTransaction.Variety = group.First().Variety;
                                   newTransaction.Quantity = group.Sum(trans => trans.Quantity);
                                   return newTransaction;
                               }).OrderBy(group => group.Variety?.Description)
                                 .ThenBy(group => group.Unit)
                                 .ThenByDescending(group => group.Size);
        }

        public static IEnumerable<T> CalculateTotals<T>(this IEnumerable<T> products, IEnumerable<Tuple<int?, string>> productUnits) where T : IItemQuantity
        {
            if (products == null || !products.Any())
            {
                return null;
            }

            if(productUnits == default)
            {
                productUnits = products.Select(p => new Tuple<int?, string>(p.ProductId, p.Product?.DefaultUnit ?? ""));
            }

            return products.SanitizeUnits()
                           .CombineUnits()
                           .ConvertUnits(productUnits.SanitizeUnits())
                           .CombineUnits();
        }

        private static IEnumerable<Tuple<int?, string>> SanitizeUnits(this IEnumerable<Tuple<int?, string>> units)
        {
            var sanitizedUnits = new List<Tuple<int?, string>>();
            var unitAliases = new UnitAliases();

            foreach (var entry in units)
            {
                var officialUnit = unitAliases.FirstOrDefault(a => a.Item2.Equals(entry.Item2, StringComparison.CurrentCultureIgnoreCase))
                                             ?.Item1;
                if (officialUnit != null)
                {
                    sanitizedUnits.Add(new Tuple<int?, string>(entry.Item1, officialUnit));
                    Console.WriteLine($"2. Found {entry.Item2} to be an alternative version of {officialUnit}");
                }
                else
                {
                    sanitizedUnits.Add(entry);
                }
            }

            return sanitizedUnits;
        }

        private static IEnumerable<T> SanitizeUnits<T>(this IEnumerable<T> entries) where T : IItemQuantity
        {
            var unitAliases = new UnitAliases();

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

        private static IEnumerable<T> ConvertUnits<T>(this IEnumerable<T> entries, IEnumerable<Tuple<int?, string>> productUnits) where T : IItemQuantity
        {
            var convertedElements = new List<T>();
            var unitConversions = new UnitConversions();

            foreach (var entry in entries)
            {
                var destinationUnit = productUnits.FirstOrDefault(productUnit => productUnit.Item1 == entry.ProductId)?.Item2;
                var sanitizedUnit = (entry.Unit ?? "").Replace(".", string.Empty);

                var conversion = unitConversions.Where(convert => ((convert.DestinationUnit == sanitizedUnit && convert.SourceUnit == destinationUnit) ||
                                                                    (convert.DestinationUnit == destinationUnit && convert.SourceUnit == sanitizedUnit)) &&
                                                                       (convert.Products == null || convert.Products.Contains(entry.ProductId)))
                                                     .OrderByDescending(c => c.Products?.Contains(entry.ProductId) ?? false)
                                                     .FirstOrDefault();

                ConvertUnit(entry, conversion, sanitizedUnit);
                convertedElements.Add(entry);
            }

            return convertedElements;
        }

        private static void ConvertUnit<T>(T entry, UnitConversionRate conversion, string sourceUnit) where T : IItemQuantity
        {
            if (conversion == null || conversion.ConversionScale.IsApproximatelyEqual(0, 0.01))
            {
                Console.WriteLine($"No need to convert {entry.Unit} because it's already the requested unit or has no conversion defined.");
                return;
            }

            entry.Unit = conversion.DestinationUnit;
            if (sourceUnit == conversion.SourceUnit)
            {
                var convertedValue = Math.Round(entry.Quantity * conversion.ConversionScale, UnitConversions.DecimalPrecision);
                Console.WriteLine($"{entry.Quantity} {entry.Unit} converted to {convertedValue} {conversion.DestinationUnit} by multiplying by {conversion.ConversionScale}");
                entry.Quantity = convertedValue;
                entry.Unit = conversion.DestinationUnit;
            }
            else
            {
                var convertedValue = Math.Round(entry.Quantity / conversion.ConversionScale, UnitConversions.DecimalPrecision);
                Console.WriteLine($"{entry.Quantity} {entry.Unit} converted to {convertedValue} {conversion.SourceUnit} by dividing by {conversion.ConversionScale}");
                entry.Quantity = convertedValue;
                entry.Unit = conversion.SourceUnit;
            }
        }
    }
}
