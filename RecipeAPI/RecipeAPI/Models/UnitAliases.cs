using RecipeAPI.Models;
using System;
using System.Collections.Generic;

namespace RecipeAPI.MockData
{
    public class UnitAliases : List<Tuple<string, string>>
    {
        public UnitAliases()
        {
            AddRange(new List<Tuple<string, string>>
            {   // **** lb **** \\
                new Tuple<string, string>(UnitsOfWeight.Pound, "lbs"),
                new Tuple<string, string>(UnitsOfWeight.Pound, "pound"),
                new Tuple<string, string>(UnitsOfWeight.Pound, "pounds"),

                // **** oz **** \\
                new Tuple<string, string>(UnitsOfWeight.Ounce, "ozs"),
                new Tuple<string, string>(UnitsOfWeight.Ounce, "ounce"),
                new Tuple<string, string>(UnitsOfWeight.Ounce, "ounces"),
                
                // **** mg **** \\
                new Tuple<string, string>(UnitsOfWeight.Milligram, "mgs"),
                new Tuple<string, string>(UnitsOfWeight.Milligram, "milligram"),
                new Tuple<string, string>(UnitsOfWeight.Milligram, "milligrams"),

                // **** g **** \\
                new Tuple<string, string>(UnitsOfWeight.Gram, "gs"),
                new Tuple<string, string>(UnitsOfWeight.Gram, "gram"),
                new Tuple<string, string>(UnitsOfWeight.Gram, "grams"),
                
                // **** kg **** \\
                new Tuple<string, string>(UnitsOfWeight.Kilogram, "kgs"),
                new Tuple<string, string>(UnitsOfWeight.Kilogram, "kilogram"),
                new Tuple<string, string>(UnitsOfWeight.Kilogram, "kilograms"),

                // **** ml **** \\
                new Tuple<string, string>(UnitsOfVolume.Milliliter, "mls"),
                new Tuple<string, string>(UnitsOfVolume.Milliliter, "milliliter"),
                new Tuple<string, string>(UnitsOfVolume.Milliliter, "milliliters"),

                // **** tsp **** \\
                new Tuple<string, string>(UnitsOfVolume.Teaspoon, "t"),
                new Tuple<string, string>(UnitsOfVolume.Teaspoon, "ts"),
                new Tuple<string, string>(UnitsOfVolume.Teaspoon, "tsps"),
                new Tuple<string, string>(UnitsOfVolume.Teaspoon, "teaspoon"),
                new Tuple<string, string>(UnitsOfVolume.Teaspoon, "teaspoons"),

                // **** tbsp **** \\
                new Tuple<string, string>(UnitsOfVolume.Tablespoon, "T"),
                new Tuple<string, string>(UnitsOfVolume.Tablespoon, "Ts"),
                new Tuple<string, string>(UnitsOfVolume.Tablespoon, "tbsps"),
                new Tuple<string, string>(UnitsOfVolume.Tablespoon, "tablespoon"),
                new Tuple<string, string>(UnitsOfVolume.Tablespoon, "tablespoons"),

                // **** fl oz **** \\
                new Tuple<string, string>(UnitsOfVolume.FluidOunce, "fluid ounce"),
                new Tuple<string, string>(UnitsOfVolume.FluidOunce, "fluid ounces"),
                new Tuple<string, string>(UnitsOfVolume.FluidOunce, "fl ozs"),
                new Tuple<string, string>(UnitsOfVolume.FluidOunce, "floz"),
                new Tuple<string, string>(UnitsOfVolume.FluidOunce, "flozs"),

                // **** c **** \\
                new Tuple<string, string>(UnitsOfVolume.Cup, "cs"),
                new Tuple<string, string>(UnitsOfVolume.Cup, "cup"),
                new Tuple<string, string>(UnitsOfVolume.Cup, "cups"),

                // **** pt **** \\
                new Tuple<string, string>(UnitsOfVolume.Pint, "pts"),
                new Tuple<string, string>(UnitsOfVolume.Pint, "pint"),
                new Tuple<string, string>(UnitsOfVolume.Pint, "pints"),

                // **** l **** \\
                new Tuple<string, string>(UnitsOfVolume.Liter, "ls"),
                new Tuple<string, string>(UnitsOfVolume.Liter, "liter"),
                new Tuple<string, string>(UnitsOfVolume.Liter, "liters"),

                // **** qt **** \\
                new Tuple<string, string>(UnitsOfVolume.Quart, "qts"),
                new Tuple<string, string>(UnitsOfVolume.Quart, "quart"),
                new Tuple<string, string>(UnitsOfVolume.Quart, "quarts"),

                // **** gal **** \\
                new Tuple<string, string>(UnitsOfVolume.Gallon, "gals"),
                new Tuple<string, string>(UnitsOfVolume.Gallon, "gallon"),
                new Tuple<string, string>(UnitsOfVolume.Gallon, "gallons"),

                // **** other units **** \\
                new Tuple<string, string>(OtherUnits.Square, "square"),
            });
        }
    }
}
