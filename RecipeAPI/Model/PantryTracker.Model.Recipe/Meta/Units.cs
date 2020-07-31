using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.Model.Meta
{
    public class UnitService
    {
        public IEnumerable<string> AllUnits()
        {
            var list = GetConstStrings(typeof(UnitsOfVolume));
            list.AddRange(GetConstStrings(typeof(UnitsOfVolume)));
            list.AddRange(GetConstStrings(typeof(UnitsOfVolume)));
            return list;
        }

        private List<string> GetConstStrings(Type T)
        {
            var fields = T.GetFields()
                          .Where(field => field.IsLiteral && !field.IsInitOnly)
                          .Where(field => field.GetType() == typeof(string));

            return fields.Select(field => (string)field.GetValue(null))
                         .ToList();
        }
    }

    public class UnitsOfWeight
    {
        public const string Milligram = "mg";
        public const string Ounce = "oz";
        public const string Gram = "g";
        public const string Pound = "lb";
        public const string Kilogram = "kg";
    }

    public class UnitsOfVolume
    {
        public const string Teaspoon = "tsp";
        public const string Tablespoon = "tbsp";
        public const string Cup = "c";
        public const string Pint = "pt";
        public const string Quart = "qt";
        public const string Gallon = "gal";
        public const string FluidOunce = "fl oz";
        public const string Milliliter = "ml";
        public const string Liter = "l";
    }

    public class OtherUnits
    {
        public const string Pinch = "pinch";
        public const string Dash = "dash";
        public const string Jigger = "jigger";
        public const string Cube = "cube";
        public const string Square = "sq";
        public const string Dozen = "doz";
        public const string Count = "ct";
    }
}
